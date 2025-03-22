using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    // private
    Vector3 _initialPosition; // 에너미 처음 위치
    Slider _enemyHpBar;
    Transform _player; // 플레이어 위치 (타겟)
    Animator _animator;
    Rigidbody2D _rigidbody2D;
    int _maxHp = 100; // 최대 hp
    int _currentHp; // 현재 hp
    int _attackDamage; // 화살 공격 데미지 (= _damage + _addDamage)
    float _speed; // 에너미 속도
    float _attackRange; // 공격 범위
    float _attackDistance; // 플레이어와의 거리
    bool _isAttack; // 플레이어 공격 여부

    // public
    public GameObject DamageEffect;
    public GameObject BowDamageEffect;
    public GameObject CoinEffect;
    public GameObject SpawnEffect;
    public GameObject Bullet;
    public AudioSource EnemyDieSound; // 에너미 죽을 때 효과음
    [HideInInspector] public bool IsLive;

    private void Start()
    {
        // 태그로 플레이어 찾기
        _player = GameObject.FindGameObjectWithTag("PLAYER").transform;

        // 컴포넌트 가져오기
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        // EnemyHpBar를 가져오기 위해 우선 EnemyStat(부모 오브젝트)를 찾기
        Transform enemyStat = transform.Find("EnemyStat");
        // 부모의 자식 오브젝트에서 EnemyHpBar 찾고 가져오기
        _enemyHpBar = enemyStat.Find("EnemyHpBar")?.GetComponent<Slider>();

        // 초깃값 설정
        _currentHp = _maxHp;
        _isAttack = false;
        IsLive = true;
        _speed = 2f;
        _attackRange = 5f;
        _attackDistance = 1.5f;

        // 에너미 원래 위치 저장
        _initialPosition = transform.position;
    }

    private void Update()
    {
        // 플레이어와의 거리 확인
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        // 에너미 Hp 출력
        PrintEnemyHp();

        // 플레이어와의 거리가 공격 범위 안이라면
        if (distanceToPlayer <= _attackRange)
        {
            // 플레이어와의 거리가 정해둔 플레이어와의 거리보다 멀다면
            if (distanceToPlayer > _attackDistance)
            {
                // 플레이어에게 다가감
                MoveTowardsPlayer();
            }
            // 플레이어와 가까워졌다면
            else
            {
                // 공격 여부가 false일 때
                if (!_isAttack)
                {
                    // 플레이어 공격
                    AttackPlayer();
                }
            }
        }
        else
        {
            // 원래 위치로 돌아감
            ReturnToInitialPosition();
        }

        // 사망
        if (_currentHp <= 0)
        {
            Die();
        }
    }

    // 에너미 Hp 출력하는 함수
    void PrintEnemyHp()
    {
        _enemyHpBar.value = _currentHp;
        _enemyHpBar.maxValue = _maxHp;
    }

    // 플레이어에게 다가가는 함수
    void MoveTowardsPlayer()
    {
        // 플레이어 방향 확인
        Vector3 direction = (_player.position - transform.position).normalized;
        // 플레이어에게 이동
        transform.position += direction * _speed * Time.deltaTime;
    }

    // 원래 위치로 돌아가는 함수
    void ReturnToInitialPosition()
    {
        // 원래 위치와의 거리 확인
        float distanceToInitial = Vector3.Distance(transform.position, _initialPosition);

        // 원래 위치보다 0.1이라도 멀어졌다면
        if (distanceToInitial > 0.1f)
        {
            // 원래 위치 방향으로
            Vector3 direction = (_initialPosition - transform.position).normalized;
            // 이동
            transform.position += direction * _speed * Time.deltaTime;
        }
    }

    // 플레이어를 공격함
    void AttackPlayer()
    {
        // 공격 여부가 false라면
        if (!_isAttack)
        {
            // 공격 여부 true로 바꾸고
            _isAttack = true;

            // 공격 실행하는 코루틴 시작
            StartCoroutine(CoAttackPlayer());
        }
    }

    // 공격 실행하는 코루틴
    IEnumerator CoAttackPlayer()
    {
        // 6초마다 공격
        yield return new WaitForSeconds(6);

        // 공격 여부 true
        _isAttack = true;

        // 총알 생성
        Instantiate(Bullet, transform.position, Quaternion.identity);

        // 공격 여부 false로 바꾸기
        _isAttack = false;
    }

    // 플레이어로부터 공격 받는 함수 (PlayerCtrl 스크립트에서 damage를 받음)
    public void SufferDamage(int damage)
    {
        // 애니메이션 발생
        _animator.SetTrigger("Hurt");

        // 넉백
        KnockBack();

        // 데미지 입음
        _currentHp -= damage;

        // 데미지 출력 이펙트 생성
        Instantiate(DamageEffect, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
    }

    // 에너미 사망
    void Die()
    {
        EnemyDieSound.PlayOneShot(EnemyDieSound.clip);

        // 생존 여부 false로 바꾸기
        IsLive = false;

        // 코인 이펙트 생성
        Instantiate(CoinEffect, transform.position, Quaternion.identity);

        // 플레이어 경험치 25 증가
        PlayerCtrl.Instance.CurrentExp += 25;
        // 플레이어 코인 1,000원 증가
        PlayerCtrl.Instance.Coin += 1000;

        // 플레이어 경험치, 코인 값 저장
        PlayerPrefs.SetInt("CurrentExp", PlayerCtrl.Instance.CurrentExp);
        PlayerPrefs.SetInt("Coin", PlayerCtrl.Instance.Coin);

        // 에너미 비활성화
        gameObject.SetActive(false);

        // 15초 후 에너미 재활성화
        Invoke("RespawnEnemy", 15f);
    }

    // 에너미 재생성하는 함수
    void RespawnEnemy()
    {
        // 생성 이펙트 생성
        Instantiate(SpawnEffect, transform.position, Quaternion.identity);

        // 오브젝트 활성화
        gameObject.SetActive(true);

        // 초깃값 재설정
        _isAttack = false;
        IsLive = true;
        _currentHp = 100;
    }

    // 충돌 시작
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 화살과 충돌했을 시
        if (collision.gameObject.tag == "ARROW")
        {
            // 애니메이션 생성
            _animator.SetTrigger("Hurt");

            // 넉백
            KnockBack();

            // 화살 오브젝트 비활성화
            collision.gameObject.SetActive(false);

            // 기본 데미지 10 ~ 15 사이 랜덤 발생
            int damage = Random.Range(10, 15);
            // 플레이어 레벨에 따른 추가 데미지 가져오기
            int addDamage = PlayerPrefs.GetInt("AddDamage");
            // 실제 공격 받는 데미지 = 기본 데미지 + 추가 데미지
            _attackDamage = damage + addDamage;

            // 데미지 입음
            _currentHp -= _attackDamage;

            // 데미지 출력 이펙트 생성
            GameObject bowDamageEffect = Instantiate(BowDamageEffect, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);

            // AttackDamage 값 직접 설정
            bowDamageEffect.GetComponent<BowDamageEffect>().BowDamageText.text = $"{_attackDamage}";
        }
    }

    // 넉백하는 함수
    void KnockBack()
    {
        // 플레이어가 때린 방향으로
        Vector2 direction = (transform.position - _player.position).normalized;

        // 밀려나기
        _rigidbody2D.AddForce(direction * 250, ForceMode2D.Force);
    }
}

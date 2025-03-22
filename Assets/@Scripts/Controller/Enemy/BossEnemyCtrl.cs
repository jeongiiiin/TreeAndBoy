using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossEnemyCtrl : MonoBehaviour
{
    // private
    Vector3 _initialPosition; // 에너미 처음 위치
    Slider _bossEnemyHpBar;
    Transform _player; // 플레이어 위치 (타겟)
    Animator _animator;
    Rigidbody2D _rigidbody2D;
    int _maxHp = 1000; // 최대 hp
    int _currentHp; // 현재 hp
    int _attackDamage; // 화살 공격 데미지 (= _damage + _addDamage)
    float _speed; // 에너미 속도
    float _attackRange; // 공격 범위
    float _attackDistance; // 플레이어와의 거리
    bool _isAttack = false; // 플레이어 공격 여부

    // public
    public GameObject DamageEffect_Boss;
    public GameObject BowDamageEffect_Boss;
    public GameObject CoinEffect;
    public GameObject SpawnEffect;
    public GameObject BossBullet;
    public Canvas EndingText;
    public AudioSource EnemyDieSound;
    [HideInInspector] public bool IsLive;

    private void Start()
    {
        // 태그로 플레이어를 찾기
        _player = GameObject.FindGameObjectWithTag("PLAYER").transform;

        // 컴포넌트 가져오기
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        // BossEnemyHpBar를 가져오기 위해 우선 BossEnemyStat(부모 오브젝트)를 찾기
        Transform bossEnemyStat = transform.Find("BossEnemyStat");
        // 부모의 자식 오브젝트에서 BossEnemyHpBar 찾고 가져오기
        _bossEnemyHpBar = bossEnemyStat.Find("BossEnemyHpBar")?.GetComponent<Slider>();

        // 초깃값 설정
        IsLive = true;
        _currentHp = _maxHp;
        _speed = 2f;
        _attackRange = 10f;
        _attackDistance = 4f;

        // 처음 위치 저장
        _initialPosition = transform.position;
    }

    private void Update()
    {
        // 플레이어와의 거리 확인
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        // Hp바 출력
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
            // 플레이어에게 가까이 갔다면
            else
            {
                // 공격하지 않았을 때만 공격
                if (!_isAttack)
                {
                    // 플레이어 공격
                    AttackPlayer();
                }
            }
        }
        // 플레이어와의 거리가 공격 범위 밖이라면
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

    // Hp바 출력하는 함수
    void PrintEnemyHp()
    {
        _bossEnemyHpBar.value = _currentHp;
        _bossEnemyHpBar.maxValue = _maxHp;
    }

    // 플레이어에게 다가가는 함수
    void MoveTowardsPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;
    }

    // 원래 위치로 돌아가는 함수
    void ReturnToInitialPosition()
    {
        // 원래 위치와의 거리 확인
        float distanceToInitial = Vector3.Distance(transform.position, _initialPosition);

        // 원래 위치로부터 0.1이라도 멀어졌다면
        if (distanceToInitial > 0.1f)
        {
            // 원래 위치로 이동
            Vector3 direction = (_initialPosition - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;
        }
    }

    // 플레이어를 공격하는 함수
    void AttackPlayer()
    {
        // 공격하지 않았을 때만 공격
        if (!_isAttack)
        {
            // 공격 여부 true로 바꾸고
            _isAttack = true;
            // 공격 실행하는 코루틴 시작
            StartCoroutine(CoAttackPlayer());
        }
    }

    // 플레이어를 공격하는 코루틴
    IEnumerator CoAttackPlayer()
    {
        // 3초에 한 번씩 공격
        yield return new WaitForSeconds(3);

        // 공격 여부 true
        _isAttack = true;

        // 360도로 총알을 발사한다.
        for (int i = 0; i <= 360; i+= 30)
        {
            // 총알 프리팹 생성
            GameObject bossBullet = Instantiate(BossBullet, transform.position, Quaternion.identity);
            // 총알 방향 설정
            Vector2 direction = new Vector2(Mathf.Sin(i * Mathf.Deg2Rad), Mathf.Cos(i * Mathf.Deg2Rad));

            bossBullet.transform.right = direction;
            bossBullet.transform.position = transform.position;
        }

        // 공격 여부 false로 바꾸기
        _isAttack = false;
    }

    // 플레이어로부터 공격 받는 함수 (PlayerCtrl 스크립트에서 damage를 받음)
    public void SufferDamage(int damage)
    {
        // 애니메이션 발생
        _animator.SetTrigger("Hurt");

        // 데미지 입음
        _currentHp -= damage;

        // 데미지 출력 이펙트 생성
        Instantiate(DamageEffect_Boss, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
    }

    // 에너미 사망
    void Die()
    {
        EnemyDieSound.PlayOneShot(EnemyDieSound.clip);

        // 생존 여부 false로 바꾸기
        IsLive = false;

        // 코인 이펙트 발생
        Instantiate(CoinEffect, transform.position, Quaternion.identity);

        // 플레이어 경험치 100 증가
        PlayerCtrl.Instance.CurrentExp += 100;
        // 플레이어 코인 100,000원 증가
        PlayerCtrl.Instance.Coin += 100000;

        // 플레이어 경험치, 코인 값 저장
        PlayerPrefs.SetInt("CurrentExp", PlayerCtrl.Instance.CurrentExp);
        PlayerPrefs.SetInt("Coin", PlayerCtrl.Instance.Coin);

        // 에너미 비활성화
        gameObject.SetActive(false);
        // EndingUI 활성화
        EndingText.gameObject.SetActive(true);
        // 3초 후 Main씬 로드
        Invoke("ReloadScene", 3f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene("Main");
    }

    // 충돌 시작
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 화살과 충돌했을 시
        if (collision.gameObject.tag == "ARROW")
        {
            // 애니메이션 발생
            _animator.SetTrigger("Hurt");

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
            GameObject bowDamageEffect = Instantiate(BowDamageEffect_Boss, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);

            // AttackDamage 값 직접 설정
            bowDamageEffect.GetComponent<BowDamageEffect>().BowDamageText.text = $"{_attackDamage}";
        }
    }
}

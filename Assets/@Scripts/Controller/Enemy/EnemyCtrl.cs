using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    // private
    Vector3 _initialPosition; // ���ʹ� ó�� ��ġ
    Slider _enemyHpBar;
    Transform _player; // �÷��̾� ��ġ (Ÿ��)
    Animator _animator;
    Rigidbody2D _rigidbody2D;
    int _maxHp = 100; // �ִ� hp
    int _currentHp; // ���� hp
    int _attackDamage; // ȭ�� ���� ������ (= _damage + _addDamage)
    float _speed; // ���ʹ� �ӵ�
    float _attackRange; // ���� ����
    float _attackDistance; // �÷��̾���� �Ÿ�
    bool _isAttack; // �÷��̾� ���� ����

    // public
    public GameObject DamageEffect;
    public GameObject BowDamageEffect;
    public GameObject CoinEffect;
    public GameObject SpawnEffect;
    public GameObject Bullet;
    public AudioSource EnemyDieSound; // ���ʹ� ���� �� ȿ����
    [HideInInspector] public bool IsLive;

    private void Start()
    {
        // �±׷� �÷��̾� ã��
        _player = GameObject.FindGameObjectWithTag("PLAYER").transform;

        // ������Ʈ ��������
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        // EnemyHpBar�� �������� ���� �켱 EnemyStat(�θ� ������Ʈ)�� ã��
        Transform enemyStat = transform.Find("EnemyStat");
        // �θ��� �ڽ� ������Ʈ���� EnemyHpBar ã�� ��������
        _enemyHpBar = enemyStat.Find("EnemyHpBar")?.GetComponent<Slider>();

        // �ʱ갪 ����
        _currentHp = _maxHp;
        _isAttack = false;
        IsLive = true;
        _speed = 2f;
        _attackRange = 5f;
        _attackDistance = 1.5f;

        // ���ʹ� ���� ��ġ ����
        _initialPosition = transform.position;
    }

    private void Update()
    {
        // �÷��̾���� �Ÿ� Ȯ��
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        // ���ʹ� Hp ���
        PrintEnemyHp();

        // �÷��̾���� �Ÿ��� ���� ���� ���̶��
        if (distanceToPlayer <= _attackRange)
        {
            // �÷��̾���� �Ÿ��� ���ص� �÷��̾���� �Ÿ����� �ִٸ�
            if (distanceToPlayer > _attackDistance)
            {
                // �÷��̾�� �ٰ���
                MoveTowardsPlayer();
            }
            // �÷��̾�� ��������ٸ�
            else
            {
                // ���� ���ΰ� false�� ��
                if (!_isAttack)
                {
                    // �÷��̾� ����
                    AttackPlayer();
                }
            }
        }
        else
        {
            // ���� ��ġ�� ���ư�
            ReturnToInitialPosition();
        }

        // ���
        if (_currentHp <= 0)
        {
            Die();
        }
    }

    // ���ʹ� Hp ����ϴ� �Լ�
    void PrintEnemyHp()
    {
        _enemyHpBar.value = _currentHp;
        _enemyHpBar.maxValue = _maxHp;
    }

    // �÷��̾�� �ٰ����� �Լ�
    void MoveTowardsPlayer()
    {
        // �÷��̾� ���� Ȯ��
        Vector3 direction = (_player.position - transform.position).normalized;
        // �÷��̾�� �̵�
        transform.position += direction * _speed * Time.deltaTime;
    }

    // ���� ��ġ�� ���ư��� �Լ�
    void ReturnToInitialPosition()
    {
        // ���� ��ġ���� �Ÿ� Ȯ��
        float distanceToInitial = Vector3.Distance(transform.position, _initialPosition);

        // ���� ��ġ���� 0.1�̶� �־����ٸ�
        if (distanceToInitial > 0.1f)
        {
            // ���� ��ġ ��������
            Vector3 direction = (_initialPosition - transform.position).normalized;
            // �̵�
            transform.position += direction * _speed * Time.deltaTime;
        }
    }

    // �÷��̾ ������
    void AttackPlayer()
    {
        // ���� ���ΰ� false���
        if (!_isAttack)
        {
            // ���� ���� true�� �ٲٰ�
            _isAttack = true;

            // ���� �����ϴ� �ڷ�ƾ ����
            StartCoroutine(CoAttackPlayer());
        }
    }

    // ���� �����ϴ� �ڷ�ƾ
    IEnumerator CoAttackPlayer()
    {
        // 6�ʸ��� ����
        yield return new WaitForSeconds(6);

        // ���� ���� true
        _isAttack = true;

        // �Ѿ� ����
        Instantiate(Bullet, transform.position, Quaternion.identity);

        // ���� ���� false�� �ٲٱ�
        _isAttack = false;
    }

    // �÷��̾�κ��� ���� �޴� �Լ� (PlayerCtrl ��ũ��Ʈ���� damage�� ����)
    public void SufferDamage(int damage)
    {
        // �ִϸ��̼� �߻�
        _animator.SetTrigger("Hurt");

        // �˹�
        KnockBack();

        // ������ ����
        _currentHp -= damage;

        // ������ ��� ����Ʈ ����
        Instantiate(DamageEffect, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
    }

    // ���ʹ� ���
    void Die()
    {
        EnemyDieSound.PlayOneShot(EnemyDieSound.clip);

        // ���� ���� false�� �ٲٱ�
        IsLive = false;

        // ���� ����Ʈ ����
        Instantiate(CoinEffect, transform.position, Quaternion.identity);

        // �÷��̾� ����ġ 25 ����
        PlayerCtrl.Instance.CurrentExp += 25;
        // �÷��̾� ���� 1,000�� ����
        PlayerCtrl.Instance.Coin += 1000;

        // �÷��̾� ����ġ, ���� �� ����
        PlayerPrefs.SetInt("CurrentExp", PlayerCtrl.Instance.CurrentExp);
        PlayerPrefs.SetInt("Coin", PlayerCtrl.Instance.Coin);

        // ���ʹ� ��Ȱ��ȭ
        gameObject.SetActive(false);

        // 15�� �� ���ʹ� ��Ȱ��ȭ
        Invoke("RespawnEnemy", 15f);
    }

    // ���ʹ� ������ϴ� �Լ�
    void RespawnEnemy()
    {
        // ���� ����Ʈ ����
        Instantiate(SpawnEffect, transform.position, Quaternion.identity);

        // ������Ʈ Ȱ��ȭ
        gameObject.SetActive(true);

        // �ʱ갪 �缳��
        _isAttack = false;
        IsLive = true;
        _currentHp = 100;
    }

    // �浹 ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ȭ��� �浹���� ��
        if (collision.gameObject.tag == "ARROW")
        {
            // �ִϸ��̼� ����
            _animator.SetTrigger("Hurt");

            // �˹�
            KnockBack();

            // ȭ�� ������Ʈ ��Ȱ��ȭ
            collision.gameObject.SetActive(false);

            // �⺻ ������ 10 ~ 15 ���� ���� �߻�
            int damage = Random.Range(10, 15);
            // �÷��̾� ������ ���� �߰� ������ ��������
            int addDamage = PlayerPrefs.GetInt("AddDamage");
            // ���� ���� �޴� ������ = �⺻ ������ + �߰� ������
            _attackDamage = damage + addDamage;

            // ������ ����
            _currentHp -= _attackDamage;

            // ������ ��� ����Ʈ ����
            GameObject bowDamageEffect = Instantiate(BowDamageEffect, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);

            // AttackDamage �� ���� ����
            bowDamageEffect.GetComponent<BowDamageEffect>().BowDamageText.text = $"{_attackDamage}";
        }
    }

    // �˹��ϴ� �Լ�
    void KnockBack()
    {
        // �÷��̾ ���� ��������
        Vector2 direction = (transform.position - _player.position).normalized;

        // �з�����
        _rigidbody2D.AddForce(direction * 250, ForceMode2D.Force);
    }
}

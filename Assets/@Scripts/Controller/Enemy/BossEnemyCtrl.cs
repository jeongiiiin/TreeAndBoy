using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossEnemyCtrl : MonoBehaviour
{
    // private
    Vector3 _initialPosition; // ���ʹ� ó�� ��ġ
    Slider _bossEnemyHpBar;
    Transform _player; // �÷��̾� ��ġ (Ÿ��)
    Animator _animator;
    Rigidbody2D _rigidbody2D;
    int _maxHp = 1000; // �ִ� hp
    int _currentHp; // ���� hp
    int _attackDamage; // ȭ�� ���� ������ (= _damage + _addDamage)
    float _speed; // ���ʹ� �ӵ�
    float _attackRange; // ���� ����
    float _attackDistance; // �÷��̾���� �Ÿ�
    bool _isAttack = false; // �÷��̾� ���� ����

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
        // �±׷� �÷��̾ ã��
        _player = GameObject.FindGameObjectWithTag("PLAYER").transform;

        // ������Ʈ ��������
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        // BossEnemyHpBar�� �������� ���� �켱 BossEnemyStat(�θ� ������Ʈ)�� ã��
        Transform bossEnemyStat = transform.Find("BossEnemyStat");
        // �θ��� �ڽ� ������Ʈ���� BossEnemyHpBar ã�� ��������
        _bossEnemyHpBar = bossEnemyStat.Find("BossEnemyHpBar")?.GetComponent<Slider>();

        // �ʱ갪 ����
        IsLive = true;
        _currentHp = _maxHp;
        _speed = 2f;
        _attackRange = 10f;
        _attackDistance = 4f;

        // ó�� ��ġ ����
        _initialPosition = transform.position;
    }

    private void Update()
    {
        // �÷��̾���� �Ÿ� Ȯ��
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        // Hp�� ���
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
            // �÷��̾�� ������ ���ٸ�
            else
            {
                // �������� �ʾ��� ���� ����
                if (!_isAttack)
                {
                    // �÷��̾� ����
                    AttackPlayer();
                }
            }
        }
        // �÷��̾���� �Ÿ��� ���� ���� ���̶��
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

    // Hp�� ����ϴ� �Լ�
    void PrintEnemyHp()
    {
        _bossEnemyHpBar.value = _currentHp;
        _bossEnemyHpBar.maxValue = _maxHp;
    }

    // �÷��̾�� �ٰ����� �Լ�
    void MoveTowardsPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;
    }

    // ���� ��ġ�� ���ư��� �Լ�
    void ReturnToInitialPosition()
    {
        // ���� ��ġ���� �Ÿ� Ȯ��
        float distanceToInitial = Vector3.Distance(transform.position, _initialPosition);

        // ���� ��ġ�κ��� 0.1�̶� �־����ٸ�
        if (distanceToInitial > 0.1f)
        {
            // ���� ��ġ�� �̵�
            Vector3 direction = (_initialPosition - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;
        }
    }

    // �÷��̾ �����ϴ� �Լ�
    void AttackPlayer()
    {
        // �������� �ʾ��� ���� ����
        if (!_isAttack)
        {
            // ���� ���� true�� �ٲٰ�
            _isAttack = true;
            // ���� �����ϴ� �ڷ�ƾ ����
            StartCoroutine(CoAttackPlayer());
        }
    }

    // �÷��̾ �����ϴ� �ڷ�ƾ
    IEnumerator CoAttackPlayer()
    {
        // 3�ʿ� �� ���� ����
        yield return new WaitForSeconds(3);

        // ���� ���� true
        _isAttack = true;

        // 360���� �Ѿ��� �߻��Ѵ�.
        for (int i = 0; i <= 360; i+= 30)
        {
            // �Ѿ� ������ ����
            GameObject bossBullet = Instantiate(BossBullet, transform.position, Quaternion.identity);
            // �Ѿ� ���� ����
            Vector2 direction = new Vector2(Mathf.Sin(i * Mathf.Deg2Rad), Mathf.Cos(i * Mathf.Deg2Rad));

            bossBullet.transform.right = direction;
            bossBullet.transform.position = transform.position;
        }

        // ���� ���� false�� �ٲٱ�
        _isAttack = false;
    }

    // �÷��̾�κ��� ���� �޴� �Լ� (PlayerCtrl ��ũ��Ʈ���� damage�� ����)
    public void SufferDamage(int damage)
    {
        // �ִϸ��̼� �߻�
        _animator.SetTrigger("Hurt");

        // ������ ����
        _currentHp -= damage;

        // ������ ��� ����Ʈ ����
        Instantiate(DamageEffect_Boss, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
    }

    // ���ʹ� ���
    void Die()
    {
        EnemyDieSound.PlayOneShot(EnemyDieSound.clip);

        // ���� ���� false�� �ٲٱ�
        IsLive = false;

        // ���� ����Ʈ �߻�
        Instantiate(CoinEffect, transform.position, Quaternion.identity);

        // �÷��̾� ����ġ 100 ����
        PlayerCtrl.Instance.CurrentExp += 100;
        // �÷��̾� ���� 100,000�� ����
        PlayerCtrl.Instance.Coin += 100000;

        // �÷��̾� ����ġ, ���� �� ����
        PlayerPrefs.SetInt("CurrentExp", PlayerCtrl.Instance.CurrentExp);
        PlayerPrefs.SetInt("Coin", PlayerCtrl.Instance.Coin);

        // ���ʹ� ��Ȱ��ȭ
        gameObject.SetActive(false);
        // EndingUI Ȱ��ȭ
        EndingText.gameObject.SetActive(true);
        // 3�� �� Main�� �ε�
        Invoke("ReloadScene", 3f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene("Main");
    }

    // �浹 ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ȭ��� �浹���� ��
        if (collision.gameObject.tag == "ARROW")
        {
            // �ִϸ��̼� �߻�
            _animator.SetTrigger("Hurt");

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
            GameObject bowDamageEffect = Instantiate(BowDamageEffect_Boss, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);

            // AttackDamage �� ���� ����
            bowDamageEffect.GetComponent<BowDamageEffect>().BowDamageText.text = $"{_attackDamage}";
        }
    }
}

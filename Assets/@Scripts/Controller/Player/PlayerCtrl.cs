using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerCtrl : MonoBehaviour
{
    #region �ν��Ͻ� ����
    public static PlayerCtrl Instance;

    void Awake()
    {
        if (Instance == null)
        {

            Instance = this;
        }
    }
    #endregion

    // private
    Rigidbody2D _rigidbody2D;
    Animator _animator;
    GameButtonCtrl _gameButtonCtrl;
    GameBowButtonCtrl _gameBowButtonCtrl;
    MainButtonCtrl _mainButtonCtrl;
    Button _interactionButton; // ��ȣ�ۿ� ��ư
    Button _bowButton; // Ȱ ��ư
    TMP_Text _buttonText;
    TMP_Text _bowButtonText;
    Image _bowIconBefore; // Ȱ ������ (���� ��)
    Vector2 _moveDir; // �̵� ����
    int _damage; // �⺻ ������
    int _addDamage; // ������ ���� �߰��Ǵ� ������
    int _attackDamage; // ���ʹ� ���� ������ (= _damage + _addDamage)
    int _level = 1;
    int _coin = 0;
    int _currentHp;
    int _maxHp;
    int _currentExp;
    int _maxExp;
    float _speed = 4f; // �÷��̾� �ӵ�
    bool _isLive = true; // �÷��̾� ���� ���� Ȯ��
    bool _isPreparedAttack = false; // ���� �غ� ���� Ȯ��
    bool _hasAttackedSound = false; // �÷��̾� ���� ���� (ȿ����) Ȯ��

    // public
    public GameObject BuyEffect;
    public GameObject PlayerAttackEffect;
    public Canvas DieText;
    public Sprite BowIconAfter;
    public AudioSource WalkSound; // �÷��̾� �̵� ȿ����
    public AudioSource HitSound; // �÷��̾� ���� ���� ȿ����
    public AudioSource BowSound; // �÷��̾� Ȱ ���� ȿ����
    public AudioSource AttackPlayerSound; // �÷��̾� ���� ���� �� ȿ����
    public AudioSource BuySound; // ���� ȿ����
    [HideInInspector] public Vector3 Scale; // ���ý�����
    [HideInInspector] public bool IsBuy = false; // ���� ���� (��ư üũ��)
    [HideInInspector] public bool IsBuyBow = false; // ���� ���� (��ư üũ��)
    [HideInInspector] public bool IsBoughtBow = false; // ���� ���� (�۵���)
    [HideInInspector] public bool IsAttack = false; // ���� ����
    [HideInInspector] public bool HasAttacked = false; // ���� ���� ���� Ȯ��
    [HideInInspector] public bool IsAttackBow = false; // Ȱ ���� ����
    [HideInInspector] public bool HasAttackedBow = false; // Ȱ ���� ���� ���� Ȯ��


    #region ������Ƽ
    public Vector2 MoveDir { get { return _moveDir; } set { _moveDir = value; } }
    public int CurrentHp { get { return _currentHp; } set { _currentHp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int CurrentExp { get { return _currentExp; } set { _currentExp = value; } }
    public int MaxExp { get { return _maxExp; } set { _maxExp = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    public int Coin { get { return _coin; } set { _coin = value; } }
    public int AddDamage { get { return _addDamage; } set { _addDamage = value; } }
    public int AttackDamage { get { return _attackDamage; } set { _attackDamage = value; } }
    #endregion

    private void Start()
    {
        // ������Ʈ ��������
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _interactionButton = GameObject.Find("InteractionButton")?.GetComponent<Button>();
        _bowButton = GameObject.Find("BowButton")?.GetComponent<Button>();
        _buttonText = GameObject.Find("ButtonText")?.GetComponent<TMP_Text>();
        _bowButtonText = GameObject.Find("BowButtonText")?.GetComponent<TMP_Text>();
        _bowIconBefore = GameObject.Find("BowIcon")?.GetComponent<Image>();

        // �ʱ갪 ����
        Initialize();

        // ��ȣ�ۿ� ��ư�� �����Ѵٸ�
        if (_interactionButton != null)
        {
            // ��Ȱ��ȭ (��ȣ�ۿ��� �ʿ��� Ư�� ��Ȳ������ Ȱ��ȭ�ϵ��� �ϱ� ����)
            _interactionButton.interactable = false;
        }

        // Ȱ ��ư�� �����Ѵٸ�
        if (_bowButton != null)
        {
            // ��Ȱ��ȭ (Ȱ ���� �Ŀ��� Ȱ��ȭ�ϵ��� �ϱ� ����)
            _bowButton.interactable = false;
        }

        // Null ���� ����
        if (GameButtonCtrl.Instance == null)
        {
            _gameButtonCtrl = gameObject.AddComponent<GameButtonCtrl>();
        }
        if (MainButtonCtrl.Instance == null)
        {
            _mainButtonCtrl = gameObject.AddComponent<MainButtonCtrl>();
        }
        if (GameBowButtonCtrl.Instance == null)
        {
            _gameBowButtonCtrl = gameObject.AddComponent<GameBowButtonCtrl>();
        }
    }

    // �ʱ갪 ����
    void Initialize()
    {
        // Ű ���� �ִٸ� ����� �� ��������
        if (PlayerPrefs.HasKey("MaxHp") && PlayerPrefs.HasKey("CurrentHp")
         && PlayerPrefs.HasKey("MaxExp") && PlayerPrefs.HasKey("CurrentExp")
         && PlayerPrefs.HasKey("Level") && PlayerPrefs.HasKey("Coin")
         && PlayerPrefs.HasKey("AddDamage") && PlayerPrefs.HasKey("IsBoughtBow"))
        {
            _maxHp = PlayerPrefs.GetInt("MaxHp");
            _currentHp = PlayerPrefs.GetInt("CurrentHp");
            _maxExp = PlayerPrefs.GetInt("MaxExp");
            _currentExp = PlayerPrefs.GetInt("CurrentExp");
            _level = PlayerPrefs.GetInt("Level");
            _coin = PlayerPrefs.GetInt("Coin");
            _addDamage = PlayerPrefs.GetInt("AddDamage");
            // * bool�� ������ ������ ���� int������ ó��
            // : Ű�� ����� �� ������ �⺻�� 0 ��ȯ
            // : ��ȯ ���� 1���� Ȯ�� (1�̸� true, �׷��� ������ false)
            IsBoughtBow = PlayerPrefs.GetInt("IsBoughtBow", 0) == 1;
        }
        // Ű ���� ���ٸ� �� �ʱ�ȭ
        else
        {
            _maxHp = 100;
            _currentHp = _maxHp;
            _maxExp = 100;
            _currentExp = 0;
            _level = 1;
            _coin = 0;
            _addDamage = 0;
            IsBoughtBow = false;

            PlayerPrefs.SetInt("MaxHp", _maxHp);
            PlayerPrefs.SetInt("CurrentHp", _currentHp);
            PlayerPrefs.SetInt("MaxExp", _maxExp);
            PlayerPrefs.SetInt("CurrentExp", _currentExp);
            PlayerPrefs.SetInt("Level", _level);
            PlayerPrefs.SetInt("Coin", _coin);
            PlayerPrefs.SetInt("AddDamage", _addDamage);
            PlayerPrefs.SetInt("IsBoughtBow", IsBoughtBow ? 1 : 0);
        }
    }

    private void FixedUpdate()
    {
        Move(); // �̵�
        Attack(); // ���� ����

        // Ȱ�� �����Ͽ��ٸ�
        if (IsBoughtBow)
        {
            // Ȱ ������ (���� ��) �� ���� �ƴ϶��
            if (_bowIconBefore != null)
            {
                // Ȱ ������ ����
                _bowIconBefore.sprite = BowIconAfter;
            }

            AttackBow(); // Ȱ ����
        }

        // Game�� �Ǵ� BossGame������
        if (SceneManager.GetActiveScene().name == "Game" || SceneManager.GetActiveScene().name == "BossGame")
        {
            // ���� �浹���� ���� ���¶�� ��� HIT ��ư�� Ȱ��ȭ
            if (_isPreparedAttack == true)
            {
                // ��ȣ�ۿ� ��ư�� �����Ѵٸ�
                if (_interactionButton != null)
                {
                    // ��ư Ȱ��ȭ
                    _interactionButton.interactable = true;
                }

                _buttonText.text = "HIT";
            }

            // �÷��̾� ���
            DiePlayer();
        }
    }

    // �÷��̾� ��� ó���ϴ� �Լ�
    void DiePlayer()
    {
        if (_currentHp <= 0)
        {
            _isLive = false;
        }

        // ������� �ʴٸ� (����Ͽ��ٸ�)
        if (!_isLive)
        {
            // �÷��̾� ������Ʈ ��Ȱ��ȭ
            gameObject.SetActive(false);
            // DieUI Ȱ��ȭ
            DieText.gameObject.SetActive(true);
            // 3�� �� Main�� �ε�
            Invoke("ReloadScene", 3f);
        }
    }

    // �÷��̾� ��� �� Main�� �ε�
    void ReloadScene()
    {
        SceneManager.LoadScene("Main");

        // �÷��̾� Hp 10���� �ʱ�ȭ
        _currentHp = 10;
        // �÷��̾� Exp 0���� �ʱ�ȭ
        _currentExp = 0;

        // Hp, Exp �� ����
        PlayerPrefs.SetInt("CurrentHp", _currentHp);
        PlayerPrefs.SetInt("CurrentExp", _currentExp);
    }

    // �̵�
    private void Move()
    {
        // ���̽�ƽ���κ��� _moveDir�� ���� �޾� �̵���
        Vector3 moveDir = transform.position + (Vector3)_moveDir * _speed * Time.deltaTime;

        // �÷��̾� �̵� ���⿡ ���� �ִϸ��̼� Ȯ��
        _animator.SetFloat("AxisX", _moveDir.x);
        _animator.SetFloat("AxisY", _moveDir.y);

        // �÷��̾� �������� �������� ������
        if (_moveDir.magnitude > 0)
        {
            // �ִϸ��̼� Ȱ��ȭ
            _animator.SetBool("IsMove", true);

            if (!WalkSound.isPlaying)
            {
                WalkSound.Play();
            }
        }
        // �������� ������
        else
        {
            // �ִϸ��̼� ��Ȱ��ȭ
            _animator.SetBool("IsMove", false);
            WalkSound.Stop();
        }

        // �ִϸ��̼� �¿���� ����
        if (_moveDir.x != 0)
        {
            Scale = transform.localScale;
            Scale.x = (_moveDir.x > 0) ? 1 : -1;
            transform.localScale = Scale;
        }

        // * rigidbody2D.MovePosition()
        // : �̵�
        // : transform.position�� �����غ��̱� �ϳ� transform.position�� �ڷ���Ʈ�� �����̰�, ��� ���̿� �����ۿ��� �ϴ� ��ü�� �ִ��� �ƹ� ���� �Ͼ�� �ʴ´�.
        // : rigidbody2D.MovePosition�� ���� ��ġ�� �� ��ġ ������ ���� ��θ� �����ϰ� �ش� ��θ� ���� ������ �̵��� ��ó�� ó���ϰ�, ���� ��ο� ���ο� ��� ���̿� � �����ۿ��� �ϴ� ��ü�� �����ϸ� ���� �ۿ��� �Ͼ��.
        _rigidbody2D.MovePosition(moveDir);
    }

    // ���� ����
    void Attack()
    {
        // ���� ��ư�� �����ٸ�
        if (IsAttack)
        {
            if (!_hasAttackedSound)
            {
                // ���� �ִϸ��̼� �߻�
                _animator.SetTrigger("Attack");
                HitSound.PlayOneShot(HitSound.clip);
                _hasAttackedSound = true;
            }
        }
        // ���� ��ư�� ������ �ʾҴٸ�
        if (!IsAttack)
        {
            // ���� �ִϸ��̼� ��Ȱ��ȭ
            _animator.SetBool("IsAttack", false);
            _hasAttackedSound = false;
        }
    }

    // Ȱ ����
    void AttackBow()
    {
        // Ȱ ���� ��ư�� �����ٸ�
        if (IsAttackBow)
        {
            // Ȱ ������ ���� �ʾҴٸ�
            if (!HasAttackedBow)
            {
                // Ȱ ���� �ִϸ��̼� �߻�
                _animator.SetTrigger("AttackBow");

                BowSound.PlayOneShot(BowSound.clip);

                // ȭ�� ������Ʈ Ǯ �����Ͽ� �߻�
                ArrowPool.Instance.GetArrow();

                // Ȱ ���� ���� true�� ����
                HasAttackedBow = true;
            }
        }
        // Ȱ ���� ��ư�� ������ �ʾҴٸ�
        if (!IsAttackBow)
        {
            // Ȱ ���� �ִϸ��̼� ��Ȱ��ȭ
            _animator.SetBool("IsAttackBow", false);
        }
    }


    // �浹 ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �� �浹 ��
        if (collision.transform.CompareTag("DOOR"))
        {
            GameButtonCtrl.Instance.SetCanAttack(false);

            // ������ �غ� ���� ���� ������ Ȯ�� (HIT ���¿� �����ϱ� ����)
            _isPreparedAttack = false;

            // ��ȣ�ۿ� ��ư�� �����Ѵٸ�
            if (_interactionButton != null)
            {
                // ��ư Ȱ��ȭ
                _interactionButton.interactable = true;
                _buttonText.text = "ENTER";
            }

            MainButtonCtrl.Instance.IsLoad = true; // ���� ���� Game������ �̵�
            GameButtonCtrl.Instance.IsLoad = true; // ���� ���� Main������ �̵�

            // ���� �浹 �� Ȱ ���� ��ư ��Ȱ��ȭ (���� ������� ���� ������ ���� �ʵ��� �ϱ� ����)
            // Ȱ ���� ��ư�� ���� �ƴ϶��
            if (_bowButton != null)
            {
                // Ȱ ���� ��ư ��Ȱ��ȭ
                _bowButton.interactable = false;
                _bowButtonText.text = "";
            }
        }

        // ������ �� �浹 ��
        // * collision.transform.CompareTag�� �ۼ� �� ������ �����ϴ� �±����� Ȯ���ϱ� ������ ������ �߻��� (���� ������Ʈ������ ���� ���� �ش� �±װ� ���� ������)
        if (collision.gameObject.tag == "BOSSDOOR")
        {
            GameButtonCtrl.Instance.SetCanAttack(false);

            // ������ �غ� ���� ���� ������ Ȯ�� (HIT ���¿� �����ϱ� ����)
            _isPreparedAttack = false;

            // ��ȣ�ۿ� ��ư�� �����Ѵٸ�
            if (_interactionButton != null)
            {
                // ��ư Ȱ��ȭ
                _interactionButton.interactable = true;
                _buttonText.text = "ENTER";
            }

            GameButtonCtrl.Instance.IsLoadBoss = true; // ���� ���� BossGame������ �̵�

            // ���� �浹 �� Ȱ ���� ��ư ��Ȱ��ȭ (���� ������� ���� ������ ���� �ʵ��� �ϱ� ����)
            // Ȱ ���� ��ư�� ���� �ƴ϶��
            if (_bowButton != null)
            {
                // Ȱ ���� ��ư ��Ȱ��ȭ
                _bowButton.interactable = false;
                _bowButtonText.text = "";
            }
        }

        // Ż�⹮ �浹 ��
        // * collision.transform.CompareTag�� �ۼ� �� ������ �����ϴ� �±����� Ȯ���ϱ� ������ ������ �߻��� (���� ������Ʈ������ ���� ���� �ش� �±װ� ���� ������)
        if (collision.gameObject.tag == "EXITDOOR")
        {
            GameButtonCtrl.Instance.SetCanAttack(false);

            // ������ �غ� ���� ���� ������ Ȯ�� (HIT ���¿� �����ϱ� ����)
            _isPreparedAttack = false;

            // ��ȣ�ۿ� ��ư�� �����Ѵٸ�
            if (_interactionButton != null)
            {
                // ��ư Ȱ��ȭ
                _interactionButton.interactable = true;
                _buttonText.text = "ENTER";
            }

            GameButtonCtrl.Instance.IsLoadExitBoss = true; // ���� ���� Game������ �̵�

            // ���� �浹 �� Ȱ ���� ��ư ��Ȱ��ȭ (���� ������� ���� ������ ���� �ʵ��� �ϱ� ����)
            // Ȱ ���� ��ư�� ���� �ƴ϶��
            if (_bowButton != null)
            {
                // Ȱ ���� ��ư ��Ȱ��ȭ
                _bowButton.interactable = false;
                _bowButtonText.text = "";
            }
        }

        // NPC_M(Hp ����)�� �浹 ��
        if (collision.gameObject.tag == "NPC_M")
        {
            // ��ȣ�ۿ� ��ư�� �����Ѵٸ�
            if (_interactionButton != null)
            {
                // ��ư Ȱ��ȭ
                _interactionButton.interactable = true;
            }

            // ��ư�� BUY ǥ��
            _buttonText.text = "BUY";
        }

        // NPC_W(Ȱ ���� ����)�� �浹 ��, �׸��� ������ 5 �̻��� �� (Ȱ ���� ���� 5 �̻� �����ϵ��� �ϱ� ����)
        if (collision.gameObject.tag == "NPC_W" && _level >= 5)
        {
            // ��ȣ�ۿ� ��ư�� �����Ѵٸ�
            if (_interactionButton != null)
            {
                // ��ư Ȱ��ȭ
                _interactionButton.interactable = true;
            }

            // ��ư�� BUY ǥ��
            _buttonText.text = "BUY";
        }
    }

    // �浹 ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ѿ� �浹 ��
        if (collision.gameObject.tag == "BULLET")
        {
            AttackPlayerSound.PlayOneShot(AttackPlayerSound.clip);

            // �Ѿ� ������Ʈ ����
            Destroy(collision.gameObject);

            // �÷��̾� ���� ����Ʈ ����
            Instantiate(PlayerAttackEffect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);

            // Hp 2 ����
            _currentHp -= 2;

            // Hp �� ����
            PlayerPrefs.SetInt("CurrentHp", _currentHp);
        }

        // �����Ѿ� �浹 ��
        if (collision.gameObject.tag == "BOSSBULLET")
        {
            AttackPlayerSound.PlayOneShot(AttackPlayerSound.clip);

            // �Ѿ� ������Ʈ ����
            Destroy(collision.gameObject);

            // �÷��̾� ���� ����Ʈ ����
            Instantiate(PlayerAttackEffect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);

            // Hp 10 ����
            _currentHp -= 10;

            // Hp �� ����
            PlayerPrefs.SetInt("CurrentHp", _currentHp);
        }
    }

    // �浹 ��
    private void OnTriggerStay2D(Collider2D collision)
    {
        // ���ʹ̿� �浹 ���� �� (���� ���� ó���� ����)
        if (collision.gameObject.tag == "ENEMY")
        {
            // EnemyCtrl ��ũ��Ʈ ��������
            EnemyCtrl enemyCtrl = collision.gameObject.GetComponent<EnemyCtrl>();

            // ��ũ��Ʈ�� ���� �ƴϰ�, ���� ��ư�� ������, ������ ���� ���� ���°�, ���ʹ̰� ���������
            if (enemyCtrl != null && IsAttack && !HasAttacked && enemyCtrl.IsLive)
            {
                // �⺻ ������ 10 ~ 15 ���̿��� ���� �߻�
                _damage = Random.Range(10, 15);
                // �÷��̾� ������ ���� �߰� ������ �������� 
                _addDamage = PlayerPrefs.GetInt("AddDamage");
                // ���� �����ϴ� ������ = �⺻ ������ + �߰� ������
                _attackDamage = _damage + _addDamage;

                // ���ʹ̿��� ������ ���� (EnemyCtrl ��ũ��Ʈ �Լ����� ����)
                enemyCtrl.SufferDamage(_attackDamage);

                // ������ �� ���·� ����
                HasAttacked = true;
            }
        }
    }

    // �浹 ��
    private void OnCollisionStay2D(Collision2D collision)
    {
        // ���� �浹 ���� ��
        // * collision.transform.CompareTag�� �ۼ� �� ������ �����ϴ� �±����� Ȯ���ϱ� ������ ������ �߻��� (���� ������Ʈ������ ���� ���� �ش� �±װ� ���� ������)
        if (collision.transform.CompareTag("DOOR") || collision.gameObject.tag == "BOSSDOOR" || collision.gameObject.tag == "EXITDOOR")
        {
            // Ȱ ���� �Ұ����ϵ��� ��
            GameBowButtonCtrl.Instance.IsBowButton = false;
        }

        // NPC_M(Hp ����)�� �浹 ���� ��
        if (collision.gameObject.tag == "NPC_M")
        {
            // ���� ��ư�� ������, ������ 5000�� �̻��̸�, ���� Hp�� �ִ� Hp���� ���� ���
            if (IsBuy && _coin >= 5000 && _currentHp < _maxHp)
            {
                BuySound.PlayOneShot(BuySound.clip);

                // ���� ����Ʈ �߻�
                Instantiate(BuyEffect, transform.position, Quaternion.identity);

                // ���� 5,000�� ����
                _coin -= 5000;
                // Hp �ִ�� ȸ��
                _currentHp = _maxHp;

                // Coin, Hp �� ����
                PlayerPrefs.SetInt("Coin", _coin);
                PlayerPrefs.SetInt("CurrentHp", _currentHp);

                // ���� ��ư false�� ó��
                IsBuy = false;
            }
            else
            {
                IsBuy = false;
            }
        }

        // NPC_W(Ȱ ���� ����)�� �浹 ���� ��
        if (collision.gameObject.tag == "NPC_W")
        {
            // Ȱ ���� ��ư�� ������, ������ 50,000�� �̻��̰�, ������ 5 �̻��̰�, Ȱ�� �������� ���� ���¶��
            if (IsBuyBow && _coin >= 50000 && _level >= 5 && !IsBoughtBow)
            {
                BuySound.PlayOneShot(BuySound.clip);

                // Ȱ ���� ���� true�� ����
                IsBoughtBow = true;

                // Ȱ ���� ���� ����
                PlayerPrefs.SetInt("IsBoughtBow", IsBoughtBow ? 1 : 0); // 1 ��ȯ

                // ���� ����Ʈ ����
                Instantiate(BuyEffect, transform.position, Quaternion.identity);

                // ���� 50,000�� ����
                _coin -= 50000;

                // Coin �� ����
                PlayerPrefs.SetInt("Coin", _coin);

                // ���� ��ư false�� ó��
                IsBuyBow = false;
            }
            else
            {
                IsBuyBow = false;
            }
        }

        // �������ʹ̿� �浹 ���� ��
        // * collision.transform.CompareTag�� �ۼ� �� ������ �����ϴ� �±����� Ȯ���ϱ� ������ ������ �߻��� (���� ������Ʈ������ ���� ���� �ش� �±װ� ���� ������)
        if (collision.gameObject.tag == "BOSSENEMY")
        {
            // BossEnemyCtrl ��ũ��Ʈ ��������
            BossEnemyCtrl bossEnemyCtrl = collision.gameObject.GetComponent<BossEnemyCtrl>();

            // ��ũ��Ʈ�� ���� �ƴϰ�, ���� ��ư�� ������, ������ ���� ���� ���°�, �������ʹ̰� ���������
            if (bossEnemyCtrl != null && IsAttack && !HasAttacked && bossEnemyCtrl.IsLive)
            {
                // �⺻ ������ 10 ~ 15 ���� ���� �߻�
                _damage = Random.Range(10, 15);
                // �÷��̾� ������ ���� ������ ��������
                _addDamage = PlayerPrefs.GetInt("AddDamage");
                // ���� �����ϴ� ������ = �⺻ ������ + �߰� ������
                _attackDamage = _damage + _addDamage;

                // ���ʹ̿��� ������ ����(BossEnemyCtrl ��ũ��Ʈ �Լ����� ����)
                bossEnemyCtrl.SufferDamage(_attackDamage);

                // ������ �� ���·� ����
                HasAttacked = true;
            }
        }
    }

    // �浹 ����
    private void OnCollisionExit2D(Collision2D collision)
    {
        // �� �浹 ���� ��
        if (collision.transform.CompareTag("DOOR"))
        {
            GameButtonCtrl.Instance.SetCanAttack(true);

            _buttonText.text = "";

            // ��ȣ�ۿ� ��ư�� �����Ѵٸ�
            if (_interactionButton != null)
            {
                // ��ư ��Ȱ��ȭ
                _interactionButton.interactable = false;
            }

            MainButtonCtrl.Instance.IsLoad = false;
            GameButtonCtrl.Instance.IsLoad = false;

            _isPreparedAttack = true;

            // Ȱ�� �����Ͽ���, Ȱ ��ư�� �����Ѵٸ�
            if (IsBoughtBow)
            {
                if (_bowButton != null)
                {
                    // Ȱ ��ư Ȱ��ȭ
                    _bowButton.interactable = true;
                    _bowButtonText.text = "SHOOT";
                }

                GameBowButtonCtrl.Instance.IsBowButton = true;
            }
        }

        // ������ �� �浹 ���� ��
        // * collision.transform.CompareTag�� �ۼ� �� ������ �����ϴ� �±����� Ȯ���ϱ� ������ ������ �߻��� (���� ������Ʈ������ ���� ���� �ش� �±װ� ���� ������)
        if (collision.gameObject.tag == "BOSSDOOR")
        {
            GameButtonCtrl.Instance.SetCanAttack(true);

            _buttonText.text = "";

            // ��ȣ�ۿ� ��ư�� �����Ѵٸ�
            if (_interactionButton != null)
            {
                // ��ư ��Ȱ��ȭ
                _interactionButton.interactable = false;
            }

            GameButtonCtrl.Instance.IsLoadBoss = false;

            _isPreparedAttack = true;

            // Ȱ�� �����Ͽ���, Ȱ ��ư�� �����Ѵٸ�
            if (IsBoughtBow)
            {
                if (_bowButton != null)
                {
                    // Ȱ ���� ��ư Ȱ��ȭ
                    _bowButton.interactable = true;
                    _bowButtonText.text = "SHOOT";
                }

                GameBowButtonCtrl.Instance.IsBowButton = true;
            }
        }

        // Ż�⹮ �浹 ���� ��
        // * collision.transform.CompareTag�� �ۼ� �� ������ �����ϴ� �±����� Ȯ���ϱ� ������ ������ �߻��� (���� ������Ʈ������ ���� ���� �ش� �±װ� ���� ������)
        if (collision.gameObject.tag == "EXITDOOR")
        {
            GameButtonCtrl.Instance.SetCanAttack(true);

            _buttonText.text = "";

            // ��ȣ�ۿ� ��ư�� �����Ѵٸ�
            if (_interactionButton != null)
            {
                // ��ư ��Ȱ��ȭ
                _interactionButton.interactable = false;
            }

            GameButtonCtrl.Instance.IsLoadExitBoss = false;

            _isPreparedAttack = true;

            // Ȱ�� �����Ͽ���, Ȱ ��ư�� �����Ѵٸ�
            if (IsBoughtBow)
            {
                // Ȱ ��ư�� ���� �ƴ϶��
                if (_bowButton != null)
                {
                    // Ȱ ��ư Ȱ��ȭ
                    _bowButton.interactable = true;
                    _bowButtonText.text = "SHOOT";
                }

                GameBowButtonCtrl.Instance.IsBowButton = true;
            }
        }

        // NPC��� �浹 ���� ��
        if (collision.gameObject.tag == "NPC_W" || collision.gameObject.tag == "NPC_M")
        {
            _buttonText.text = "";

            // ��ȣ�ۿ� ��ư�� �����Ѵٸ�
            if (_interactionButton != null)
            {
                // ��ȣ�ۿ� ��ư ��Ȱ��ȭ
                _interactionButton.interactable = false;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerCtrl : MonoBehaviour
{
    #region 인스턴스 생성
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
    Button _interactionButton; // 상호작용 버튼
    Button _bowButton; // 활 버튼
    TMP_Text _buttonText;
    TMP_Text _bowButtonText;
    Image _bowIconBefore; // 활 아이콘 (변경 전)
    Vector2 _moveDir; // 이동 방향
    int _damage; // 기본 데미지
    int _addDamage; // 레벨에 따라 추가되는 데미지
    int _attackDamage; // 에너미 공격 데미지 (= _damage + _addDamage)
    int _level = 1;
    int _coin = 0;
    int _currentHp;
    int _maxHp;
    int _currentExp;
    int _maxExp;
    float _speed = 4f; // 플레이어 속도
    bool _isLive = true; // 플레이어 생존 여부 확인
    bool _isPreparedAttack = false; // 공격 준비 여부 확인
    bool _hasAttackedSound = false; // 플레이어 근접 공격 (효과음) 확인

    // public
    public GameObject BuyEffect;
    public GameObject PlayerAttackEffect;
    public Canvas DieText;
    public Sprite BowIconAfter;
    public AudioSource WalkSound; // 플레이어 이동 효과음
    public AudioSource HitSound; // 플레이어 근접 공격 효과음
    public AudioSource BowSound; // 플레이어 활 공격 효과음
    public AudioSource AttackPlayerSound; // 플레이어 공격 받을 때 효과음
    public AudioSource BuySound; // 구매 효과음
    [HideInInspector] public Vector3 Scale; // 로컬스케일
    [HideInInspector] public bool IsBuy = false; // 구매 여부 (버튼 체크용)
    [HideInInspector] public bool IsBuyBow = false; // 구매 여부 (버튼 체크용)
    [HideInInspector] public bool IsBoughtBow = false; // 구매 여부 (작동용)
    [HideInInspector] public bool IsAttack = false; // 공격 여부
    [HideInInspector] public bool HasAttacked = false; // 공격 실행 여부 확인
    [HideInInspector] public bool IsAttackBow = false; // 활 공격 여부
    [HideInInspector] public bool HasAttackedBow = false; // 활 공격 실행 여부 확인


    #region 프로퍼티
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
        // 컴포넌트 가져오기
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _interactionButton = GameObject.Find("InteractionButton")?.GetComponent<Button>();
        _bowButton = GameObject.Find("BowButton")?.GetComponent<Button>();
        _buttonText = GameObject.Find("ButtonText")?.GetComponent<TMP_Text>();
        _bowButtonText = GameObject.Find("BowButtonText")?.GetComponent<TMP_Text>();
        _bowIconBefore = GameObject.Find("BowIcon")?.GetComponent<Image>();

        // 초깃값 설정
        Initialize();

        // 상호작용 버튼이 존재한다면
        if (_interactionButton != null)
        {
            // 비활성화 (상호작용이 필요한 특정 상황에서만 활성화하도록 하기 위함)
            _interactionButton.interactable = false;
        }

        // 활 버튼이 존재한다면
        if (_bowButton != null)
        {
            // 비활성화 (활 구매 후에만 활성화하도록 하기 위함)
            _bowButton.interactable = false;
        }

        // Null 오류 방지
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

    // 초깃값 설정
    void Initialize()
    {
        // 키 값이 있다면 저장된 값 가져오기
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
            // * bool형 변수는 다음과 같이 int형으로 처리
            // : 키에 저장된 값 없으면 기본값 0 반환
            // : 반환 값이 1인지 확인 (1이면 true, 그렇지 않으면 false)
            IsBoughtBow = PlayerPrefs.GetInt("IsBoughtBow", 0) == 1;
        }
        // 키 값이 없다면 값 초기화
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
        Move(); // 이동
        Attack(); // 근접 공격

        // 활을 구매하였다면
        if (IsBoughtBow)
        {
            // 활 아이콘 (변경 전) 이 널이 아니라면
            if (_bowIconBefore != null)
            {
                // 활 아이콘 변경
                _bowIconBefore.sprite = BowIconAfter;
            }

            AttackBow(); // 활 공격
        }

        // Game씬 또는 BossGame씬에서
        if (SceneManager.GetActiveScene().name == "Game" || SceneManager.GetActiveScene().name == "BossGame")
        {
            // 문과 충돌하지 않은 상태라면 계속 HIT 버튼이 활성화
            if (_isPreparedAttack == true)
            {
                // 상호작용 버튼이 존재한다면
                if (_interactionButton != null)
                {
                    // 버튼 활성화
                    _interactionButton.interactable = true;
                }

                _buttonText.text = "HIT";
            }

            // 플레이어 사망
            DiePlayer();
        }
    }

    // 플레이어 사망 처리하는 함수
    void DiePlayer()
    {
        if (_currentHp <= 0)
        {
            _isLive = false;
        }

        // 살아있지 않다면 (사망하였다면)
        if (!_isLive)
        {
            // 플레이어 오브젝트 비활성화
            gameObject.SetActive(false);
            // DieUI 활성화
            DieText.gameObject.SetActive(true);
            // 3초 후 Main씬 로드
            Invoke("ReloadScene", 3f);
        }
    }

    // 플레이어 사망 후 Main씬 로드
    void ReloadScene()
    {
        SceneManager.LoadScene("Main");

        // 플레이어 Hp 10으로 초기화
        _currentHp = 10;
        // 플레이어 Exp 0으로 초기화
        _currentExp = 0;

        // Hp, Exp 값 저장
        PlayerPrefs.SetInt("CurrentHp", _currentHp);
        PlayerPrefs.SetInt("CurrentExp", _currentExp);
    }

    // 이동
    private void Move()
    {
        // 조이스틱으로부터 _moveDir를 전달 받아 이동함
        Vector3 moveDir = transform.position + (Vector3)_moveDir * _speed * Time.deltaTime;

        // 플레이어 이동 방향에 따른 애니메이션 확인
        _animator.SetFloat("AxisX", _moveDir.x);
        _animator.SetFloat("AxisY", _moveDir.y);

        // 플레이어 기준으로 움직임이 있으면
        if (_moveDir.magnitude > 0)
        {
            // 애니메이션 활성화
            _animator.SetBool("IsMove", true);

            if (!WalkSound.isPlaying)
            {
                WalkSound.Play();
            }
        }
        // 움직임이 없으면
        else
        {
            // 애니메이션 비활성화
            _animator.SetBool("IsMove", false);
            WalkSound.Stop();
        }

        // 애니메이션 좌우반전 적용
        if (_moveDir.x != 0)
        {
            Scale = transform.localScale;
            Scale.x = (_moveDir.x > 0) ? 1 : -1;
            transform.localScale = Scale;
        }

        // * rigidbody2D.MovePosition()
        // : 이동
        // : transform.position과 유사해보이긴 하나 transform.position은 텔레포트의 개념이고, 경로 사이에 물리작용을 하는 물체가 있더라도 아무 일이 일어나지 않는다.
        // : rigidbody2D.MovePosition은 이전 위치와 새 위치 사이의 직선 경로를 추적하고 해당 경로를 따라 빠르게 이동한 것처럼 처리하고, 이전 경로와 새로운 경로 사이에 어떤 물리작용을 하는 물체가 존재하면 물리 작용이 일어난다.
        _rigidbody2D.MovePosition(moveDir);
    }

    // 근접 공격
    void Attack()
    {
        // 공격 버튼을 눌렀다면
        if (IsAttack)
        {
            if (!_hasAttackedSound)
            {
                // 공격 애니메이션 발생
                _animator.SetTrigger("Attack");
                HitSound.PlayOneShot(HitSound.clip);
                _hasAttackedSound = true;
            }
        }
        // 공격 버튼을 누르지 않았다면
        if (!IsAttack)
        {
            // 공격 애니메이션 비활성화
            _animator.SetBool("IsAttack", false);
            _hasAttackedSound = false;
        }
    }

    // 활 공격
    void AttackBow()
    {
        // 활 공격 버튼을 눌렀다면
        if (IsAttackBow)
        {
            // 활 공격을 하지 않았다면
            if (!HasAttackedBow)
            {
                // 활 공격 애니메이션 발생
                _animator.SetTrigger("AttackBow");

                BowSound.PlayOneShot(BowSound.clip);

                // 화살 오브젝트 풀 생성하여 발생
                ArrowPool.Instance.GetArrow();

                // 활 공격 여부 true로 변경
                HasAttackedBow = true;
            }
        }
        // 활 공격 버튼을 누르지 않았다면
        if (!IsAttackBow)
        {
            // 활 공격 애니메이션 비활성화
            _animator.SetBool("IsAttackBow", false);
        }
    }


    // 충돌 시작
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 문 충돌 시
        if (collision.transform.CompareTag("DOOR"))
        {
            GameButtonCtrl.Instance.SetCanAttack(false);

            // 공격할 준비가 되지 않은 것으로 확인 (HIT 상태와 구분하기 위함)
            _isPreparedAttack = false;

            // 상호작용 버튼이 존재한다면
            if (_interactionButton != null)
            {
                // 버튼 활성화
                _interactionButton.interactable = true;
                _buttonText.text = "ENTER";
            }

            MainButtonCtrl.Instance.IsLoad = true; // 문을 통해 Game씬으로 이동
            GameButtonCtrl.Instance.IsLoad = true; // 문을 통해 Main씬으로 이동

            // 문과 충돌 시 활 공격 버튼 비활성화 (문에 닿아있을 때는 공격을 하지 않도록 하기 위함)
            // 활 공격 버튼이 널이 아니라면
            if (_bowButton != null)
            {
                // 활 공격 버튼 비활성화
                _bowButton.interactable = false;
                _bowButtonText.text = "";
            }
        }

        // 보스맵 문 충돌 시
        // * collision.transform.CompareTag로 작성 시 실제로 존재하는 태그임을 확인하기 때문에 오류가 발생함 (현재 프로젝트에서는 시작 씬에 해당 태그가 없기 때문에)
        if (collision.gameObject.tag == "BOSSDOOR")
        {
            GameButtonCtrl.Instance.SetCanAttack(false);

            // 공격할 준비가 되지 않은 것으로 확인 (HIT 상태와 구분하기 위함)
            _isPreparedAttack = false;

            // 상호작용 버튼이 존재한다면
            if (_interactionButton != null)
            {
                // 버튼 활성화
                _interactionButton.interactable = true;
                _buttonText.text = "ENTER";
            }

            GameButtonCtrl.Instance.IsLoadBoss = true; // 문을 통해 BossGame씬으로 이동

            // 문과 충돌 시 활 공격 버튼 비활성화 (문에 닿아있을 때는 공격을 하지 않도록 하기 위함)
            // 활 공격 버튼이 널이 아니라면
            if (_bowButton != null)
            {
                // 활 공격 버튼 비활성화
                _bowButton.interactable = false;
                _bowButtonText.text = "";
            }
        }

        // 탈출문 충돌 시
        // * collision.transform.CompareTag로 작성 시 실제로 존재하는 태그임을 확인하기 때문에 오류가 발생함 (현재 프로젝트에서는 시작 씬에 해당 태그가 없기 때문에)
        if (collision.gameObject.tag == "EXITDOOR")
        {
            GameButtonCtrl.Instance.SetCanAttack(false);

            // 공격할 준비가 되지 않은 것으로 확인 (HIT 상태와 구분하기 위함)
            _isPreparedAttack = false;

            // 상호작용 버튼이 존재한다면
            if (_interactionButton != null)
            {
                // 버튼 활성화
                _interactionButton.interactable = true;
                _buttonText.text = "ENTER";
            }

            GameButtonCtrl.Instance.IsLoadExitBoss = true; // 문을 통해 Game씬으로 이동

            // 문과 충돌 시 활 공격 버튼 비활성화 (문에 닿아있을 때는 공격을 하지 않도록 하기 위함)
            // 활 공격 버튼이 널이 아니라면
            if (_bowButton != null)
            {
                // 활 공격 버튼 비활성화
                _bowButton.interactable = false;
                _bowButtonText.text = "";
            }
        }

        // NPC_M(Hp 관리)과 충돌 시
        if (collision.gameObject.tag == "NPC_M")
        {
            // 상호작용 버튼이 존재한다면
            if (_interactionButton != null)
            {
                // 버튼 활성화
                _interactionButton.interactable = true;
            }

            // 버튼에 BUY 표시
            _buttonText.text = "BUY";
        }

        // NPC_W(활 구매 관리)와 충돌 시, 그리고 레벨이 5 이상일 시 (활 구매 레벨 5 이상 가능하도록 하기 위함)
        if (collision.gameObject.tag == "NPC_W" && _level >= 5)
        {
            // 상호작용 버튼이 존재한다면
            if (_interactionButton != null)
            {
                // 버튼 활성화
                _interactionButton.interactable = true;
            }

            // 버튼에 BUY 표시
            _buttonText.text = "BUY";
        }
    }

    // 충돌 시작
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 총알 충돌 시
        if (collision.gameObject.tag == "BULLET")
        {
            AttackPlayerSound.PlayOneShot(AttackPlayerSound.clip);

            // 총알 오브젝트 제거
            Destroy(collision.gameObject);

            // 플레이어 어택 이펙트 생성
            Instantiate(PlayerAttackEffect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);

            // Hp 2 감소
            _currentHp -= 2;

            // Hp 값 저장
            PlayerPrefs.SetInt("CurrentHp", _currentHp);
        }

        // 보스총알 충돌 시
        if (collision.gameObject.tag == "BOSSBULLET")
        {
            AttackPlayerSound.PlayOneShot(AttackPlayerSound.clip);

            // 총알 오브젝트 제거
            Destroy(collision.gameObject);

            // 플레이어 어택 이펙트 생성
            Instantiate(PlayerAttackEffect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);

            // Hp 10 감소
            _currentHp -= 10;

            // Hp 값 저장
            PlayerPrefs.SetInt("CurrentHp", _currentHp);
        }
    }

    // 충돌 중
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 에너미와 충돌 중일 시 (근접 공격 처리를 위함)
        if (collision.gameObject.tag == "ENEMY")
        {
            // EnemyCtrl 스크립트 가져오기
            EnemyCtrl enemyCtrl = collision.gameObject.GetComponent<EnemyCtrl>();

            // 스크립트가 널이 아니고, 공격 버튼을 눌렀고, 공격을 하지 않은 상태고, 에너미가 살아있으면
            if (enemyCtrl != null && IsAttack && !HasAttacked && enemyCtrl.IsLive)
            {
                // 기본 데미지 10 ~ 15 사이에서 랜덤 발생
                _damage = Random.Range(10, 15);
                // 플레이어 레벨에 따른 추가 데미지 가져오기 
                _addDamage = PlayerPrefs.GetInt("AddDamage");
                // 실제 공격하는 데미지 = 기본 데미지 + 추가 데미지
                _attackDamage = _damage + _addDamage;

                // 에너미에게 데미지 전달 (EnemyCtrl 스크립트 함수에게 전달)
                enemyCtrl.SufferDamage(_attackDamage);

                // 공격을 한 상태로 변경
                HasAttacked = true;
            }
        }
    }

    // 충돌 중
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 문과 충돌 중일 시
        // * collision.transform.CompareTag로 작성 시 실제로 존재하는 태그임을 확인하기 때문에 오류가 발생함 (현재 프로젝트에서는 시작 씬에 해당 태그가 없기 때문에)
        if (collision.transform.CompareTag("DOOR") || collision.gameObject.tag == "BOSSDOOR" || collision.gameObject.tag == "EXITDOOR")
        {
            // 활 공격 불가능하도록 함
            GameBowButtonCtrl.Instance.IsBowButton = false;
        }

        // NPC_M(Hp 관리)과 충돌 중일 시
        if (collision.gameObject.tag == "NPC_M")
        {
            // 구매 버튼을 눌렀고, 코인이 5000원 이상이며, 현재 Hp가 최대 Hp보다 적을 경우
            if (IsBuy && _coin >= 5000 && _currentHp < _maxHp)
            {
                BuySound.PlayOneShot(BuySound.clip);

                // 구매 이펙트 발생
                Instantiate(BuyEffect, transform.position, Quaternion.identity);

                // 코인 5,000원 차감
                _coin -= 5000;
                // Hp 최대로 회복
                _currentHp = _maxHp;

                // Coin, Hp 값 저장
                PlayerPrefs.SetInt("Coin", _coin);
                PlayerPrefs.SetInt("CurrentHp", _currentHp);

                // 구매 버튼 false로 처리
                IsBuy = false;
            }
            else
            {
                IsBuy = false;
            }
        }

        // NPC_W(활 구매 관리)와 충돌 중일 시
        if (collision.gameObject.tag == "NPC_W")
        {
            // 활 구매 버튼을 눌렀고, 코인이 50,000원 이상이고, 레벨이 5 이상이고, 활을 구매하지 않은 상태라면
            if (IsBuyBow && _coin >= 50000 && _level >= 5 && !IsBoughtBow)
            {
                BuySound.PlayOneShot(BuySound.clip);

                // 활 구매 여부 true로 변경
                IsBoughtBow = true;

                // 활 구매 여부 저장
                PlayerPrefs.SetInt("IsBoughtBow", IsBoughtBow ? 1 : 0); // 1 반환

                // 구매 이펙트 생성
                Instantiate(BuyEffect, transform.position, Quaternion.identity);

                // 코인 50,000원 감소
                _coin -= 50000;

                // Coin 값 저장
                PlayerPrefs.SetInt("Coin", _coin);

                // 구매 버튼 false로 처리
                IsBuyBow = false;
            }
            else
            {
                IsBuyBow = false;
            }
        }

        // 보스에너미와 충돌 중일 시
        // * collision.transform.CompareTag로 작성 시 실제로 존재하는 태그임을 확인하기 때문에 오류가 발생함 (현재 프로젝트에서는 시작 씬에 해당 태그가 없기 때문에)
        if (collision.gameObject.tag == "BOSSENEMY")
        {
            // BossEnemyCtrl 스크립트 가져오기
            BossEnemyCtrl bossEnemyCtrl = collision.gameObject.GetComponent<BossEnemyCtrl>();

            // 스크립트가 널이 아니고, 공격 버튼을 눌렀고, 공격을 하지 않은 상태고, 보스에너미가 살아있으면
            if (bossEnemyCtrl != null && IsAttack && !HasAttacked && bossEnemyCtrl.IsLive)
            {
                // 기본 데미지 10 ~ 15 사이 랜덤 발생
                _damage = Random.Range(10, 15);
                // 플레이어 레벨에 따른 데미지 가져오기
                _addDamage = PlayerPrefs.GetInt("AddDamage");
                // 실제 공격하는 데미지 = 기본 데미지 + 추가 데미지
                _attackDamage = _damage + _addDamage;

                // 에너미에게 데미지 전달(BossEnemyCtrl 스크립트 함수에게 전달)
                bossEnemyCtrl.SufferDamage(_attackDamage);

                // 공격을 한 상태로 변경
                HasAttacked = true;
            }
        }
    }

    // 충돌 해제
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 문 충돌 해제 시
        if (collision.transform.CompareTag("DOOR"))
        {
            GameButtonCtrl.Instance.SetCanAttack(true);

            _buttonText.text = "";

            // 상호작용 버튼이 존재한다면
            if (_interactionButton != null)
            {
                // 버튼 비활성화
                _interactionButton.interactable = false;
            }

            MainButtonCtrl.Instance.IsLoad = false;
            GameButtonCtrl.Instance.IsLoad = false;

            _isPreparedAttack = true;

            // 활을 구매하였고, 활 버튼이 존재한다면
            if (IsBoughtBow)
            {
                if (_bowButton != null)
                {
                    // 활 버튼 활성화
                    _bowButton.interactable = true;
                    _bowButtonText.text = "SHOOT";
                }

                GameBowButtonCtrl.Instance.IsBowButton = true;
            }
        }

        // 보스맵 문 충돌 해제 시
        // * collision.transform.CompareTag로 작성 시 실제로 존재하는 태그임을 확인하기 때문에 오류가 발생함 (현재 프로젝트에서는 시작 씬에 해당 태그가 없기 때문에)
        if (collision.gameObject.tag == "BOSSDOOR")
        {
            GameButtonCtrl.Instance.SetCanAttack(true);

            _buttonText.text = "";

            // 상호작용 버튼이 존재한다면
            if (_interactionButton != null)
            {
                // 버튼 비활성화
                _interactionButton.interactable = false;
            }

            GameButtonCtrl.Instance.IsLoadBoss = false;

            _isPreparedAttack = true;

            // 활을 구매하였고, 활 버튼이 존재한다면
            if (IsBoughtBow)
            {
                if (_bowButton != null)
                {
                    // 활 공격 버튼 활성화
                    _bowButton.interactable = true;
                    _bowButtonText.text = "SHOOT";
                }

                GameBowButtonCtrl.Instance.IsBowButton = true;
            }
        }

        // 탈출문 충돌 해제 시
        // * collision.transform.CompareTag로 작성 시 실제로 존재하는 태그임을 확인하기 때문에 오류가 발생함 (현재 프로젝트에서는 시작 씬에 해당 태그가 없기 때문에)
        if (collision.gameObject.tag == "EXITDOOR")
        {
            GameButtonCtrl.Instance.SetCanAttack(true);

            _buttonText.text = "";

            // 상호작용 버튼이 존재한다면
            if (_interactionButton != null)
            {
                // 버튼 비활성화
                _interactionButton.interactable = false;
            }

            GameButtonCtrl.Instance.IsLoadExitBoss = false;

            _isPreparedAttack = true;

            // 활을 구매하였고, 활 버튼이 존재한다면
            if (IsBoughtBow)
            {
                // 활 버튼이 널이 아니라면
                if (_bowButton != null)
                {
                    // 활 버튼 활성화
                    _bowButton.interactable = true;
                    _bowButtonText.text = "SHOOT";
                }

                GameBowButtonCtrl.Instance.IsBowButton = true;
            }
        }

        // NPC들과 충돌 해제 시
        if (collision.gameObject.tag == "NPC_W" || collision.gameObject.tag == "NPC_M")
        {
            _buttonText.text = "";

            // 상호작용 버튼이 존재한다면
            if (_interactionButton != null)
            {
                // 상호작용 버튼 비활성화
                _interactionButton.interactable = false;
            }
        }
    }
}

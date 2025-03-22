using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtonCtrl : MonoBehaviour
{
    #region 인스턴스 생성
    public static GameButtonCtrl Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }
    #endregion

    // private
    GameObject _player;
    PlayerCtrl _playerCtrl;
    bool _canAttack = true;

    // public
    [HideInInspector] public bool IsLoad = false;
    [HideInInspector] public bool IsLoadBoss = false;
    [HideInInspector] public bool IsLoadExitBoss = false;

    void Start()
    {
        // 플레이어 오브젝트를 찾고, 스크립트 가져오기
        _player = GameObject.Find("Player");
        _playerCtrl = _player.GetComponent<PlayerCtrl>();
    }

    // 클릭 시 메인씬 로드하는 함수
    public void LoadPointerClick()
    {
        if (IsLoad == true)
        {
            IsLoad = false;
            SceneManager.LoadScene("Main");
        }
    }

    // 클릭 시 보스씬 로드하는 함수
    public void LoadBossPointerClick()
    {
        if (IsLoadBoss == true)
        {
            IsLoadBoss = false;
            SceneManager.LoadScene("BossGame");
        }
    }

    // 클릭 시 게임씬 로드하는 함수
    public void LoadExitBossPointerClick()
    {
        if (IsLoadExitBoss == true)
        {
            IsLoadExitBoss = false;
            SceneManager.LoadScene("Game");
        }
    }

    // 클릭 시작 시 공격 상태로 설정하는 함수
    public void AttackPointerDown()
    {
        if (_canAttack)
        {
            PlayerCtrl.Instance.HasAttacked = false;
            _playerCtrl.IsAttack = true;
        }
    }

    // 클릭 종료 시 공격 상태를 해제하는 함수
    public void AttackPointerUp()
    {
        _playerCtrl.IsAttack = false;
    }

    public void SetCanAttack(bool canAttack)
    {
        _canAttack = canAttack;
    }
}

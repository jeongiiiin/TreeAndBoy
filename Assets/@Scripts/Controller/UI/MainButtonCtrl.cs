using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtonCtrl : MonoBehaviour
{
    #region 인스턴스 생성
    public static MainButtonCtrl Instance;

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
    
    // public
    [HideInInspector] public bool IsLoad = false;

    void Start()
    {
        // 플레이어 오브젝트를 찾고, 스크립트 컴포넌트 가져오기
        _player = GameObject.Find("Player");
        _playerCtrl = _player.GetComponent<PlayerCtrl>();
    }

    // 클릭 시 씬 로드하는 함수
    public void LoadPointerClick()
    {
        if (IsLoad == true)
        {
            IsLoad = false;
            SceneManager.LoadScene("Game");
        }
    }

    // 클릭 시작 시 힐을 구매할 수 있는 상태로 설정하는 함수
    public void BuyHealPointerDown()
    {
        _playerCtrl.IsBuy = true;
    }

    // 클릭 종료 시 힐을 구매할 수 없는 상태로 설정하는 함수
    public void BuyHealPointerUp()
    {
        _playerCtrl.IsBuy = false;
    }

    // 클릭 시작 시 활을 구매할 수 있는 상태로 설정하는 함수
    public void BuyBowPointerDown()
    {
        _playerCtrl.IsBuyBow = true;
    }

    // 클릭 종료 시 활을 구매할 수 없는 상태로 설정하는 함수
    public void BuyBowPointerUp()
    {
        _playerCtrl.IsBuyBow = false;
    }
}

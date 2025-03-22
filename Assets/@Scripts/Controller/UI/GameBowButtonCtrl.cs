using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBowButtonCtrl : MonoBehaviour
{
    #region 인스턴스 생성
    public static GameBowButtonCtrl Instance;

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
    [HideInInspector] public bool IsBowButton = false;

    void Start()
    {
        // 플레이어 오브젝트를 찾고, 스크립트 가져오기
        _player = GameObject.Find("Player");
        _playerCtrl = _player.GetComponent<PlayerCtrl>();
    }

    // 클릭 시작 시 공격 상태로 설정하는 함수
    public void AttackBowPointerDown()
    {
        if (IsBowButton == true)
        {
            IsBowButton = false;
            PlayerCtrl.Instance.HasAttackedBow = false;
            _playerCtrl.IsAttackBow = true;
        }
    }

    // 클릭 종료 시 공격 상태를 해제하는 함수
    public void AttackBowPointerUp()
    {

        IsBowButton = true;
        _playerCtrl.IsAttackBow = false;
    }
}

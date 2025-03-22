using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBowButtonCtrl : MonoBehaviour
{
    #region �ν��Ͻ� ����
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
        // �÷��̾� ������Ʈ�� ã��, ��ũ��Ʈ ��������
        _player = GameObject.Find("Player");
        _playerCtrl = _player.GetComponent<PlayerCtrl>();
    }

    // Ŭ�� ���� �� ���� ���·� �����ϴ� �Լ�
    public void AttackBowPointerDown()
    {
        if (IsBowButton == true)
        {
            IsBowButton = false;
            PlayerCtrl.Instance.HasAttackedBow = false;
            _playerCtrl.IsAttackBow = true;
        }
    }

    // Ŭ�� ���� �� ���� ���¸� �����ϴ� �Լ�
    public void AttackBowPointerUp()
    {

        IsBowButton = true;
        _playerCtrl.IsAttackBow = false;
    }
}

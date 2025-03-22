using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtonCtrl : MonoBehaviour
{
    #region �ν��Ͻ� ����
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
        // �÷��̾� ������Ʈ�� ã��, ��ũ��Ʈ ������Ʈ ��������
        _player = GameObject.Find("Player");
        _playerCtrl = _player.GetComponent<PlayerCtrl>();
    }

    // Ŭ�� �� �� �ε��ϴ� �Լ�
    public void LoadPointerClick()
    {
        if (IsLoad == true)
        {
            IsLoad = false;
            SceneManager.LoadScene("Game");
        }
    }

    // Ŭ�� ���� �� ���� ������ �� �ִ� ���·� �����ϴ� �Լ�
    public void BuyHealPointerDown()
    {
        _playerCtrl.IsBuy = true;
    }

    // Ŭ�� ���� �� ���� ������ �� ���� ���·� �����ϴ� �Լ�
    public void BuyHealPointerUp()
    {
        _playerCtrl.IsBuy = false;
    }

    // Ŭ�� ���� �� Ȱ�� ������ �� �ִ� ���·� �����ϴ� �Լ�
    public void BuyBowPointerDown()
    {
        _playerCtrl.IsBuyBow = true;
    }

    // Ŭ�� ���� �� Ȱ�� ������ �� ���� ���·� �����ϴ� �Լ�
    public void BuyBowPointerUp()
    {
        _playerCtrl.IsBuyBow = false;
    }
}

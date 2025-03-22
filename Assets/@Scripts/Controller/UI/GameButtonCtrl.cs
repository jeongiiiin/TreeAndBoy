using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtonCtrl : MonoBehaviour
{
    #region �ν��Ͻ� ����
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
        // �÷��̾� ������Ʈ�� ã��, ��ũ��Ʈ ��������
        _player = GameObject.Find("Player");
        _playerCtrl = _player.GetComponent<PlayerCtrl>();
    }

    // Ŭ�� �� ���ξ� �ε��ϴ� �Լ�
    public void LoadPointerClick()
    {
        if (IsLoad == true)
        {
            IsLoad = false;
            SceneManager.LoadScene("Main");
        }
    }

    // Ŭ�� �� ������ �ε��ϴ� �Լ�
    public void LoadBossPointerClick()
    {
        if (IsLoadBoss == true)
        {
            IsLoadBoss = false;
            SceneManager.LoadScene("BossGame");
        }
    }

    // Ŭ�� �� ���Ӿ� �ε��ϴ� �Լ�
    public void LoadExitBossPointerClick()
    {
        if (IsLoadExitBoss == true)
        {
            IsLoadExitBoss = false;
            SceneManager.LoadScene("Game");
        }
    }

    // Ŭ�� ���� �� ���� ���·� �����ϴ� �Լ�
    public void AttackPointerDown()
    {
        if (_canAttack)
        {
            PlayerCtrl.Instance.HasAttacked = false;
            _playerCtrl.IsAttack = true;
        }
    }

    // Ŭ�� ���� �� ���� ���¸� �����ϴ� �Լ�
    public void AttackPointerUp()
    {
        _playerCtrl.IsAttack = false;
    }

    public void SetCanAttack(bool canAttack)
    {
        _canAttack = canAttack;
    }
}

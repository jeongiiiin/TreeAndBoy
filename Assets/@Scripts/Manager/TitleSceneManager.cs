using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    // private
    Button _startButton; // �̾��ϱ�
    Button _resetButton; // �����ϱ�

    void Start()
    {
        // ������Ʈ ��������
        _startButton = GameObject.Find("StartButton")?.GetComponent<Button>();
        _resetButton = GameObject.Find("ResetButton")?.GetComponent<Button>();

        // �̾��ϱ� ��ư�� ���� �ƴ϶��
        if (_startButton != null)
        {
            _startButton.onClick.AddListener(StartButtonClick);
        }
        // �����ϱ� ��ư�� ���� �ƴ϶��
        if (_resetButton != null)
        {
            _resetButton.onClick.AddListener(ResetButtonClick);
        }
    }

    // �̾��ϱ� ��ư ó���ϴ� �Լ�
    void StartButtonClick()
    {
        SceneManager.LoadScene("Main"); // Main������ �̵�
    }

    // �����ϱ� ��ư
    void ResetButtonClick()
    {
        // ��� ������ �ʱ�ȭ
        PlayerPrefs.DeleteAll();

        // �ʱ갪 ����
        PlayerCtrl.Instance.MaxHp = 100;
        PlayerCtrl.Instance.CurrentHp = PlayerCtrl.Instance.MaxHp;
        PlayerCtrl.Instance.MaxExp = 100;
        PlayerCtrl.Instance.CurrentExp = 0;
        PlayerCtrl.Instance.Level = 1;
        PlayerCtrl.Instance.Coin = 0;
        PlayerCtrl.Instance.AddDamage = 0;
        PlayerCtrl.Instance.IsBoughtBow = false;

        PlayerPrefs.SetInt("MaxHp", PlayerCtrl.Instance.MaxHp);
        PlayerPrefs.SetInt("CurrentHp", PlayerCtrl.Instance.CurrentHp);
        PlayerPrefs.SetInt("MaxExp", PlayerCtrl.Instance.MaxExp);
        PlayerPrefs.SetInt("CurrentExp", PlayerCtrl.Instance.CurrentExp);
        PlayerPrefs.SetInt("Level", PlayerCtrl.Instance.Level);
        PlayerPrefs.SetInt("Coin", PlayerCtrl.Instance.Coin);
        PlayerPrefs.SetInt("AddDamage", PlayerCtrl.Instance.AddDamage);
        PlayerPrefs.SetInt("IsBoughtBow", PlayerCtrl.Instance.IsBoughtBow ? 1 : 0);

        SceneManager.LoadScene("Main"); // �ʱ�ȭ �� Main������ �̵�
    }
}

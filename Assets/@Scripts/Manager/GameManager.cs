using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // private
    TMP_Text _hpText;
    TMP_Text _expText;
    TMP_Text _levelText;
    TMP_Text _coinText;
    TMP_Text _questNumText; // ����Ʈ ����
    Slider _hpBar;
    Slider _expBar;
    Button _homeButton;
    List<string> _questList = new List<string>(); // ����Ʈ ����Ʈ

    // public
    public TMP_Text QuestText;
    public TMP_Text QuestNumText;
    public GameObject LevelUpEffect;
    public Canvas HomeChoice;
    public AudioSource LevelUpSound; // �÷��̾� ������ ȿ����

    private void Start()
    {
        // ������Ʈ ��������
        _hpText = GameObject.Find("HpText")?.GetComponent<TMP_Text>();
        _expText = GameObject.Find("ExpText")?.GetComponent<TMP_Text>();
        _levelText = GameObject.Find("LevelText")?.GetComponent<TMP_Text>();
        _coinText = GameObject.Find("CoinText")?.GetComponent<TMP_Text>();
        _hpBar = GameObject.Find("HpBar")?.GetComponent<Slider>();
        _expBar = GameObject.Find("ExpBar")?.GetComponent<Slider>();
        _homeButton = GameObject.Find("HomeButton")?.GetComponent<Button>();

        // Ȩ��ư�� ���� �ƴ϶��
        if (_homeButton != null)
        {
            // ��ư�� ������ �� HomeButtonClick �Լ� ����
            _homeButton.onClick.AddListener(HomeButtonClick);
        }
    }

    private void FixedUpdate()
    {
        PrintPlayerInfo(); // �÷��̾� ���� ���� ���
        PrintQuestList(); // ����Ʈ ���
    }

    // �÷��̾��� ���� ����(����)����ϴ� �Լ�
    void PrintPlayerInfo()
    {
        #region <<< Hp >>>
        PlayerCtrl.Instance.CurrentHp = PlayerPrefs.GetInt("CurrentHp");
        // _maxHp = PlayerPrefs.GetInt("MaxHp");
        // * ����� MaxHp�� �׻� 100���� �����Ǿ��ֱ� ������ �ʿ� ����

        _hpBar.value = PlayerCtrl.Instance.CurrentHp;
        _hpBar.maxValue = PlayerCtrl.Instance.MaxHp;
        _hpText.text = $"{PlayerCtrl.Instance.CurrentHp} / {PlayerCtrl.Instance.MaxHp}";

        // ���� Hp�� 0 ���϶�� ���������� 0���� ����ϵ��� �ϱ� ����
        if (PlayerCtrl.Instance.CurrentHp <= 0)
        {
            _hpText.text = $"0 / {PlayerCtrl.Instance.MaxHp}";
        }
        #endregion

        #region <<< Exp >>>
        PlayerCtrl.Instance.CurrentExp = PlayerPrefs.GetInt("CurrentExp");
        PlayerCtrl.Instance.MaxExp = PlayerPrefs.GetInt("MaxExp");

        _expBar.value = PlayerCtrl.Instance.CurrentExp;
        _expBar.maxValue = PlayerCtrl.Instance.MaxExp;
        _expText.text = $"{PlayerCtrl.Instance.CurrentExp} / {PlayerCtrl.Instance.MaxExp}";
        #endregion

        // ������
        LevelUp();

        #region <<< Level >>>
        PlayerCtrl.Instance.Level = PlayerPrefs.GetInt("Level");

        // ������ 10 ���̶��
        if (PlayerCtrl.Instance.Level < 10)
        {
            // ���� �տ� 0�� �ٿ��� ��� (01, 02 ... �� ���� ���̱� ����)
            _levelText.text = $"0{PlayerCtrl.Instance.Level}";
        }
        else
        {
            // 10 �̻��̶�� �״�� ���
            _levelText.text = $"{PlayerCtrl.Instance.Level}";
        }
        #endregion

        #region <<< Coin >>>
        PlayerCtrl.Instance.Coin = PlayerPrefs.GetInt("Coin");

        // ������ 0 ���϶��
        if (PlayerCtrl.Instance.Coin <= 0)
        {
            // ������ 0���� ���
            _coinText.text = "0";
        }
        else
        {
            // ���� �޸� �� ���
            _coinText.text = string.Format("{0:#,###}", PlayerCtrl.Instance.Coin);

        }
        #endregion
    }

    // ������ ó���ϴ� �Լ�
    void LevelUp()
    {
        // ���� ����ġ�� �ְ� ����ġ�� �����ϸ�
        if (PlayerCtrl.Instance.CurrentExp >= PlayerCtrl.Instance.MaxExp)
        {
            LevelUpSound.PlayOneShot(LevelUpSound.clip);

            // ������ ������Ű��
            PlayerCtrl.Instance.Level++;
            Instantiate(LevelUpEffect, PlayerCtrl.Instance.transform.position, Quaternion.identity);
            // 10,000���� ������ �߰�
            PlayerCtrl.Instance.Coin += 10000;
            // ����ġ�� �ٽ� 0���� �ʱ�ȭ
            PlayerCtrl.Instance.CurrentExp = 0;
            // �ְ� ����ġ 25 �߰� (���� �������� ���ʹ̸� �� �� �� ��ƾ� ������ �� �� �ֵ��� ������)
            PlayerCtrl.Instance.MaxExp += 25;
            // ���ʹ̿��� ���� �� �ִ� �÷��̾��� ������ 3 �߰�
            PlayerCtrl.Instance.AddDamage += 3;

            // �� ����
            PlayerPrefs.SetInt("Level", PlayerCtrl.Instance.Level);
            PlayerPrefs.SetInt("Coin", PlayerCtrl.Instance.Coin);
            PlayerPrefs.SetInt("AddDamage", PlayerCtrl.Instance.AddDamage);
            PlayerPrefs.SetInt("MaxExp", PlayerCtrl.Instance.MaxExp);
            PlayerPrefs.SetInt("CurrentExp", PlayerCtrl.Instance.CurrentExp);
        }
    }

    // ����Ʈ ����ϴ� �Լ�
    void PrintQuestList()
    {
        // Q1. �� ���� �� ġ�� �ȳ�
        // �÷��̾��� �ִ� Hp���� ���� Hp�� ����, ����Ʈ ����Ʈ�� �ش� ����Ʈ �޽����� ���ԵǾ� ���� �ʴٸ�
        if (PlayerCtrl.Instance.CurrentHp < PlayerCtrl.Instance.MaxHp && !_questList.Contains(Define.HealQuest))
        {
            // ����Ʈ ����Ʈ�� �޽��� �߰�
            _questList.Add(Define.HealQuest);
        }
        // �� �ݴ���
        if (PlayerCtrl.Instance.CurrentHp >= PlayerCtrl.Instance.MaxHp && _questList.Contains(Define.HealQuest))
        {
            // ����Ʈ ����Ʈ���� �޽��� ����
            _questList.Remove(Define.HealQuest);
        }

        // Q2. ���� 5 �޼�, Ȱ ���� ���� �� Ȱ ���� �ȳ�
        // �÷��̾��� ������ 5���� ����, Ȱ�� �������� �ʾҰ�, ����Ʈ�� �ش� ����Ʈ �޽����� ���ԵǾ� ���� �ʴٸ�
        if (PlayerCtrl.Instance.Level >= 5 && !PlayerCtrl.Instance.IsBoughtBow && !_questList.Contains(Define.BuyBowQuest))
        {
            // ����Ʈ ����Ʈ�� �޽��� �߰�
            _questList.Add(Define.BuyBowQuest);
        }
        // �� �ݴ���
        if (PlayerCtrl.Instance.IsBoughtBow && _questList.Contains(Define.BuyBowQuest))
        {
            // ����Ʈ ����Ʈ���� �޽��� ����
            _questList.Remove(Define.BuyBowQuest);
        }

        // ����Ʈ�� ����� ���ڿ�
        string printQuestList = "";

        // ����Ʈ ���
        foreach (string quest in _questList)
        {
            printQuestList += quest + "\n";
        }

        QuestText.text = printQuestList;

        QuestNumText.text = $"{_questList.Count}";
    }

    // Ȩ��ư�� ������ Ȩ ȭ�� �̵� ���� â UI�� Ȱ��ȭ�ϴ� �Լ�
    void HomeButtonClick()
    {
        // �Ͻ�����
        Time.timeScale = 0;
        // Ȩ ȭ�� �̵� ���� â UI Ȱ��ȭ
        HomeChoice.gameObject.SetActive(true);
    }
}
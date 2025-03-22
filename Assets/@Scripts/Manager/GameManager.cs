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
    TMP_Text _questNumText; // 퀘스트 갯수
    Slider _hpBar;
    Slider _expBar;
    Button _homeButton;
    List<string> _questList = new List<string>(); // 퀘스트 리스트

    // public
    public TMP_Text QuestText;
    public TMP_Text QuestNumText;
    public GameObject LevelUpEffect;
    public Canvas HomeChoice;
    public AudioSource LevelUpSound; // 플레이어 레벨업 효과음

    private void Start()
    {
        // 컴포넌트 가져오기
        _hpText = GameObject.Find("HpText")?.GetComponent<TMP_Text>();
        _expText = GameObject.Find("ExpText")?.GetComponent<TMP_Text>();
        _levelText = GameObject.Find("LevelText")?.GetComponent<TMP_Text>();
        _coinText = GameObject.Find("CoinText")?.GetComponent<TMP_Text>();
        _hpBar = GameObject.Find("HpBar")?.GetComponent<Slider>();
        _expBar = GameObject.Find("ExpBar")?.GetComponent<Slider>();
        _homeButton = GameObject.Find("HomeButton")?.GetComponent<Button>();

        // 홈버튼이 널이 아니라면
        if (_homeButton != null)
        {
            // 버튼을 눌렀을 때 HomeButtonClick 함수 실행
            _homeButton.onClick.AddListener(HomeButtonClick);
        }
    }

    private void FixedUpdate()
    {
        PrintPlayerInfo(); // 플레이어 현재 스탯 출력
        PrintQuestList(); // 퀘스트 출력
    }

    // 플레이어의 현재 스탯(정보)출력하는 함수
    void PrintPlayerInfo()
    {
        #region <<< Hp >>>
        PlayerCtrl.Instance.CurrentHp = PlayerPrefs.GetInt("CurrentHp");
        // _maxHp = PlayerPrefs.GetInt("MaxHp");
        // * 현재는 MaxHp가 항상 100으로 설정되어있기 때문에 필요 없음

        _hpBar.value = PlayerCtrl.Instance.CurrentHp;
        _hpBar.maxValue = PlayerCtrl.Instance.MaxHp;
        _hpText.text = $"{PlayerCtrl.Instance.CurrentHp} / {PlayerCtrl.Instance.MaxHp}";

        // 현재 Hp가 0 이하라면 고정적으로 0으로 출력하도록 하기 위함
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

        // 레벨업
        LevelUp();

        #region <<< Level >>>
        PlayerCtrl.Instance.Level = PlayerPrefs.GetInt("Level");

        // 레벨이 10 밑이라면
        if (PlayerCtrl.Instance.Level < 10)
        {
            // 숫자 앞에 0을 붙여서 출력 (01, 02 ... 와 같이 보이기 위함)
            _levelText.text = $"0{PlayerCtrl.Instance.Level}";
        }
        else
        {
            // 10 이상이라면 그대로 출력
            _levelText.text = $"{PlayerCtrl.Instance.Level}";
        }
        #endregion

        #region <<< Coin >>>
        PlayerCtrl.Instance.Coin = PlayerPrefs.GetInt("Coin");

        // 코인이 0 이하라면
        if (PlayerCtrl.Instance.Coin <= 0)
        {
            // 코인을 0으로 출력
            _coinText.text = "0";
        }
        else
        {
            // 숫자 콤마 찍어서 출력
            _coinText.text = string.Format("{0:#,###}", PlayerCtrl.Instance.Coin);

        }
        #endregion
    }

    // 레벨업 처리하는 함수
    void LevelUp()
    {
        // 현재 경험치가 최고 경험치에 도달하면
        if (PlayerCtrl.Instance.CurrentExp >= PlayerCtrl.Instance.MaxExp)
        {
            LevelUpSound.PlayOneShot(LevelUpSound.clip);

            // 레벨을 증가시키고
            PlayerCtrl.Instance.Level++;
            Instantiate(LevelUpEffect, PlayerCtrl.Instance.transform.position, Quaternion.identity);
            // 10,000원의 코인을 추가
            PlayerCtrl.Instance.Coin += 10000;
            // 경험치는 다시 0으로 초기화
            PlayerCtrl.Instance.CurrentExp = 0;
            // 최고 경험치 25 추가 (이전 레벨보다 에너미를 한 명 더 잡아야 레벨업 할 수 있도록 설정함)
            PlayerCtrl.Instance.MaxExp += 25;
            // 에너미에게 가할 수 있는 플레이어의 데미지 3 추가
            PlayerCtrl.Instance.AddDamage += 3;

            // 값 저장
            PlayerPrefs.SetInt("Level", PlayerCtrl.Instance.Level);
            PlayerPrefs.SetInt("Coin", PlayerCtrl.Instance.Coin);
            PlayerPrefs.SetInt("AddDamage", PlayerCtrl.Instance.AddDamage);
            PlayerPrefs.SetInt("MaxExp", PlayerCtrl.Instance.MaxExp);
            PlayerPrefs.SetInt("CurrentExp", PlayerCtrl.Instance.CurrentExp);
        }
    }

    // 퀘스트 출력하는 함수
    void PrintQuestList()
    {
        // Q1. 피 깎였을 때 치료 안내
        // 플레이어의 최대 Hp보다 현재 Hp가 적고, 퀘스트 리스트에 해당 퀘스트 메시지가 포함되어 있지 않다면
        if (PlayerCtrl.Instance.CurrentHp < PlayerCtrl.Instance.MaxHp && !_questList.Contains(Define.HealQuest))
        {
            // 퀘스트 리스트에 메시지 추가
            _questList.Add(Define.HealQuest);
        }
        // 그 반대라면
        if (PlayerCtrl.Instance.CurrentHp >= PlayerCtrl.Instance.MaxHp && _questList.Contains(Define.HealQuest))
        {
            // 퀘스트 리스트에서 메시지 제거
            _questList.Remove(Define.HealQuest);
        }

        // Q2. 레벨 5 달성, 활 구매 전일 때 활 구매 안내
        // 플레이어의 레벨이 5보다 높고, 활을 구매하지 않았고, 리스트에 해당 퀘스트 메시지가 포함되어 있지 않다면
        if (PlayerCtrl.Instance.Level >= 5 && !PlayerCtrl.Instance.IsBoughtBow && !_questList.Contains(Define.BuyBowQuest))
        {
            // 퀘스트 리스트에 메시지 추가
            _questList.Add(Define.BuyBowQuest);
        }
        // 그 반대라면
        if (PlayerCtrl.Instance.IsBoughtBow && _questList.Contains(Define.BuyBowQuest))
        {
            // 퀘스트 리스트에서 메시지 제거
            _questList.Remove(Define.BuyBowQuest);
        }

        // 리스트를 출력할 문자열
        string printQuestList = "";

        // 리스트 출력
        foreach (string quest in _questList)
        {
            printQuestList += quest + "\n";
        }

        QuestText.text = printQuestList;

        QuestNumText.text = $"{_questList.Count}";
    }

    // 홈버튼을 누르면 홈 화면 이동 여부 창 UI를 활성화하는 함수
    void HomeButtonClick()
    {
        // 일시정지
        Time.timeScale = 0;
        // 홈 화면 이동 여부 창 UI 활성화
        HomeChoice.gameObject.SetActive(true);
    }
}
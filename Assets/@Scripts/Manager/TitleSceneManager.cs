using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    // private
    Button _startButton; // 이어하기
    Button _resetButton; // 새로하기

    void Start()
    {
        // 컴포넌트 가져오기
        _startButton = GameObject.Find("StartButton")?.GetComponent<Button>();
        _resetButton = GameObject.Find("ResetButton")?.GetComponent<Button>();

        // 이어하기 버튼이 널이 아니라면
        if (_startButton != null)
        {
            _startButton.onClick.AddListener(StartButtonClick);
        }
        // 새로하기 버튼이 널이 아니라면
        if (_resetButton != null)
        {
            _resetButton.onClick.AddListener(ResetButtonClick);
        }
    }

    // 이어하기 버튼 처리하는 함수
    void StartButtonClick()
    {
        SceneManager.LoadScene("Main"); // Main씬으로 이동
    }

    // 새로하기 버튼
    void ResetButtonClick()
    {
        // 모든 데이터 초기화
        PlayerPrefs.DeleteAll();

        // 초깃값 설정
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

        SceneManager.LoadScene("Main"); // 초기화 후 Main씬으로 이동
    }
}

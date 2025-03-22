using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtonCtrl : MonoBehaviour
{
    // public
    public Canvas HomeChoice; // 홈 화면 이동 여부 창 UI

    // YES 버튼을 누르면 Title씬으로 이동하는 함수
    public void YesHomePointerClick()
    {
        SceneManager.LoadScene("Title");
        // 일시정지 해제
        Time.timeScale = 1;
    }

    // NO 버튼을 누르면 UI가 비활성화되는 함수
    public void NoHomePointerClick()
    {
        // UI 비활성화
        HomeChoice.gameObject.SetActive(false);
        // 일시정지 해제
        Time.timeScale = 1;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtonCtrl : MonoBehaviour
{
    // public
    public Canvas HomeChoice; // Ȩ ȭ�� �̵� ���� â UI

    // YES ��ư�� ������ Title������ �̵��ϴ� �Լ�
    public void YesHomePointerClick()
    {
        SceneManager.LoadScene("Title");
        // �Ͻ����� ����
        Time.timeScale = 1;
    }

    // NO ��ư�� ������ UI�� ��Ȱ��ȭ�Ǵ� �Լ�
    public void NoHomePointerClick()
    {
        // UI ��Ȱ��ȭ
        HomeChoice.gameObject.SetActive(false);
        // �Ͻ����� ����
        Time.timeScale = 1;
    }
}

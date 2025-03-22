using System.Collections;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{
    // private
    float _speed = 10f;

    void OnEnable()
    {
        // �÷��̾ ���� �¿� ����
        if (PlayerCtrl.Instance.Scale.x == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        if (PlayerCtrl.Instance.Scale.x == -1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        // �⺻�� ������ �߻�
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }

        // ȭ�� ��Ȱ��ȭ�ϴ� �ڷ�ƾ ����
        StartCoroutine(CoDeactivate());
    }

    // ȭ�� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    IEnumerator CoDeactivate()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        // ȭ�� �̵�
        transform.Translate(Vector2.up * _speed * Time.deltaTime);
    }
}

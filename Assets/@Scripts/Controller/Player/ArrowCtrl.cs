using System.Collections;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{
    // private
    float _speed = 10f;

    void OnEnable()
    {
        // 플레이어에 따른 좌우 구분
        if (PlayerCtrl.Instance.Scale.x == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        if (PlayerCtrl.Instance.Scale.x == -1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        // 기본은 오른쪽 발사
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }

        // 화살 비활성화하는 코루틴 시작
        StartCoroutine(CoDeactivate());
    }

    // 화살 비활성화하는 코루틴
    IEnumerator CoDeactivate()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        // 화살 이동
        transform.Translate(Vector2.up * _speed * Time.deltaTime);
    }
}

using System.Collections;
using UnityEngine;

public class BossEnemyBullet : MonoBehaviour
{
    // private
    Rigidbody2D _rigidbody2D;

    void Start()
    {
        // 총알 이동
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody2D.linearVelocity = 10 * transform.right;

        // 총알 제거하는 코루틴
        StartCoroutine(CoDestroyBullet());
    }

    IEnumerator CoDestroyBullet()
    {
        // 1.5초 후 오브젝트 제거
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}

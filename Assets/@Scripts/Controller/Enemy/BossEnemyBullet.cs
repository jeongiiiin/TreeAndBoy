using System.Collections;
using UnityEngine;

public class BossEnemyBullet : MonoBehaviour
{
    // private
    Rigidbody2D _rigidbody2D;

    void Start()
    {
        // �Ѿ� �̵�
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody2D.linearVelocity = 10 * transform.right;

        // �Ѿ� �����ϴ� �ڷ�ƾ
        StartCoroutine(CoDestroyBullet());
    }

    IEnumerator CoDestroyBullet()
    {
        // 1.5�� �� ������Ʈ ����
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}

using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // private
    Transform _player; // �÷��̾� ��ġ (Ÿ��)
    Rigidbody2D _rigidbody2D;
    float _speed = 8f; // �Ѿ� �ӵ�

    void Start()
    {
        // �÷��̾ ���� �ƴ϶��
        if (_player == null)
        {
            // �±׷� �÷��̾� ã��
            _player = GameObject.FindGameObjectWithTag("PLAYER").transform;
        }

        // ������Ʈ ��������
        _rigidbody2D = GetComponent<Rigidbody2D>();

        // �÷��̾� ���� Ȯ��
        Vector2 direction = (_player.position - transform.position).normalized;

        // ������ٵ� ���� �ƴ϶��
        if (_rigidbody2D != null)
        {
            // �÷��̾�� �Ѿ� �߻�
            _rigidbody2D.AddForce(direction * _speed, ForceMode2D.Impulse);
        }

        // �Ѿ� �����ϴ� �ڷ�ƾ
        StartCoroutine(CoDestroyBullet());
    }

    IEnumerator CoDestroyBullet()
    {
        // 4�� �� �Ѿ� ����
        yield return new WaitForSeconds(4);

        Destroy(gameObject);
    }

}

using UnityEngine;

public class CoinEffect : MonoBehaviour
{
    // private
    Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        // ���� Ƣ������� ȿ��
        _rigidbody2D.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);

        // 1�� �� ������Ʈ ����
        Destroy(gameObject, 1);
    }
}

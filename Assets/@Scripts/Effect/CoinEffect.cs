using UnityEngine;

public class CoinEffect : MonoBehaviour
{
    // private
    Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        // 위로 튀어오르는 효과
        _rigidbody2D.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);

        // 1초 후 오브젝트 제거
        Destroy(gameObject, 1);
    }
}

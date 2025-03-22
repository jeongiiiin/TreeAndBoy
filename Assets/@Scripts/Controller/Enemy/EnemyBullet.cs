using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // private
    Transform _player; // 플레이어 위치 (타겟)
    Rigidbody2D _rigidbody2D;
    float _speed = 8f; // 총알 속도

    void Start()
    {
        // 플레이어가 널이 아니라면
        if (_player == null)
        {
            // 태그로 플레이어 찾기
            _player = GameObject.FindGameObjectWithTag("PLAYER").transform;
        }

        // 컴포넌트 가져오기
        _rigidbody2D = GetComponent<Rigidbody2D>();

        // 플레이어 방향 확인
        Vector2 direction = (_player.position - transform.position).normalized;

        // 리지드바디가 널이 아니라면
        if (_rigidbody2D != null)
        {
            // 플레이어에게 총알 발사
            _rigidbody2D.AddForce(direction * _speed, ForceMode2D.Impulse);
        }

        // 총알 제거하는 코루틴
        StartCoroutine(CoDestroyBullet());
    }

    IEnumerator CoDestroyBullet()
    {
        // 4초 후 총알 제거
        yield return new WaitForSeconds(4);

        Destroy(gameObject);
    }

}

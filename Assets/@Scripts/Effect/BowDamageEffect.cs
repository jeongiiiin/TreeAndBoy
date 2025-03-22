using UnityEngine;
using TMPro;

public class BowDamageEffect : MonoBehaviour
{
    // private
    float _speed = 1f;

    // public
    public TextMeshPro BowDamageText;

    void Start()
    {
        // 아웃라인 두께, 색상 설정
        BowDamageText.outlineWidth = 0.1f;
        BowDamageText.outlineColor = Color.black;

        // 0.5초 뒤 오브젝트 제거
        Destroy(gameObject, 0.5f);
    }

    private void Update()
    {
        // 왼쪽 위로 이동하는 모션
        transform.position += new Vector3(_speed * Time.deltaTime, _speed * Time.deltaTime, 0f);
    }
}

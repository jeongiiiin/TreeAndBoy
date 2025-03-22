using UnityEngine;
using TMPro;

public class DamageEffect : MonoBehaviour
{
    // private
    float _speed = 1f;

    // public
    public TextMeshPro DamageText;

    void Start()
    {
        // 플레이어가 에너미에게 주고 있는 데미지 출력
        DamageText.text = $"{PlayerCtrl.Instance.AttackDamage}";

        // 아웃라인 두께, 색상 설정
        DamageText.outlineWidth = 0.1f;
        DamageText.outlineColor = Color.black;

        // 0.5초 후 오브젝트 제거
        Destroy(gameObject, 0.5f);
    }

    private void Update()
    {
        // 왼쪽 위로 이동하는 모션
        transform.position += new Vector3(_speed * Time.deltaTime, _speed * Time.deltaTime, 0f);
    }
}

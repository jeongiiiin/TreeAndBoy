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
        // �÷��̾ ���ʹ̿��� �ְ� �ִ� ������ ���
        DamageText.text = $"{PlayerCtrl.Instance.AttackDamage}";

        // �ƿ����� �β�, ���� ����
        DamageText.outlineWidth = 0.1f;
        DamageText.outlineColor = Color.black;

        // 0.5�� �� ������Ʈ ����
        Destroy(gameObject, 0.5f);
    }

    private void Update()
    {
        // ���� ���� �̵��ϴ� ���
        transform.position += new Vector3(_speed * Time.deltaTime, _speed * Time.deltaTime, 0f);
    }
}

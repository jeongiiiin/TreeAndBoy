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
        // �ƿ����� �β�, ���� ����
        BowDamageText.outlineWidth = 0.1f;
        BowDamageText.outlineColor = Color.black;

        // 0.5�� �� ������Ʈ ����
        Destroy(gameObject, 0.5f);
    }

    private void Update()
    {
        // ���� ���� �̵��ϴ� ���
        transform.position += new Vector3(_speed * Time.deltaTime, _speed * Time.deltaTime, 0f);
    }
}

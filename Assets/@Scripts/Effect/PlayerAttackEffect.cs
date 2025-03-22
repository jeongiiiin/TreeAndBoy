using System.Collections;
using UnityEngine;

public class PlayerAttackEffect : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(CoDestroyEffect());
    }

    IEnumerator CoDestroyEffect()
    {
        // 0.3�� �� ������Ʈ ����
        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
    }
}

using System.Collections;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(CoDestroyEffect());
    }

    IEnumerator CoDestroyEffect()
    {
        // 0.3초 후 오브젝트 제거
        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
    }
}

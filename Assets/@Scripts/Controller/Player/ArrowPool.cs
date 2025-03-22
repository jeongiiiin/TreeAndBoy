using System.Collections.Generic;
using UnityEngine;

// 화살 오브젝트 풀

public class ArrowPool : MonoBehaviour
{
    #region 인스턴스 생성
    public static ArrowPool Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    // private
    GameObject _arrowPool;

    // public
    public GameObject Arrow;
    public List<GameObject> ArrowList = new List<GameObject>();


    void Start()
    {
        _arrowPool = new GameObject("ArrowPool");
    }

    public GameObject GetArrow()
    {
        for (int i = 0; i < ArrowList.Count; i++)
        {
            if (!ArrowList[i].activeSelf)
            {
                ArrowList[i].SetActive(true);
                ArrowList[i].transform.position = transform.position;
                return ArrowList[i];
            }
        }

        GameObject arrow = Instantiate(Arrow);
        arrow.transform.parent = _arrowPool.transform;
        arrow.transform.position = transform.position;
        ArrowList.Add(arrow);
        return arrow;
    }
}

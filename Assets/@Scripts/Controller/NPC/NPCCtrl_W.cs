using System.Collections;
using TMPro;
using UnityEngine;

public class NPCCtrl_W : MonoBehaviour
{
    TMP_Text _npcText_W;
    bool _isCollisionActive; // �浹 ����

    void Start()
    {
        // ������Ʈ ��������
        _npcText_W = GameObject.Find("NPCText_W")?.GetComponent<TMP_Text>();
    }

    // �浹 ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾�� �浹�Ͽ��ٸ�
        if (collision.transform.CompareTag("PLAYER"))
        {
            // _isCollisionActive�� false���
            if (!_isCollisionActive)
            {
                // _isCollisionActive�� true�� �ٲٰ�
                _isCollisionActive = true;

                // NPC�� �޽����� ����ϴ� �ڷ�ƾ ����
                StartCoroutine(CoStoneMessage());
            }
        }
    }

    // �浹 ����
    private void OnCollisionExit2D(Collision2D collision)
    {
        // �÷��̾�� �浹�� �����Ǹ�
        if (collision.transform.CompareTag("PLAYER"))
        {
            _isCollisionActive = false;
            _npcText_W.text = "";

            // NPC�� �޽����� ����ϴ� �ڷ�ƾ ����
            StopCoroutine(CoStoneMessage());
        }
    }

    // NPC�� �޽����� ����ϴ� �ڷ�ƾ
    IEnumerator CoStoneMessage()
    {
        // �÷��̾�� �浹���ִٸ� ��� �ݺ�
        while (_isCollisionActive)
        {
            // �ƿ����� �β�, ���� ����
            _npcText_W.outlineWidth = 0.1f;
            _npcText_W.outlineColor = Color.black;

            // �÷��̾� ���� 5 �̻��� ���� Ȱ ���� ��Ʈ ���
            if (PlayerCtrl.Instance.Level >= 5)
            {
                _npcText_W.text = "�� Ȱ�� �� Ȱ�̳�";

                // �޽��� ��� ������
                yield return new WaitForSeconds(1f);

                // �÷��̾�� �浹�� �����Ǹ�
                if (!_isCollisionActive)
                {
                    // �ڷ�ƾ ������
                    yield break;
                }

                _npcText_W.text = "50,000���̴�";

                yield return new WaitForSeconds(1f);

                if (!_isCollisionActive)
                {
                    yield break;
                }
            }
            if (PlayerCtrl.Instance.Level < 5)
            {
                _npcText_W.text = "???";

                yield return new WaitForSeconds(1f);

                if (!_isCollisionActive)
                {
                    yield break;
                }
            }

        }
    }
}

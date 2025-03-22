using System.Collections;
using TMPro;
using UnityEngine;

public class NPCCtrl_M : MonoBehaviour
{
    // private
    TMP_Text _npcText_M;
    bool _isCollisionActive; // �浹 ����

    void Start()
    {
        // ������Ʈ �������� 
        _npcText_M = GameObject.Find("NPCText_M")?.GetComponent<TMP_Text>();
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
                StartCoroutine(CoNPCMessage());
            }
        }
    }

    // �浹 ����
    private void OnCollisionExit2D(Collision2D collision)
    {
        // �÷��̾�� �浹�� �����Ǿ��ٸ�
        if (collision.transform.CompareTag("PLAYER"))
        {
            // _isCollisionActive�� false�� �ٲٰ�
            _isCollisionActive = false;
            _npcText_M.text = "";

            // NPC�� �޽����� ����ϴ� �ڷ�ƾ ����
            StopCoroutine(CoNPCMessage());
        }
    }

    // NPC�� �޽����� ����ϴ� �ڷ�ƾ
    IEnumerator CoNPCMessage()
    {
        // �÷��̾�� �浹���ִٸ� ��� �ݺ�
        while (_isCollisionActive)
        {
            // �ƿ����� �β�, ���� ����
            _npcText_M.outlineWidth = 0.1f;
            _npcText_M.outlineColor = Color.black;

            _npcText_M.text = "���� ġ�����ٰ�!";

            // �޽��� ��� ������
            yield return new WaitForSeconds(1f);

            // �÷��̾�� �浹�� �����Ǹ�
            if (!_isCollisionActive)
            {
                // �ڷ�ƾ ������
                yield break;
            }

            _npcText_M.text = "�� ��!";

            yield return new WaitForSeconds(1f);

            if (!_isCollisionActive)
            {
                yield break;
            }

            _npcText_M.text = "5,000��!";

            yield return new WaitForSeconds(1f);

            if (!_isCollisionActive)
            {
                yield break;
            }
        }
    }

}

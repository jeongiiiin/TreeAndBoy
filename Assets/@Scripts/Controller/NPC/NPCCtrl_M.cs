using System.Collections;
using TMPro;
using UnityEngine;

public class NPCCtrl_M : MonoBehaviour
{
    // private
    TMP_Text _npcText_M;
    bool _isCollisionActive; // 충돌 여부

    void Start()
    {
        // 컴포넌트 가져오기 
        _npcText_M = GameObject.Find("NPCText_M")?.GetComponent<TMP_Text>();
    }

    // 충돌 시작
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 충돌하였다면
        if (collision.transform.CompareTag("PLAYER"))
        {
            // _isCollisionActive가 false라면
            if (!_isCollisionActive)
            {
                // _isCollisionActive를 true로 바꾸고
                _isCollisionActive = true;

                // NPC의 메시지를 출력하는 코루틴 시작
                StartCoroutine(CoNPCMessage());
            }
        }
    }

    // 충돌 해제
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 플레이어와 충돌이 해제되었다면
        if (collision.transform.CompareTag("PLAYER"))
        {
            // _isCollisionActive를 false로 바꾸고
            _isCollisionActive = false;
            _npcText_M.text = "";

            // NPC의 메시지를 출력하는 코루틴 중지
            StopCoroutine(CoNPCMessage());
        }
    }

    // NPC의 메시지를 출력하는 코루틴
    IEnumerator CoNPCMessage()
    {
        // 플레이어와 충돌해있다면 계속 반복
        while (_isCollisionActive)
        {
            // 아웃라인 두께, 색상 설정
            _npcText_M.outlineWidth = 0.1f;
            _npcText_M.outlineColor = Color.black;

            _npcText_M.text = "내가 치료해줄게!";

            // 메시지 출력 딜레이
            yield return new WaitForSeconds(1f);

            // 플레이어와 충돌이 해제되면
            if (!_isCollisionActive)
            {
                // 코루틴 나가기
                yield break;
            }

            _npcText_M.text = "돈 줘!";

            yield return new WaitForSeconds(1f);

            if (!_isCollisionActive)
            {
                yield break;
            }

            _npcText_M.text = "5,000원!";

            yield return new WaitForSeconds(1f);

            if (!_isCollisionActive)
            {
                yield break;
            }
        }
    }

}

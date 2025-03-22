using System.Collections;
using TMPro;
using UnityEngine;

public class NPCCtrl_W : MonoBehaviour
{
    TMP_Text _npcText_W;
    bool _isCollisionActive; // 충돌 여부

    void Start()
    {
        // 컴포넌트 가져오기
        _npcText_W = GameObject.Find("NPCText_W")?.GetComponent<TMP_Text>();
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
                StartCoroutine(CoStoneMessage());
            }
        }
    }

    // 충돌 해제
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 플레이어와 충돌이 해제되면
        if (collision.transform.CompareTag("PLAYER"))
        {
            _isCollisionActive = false;
            _npcText_W.text = "";

            // NPC의 메시지를 출력하는 코루틴 중지
            StopCoroutine(CoStoneMessage());
        }
    }

    // NPC의 메시지를 출력하는 코루틴
    IEnumerator CoStoneMessage()
    {
        // 플레이어와 충돌해있다면 계속 반복
        while (_isCollisionActive)
        {
            // 아웃라인 두께, 색상 설정
            _npcText_W.outlineWidth = 0.1f;
            _npcText_W.outlineColor = Color.black;

            // 플레이어 레벨 5 이상일 때만 활 구매 멘트 출력
            if (PlayerCtrl.Instance.Level >= 5)
            {
                _npcText_W.text = "이 활이 니 활이냐";

                // 메시지 출력 딜레이
                yield return new WaitForSeconds(1f);

                // 플레이어와 충돌이 해제되면
                if (!_isCollisionActive)
                {
                    // 코루틴 나가기
                    yield break;
                }

                _npcText_W.text = "50,000원이다";

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

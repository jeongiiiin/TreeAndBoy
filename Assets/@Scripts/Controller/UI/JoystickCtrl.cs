using UnityEngine;
using UnityEngine.EventSystems;

// * IPointer, IDragHandler
// : 이미지에 이벤트를 발생시킬 수 있는 인터페이스 (EventSystems 네임스페이스)

public class JoystickCtrl : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    // private
    Vector2 _moveDir;
    Vector2 _touchPos;
    Vector2 _originPos;
    Vector2 _handlerBasePos = new Vector2(200, 200);
    float _radius;
    #region 핸들러 범위
    float _minX = 50;
    float _maxX = 350;
    float _minY = 50;
    float _maxY = 350;
    #endregion

    // public
    public GameObject JoyStick;
    public GameObject Handler;

    void Awake()
    {
        _originPos = JoyStick.transform.position;
        Handler.transform.position = _handlerBasePos;
        _radius = JoyStick.GetComponent<RectTransform>().sizeDelta.y / 3;
        // * sizeDelta.y : y축 크기
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsWithinBounds(eventData.position))
        {
            _touchPos = eventData.position;
            Handler.transform.position = _handlerBasePos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 이벤트가 발생한 현재 위치
        Vector2 dragPos = eventData.position;

        // * Mathf.Clamp
        // : 최소, 최댓값을 설정하여 float 값이 범위 이외의 값을 넘지 않도록 함
        dragPos.x = Mathf.Clamp(dragPos.x, _minX, _maxX);
        dragPos.y = Mathf.Clamp(dragPos.y, _minY, _maxY);

        // 드래그된 위치와 조이스틱의 초기 위치 차이를 계산하여 방향 확인
        _moveDir = (dragPos - _touchPos).normalized;
        // 드래그된 위치와 원래 위치 사이의 거리 계산
        // * sqrMagnitude : 벡터의 제곱된 크기 반환 (제곱 계산하지 않기 때문에 성능적으로 빠름)
        float distance = (dragPos - _originPos).sqrMagnitude;

        // 조이스틱의 새 위치 계산할 변수
        Vector3 newPos;

        // 조이스틱의 안에서만 핸들러 이동
        if (distance < _radius)
        {
            newPos = _handlerBasePos + (_moveDir * distance);
        }
        else
        {
            newPos = _handlerBasePos + (_moveDir * _radius);
        }

        newPos.x = Mathf.Clamp(newPos.x, _minX, _maxX);
        newPos.y = Mathf.Clamp(newPos.y, _minY, _maxY);

        // 계산된 새 위치로 조이스틱 핸들러 이동
        Handler.transform.position = newPos;

        // PlayerCtrl에 현재 이동방향 전달
        PlayerCtrl.Instance.MoveDir = _moveDir;
    }

    // 포인터를 떼면 다시 원래 위치로 돌아가는 함수
    public void OnPointerUp(PointerEventData eventData)
    {
        // 이동 방향 초기화 (원래 위치로)
        _moveDir = Vector2.zero;
        Handler.transform.position = _handlerBasePos;
        // PlayerCtrl에 초기화된 이동 방향 값 전달
        PlayerCtrl.Instance.MoveDir = _moveDir;
    }

    // 핸들러가 경계 내에서만 움직이도록 하는 함수
    private bool IsWithinBounds(Vector2 position)
    {
        return position.x >= _minX && position.x <= _maxX && position.y >= _minY && position.y <= _maxY;
    }
}

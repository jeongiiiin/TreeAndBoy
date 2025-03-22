using UnityEngine;
using UnityEngine.EventSystems;

// * IPointer, IDragHandler
// : �̹����� �̺�Ʈ�� �߻���ų �� �ִ� �������̽� (EventSystems ���ӽ����̽�)

public class JoystickCtrl : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    // private
    Vector2 _moveDir;
    Vector2 _touchPos;
    Vector2 _originPos;
    Vector2 _handlerBasePos = new Vector2(200, 200);
    float _radius;
    #region �ڵ鷯 ����
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
        // * sizeDelta.y : y�� ũ��
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
        // �巡�� �̺�Ʈ�� �߻��� ���� ��ġ
        Vector2 dragPos = eventData.position;

        // * Mathf.Clamp
        // : �ּ�, �ִ��� �����Ͽ� float ���� ���� �̿��� ���� ���� �ʵ��� ��
        dragPos.x = Mathf.Clamp(dragPos.x, _minX, _maxX);
        dragPos.y = Mathf.Clamp(dragPos.y, _minY, _maxY);

        // �巡�׵� ��ġ�� ���̽�ƽ�� �ʱ� ��ġ ���̸� ����Ͽ� ���� Ȯ��
        _moveDir = (dragPos - _touchPos).normalized;
        // �巡�׵� ��ġ�� ���� ��ġ ������ �Ÿ� ���
        // * sqrMagnitude : ������ ������ ũ�� ��ȯ (���� ������� �ʱ� ������ ���������� ����)
        float distance = (dragPos - _originPos).sqrMagnitude;

        // ���̽�ƽ�� �� ��ġ ����� ����
        Vector3 newPos;

        // ���̽�ƽ�� �ȿ����� �ڵ鷯 �̵�
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

        // ���� �� ��ġ�� ���̽�ƽ �ڵ鷯 �̵�
        Handler.transform.position = newPos;

        // PlayerCtrl�� ���� �̵����� ����
        PlayerCtrl.Instance.MoveDir = _moveDir;
    }

    // �����͸� ���� �ٽ� ���� ��ġ�� ���ư��� �Լ�
    public void OnPointerUp(PointerEventData eventData)
    {
        // �̵� ���� �ʱ�ȭ (���� ��ġ��)
        _moveDir = Vector2.zero;
        Handler.transform.position = _handlerBasePos;
        // PlayerCtrl�� �ʱ�ȭ�� �̵� ���� �� ����
        PlayerCtrl.Instance.MoveDir = _moveDir;
    }

    // �ڵ鷯�� ��� �������� �����̵��� �ϴ� �Լ�
    private bool IsWithinBounds(Vector2 position)
    {
        return position.x >= _minX && position.x <= _maxX && position.y >= _minY && position.y <= _maxY;
    }
}

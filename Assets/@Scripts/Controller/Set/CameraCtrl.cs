using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    // private
    Vector3 _offset = new Vector3(0, 0, -10);
    Vector3 _movePos;
    float _speed = 3;

    // public
    public Transform Player;
    public Vector2 MinRange; // ī�޶� �̵� �ּ� ����
    public Vector2 MaxRange; // ī�޶� �̵� �ִ� ����

    private void Awake()
    {
        Player = Player.transform;
    }

    private void FixedUpdate()
    {
        _movePos = Player.position + _offset;

        // ī�޶� �̵� ���� ����
        // * Mathf.Clamp (float value, float min, float max);
        // : �ּ�/�ִ��� �����Ͽ� ���� �̿��� ���� ���� �ʵ��� �Ѵ�.
        _movePos.x = Mathf.Clamp(_movePos.x, MinRange.x, MaxRange.x);
        _movePos.y = Mathf.Clamp(_movePos.y, MinRange.y, MaxRange.y);

        // * Lerp
        // : ���� ���� (������Ʈ�� �ε巴�� �̵�, ȸ���ϵ��� �ϱ� ����)
        transform.position = Vector3.Lerp(transform.position, _movePos, _speed * Time.deltaTime);
    }
}

using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    // private
    Vector3 _offset = new Vector3(0, 0, -10);
    Vector3 _movePos;
    float _speed = 3;

    // public
    public Transform Player;
    public Vector2 MinRange; // 카메라 이동 최소 범위
    public Vector2 MaxRange; // 카메라 이동 최대 범위

    private void Awake()
    {
        Player = Player.transform;
    }

    private void FixedUpdate()
    {
        _movePos = Player.position + _offset;

        // 카메라 이동 범위 설정
        // * Mathf.Clamp (float value, float min, float max);
        // : 최소/최댓값을 설정하여 범위 이외의 값을 넘지 않도록 한다.
        _movePos.x = Mathf.Clamp(_movePos.x, MinRange.x, MaxRange.x);
        _movePos.y = Mathf.Clamp(_movePos.y, MinRange.y, MaxRange.y);

        // * Lerp
        // : 선형 보간 (오브젝트를 부드럽게 이동, 회전하도록 하기 위함)
        transform.position = Vector3.Lerp(transform.position, _movePos, _speed * Time.deltaTime);
    }
}

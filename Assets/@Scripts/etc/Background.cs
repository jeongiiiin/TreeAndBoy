using UnityEngine;

public class Background : MonoBehaviour
{
    // private
    float _speed;
    MeshRenderer _meshRenderer;

    private void Start()
    {
        _speed = 0.1f;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        // 백그라운드 이동 (무한맵)
        Vector2 newOffset = _meshRenderer.material.mainTextureOffset;
        newOffset.x += _speed * Time.deltaTime;

        _meshRenderer.material.mainTextureOffset = newOffset;
    }
}

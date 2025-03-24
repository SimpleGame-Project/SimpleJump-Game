using UnityEngine;

public class JumpGuide : MonoBehaviour
{
    public LineRenderer _lineRender;
    public Transform _player;
    public int _resolution;
    public float _timeStep;
    public float _gravity;
    private Vector2 _jumpVelocity;

    void Awake()
    {
        _lineRender = GetComponent<LineRenderer>();
        _player = GetComponent<Transform>();
    }
    public void ShowJumpGuide(Vector2 jumpDirection, float dragPower)
    {
        _jumpVelocity = jumpDirection * dragPower;
        _lineRender.positionCount = _resolution;

        Vector3 startPosition = _player.position;
        Vector3 velocity = _jumpVelocity;

        for (int i = 0; i < _resolution; i++)
        {
            float t = i * _timeStep;
            float x = startPosition.x + velocity.x * t;
            float y = startPosition.y + velocity.y * t + 0.5f * _gravity * t * t;
            _lineRender.SetPosition(i, new Vector3(x, y, 0));
        }

        _lineRender.enabled = true;
    }

    public void HideJumpGuide()
    {
        _lineRender.enabled = false;
    }
}

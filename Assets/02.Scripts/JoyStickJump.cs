using UnityEngine;
using UnityEngine.EventSystems;

namespace Jang
{
    public class JoyStickJump : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private RectTransform _joyStick;
        [SerializeField] private RectTransform _handle;
        private float _radius;
        [SerializeField] private PlayerController _player;
        private JumpGuide _jumpGuide;
        [SerializeField] private Vector2 _jumpDirection;
        [SerializeField] private float _dragPower;

        void Start()
        {
            _jumpGuide = _player.GetComponent<JumpGuide>();
            _radius = _joyStick.rect.width * 0.5f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // 조이스틱에서 손을 뗐을 때 점프
            _handle.localPosition = Vector3.zero;
            _jumpGuide.HideJumpGuide();
            _player.JumpUp(_jumpDirection, _dragPower);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_player._isLand)
            {
                // 조이스틱을 드래그 중일 때 안내선 표시 
                Vector2 value = eventData.position - (Vector2)_joyStick.position;

                if(value.y > 0)
                    value.y = 0;
                    
                value = Vector2.ClampMagnitude(value, _radius);

                // 가운데 위치와 핸들 간의 거리로 파워 조정 0 ~ 1
                _dragPower = Vector2.Distance(_joyStick.position, _handle.position) / _radius;

                _handle.localPosition = value;

                // 아래로 땡기면 위로 튀어 올라가야하므로 점프는 반대 방향
                _jumpDirection = -value.normalized;

                if (_jumpDirection.x < 0)
                    _player.transform.localScale = new Vector3(3f, _player.transform.localScale.y);
                else
                    _player.transform.localScale = new Vector3(-3f, _player.transform.localScale.y);

                // 안내선 활성화
                _jumpGuide.ShowJumpGuide(_jumpDirection, _dragPower * _player._jumpForce);
            }
        }
    }
}

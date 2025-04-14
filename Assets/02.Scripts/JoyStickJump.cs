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
                // ScreenPoint를 Joystick 기준의 localPoint로 변환
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _joyStick, eventData.position, eventData.pressEventCamera, out localPoint);


                if (localPoint.y > 0)
                    localPoint.y = 0;

                // 조이스틱의 반지름을 넘지 않게 Clamp  
                localPoint = Vector2.ClampMagnitude(localPoint, _radius);

                // 핸들 이동
                _handle.localPosition = localPoint;

                _dragPower = localPoint.magnitude / _radius;

                // 아래로 땡기면 위로 튀어 올라가야하므로 점프는 반대 방향
                _jumpDirection = -localPoint.normalized;

                // 캐릭터 방향 조정
                SetCharacterDirection(_jumpDirection);

                // 안내선 활성화
                _jumpGuide.ShowJumpGuide(_jumpDirection, _dragPower * _player._jumpForce);
            }
        }

        private void SetCharacterDirection(Vector2 direction)
        {
            _player.transform.localScale = new Vector3(
            direction.x < 0 ? 3f : -3f, _player.transform.localScale.y);
        }
    }
}

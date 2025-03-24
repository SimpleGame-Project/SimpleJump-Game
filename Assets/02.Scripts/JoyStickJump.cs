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
            _handle.localPosition = Vector3.zero;
            _jumpGuide.HideJumpGuide();
            _player.JumpUp(_jumpDirection, _dragPower);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 value = eventData.position - (Vector2)_joyStick.position;

            value = Vector2.ClampMagnitude(value, _radius);

            _dragPower = Vector2.Distance(_joyStick.position, _handle.position) / _radius;

            _handle.localPosition = value;

            _jumpDirection = -value.normalized;

            _jumpGuide.ShowJumpGuide(_jumpDirection, _dragPower * _player._jumpForce);
        }
    }
}

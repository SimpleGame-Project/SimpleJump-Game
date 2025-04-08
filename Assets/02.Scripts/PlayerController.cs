using Unity.VisualScripting;
using UnityEngine;

namespace Jang
{
    public abstract class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Animator _anim;
        public bool _isLand;
        public int _hp;
        public float _jumpForce;
        public Vector2 _jumpDirection;
        public VScrollBackground vscroll;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();

            InitCharacter();
        }

        protected abstract void InitCharacter();

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpUp(_jumpDirection, 0.6f);
            }
        }

        public void JumpUp(Vector2 direction, float dragPower)
        {
            if (_isLand)
            {
                _isLand = false;
                _rb.linearVelocity = direction * _jumpForce * dragPower;
                _anim.SetBool("IsLand", _isLand);
            }
        }

        protected void JumpLand()
        {
            if (!_isLand)
            {
                _isLand = true;
                //_rb.linearVelocity = Vector2.zero;
                vscroll.MoveToY(transform.position.y);

                _anim.SetBool("IsLand", _isLand);
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ground"))
            {
                JumpLand();
            }
        }
    }
}

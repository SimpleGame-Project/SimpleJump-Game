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

        private float pre_Y;
        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();

            InitCharacter();

            pre_Y = transform.position.y;
        }

        protected abstract void InitCharacter();

        void FixedUpdate()
        {
            if(_rb.linearVelocityX < 0)
                _anim.transform.localScale = new Vector3(3f, _anim.transform.localScale.y);

            else if(_rb.linearVelocityX > 0)
                _anim.transform.localScale = new Vector3(-3f, _anim.transform.localScale.y);
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
                _rb.linearVelocity = Vector2.zero;
                
                _anim.SetBool("IsLand", _isLand);

                if(pre_Y < transform.position.y)
                    vscroll.MoveToY(transform.position.y);
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

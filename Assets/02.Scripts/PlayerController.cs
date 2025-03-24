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

        void FixedUpdate()
        {
            if (_rb.linearVelocityY > 0f)
                _anim.SetFloat("Velocity", 1f);

            else
                _anim.SetFloat("Velocity", -1f);

            if(!_isLand)
                RotatePlayer();
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
            transform.rotation = Quaternion.identity;
            _isLand = true;
            _rb.linearVelocity = Vector2.zero;

            _anim.SetBool("IsLand", _isLand);
            _anim.SetFloat("Velocity", 0f);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if(col.gameObject.CompareTag("Ground"))
            {
                JumpLand();
            }
        }

        private void RotatePlayer()
        {
            float angle = Mathf.Atan2(_rb.linearVelocityY, _rb.linearVelocityX) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0,0, angle - 90);
        }
    }
}

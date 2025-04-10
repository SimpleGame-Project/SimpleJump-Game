using System;
using System.Collections;
using UnityEngine;

namespace Jang
{
    public abstract class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Animator _anim;
        public bool _isLand;
        private int _maxHp;
        private int _hp;
        private int _attack;
        public int MaxHp
        {
            set
            {
                _maxHp = Math.Max(0, value);

                if (GameUIManager.Instance != null)
                {
                    GameUIManager.Instance.UpdateHpUI(_maxHp, _hp);
                }
            }

            get => _maxHp;
        }
        public int Hp
        {
            set
            {
                _hp = Math.Max(0, value);

                if (GameUIManager.Instance != null)
                {
                    GameUIManager.Instance.UpdateHpUI(_maxHp, _hp);
                }
            }

            get => _hp;
        }
        public int Attack{set => _attack = Math.Max(0, value); get => _attack;}

        public float _jumpForce;
        public Vector2 _jumpDirection;
        public VScrollBackground vscroll;

        private float pre_Y;
        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();

            pre_Y = transform.position.y;
        }

        void Start()
        {
            InitCharacter();
        }

        protected abstract void InitCharacter();

        void FixedUpdate()
        {
            if (_rb.linearVelocityX < 0)
                _anim.transform.localScale = new Vector3(3f, _anim.transform.localScale.y);

            else if (_rb.linearVelocityX > 0)
                _anim.transform.localScale = new Vector3(-3f, _anim.transform.localScale.y);
        }
        public void JumpUp(Vector2 direction, float dragPower)
        {
            if (_isLand)
            {
                _isLand = false;
                StartCoroutine(JumpUpCoroutine(direction, dragPower));
                _anim.SetBool("IsLand", _isLand);
            }
        }

        private IEnumerator JumpUpCoroutine(Vector2 direction, float dragPower)
        {
            yield return new WaitForSeconds(0.3f);
            _rb.linearVelocity = direction * _jumpForce * dragPower;
        }

        protected void JumpLand()
        {
            if (!_isLand)
            {
                _isLand = true;
                _rb.linearVelocity = Vector2.zero;

                _anim.SetBool("IsLand", _isLand);

                if (pre_Y < transform.position.y)
                {
                    GameManager.Instance.GameScore++;
                    vscroll.MoveToY(transform.position.y);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ground"))
            {
                // 위에서 충돌할 때만 착지 실행
                if (col.contacts[0].normal.y > 0.5f)
                    JumpLand();
            }
        }
    }
}

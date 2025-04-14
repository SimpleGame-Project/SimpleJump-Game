using UnityEngine;

namespace Jang
{
    public class CameraFollow : MonoBehaviour
    {
        private Transform _playerT;
        public Vector3 _offSet;

        void Awake()
        {
            _playerT = GameObject.FindWithTag("Player").GetComponent<Transform>();
            transform.position = _playerT.position;
        }
        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, _playerT.position.y, 0) + _offSet, Time.fixedDeltaTime * 20f);
        }

    }
}

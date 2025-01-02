using UnityEngine;

namespace Game.UI.Views
{
    public class HealthAffector : MonoBehaviour
    {
        private Vector2 _lastPosition;
        private Rigidbody2D _rb;

        [SerializeField]
        private Vector2 _contactVelocityLimit = new Vector2(0.4f, 0.4f);
        [SerializeField]
        private int _power;

        public int Power { get { return _power; } }
        public Vector2 LastPosition { get { return _lastPosition; } }
        public Rigidbody2D Rb { get { return _rb; } }
        public Vector2 ContactVelocityLimit { get { return _contactVelocityLimit; } }

        void Start()
        {
            _rb = GetComponentInChildren<Rigidbody2D>();
            _lastPosition = _rb.position;
        }

        void FixedUpdate()
        {
            _lastPosition = _rb.position;
        }
    }
}
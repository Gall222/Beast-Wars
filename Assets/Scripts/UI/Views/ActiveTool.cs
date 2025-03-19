using UnityEngine;

namespace Game.UI.Views
{
    public class ActiveTool : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _toolPosition = new Vector3(0.4f, -0.8f, 0f);
        [SerializeField]
        private Vector3 _handPosition = new Vector3(0f, -1.1f, 10f);

        private bool _isTouchingAnotherObject;
        private Collider2D _collider;
        public Vector3 ToolPosition { get { return _toolPosition; } }
        public Vector3 HandPosition { get { return _handPosition; } }

        public bool IsTouchingAnotherObject { get { return _isTouchingAnotherObject; } }

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            _isTouchingAnotherObject = _collider.IsTouchingLayers(-1);
        }
    }
}
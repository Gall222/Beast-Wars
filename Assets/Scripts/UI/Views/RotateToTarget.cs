using UnityEngine;

namespace Game.UI.Views
{
    public class RotateToTarget : MonoBehaviour
    {
        public float rotationSpeed;
        private Vector2 direction;
        public float moveSpeed;

        void Start()
        {

        }

        void Update()
        {
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector2.MoveTowards(transform.position, cursorPos, moveSpeed * Time.deltaTime);
        }
    }
}
using UnityEngine;

namespace Components
{
    public struct MoveComponent
    {
        public float moveSpeed;
        public float jumpPower;
        public bool isMoving;
        public Transform transform;
        public bool jumpButtonPressed;
        public float moveDirection;
        public float faceDirection;
    }
}
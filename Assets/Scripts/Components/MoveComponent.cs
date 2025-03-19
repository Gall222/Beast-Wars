using UnityEngine;

namespace Game.Components
{
    public struct MoveComponent
    {
        public float moveSpeed;
        public bool isMoving;
        public Transform transform;
        public float moveDirection;
        public float faceDirection;
        public float maxSpeed;
    }
}
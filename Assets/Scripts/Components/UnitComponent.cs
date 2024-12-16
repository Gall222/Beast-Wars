using UnityEngine;

namespace Components
{
    public struct UnitComponent
    {
        public BoxCollider2D footCollider;
        public Transform transform;
        public Rigidbody2D rb;
        public Animator animator;
        public bool isJumped;
        public states state;

        public enum states
        {
            Idle,
            Moving,
            BackMoving,
            Jumping,
            Flying,
            Landing,
        };
    }
}

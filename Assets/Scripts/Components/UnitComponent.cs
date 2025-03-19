using Game.UI.Views;
using UnityEngine;

namespace Game.Components
{
    public struct UnitComponent
    {
        public BoxCollider2D footCollider;
        public Collider2D bodyCollider;
        public Transform transform;
        public Rigidbody2D rb;
        public Animator animator;
        public Unit view;
        public float jumpPower;
        public bool jumpButtonPressed;
        public bool isTouchingGround;
        public bool isForwardMove; 
        public bool isBackMove;

        public states state;

        public enum states
        {
            Idle,
            Moving,
            BackMoving,
            PrepareJump,
            Jumping,
            Flying,
            Landing,
        };
    }
}

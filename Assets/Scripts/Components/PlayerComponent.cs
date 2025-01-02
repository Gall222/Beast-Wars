using Game.UI.Views;
using UnityEngine;

namespace Game.Components
{
    public struct PlayerComponent
    {
        public Player view;
        public Collider2D bodyCollider;
        public BoxCollider2D footCollider;
        public CircleCollider2D headCollider;
        public AudioSource audioSource;
        public float jumpPower;

        public bool leftMouseDown;
        public float maxHandDistance;
        public Vector3 handInitialPosition;
        public float handMoveSpeed;
        public Weapon currentWeapon;
    }
}
using Game.UI.Views;
using UnityEngine;

namespace Game.Components
{
    public struct HealthComponent {
        public float maxHp;
        public float currentHp;
        public Collider2D collider;
        public Health view;
        public bool isDead;
    }
}
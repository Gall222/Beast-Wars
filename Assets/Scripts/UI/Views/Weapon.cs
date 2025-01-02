using UnityEngine;

namespace Game.UI.Views
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private Collider2D _bodyCollider;

        public Collider2D BodyCollider { get { return _bodyCollider; } }
    }
}
using Game.UI.Views;
using UnityEngine;

namespace Game.Components
{
    public struct ToolComponent {
        public ActiveTool view;
        public Rigidbody2D rb;
        public Collider2D collider;
    }
}
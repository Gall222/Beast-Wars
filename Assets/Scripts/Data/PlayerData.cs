using Game.UI.Views;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject
    {
        public GameObject playerPref;
        public float moveSpeed = 15f;
        public float backMoveSpeedSlowCoef = 2f;
        public float flySpeedSlowCoef = 5f;
        public float jumpPower = 10f;
        public ActiveTool weapon;
        public float maxHandDistance = 1.5f;
        public float handMoveSpeed = 10f;
        public PhysicsMaterial2D noFrictionMaterial;
        public PhysicsMaterial2D normalFrictionMaterial;
    }
}
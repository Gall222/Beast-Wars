using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject
    {
        public GameObject playerPref;
        public float moveSpeed = 15f;
        public float jumpPower = 10f;
        public GameObject weapon;
        public float maxHandDistance = 1.5f;
        public float handMoveSpeed = 10f;
    }
}
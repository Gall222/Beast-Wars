using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu]
    public class WorldData : ScriptableObject
    {
        public GameObject testPref;
        //public float boost = 500f;
        public float speed = 1500f;
        public float jumpSpeed = 2000f;
    }
}
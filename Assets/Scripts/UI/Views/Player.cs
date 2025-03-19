using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Views
{
    public class Player : MonoBehaviour
    {
        [Header("Stuff")]
        public GameObject weapon;
        [Header("Hand")]
        [SerializeField]
        public GameObject handSprites;
        [SerializeField]
        private GameObject _stuffPlace;
        [Header("Body parts")]
        [SerializeField]
        private SpriteRenderer _eyes;
        [Header("Other")]
        [SerializeField]
        private GameObject _hp;

        public GameObject StuffPlace { get { return _stuffPlace; } }
        public SpriteRenderer Eyes { get { return _eyes; } }
        public GameObject Hp { get { return _hp; } }
    }
}
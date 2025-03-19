using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Views
{
    public class Health : MonoBehaviour
    {
        [Header("Parts wich will activate in death")]
        [SerializeField]
        private List<GameObject> _dethParts;
        [SerializeField]
        private GameObject _backHpIndicator;
        [SerializeField]
        private GameObject _hpIndicator;
        [SerializeField]
        private float _maxHp = 100f;
        private SpriteRenderer _spriteRendererHpIndicator;
        public bool isTouched = false;
        public Collision2D affectorCollision;
        private Animator _animator;

        public List<GameObject> DethParts { get { return _dethParts; } }
        public GameObject BackHpIndicator { get { return _backHpIndicator; } }
        public GameObject HpIndicator { get { return _hpIndicator; } }
        public SpriteRenderer SpriteRendererHpIndicator { get { return _spriteRendererHpIndicator; } }
        public float MaxHp { get { return _maxHp; } }

        public void Awake()
        {
            if(_hpIndicator != null)
            {
                _spriteRendererHpIndicator = _hpIndicator.GetComponent<SpriteRenderer>();
            }
            _animator = GetComponent<Animator>();
        }
        public void OnCollisionEnter2D(Collision2D collision)
        {
            affectorCollision = collision;
            isTouched = true;
            //Debug.Log(gameObject.name + " " + collision.gameObject.name);
        }
        public void SetHpIndicatorSizeAndColor(Vector2 indicatorCize, Color color)
        {
            _hpIndicator.transform.localScale = indicatorCize;
            _spriteRendererHpIndicator.color = color;
        }
    }
}
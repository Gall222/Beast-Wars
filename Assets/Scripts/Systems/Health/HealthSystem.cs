using Game.Components;
using Game.UI.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems.Health
{
    public class HealthSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<HealthComponent>> _healthFilter = default;
        public void Run (IEcsSystems systems) {
            foreach (var entity in _healthFilter.Value)
            {
                ref var healthComponent = ref _healthFilter.Pools.Inc1.Get(entity);
                var view = healthComponent.view;

                if (view.isTouched && healthComponent.isDead == false)
                {
                    var sprite = healthComponent.view.affectorCollision.gameObject;
                    HealthAffector healthAffector = sprite.GetComponentInParent<HealthAffector>();
                    
                    if (healthAffector != null && IsTouchReached(healthAffector))
                    {
                        healthComponent.currentHp -= healthAffector.Power;

                        if (healthComponent.currentHp <= 0)
                        {
                            Die(ref healthComponent, entity);
                        }
                        ChangeVisualHP(ref healthComponent);
                    }
                    view.isTouched = false;
                }
            }
        }

        private void Die(ref HealthComponent healthComponent, int entity)
        {
            healthComponent.isDead = true;
            DisableColliders(ref healthComponent);
            DisableAnimator(ref healthComponent);
            ActivateDeathParts(ref healthComponent);
            FreezePersonParts(ref healthComponent);
        }

        private void DisableColliders(ref HealthComponent healthComponent)
        {
            var bodyColliders = healthComponent.view.gameObject.GetComponents<Collider2D>();
            foreach (var collider in bodyColliders)
            {
                collider.enabled = false;
            }
        }

        private void DisableAnimator(ref HealthComponent healthComponent)
        {
            var animator = healthComponent.view.gameObject.GetComponent<Animator>();
            if (animator != null) { animator.enabled = false; }
        }

        private void ActivateDeathParts(ref HealthComponent healthComponent)
        {
            foreach (var part in healthComponent.view.DethParts)
            {
                var collider = part.GetComponent<Collider2D>();
                var rb = part.GetComponent<Rigidbody2D>();
                if (collider != null) { collider.enabled = true; }
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                    rb.interpolation = RigidbodyInterpolation2D.Interpolate;
                }
            }
        }

        private void FreezePersonParts(ref HealthComponent healthComponent)
        {
            var person = healthComponent.view.gameObject.GetComponent<Player>();
            if (person == null) { return; }
            
            person.handSprites.transform.SetParent(healthComponent.view.DethParts[0].transform);
            var joint = person.handSprites.AddComponent<HingeJoint2D>();
            joint.connectedBody = healthComponent.view.GetComponent<Rigidbody2D>();
            person.handSprites.transform.localPosition = Vector3.zero;
            person.Eyes.enabled = true;
            Object.Destroy(person.Hp);
        }

        private bool IsTouchReached(HealthAffector healthAffector)
        {
            if(healthAffector.isActive == false) return false;

            var affectorVelocity = healthAffector.Rb.position - healthAffector.LastPosition;

            return affectorVelocity.x >= healthAffector.ContactVelocityLimit.x || 
                affectorVelocity.y >= healthAffector.ContactVelocityLimit.y;
        }

        public void ChangeVisualHP(ref HealthComponent healthComponent)
        {
            var backHpIndicator = healthComponent.view.BackHpIndicator;
            var hpIndicator = healthComponent.view.HpIndicator;
            Color color = healthComponent.view.SpriteRendererHpIndicator.color;

            if (backHpIndicator == null || hpIndicator == null) { return; }

            if (healthComponent.currentHp <= healthComponent.maxHp / 2 && 
                healthComponent.currentHp > healthComponent.maxHp / 3)
                color = Color.yellow;
            else if (healthComponent.currentHp <= healthComponent.maxHp / 3)
                color = Color.red;

            float scalePersent = healthComponent.currentHp * backHpIndicator.transform.localScale.x / 
                healthComponent.maxHp;

            Vector2 newSize = new Vector2(scalePersent, hpIndicator.transform.localScale.y);
            healthComponent.view.SetHpIndicatorSizeAndColor(newSize, color);
        }
    }
}
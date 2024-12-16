using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Systems
{
    sealed class AnimationSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<AnimationComponent, ChangeAnimationComponent>> _animationFilter = default;
        readonly EcsPoolInject<ChangeAnimationComponent> _changeAnimationPool = default;
        public void Run (IEcsSystems systems) {
            foreach (var entity in _animationFilter.Value)
            {
                ref var animationComponent = ref _animationFilter.Pools.Inc1.Get(entity);
                ref var changeAnimationComponent = ref _animationFilter.Pools.Inc2.Get(entity);

                animationComponent.animator.StopPlayback();
                animationComponent.animator.CrossFade(
                    changeAnimationComponent.animationName, 
                    changeAnimationComponent.duration
                    );
                _changeAnimationPool.Value.Del(entity);
            }
        }
    }
}
using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Game.Systems;
using UnityEngine;

namespace Game.Systems.Animation
{
    public class AnimationSystem : IEcsRunSystem {
        //readonly EcsFilterInject<Inc<AnimationComponent, ChangeAnimationComponent>> _animationFilter = default;
        //readonly EcsPoolInject<ChangeAnimationComponent> _changeAnimationPool = default;
        public void Run (IEcsSystems systems)
        {
            //ChangeAnimation();
        }
    }
}
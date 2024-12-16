using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Systems.Player
{
    sealed class HandControlSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<PlayerComponent, UnitComponent, MoveComponent>> _playerFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _playerFilter.Value)
            {
                ref var playerComponent = ref _playerFilter.Pools.Inc1.Get(entity);
                ref var unitComponent = ref _playerFilter.Pools.Inc2.Get(entity);

                var hand = playerComponent.view.hand;

                if (playerComponent.leftMouseDown)
                {
                    // ѕолучаем позицию мыши
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0; // ”бедитесь, что z равно 0

                    // ¬ычисл€ем направление от родител€ руки до позиции мыши
                    Vector3 direction = mousePosition - hand.transform.parent.position;

                    // ѕровер€ем длину вектора направлени€
                    if (direction.magnitude > playerComponent.maxHandDistance)
                    {
                        direction.Normalize(); // Ќормализуем вектор
                        direction *= playerComponent.maxHandDistance; // ”множаем на максимальное рассто€ние
                    }
                        // ѕереводим направление в локальные координаты относительно bone_1
                        Vector3 localDirection = hand.transform.parent.InverseTransformDirection(direction);
                    
                        // –ассчитываем новую позицию дл€ руки с использованием Lerp дл€ плавного движени€
                        Vector3 targetPosition = playerComponent.handInitialPosition + localDirection;
                        hand.transform.localPosition = Vector3.Lerp(hand.transform.localPosition, targetPosition, Time.deltaTime * playerComponent.handMoveSpeed);
                        // ¬ычисл€ем угол дл€ вращени€ руки
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // ѕолучаем угол в градусах
                                                                                             // ”читываем зеркальное отражение персонажа
                        if (unitComponent.transform.localScale.x < 0)
                        {
                            angle += 180f; // »нвертируем угол на 180 градусов дл€ зеркального отражени€
                        }
                        // ”станавливаем вращение руки
                        hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }
                else
                {
                    // ≈сли кнопка не нажата, возвращаем руку в начальное положение
                    hand.transform.localPosition = playerComponent.handInitialPosition;

                    // ¬ращение можно вернуть в исходное состо€ние, если это необходимо
                    hand.transform.rotation = Quaternion.identity; // »ли установите нужное начальное вращение
                }
            }
        }
    }
}
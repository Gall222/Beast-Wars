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
                    // �������� ������� ����
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0; // ���������, ��� z ����� 0

                    // ��������� ����������� �� �������� ���� �� ������� ����
                    Vector3 direction = mousePosition - hand.transform.parent.position;

                    // ��������� ����� ������� �����������
                    if (direction.magnitude > playerComponent.maxHandDistance)
                    {
                        direction.Normalize(); // ����������� ������
                        direction *= playerComponent.maxHandDistance; // �������� �� ������������ ����������
                    }
                        // ��������� ����������� � ��������� ���������� ������������ bone_1
                        Vector3 localDirection = hand.transform.parent.InverseTransformDirection(direction);
                    
                        // ������������ ����� ������� ��� ���� � �������������� Lerp ��� �������� ��������
                        Vector3 targetPosition = playerComponent.handInitialPosition + localDirection;
                        hand.transform.localPosition = Vector3.Lerp(hand.transform.localPosition, targetPosition, Time.deltaTime * playerComponent.handMoveSpeed);
                        // ��������� ���� ��� �������� ����
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // �������� ���� � ��������
                                                                                             // ��������� ���������� ��������� ���������
                        if (unitComponent.transform.localScale.x < 0)
                        {
                            angle += 180f; // ����������� ���� �� 180 �������� ��� ����������� ���������
                        }
                        // ������������� �������� ����
                        hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }
                else
                {
                    // ���� ������ �� ������, ���������� ���� � ��������� ���������
                    hand.transform.localPosition = playerComponent.handInitialPosition;

                    // �������� ����� ������� � �������� ���������, ���� ��� ����������
                    hand.transform.rotation = Quaternion.identity; // ��� ���������� ������ ��������� ��������
                }
            }
        }
    }
}
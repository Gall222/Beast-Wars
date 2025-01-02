using Leopotam.EcsLite.Di;
using Leopotam.EcsLite;
using UnityEngine;
using Game.Components;
using Game.Data;
using Game.UI.Views;

namespace Game.Systems.PlayerControl
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        readonly EcsCustomInject<SceneDataComponent> _sceneData = default;
        readonly EcsCustomInject<StaticData> _staticData = default;
        readonly EcsPoolInject<UnitComponent> _unitPool = default;
        readonly EcsPoolInject<PlayerComponent> _playerComponentPool = default;
        readonly EcsPoolInject<MoveComponent> _moveComponentPool = default;
        readonly EcsPoolInject<WeaponComponent> _weaponComponentPool = default;
        readonly EcsPoolInject<HealthComponent> _healthComponentPool = default;

        public void Init(IEcsSystems systems)
        {
            var playerEntity = _unitPool.Value.GetWorld().NewEntity();

            ref var unit = ref _unitPool.Value.Add(playerEntity);
            ref var playerComponent = ref _playerComponentPool.Value.Add(playerEntity);
            ref var moveComponent = ref _moveComponentPool.Value.Add(playerEntity);
            //ref var healthComponent = ref _healthComponentPool.Value.Add(playerEntity);
            
            var playerPrefab = _staticData.Value.playerData.playerPref;
            var playerObject = GameObject.Instantiate(_staticData.Value.playerData.playerPref,
                _sceneData.Value.playerSpawnPosition.position, Quaternion.identity.normalized);
            moveComponent.moveSpeed = _staticData.Value.playerData.moveSpeed;
            moveComponent.jumpPower = _staticData.Value.playerData.jumpPower;
            moveComponent.isMoving = false;

            unit.transform = playerObject.transform;
            unit.rb = playerObject.GetComponent<Rigidbody2D>();
            unit.footCollider = playerObject.GetComponent<BoxCollider2D>();
            unit.animator = playerObject.GetComponent<Animator>();
            _sceneData.Value.playerCamera.Follow = playerObject.GetComponentInChildren<Transform>();

            playerComponent.view = playerObject.GetComponent<Player>();

            //healthComponent.view = playerObject.GetComponent<Health>();

            HandInitialize(ref playerComponent);
            Test();
        }

        private void HandInitialize(ref PlayerComponent playerComponent)
        {
            var weapon = GameObject.Instantiate(_staticData.Value.playerData.weapon,
                playerComponent.view.StuffPlace.transform.position, Quaternion.identity.normalized);
            weapon.transform.SetParent(playerComponent.view.StuffPlace.transform, true);
            //var weaponView = weapon.GetComponent<Weapon>();
            //weaponView.hand = playerComponent.view.hand;
            //weapon.GetComponent<SpriteRenderer>().sortingOrder = 8;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.rotation = new Quaternion();
            //weapon.transform.localRotation = Quaternion.Euler(new Vector3(0,0,180));
            playerComponent.handInitialPosition = playerComponent.view.hand.transform.localPosition;
            playerComponent.maxHandDistance = _staticData.Value.playerData.maxHandDistance;
            playerComponent.handMoveSpeed = _staticData.Value.playerData.handMoveSpeed;

            var weaponEntity = _weaponComponentPool.Value.GetWorld().NewEntity();
            ref var weaponComponent = ref _weaponComponentPool.Value.Add(weaponEntity);
            weaponComponent.view = weapon.GetComponent<Weapon>();

            playerComponent.currentWeapon = weaponComponent.view;
        }

        private void Test()
        {
            var testEntity = _unitPool.Value.GetWorld().NewEntity();
            var test = GameObject.Instantiate(_staticData.Value.worldData.testPref,
                _sceneData.Value.playerSpawnPosition.position + new Vector3(3,3,0), Quaternion.identity.normalized);

            ref var healthComponent = ref _healthComponentPool.Value.Add(testEntity);
            healthComponent.view = test.GetComponent<Health>();
            healthComponent.currentHp = healthComponent.maxHp = healthComponent.view.MaxHp;
        }
    }
}

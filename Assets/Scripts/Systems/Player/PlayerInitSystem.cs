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
        readonly EcsPoolInject<ToolComponent> _toolComponentPool = default;
        readonly EcsPoolInject<HealthComponent> _healthComponentPool = default;

        public void Init(IEcsSystems systems)
        {
            var playerEntity = _unitPool.Value.GetWorld().NewEntity();

            ref var unit = ref _unitPool.Value.Add(playerEntity);
            ref var playerComponent = ref _playerComponentPool.Value.Add(playerEntity);
            ref var moveComponent = ref _moveComponentPool.Value.Add(playerEntity);
            //ref var healthComponent = ref _healthComponentPool.Value.Add(playerEntity);
            var spawnPosition = new Vector2(_sceneData.Value.verticalSize, _sceneData.Value.horizontalSize);
            var playerPrefab = _staticData.Value.playerData.playerPref;
            var playerObject = GameObject.Instantiate(_staticData.Value.playerData.playerPref,
                spawnPosition, Quaternion.identity.normalized);
            moveComponent.moveSpeed = _staticData.Value.playerData.moveSpeed;
            moveComponent.isMoving = false;
            moveComponent.maxSpeed = 5f;

            unit.transform = playerObject.transform;
            unit.jumpPower = _staticData.Value.playerData.jumpPower;
            unit.rb = playerObject.GetComponent<Rigidbody2D>();
            unit.footCollider = playerObject.GetComponent<BoxCollider2D>();
            unit.bodyCollider = playerObject.GetComponent<CapsuleCollider2D>();
            unit.animator = playerObject.GetComponent<Animator>();
            unit.view = playerObject.GetComponent<Unit>();
            _sceneData.Value.playerCamera.Follow = playerObject.GetComponentInChildren<Transform>();

            playerComponent.view = playerObject.GetComponent<Player>();

            //healthComponent.view = playerObject.GetComponent<Health>();

            HandInitialize(ref playerComponent);
            Test();
        }

        private void HandInitialize(ref PlayerComponent playerComponent)
        {
            var tool = GameObject.Instantiate(_staticData.Value.playerData.weapon,
                playerComponent.view.StuffPlace.transform.position, Quaternion.identity.normalized);
            tool.transform.SetParent(playerComponent.view.StuffPlace.transform.parent.transform, true);
            playerComponent.view.handSprites.transform.SetParent(tool.transform, true);
            
            tool.transform.localPosition = (Vector2)tool.ToolPosition;
            tool.transform.Rotate(new Vector3(0, 0, tool.ToolPosition.z));

            playerComponent.toolInitialPosition = tool.transform.localPosition;
            playerComponent.maxHandDistance = _staticData.Value.playerData.maxHandDistance;
            playerComponent.handMoveSpeed = _staticData.Value.playerData.handMoveSpeed;

            playerComponent.view.handSprites.transform.localPosition = (Vector2)tool.HandPosition;
            playerComponent.view.handSprites.transform.Rotate(new Vector3(0, 0, tool.HandPosition.z));

            var toolEntity = _toolComponentPool.Value.GetWorld().NewEntity();
            ref var toolComponent = ref _toolComponentPool.Value.Add(toolEntity);
            toolComponent.view = tool;
            toolComponent.rb = tool.GetComponent<Rigidbody2D>();
            toolComponent.collider = tool.GetComponent<Collider2D>();
            Debug.Log(playerComponent.toolInitialPosition);
            playerComponent.currentTool = toolComponent;
            //playerComponent.handAndStuffPositionOffset =
            //    playerComponent.view.handSprites.transform.position - tool.transform.position;
            //var joint = tool.gameObject.AddComponent<FixedJoint2D>();
            //joint.connectedBody = playerComponent.view.hand.GetComponent<Rigidbody2D>();
            //joint.autoConfigureConnectedAnchor = false;
        }

        private void Test()
        {
            var testEntity = _unitPool.Value.GetWorld().NewEntity();
            var test = GameObject.Instantiate(_staticData.Value.worldData.testPref,
                new Vector2(_sceneData.Value.verticalSize, _sceneData.Value.horizontalSize) + new Vector2(3, 3),
                Quaternion.identity.normalized);

            ref var healthComponent = ref _healthComponentPool.Value.Add(testEntity);
            healthComponent.view = test.GetComponent<Health>();
            healthComponent.currentHp = healthComponent.maxHp = healthComponent.view.MaxHp;
        }
    }
}

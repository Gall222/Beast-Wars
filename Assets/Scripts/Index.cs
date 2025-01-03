using Game.Data;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Game.Systems.PlayerControl;
using UnityEngine;
using Game.Systems.Move;
using Systems;
using Game.Systems.Health;
using Game.Systems.Animation;
using Game.UI.Views;

namespace Game
{
    public class Index : MonoBehaviour
    {
        public StaticData staticData;
        public SceneDataComponent sceneData;
        public PlayerInput playerInput;
        EcsWorld world;
        EcsSystems updateSystems;
        EcsSystems fixedUpdateSystems;

        void Start()
        {
            world = new EcsWorld();
            updateSystems = new EcsSystems(world);
            fixedUpdateSystems = new EcsSystems(world);
            playerInput = new PlayerInput();
            playerInput.Enable();

            updateSystems
                .Add(new GameInitSystem())
                .Add(new PlayerInitSystem())
                .Add(new PlayerInputSystem())
                .Add(new HealthSystem())
                .Add(new AnimationSystem())
                .Inject(staticData, sceneData, playerInput);

            fixedUpdateSystems
                .Add(new HandControlSystem())
                .Add(new PlayerRotationSystem())
                .Add(new MoveSystem())
                //.Add(new UnitStateSystem())
                .Inject(staticData, sceneData, playerInput);

            updateSystems.Init();
            fixedUpdateSystems.Init();

        }
        void FixedUpdate()
        {
            fixedUpdateSystems?.Run();
        }

        void Update()
        {
            updateSystems?.Run();
        }

        private void OnDestroy()
        {
            playerInput.Disable();
            updateSystems.Destroy();
            fixedUpdateSystems.Destroy();
            world.Destroy();
        }
    }
}
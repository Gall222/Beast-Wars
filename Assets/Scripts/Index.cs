using Game.Data;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Game.Systems.PlayerControl;
using UnityEngine;
using Game.Systems.Move;
using Game.Systems.Health;
using Game.Systems.Animation;
using Game.UI.Views;
using Game.Management;
using Game.Systems.ButtonControl;
using Game.Systems.World;

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

            var buttonManager = new ButtonManager();
            
            updateSystems
                .Add(buttonManager)
                .Add(new GameInitSystem())
                .Add(new PlayerInitSystem())
                .Add(new PlayerInputSystem())
                .Add(new HealthSystem())
                .Add(new AnimationSystem())
                .Add(new BuildSystem())
                .Add(new ForegroundSystem())

                .Inject(staticData, sceneData, playerInput, buttonManager);

            fixedUpdateSystems
                .Add(new HandControlSystem())
                .Add(new PlayerRotationSystem())
                .Add(new MoveSystem())
                //.Add(new UnitStateSystem())
                .Inject(staticData, sceneData, playerInput, buttonManager);


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
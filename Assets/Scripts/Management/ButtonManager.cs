using Game.Components;
using Game.Data;
using Game.UI.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using static Game.Data.ButtonDictionary;

namespace Game.Management
{
    public class ButtonManager : IEcsInitSystem
    {
        private ButtonDictionary _buttonDictionary;
        private ClickController _clickController;

        readonly EcsCustomInject<SceneDataComponent> _sceneData = default;
        readonly EcsCustomInject<StaticData> _staticData = default;
        readonly EcsPoolInject<BuildComponent> _buildComponentPool = default;
        readonly EcsPoolInject<HealthComponent> _healthComponentPool = default;

        public void SetClick(ClickController.ClickTypes type) { _clickController.currentClick = type; }
        public ClickController.ClickTypes GetClick() { return _clickController.currentClick; }


        public void Init(IEcsSystems systems)
        {
            _buttonDictionary = new ButtonDictionary();
            _clickController = new ClickController();
            _buttonDictionary.Init(BuildButton);
        }
        
        public void CreateButton(ButtonDictionary.SkillTypes skillType, ButtonDictionary.SkillNames skillName)
        {
            var buttonView = GameObject.Instantiate(_staticData.Value.UIData.buttonPrefab,
                Vector3.zero, Quaternion.identity.normalized);
            buttonView.transform.parent = _sceneData.Value.skillPanel.transform;
            var button = buttonView.gameObject.GetComponent<Button>();
            
            if (_buttonDictionary.SkillTypesList.TryGetValue(skillType, out SkillType skill))
            {
                button.onClick.AddListener(() => skill(skillName));
            }
            
            buttonView.description.text = _buttonDictionary.SkillsDescriptionList[skillType];
        }

        private void BuildButton(ButtonDictionary.SkillNames skillName)
        {
            if (_clickController.currentClick != ClickController.ClickTypes.None) { return; }

            _clickController.currentClick = ClickController.ClickTypes.Skill;

            var buildEntity = _buildComponentPool.Value.GetWorld().NewEntity();
            var buildObject = GameObject.Instantiate(
                _buttonDictionary.BuildPrefabsList[skillName],
                Vector3.zero, Quaternion.identity.normalized);
            
            ref BuildComponent buildComponent = ref _buildComponentPool.Value.Add(buildEntity);
            buildComponent.gameObject = buildObject;
            buildComponent.view = buildObject.GetComponent<Building>();
            buildComponent.sprite = buildObject.GetComponent<SpriteRenderer>();
            
            ref var healthComponent = ref _healthComponentPool.Value.Add(buildEntity);
            healthComponent.view = buildObject.GetComponent<Health>();
            healthComponent.currentHp = healthComponent.maxHp = healthComponent.view.MaxHp;

            var healthAffector = buildObject.GetComponent<HealthAffector>();
            if (healthAffector != null) { healthAffector.isActive = false; }

            Debug.Log("Кнопка Build нажата!");
        }
    }
}
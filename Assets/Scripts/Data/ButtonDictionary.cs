using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Data
{
    public class ButtonDictionary
    {
        private Dictionary<SkillTypes, string> _skillsDescriptionList;
        private Dictionary<SkillTypes, SkillType> _skillTypesList;
        private Dictionary<SkillNames, GameObject> _buildPrefabsList;
        //public readonly Dictionary<SkillNames, Skill> skills;
        
        public Dictionary<SkillTypes, string> SkillsDescriptionList { get { return _skillsDescriptionList; } }
        public Dictionary<SkillTypes, SkillType> SkillTypesList { get { return _skillTypesList; } }
        public Dictionary<SkillNames, GameObject> BuildPrefabsList { get { return _buildPrefabsList; } }

        public delegate void SkillType(ButtonDictionary.SkillNames skillName);
        public delegate void Skill();
        public enum SkillTypes
        {
            Build
        }

        public enum SkillNames
        {
            Stone
        }

        public void Init(SkillType BuildButton) 
        {
            var buildingData = Resources.Load<BuildingData>("Building Data");

            _skillTypesList = new Dictionary<SkillTypes, SkillType>()
            {
                {SkillTypes.Build, BuildButton}
            };

            _skillsDescriptionList = new Dictionary<SkillTypes, string>()
            {
                {SkillTypes.Build, "Build"}
            };
            
            _buildPrefabsList = new Dictionary<SkillNames, GameObject>()
            {
                {SkillNames.Stone, buildingData.stone}
            };
        }
    }
}
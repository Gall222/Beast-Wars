using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using Unity.Cinemachine;
using static PlasticGui.WorkspaceWindow.Items.LockRules.LockRuleGenerator;

namespace Game.UI.Views
{
    public class SceneDataComponent : MonoBehaviour
    {
        //private int _score = 0;
        private float _volume = 0.5f;
        [Header("World")]
        public Canvas canvas;
        public RuleTile groundTile;
        public RuleTile backgroundTile;
        public Tilemap background;
        public Foreground foreground;
        public GameObject skillPanel;
        [Header("Camera")]
        public CinemachineVirtualCameraBase playerCamera;
        [Header("Player")]
        //public PlayerComponent playerComponent;
        public Transform playerSpawnPosition;
        [Header("Map Settings")]
        public int horizontalSize = 100;
        public int verticalSize = 40;
        public float emptyTileChance = 2;
        public float caveChance = 70;
        //public CinemachineVirtualCamera virtualCamera;
        //[Header("UI")]
        //public TextMeshProUGUI scoreText;

        public float Volume
        {
            get => _volume;
            set { _volume = value; }
        }
        //public int Score { 
        //    get => _score;
        //    set { _score = value; scoreText.text = _score.ToString(); }   
        //}
        //public int HarvestInBasket
        //{
        //    get => _harvestInBasket;
        //    set { _harvestInBasket = value; basketSizeText.text = $"{_harvestInBasket}/{maxHarvestInBasket}"; }
        //}
        private void Start()
        {
            //scoreText.text = _score.ToString();
            //basketSizeText.text = $"{_harvestInBasket}/{maxHarvestInBasket}";
        }
        //public void StartCutting()
        //{
        //        startCutting = true;
        //}

    }
}
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SGJ.UI
{
    public class GUIPlayerInfo : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI playerNameLabel;
        [SerializeField] 
        private TextMeshProUGUI playerFatValueLabel;
        [SerializeField] 
        private TextMeshProUGUI playerFatLevelLabel;
        [SerializeField]
        Image playerBackgroundBorder; 
        [SerializeField] 
        private GameObject readyIndicator;
        [SerializeField] 
        private GameObject notReadyIndicator;

        private GameObject source;
        private PenguinController penguin;
        private PlayerInput playerInput;
        
        public GameObject Source
        {
            set 
            { 
                source = value;

                if(penguin != null)
                {
                    penguin.FatLevelChanged -= OnFatLevelChanged;
                    penguin.FatValueChanged -= OnFatValueChanged;
                }
                
                penguin = source.GetComponent<PenguinController>();
                playerInput = source.GetComponent<PlayerInput>();
                
                penguin.FatLevelChanged += OnFatLevelChanged;
                penguin.FatValueChanged += OnFatValueChanged;
                
                RenderData(); 
            }
            get
            {
                return source;
            }
        }

        void OnEnable()
        {
            readyIndicator.SetActive(false);
            notReadyIndicator.SetActive(true);
        }

        public void RenderData()
        {
            playerNameLabel.text = $"P{playerInput.PlayerId.ToString()}";

            var newColor = playerInput.IndicatorColor;
            newColor.a = 1f;
            playerNameLabel.color = newColor;

            playerBackgroundBorder.color = newColor;

            OnFatLevelChanged(penguin.CurrentFatLevel);
            OnFatValueChanged(penguin.CurrentFatValue);
        }

        void OnFatLevelChanged(int newLevel)
        {
            playerFatLevelLabel.text = $"LV {newLevel.ToString()}";
        }
        
        void OnFatValueChanged(float newValue)
        {
            playerFatValueLabel.text = $"{newValue.ToString()}/{penguin.FatValueToReach.ToString()}";
        }

        public void OnPlayerReady(object value)
        {
            if (Source && Source.Equals((GameObject) value))
            {
                readyIndicator.SetActive(true);
                notReadyIndicator.SetActive(false);
                playerFatValueLabel.gameObject.SetActive(true);
            }
        }

        public void OnPlayerDataChanged(object value)
        {
            if (Source && Source.Equals((GameObject) value))
            {
                RenderData();
            }
        }

        public void OnMatchStarted(object value)
        {
            readyIndicator.SetActive(false);
        }
    }
    

}

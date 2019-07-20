using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

namespace SGJ.UI
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI PlayerName;
        [SerializeField] private TextMeshProUGUI PlayerFatDetails;
        [SerializeField] private GameObject ReadyIndicator;

        [SerializeField] private GameObject NotReadyIndicator;

        private GameObject source;
        private PenguinController penguin;
        private PlayerInput playerInput;
        
        public GameObject Source
        {
            set { 
                source = value;
                penguin = source.GetComponent<PenguinController>();
                playerInput = source.GetComponent<PlayerInput>();
                RenderData(); 
            }
            get { return source; }
        }

        public void RenderData()
        {
            PlayerName.text = $"Player {playerInput.PlayerId}" ;
            PlayerName.color = playerInput.IndicatorColor;
            
            var sb = new StringBuilder();
            sb.Append($"Fat Level {penguin.CurrentFatLevel.ToString()}/{penguin.MaxFatLevel.ToString()} ");
            sb.Append($"Fat Value {penguin.CurrentFatValue.ToString()}/{penguin.FatValueToReach.ToString()} ");

            PlayerFatDetails.text = sb.ToString();
        }

        public void OnPlayerReady(object value)
        {
            if (Source && Source.Equals((GameObject) value))
            {
                ReadyIndicator.SetActive(true);
                NotReadyIndicator.SetActive(false);
                PlayerFatDetails.gameObject.SetActive(true);
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
            ReadyIndicator.SetActive(false);
        }
    }
    

}

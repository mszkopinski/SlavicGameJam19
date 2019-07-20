using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DG.Tweening;
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
        RectTransform playerFatRect;
        [SerializeField]
        CanvasGroup playerFatLevelGroup;
        [SerializeField]
        RectTransform playerFatLevelRect;
        [SerializeField]
        CanvasGroup playerFatGroup;
        [SerializeField]
        float rectEndWidth = 192f;
        [SerializeField]
        AnimationCurve sizeAnimationCurve;
        [SerializeField] 
        private GameObject readyIndicator;
        [SerializeField] 
        private GameObject notReadyIndicator;

        [SerializeField] private GameObject deadIndicator;

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

            playerFatLevelRect.SetSizeDelta(0f);
            playerFatRect.SetSizeDelta(0f);

            playerFatGroup.alpha = 0f;
            playerFatLevelGroup.alpha = 0f;
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

                const float transitionTime = .23f;
                
                var fatNewSizeDelta = playerFatRect.sizeDelta;
                fatNewSizeDelta.x = rectEndWidth;
                playerFatRect
                    .DOSizeDelta(fatNewSizeDelta, transitionTime)
                    .SetEase(sizeAnimationCurve)
                    .OnComplete(() => { playerFatGroup.DOFade(1f, 0.25f); });
                
                var fatLevelNewSizeDelta = playerFatLevelRect.sizeDelta;
                fatLevelNewSizeDelta.x = rectEndWidth;
                playerFatLevelRect
                    .DOSizeDelta(fatLevelNewSizeDelta, transitionTime)
                    .SetEase(sizeAnimationCurve)
                    .OnComplete(() => { playerFatLevelGroup.DOFade(1f, 0.25f); });
                
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

        public void OnPlayerDead(object value)
        {
            if (Source && Source.Equals((GameObject) value))
            {
                deadIndicator.SetActive(true);
            }
        }
    }
    

}

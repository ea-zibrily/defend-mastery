using UnityEngine;
using UnityEngine.UI;
using Defend.Enum;
using Defend.Audio;
using DanielLochner.Assets.SimpleScrollSnap;

namespace Defend.UI
{
    public class TutorialManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Tutorial")]
        [SerializeField] private Button closeButtonUI;
        [SerializeField] private Button nextButtonUI;
        [SerializeField] private Button previousButtonUI;

        [Header("Reference")]
        [SerializeField] private SimpleScrollSnap scrollSnap;

        #endregion

        #region Methods

        private void Start()
        {
            // Setup scroll-snap
            scrollSnap.StartScrollSnap();
            scrollSnap.onPanelSelected.RemoveAllListeners();
            scrollSnap.onPanelSelected.AddListener(()=>
            {
                var targetPanel = scrollSnap.TargetPanel;
                var panelNumber = scrollSnap.NumberOfPanels - 1;

                previousButtonUI.gameObject.SetActive(targetPanel != 0);
                nextButtonUI.gameObject.SetActive(targetPanel != panelNumber);
            });

            // Setup button
            closeButtonUI.onClick.AddListener(CloseTutorial);
            nextButtonUI.onClick.AddListener(TapTutorial);
            previousButtonUI.onClick.AddListener(TapTutorial);
            
            previousButtonUI.gameObject.SetActive(scrollSnap.TargetPanel != 0);
        }

        private void CloseTutorial()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            scrollSnap.StartScrollSnap();
            gameObject.SetActive(false);
        }
        
        private void TapTutorial()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
        }

        #endregion
    }
}

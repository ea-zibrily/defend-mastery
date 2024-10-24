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
        [SerializeField] private SimpleScrollSnap scrollSnap;

        #endregion

        #region Methods

        private void Start()
        {
            scrollSnap.StartScrollSnap();
            closeButtonUI.onClick.AddListener(CloseTutorial);
        }

        private void CloseTutorial()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            scrollSnap.StartScrollSnap();
            gameObject.SetActive(false);
        }

        #endregion
    }
}

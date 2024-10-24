using UnityEngine;
using UnityEngine.UI;
using Defend.Audio;
using Defend.Enum;
using Defend.Managers;

namespace Defend.UI
{
    public class GameUI : MonoBehaviour
    {
        #region Global Fields

        [Header("UI")]
        [SerializeField] protected Button homeButtonUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            InitOnAwake();
        }

        private void Start()
        {
            InitOnStart();
        }

        #endregion

        #region Methods

        // !- Initialize
        protected virtual void InitOnAwake() { }
        protected virtual void InitOnStart()
        {
            homeButtonUI.onClick.AddListener(OnHomeButton);
        }

        // !- Core
        private void OnHomeButton()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.MainMenu);
        }

        #endregion
    }
}
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Defend.UI
{
    public class GameEndManager : MonoBehaviour
    {
        private bool isPressedReplay;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isPressedReplay)
            {
                isPressedReplay = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }
}

using UnityEngine;

public class MainMenuController : MonoBehaviour {
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject videoOptionsMenu;
    public GameObject audioOptionsMenu;
    public GameObject gameplayOptionsMenu;

    public void StartGame(string startSceneName) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(startSceneName);
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

        public void SwitchToMenu(GameObject menu) {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        videoOptionsMenu.SetActive(false);
        audioOptionsMenu.SetActive(false);
        gameplayOptionsMenu.SetActive(false);
        menu.SetActive(true);
    }
}

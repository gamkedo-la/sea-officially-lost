using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {
    public float transitionTime;
    public GameObject[] menus;
    public GameObject credits;
    private GameObject activeMenu;

    [Header("Credits")]
    public float creditsPageDuration = 4;
    public GameObject[] creditsPages;
    private bool creditsActive;
    private IEnumerator creditsCoroutine;

    private void Start() {
        activeMenu = menus[0];
    }

    private void Update() {
        if(Input.anyKey && creditsActive) {
            Camera.main.GetComponent<RippleImageEffect>().Ripple(0.5f, 0.5f);
            StopCredits();
        }
    }

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
        IEnumerator fadeCoroutine = Fade(menu);
        StartCoroutine(fadeCoroutine);
    }

    IEnumerator Fade(GameObject menu) {
        CanvasGroup canvas = activeMenu.GetComponent<CanvasGroup>();
        for(float t = transitionTime/2; t > 0; t -= Time.deltaTime) {
            canvas.alpha = Mathf.Clamp(t*2, 0, 1);
            yield return null;
        }

        activeMenu.SetActive(false);
        canvas.alpha = 1;
        activeMenu = menu;
        activeMenu.SetActive(true);
        canvas = activeMenu.GetComponent<CanvasGroup>();

        for (float t = 0; t < transitionTime; t += Time.deltaTime) {
            canvas.alpha = Mathf.Clamp(t * 2, 0, 1);
            yield return null;
        }
    }

    public void StartCredits() {
        creditsCoroutine = Credits();
        StartCoroutine(creditsCoroutine);
    }

    IEnumerator Credits() {
        creditsActive = true;
        foreach(GameObject creditsPage in creditsPages) {
            SwitchToMenu(creditsPage);
            yield return new WaitForSeconds(creditsPageDuration);
            Camera.main.GetComponent<RippleImageEffect>().Ripple(0.5f, 0.5f);
        }
        creditsActive = false;
        StopCredits();
    }

    public void StopCredits() {
        StopCoroutine(creditsCoroutine);
        SwitchToMenu(menus[0]);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour {
    public float transitionTime;
    public GameObject[] menus;
    private GameObject activeMenu;

    [Header("OptionsMenu")]
    public Text graphicsQualityText;
    public Slider lookSensitivity;
    public Toggle lookInversionY;
    public Toggle lookInversionX;

    [Header("Credits")]
    public float creditsPageDuration = 4;
    public GameObject[] creditsPages;
    private bool creditsActive;
    private IEnumerator creditsCoroutine;

    private void Start() {
        activeMenu = menus[0];

        graphicsQualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        lookSensitivity.value = PlayerPrefs.GetFloat("LookSensitivityX", 2f);
        lookInversionY.isOn = PlayerPrefs.GetInt("LookInveredY", 0) > 0;
        lookInversionX.isOn = PlayerPrefs.GetInt("LookInveredX", 0) > 0;
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

    public void DecreaseGraphicsQuality() {
        QualitySettings.DecreaseLevel(true);
        graphicsQualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        EventSystem.current.SetSelectedGameObject(null);

    }

    public void IncreaseGraphicsQuality() {
        QualitySettings.IncreaseLevel(true);
        graphicsQualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void UpdateLookSensitivityX(float value) {
        Debug.Log("slider update" + value);
        PlayerPrefs.SetFloat("LookSensitivityX", value);
        PlayerPrefs.SetFloat("LookSensitivityY", value);
    }

    public void SetVerticalInvert(bool value) {
        PlayerPrefs.SetInt("LookInveredY", value ? 1 : 0);
    }

    public void SetHorizontalInvert(bool value) {
        PlayerPrefs.SetInt("LookInveredX", value ? 1 : 0);
    }
}

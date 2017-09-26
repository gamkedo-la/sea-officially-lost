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

    [Header("SpecialEffects")]
    public Animator lightBeamAnimator;
    public ParticleSystem breathingBubbles;
    public ParticleSystem deepSeaDust;

    private void Start() {
        activeMenu = menus[0];
        activeMenu.SetActive(true);

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

    public void StartLoadingGame(string startSceneName) {
        Debug.LogWarning("ASYNC LOAD STARTED - DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(startSceneName);
        asyncLoad.allowSceneActivation = false;
        IEnumerator testEffect = StartGameEffect(3f, asyncLoad);
        SwitchToMenu(null);

        lightBeamAnimator.SetTrigger("StartGame");
        var bubbleEmission = breathingBubbles.emission;
        var bubbleShape = breathingBubbles.shape;

        bubbleEmission.rateOverTimeMultiplier = 50; //10
        bubbleShape.angle = 50; //5

        breathingBubbles.GetComponent<BreathingController>().breathingTimeScale = 0;
        breathingBubbles.Play();

        StartCoroutine(testEffect);
    }

    IEnumerator StartGameEffect(float duration, AsyncOperation asyncLoad) {
        var dustMain = deepSeaDust.main;
        var dustEmission = deepSeaDust.emission;
        var bubbleMain = breathingBubbles.main;

        for (float timer = 0f; timer < duration; timer += Time.deltaTime) {
            dustMain.simulationSpeed += Time.deltaTime * (timer/duration + 1) * (timer/duration + 1) * 2; //1
            dustEmission.rateOverTimeMultiplier += Time.deltaTime * (timer / duration + 1) * (timer / duration + 1) * 2; //3
            bubbleMain.gravityModifierMultiplier = dustMain.simulationSpeed * -0.1f;
            yield return null;
        }
        yield return new WaitForSeconds(duration / 2);

        asyncLoad.allowSceneActivation = true;
    }

    public void QuitGame() {
        lightBeamAnimator.SetTrigger("QuitGame");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    public void SwitchToMenu(GameObject menu) {
        IEnumerator fadeCoroutine = FadeBetweenMenus(menu);
        StartCoroutine(fadeCoroutine);
    }

    IEnumerator FadeBetweenMenus(GameObject menu) {
        CanvasGroup canvas = activeMenu.GetComponent<CanvasGroup>();
        for(float t = transitionTime/2; t > 0; t -= Time.deltaTime) {
            canvas.alpha = Mathf.Clamp(t*2, 0, 1);
            yield return null;
        }

        activeMenu.SetActive(false);
        canvas.alpha = 1;

        if(menu == null) {
            yield break;
        }

        activeMenu = menu;
        activeMenu.SetActive(true);
        canvas = activeMenu.GetComponent<CanvasGroup>();

        for (float t = 0; t < transitionTime; t += Time.deltaTime) {
            canvas.alpha = Mathf.Clamp(t * 2, 0, 1);
            yield return null;
        }
    }

    public void OpenSubmenuSound() {
        AkSoundEngine.PostEvent("Play_UI_Menu_Click_Into", gameObject);
    }

    public void CloseSubmenuSound() {
        AkSoundEngine.PostEvent("Play_UI_Menu_Click_Outfrom", gameObject);
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

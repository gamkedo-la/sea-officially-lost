using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.PostProcessing;

public class MainMenuController : MonoBehaviour {
    public float transitionTime;
    private float startGameEffectTime = 5f;
    private float quitGameEffectTime = 5f;
    public GameObject[] menus;
    public EventSystem menuEventSystem;
    public WwiseSyncs wwiseSyncs;
    private GameObject activeMenu;
    private GameObject currentMenuObject;
    private PostProcessingProfile postProcessingProfile;
    ColorGradingModel.Settings colorGrading;
    private bool mouseInput;

    [Header("OptionsMenu")]
    public GameObject graphicsQualityPanel;
    public Button graphicsQualityDecrease;
    public Button graphicsQualityIncrease;
    public Text graphicsQualityText;
    public Text brightnessText;
    public Text fieldOfViewText;
    public Text masterVolumeText;
    public Text effectsVolumeText;
    public Text musicVolumeText;
    public Text lookSensitivityText;
    public Slider brightnessSlider;
    public Slider fieldOfViewSlider;
    public Slider masterVolumeSlider;
    public Slider effectsVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider lookSensitivitySlider;
    public Toggle lookInversionY;
    public Toggle lookInversionX;
    private string brightnessMessage;
    private string fieldOfViewMessage;
    private string masterVolumeMessage;
    private string effectsVolumeMessage;
    private string musicVolumeMessage;
    private string lookSensitivityMessage;

    [Header("Credits")]
    public float creditsPageDuration = 4;
    public GameObject[] creditsPages;
    private bool creditsActive;
    private IEnumerator creditsCoroutine;
    private bool nextCredits;

    [Header("SpecialEffects")]
    public Animator lightBeamAnimator;
    public ParticleSystem breathingBubbles;
    public ParticleSystem deepSeaDustVertical;
    public ParticleSystem deepSeaDustHorizontal;

    private void Start() {
        activeMenu = menus[0];
        activeMenu.SetActive(true);
        currentMenuObject = FindFirstEnabledSelectable(activeMenu);

        brightnessMessage = brightnessText.text;
        fieldOfViewMessage = fieldOfViewText.text;
        masterVolumeMessage = masterVolumeText.text;
        effectsVolumeMessage = effectsVolumeText.text;
        musicVolumeMessage = musicVolumeText.text;
        lookSensitivityMessage = lookSensitivityText.text;

        graphicsQualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];

        brightnessText.text = brightnessMessage + brightnessSlider.value;
        postProcessingProfile = Instantiate(Camera.main.GetComponent<PostProcessingBehaviour>().profile);
        Camera.main.GetComponent<PostProcessingBehaviour>().profile = postProcessingProfile;
        colorGrading = postProcessingProfile.colorGrading.settings;
        brightnessSlider.value = PlayerPrefs.GetInt("Brightness", 50);
        colorGrading.basic.postExposure = (brightnessSlider.value - 50)/50;
        postProcessingProfile.colorGrading.settings = colorGrading;


        fieldOfViewSlider.value = PlayerPrefs.GetInt("FieldOfView", 60);
        fieldOfViewText.text = fieldOfViewMessage + fieldOfViewSlider.value;

        masterVolumeSlider.value = PlayerPrefs.GetInt("MasterVolume", 100);
        UpdateMasterVolume(masterVolumeSlider.value);
        effectsVolumeSlider.value = PlayerPrefs.GetInt("EffectsVolume", 100);
        UpdateEffectsVolume(effectsVolumeSlider.value);
        musicVolumeSlider.value = PlayerPrefs.GetInt("MusicVolume", 100);
        UpdateMusicVolume(musicVolumeSlider.value);

        lookSensitivitySlider.value = PlayerPrefs.GetInt("LookSensitivityX", 20);
        lookInversionY.isOn = PlayerPrefs.GetInt("LookInveredY", 0) > 0;
        lookInversionX.isOn = PlayerPrefs.GetInt("LookInveredX", 0) > 0;
        lookSensitivityText.text = lookSensitivityMessage + lookSensitivitySlider.value;
    }

    private void Update() {
        if (Input.anyKeyDown) {
            //Skips to next credits page
            if (creditsActive) {
                nextCredits = true;
            }

            //If nothing is selected on vertical navigation then select the top item
            if (Input.GetAxisRaw("Vertical") != 0 && menuEventSystem.currentSelectedGameObject == null) {
                menuEventSystem.SetSelectedGameObject(currentMenuObject);
                mouseInput = false;
            }

            //Special behavior handling for graphics quality selector
            if (Input.GetAxisRaw("Horizontal") < 0 && menuEventSystem.currentSelectedGameObject == graphicsQualityPanel) {
                graphicsQualityDecrease.animator.SetTrigger("Pressed");
                graphicsQualityDecrease.onClick.Invoke();
            }
            else if (Input.GetAxisRaw("Horizontal") > 0 && menuEventSystem.currentSelectedGameObject == graphicsQualityPanel) {
                graphicsQualityIncrease.animator.SetTrigger("Pressed");
                graphicsQualityIncrease.onClick.Invoke();
            }
        }
    }

    public void StartLoadingGame(string startSceneName) {
        Debug.LogWarning("ASYNC LOAD STARTED - DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(startSceneName);
        asyncLoad.allowSceneActivation = false;
        IEnumerator startGameEffect = StartGameEffect(startGameEffectTime, asyncLoad);
        SwitchToMenu(null);

        lightBeamAnimator.SetTrigger("StartGame");
        AkSoundEngine.PostEvent("Play_Menu_Game_Start", gameObject);

        StartCoroutine(startGameEffect);
    }

    IEnumerator StartGameEffect(float duration, AsyncOperation asyncLoad) {
        yield return new WaitForSeconds(duration);

        asyncLoad.allowSceneActivation = true;
    }

    public void QuitGame() {
        IEnumerator quitGameEffect = QuitGameEffect(quitGameEffectTime);
        SwitchToMenu(null);

        lightBeamAnimator.SetTrigger("QuitGame");
        var bubbleEmission = breathingBubbles.emission;
        var bubbleShape = breathingBubbles.shape;

        bubbleEmission.rateOverTimeMultiplier = 50;
        bubbleShape.angle = 50;

        breathingBubbles.GetComponent<BreathingController>().breathingTimeScale = 0;
        breathingBubbles.Play();

        StartCoroutine(quitGameEffect);
    }

    IEnumerator QuitGameEffect(float duration) {
        yield return new WaitForSeconds(duration);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SwitchToMenu(GameObject menu) {
        if(Input.GetAxisRaw("Submit") != 0) {
            mouseInput = false;
        }
        else {
            mouseInput = true;
        }

        IEnumerator fadeCoroutine = FadeBetweenMenus(menu);
        StartCoroutine(fadeCoroutine);
    }

    IEnumerator FadeBetweenMenus(GameObject menu) {
        CanvasGroup canvas = activeMenu.GetComponent<CanvasGroup>();
        //menuEventSystem.SetSelectedGameObject(null);

        for (float t = transitionTime/2; t > 0; t -= Time.deltaTime) {
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

        currentMenuObject = FindFirstEnabledSelectable(activeMenu);
        if (!mouseInput) {
            menuEventSystem.SetSelectedGameObject(currentMenuObject);
        }

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
        nextCredits = false;
        creditsCoroutine = Credits();
        StartCoroutine(creditsCoroutine);
    }

    IEnumerator Credits() {
        creditsActive = true;

        foreach(GameObject creditsPage in creditsPages) {
            SwitchToMenu(creditsPage);

            for(float waitTimer = creditsPageDuration; waitTimer > 0; waitTimer -= Time.deltaTime) {
                if (nextCredits) {
                    nextCredits = false;
                    break;
                }
                yield return null;
            }

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
        //EventSystem.current.SetSelectedGameObject(null);

    }

    public void IncreaseGraphicsQuality() {
        QualitySettings.IncreaseLevel(true);
        graphicsQualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        //EventSystem.current.SetSelectedGameObject(null);
    }

    public void UpdateBrightness(float value) {
        PlayerPrefs.SetInt("Brightness", (int)value);
        PlayerPrefs.Save();

        brightnessText.text = brightnessMessage + (int)value;

        colorGrading.basic.postExposure = (brightnessSlider.value - 50) / 50;
        postProcessingProfile.colorGrading.settings = colorGrading;
    }

    public void UpdateMasterVolume(float value) {
        wwiseSyncs.VolumeMaster(value);
        masterVolumeText.text = masterVolumeMessage + (int)value;
        PlayerPrefs.SetInt("MasterVolume", (int)value);
    }

    public void UpdateEffectsVolume(float value) {
        wwiseSyncs.VolumeSFX(value);
        effectsVolumeText.text = effectsVolumeMessage + (int)value;
        PlayerPrefs.SetInt("EffectsVolume", (int)value);
    }

    public void UpdateMusicVolume(float value) {
        wwiseSyncs.VolumeMusic(value);
        musicVolumeText.text = musicVolumeMessage + (int)value;
        PlayerPrefs.SetInt("MusicVolume", (int)value);
    }

    public void UpdateLookSensitivityX(float value) {
        PlayerPrefs.SetInt("LookSensitivityX", (int)value);
        PlayerPrefs.SetInt("LookSensitivityY", (int)value);
        PlayerPrefs.Save();

        lookSensitivityText.text = lookSensitivityMessage + (int)value;
    }

    public void UpdateFieldOfView(float value) {
        PlayerPrefs.SetInt("FieldOfView", (int)value);
        PlayerPrefs.Save();

        fieldOfViewText.text = fieldOfViewMessage + (int)value;
    }

    public void SetVerticalInvert(bool value) {
        PlayerPrefs.SetInt("LookInveredY", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetHorizontalInvert(bool value) {
        PlayerPrefs.SetInt("LookInveredX", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    private GameObject FindFirstEnabledSelectable(GameObject gameObject) {
        GameObject go = null;
        var selectables = gameObject.GetComponentsInChildren<Selectable>(true);
        foreach (var selectable in selectables) {
            if (selectable.IsActive() && selectable.IsInteractable()) {
                go = selectable.gameObject;
                break;
            }
        }
        return go;
    }
}

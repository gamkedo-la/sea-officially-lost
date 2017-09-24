using UnityEngine;

public class CreditsController : MonoBehaviour {/*
    public MainMenuController mainMenu;
    public float pageDuration = 4;
    private float pageTimer;
    public GameObject[] creditsPages;
    private int creditsPageIndex;

    void OnEnable() {
        pageTimer = 0;
        creditsPageIndex = 0;

        creditsPages = GetComponentsInChildren<GameObject>();
        creditsPages[0].SetActive(true);
    }
	
	void Update () {
        pageTimer += Time.deltaTime;

        if(pageTimer > pageDuration) {
            pageTimer = 0;
            creditsPages[creditsPageIndex].SetActive(false);
            creditsPageIndex++;

            if (creditsPageIndex > creditsPages.Length) {
                mainMenu.StopCredits();
            }
            else {
                creditsPages[creditsPageIndex].SetActive(true);
            }
        }
    }*/
}

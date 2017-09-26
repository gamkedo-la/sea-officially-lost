using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonBehavoir : MonoBehaviour, IPointerExitHandler {
    public void OnPointerExit(PointerEventData eventData) {
        AkSoundEngine.PostEvent("Play_UI_Menu_Unhover", gameObject);
        EventSystem.current.SetSelectedGameObject(null);
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Pressed", false);
        anim.SetBool("Highlighted", false);
        anim.SetBool("Normal", true);
    }

    public void OnPointerEnter(PointerEventData eventDate) {
        AkSoundEngine.PostEvent("Play_UI_Menu_Hover", gameObject);
    }
}

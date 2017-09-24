using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonBehavoir : MonoBehaviour, IPointerExitHandler {
    public void OnPointerExit(PointerEventData eventData) {
        EventSystem.current.SetSelectedGameObject(null);
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Pressed", false);
        anim.SetBool("Highlighted", false);
        anim.SetBool("Normal", true);
    }
}

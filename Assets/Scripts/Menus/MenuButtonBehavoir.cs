using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonBehavoir : MonoBehaviour, IPointerExitHandler {
    public void OnPointerExit(PointerEventData eventData) {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

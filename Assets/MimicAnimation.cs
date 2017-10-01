using UnityEngine;

public class MimicAnimation : MonoBehaviour {
    public GameObject master;
    private UnityEngine.EventSystems.EventSystem eventSystem;
    private Animator animator;
    private bool selected;

    private void Start() {
        selected = false;
        eventSystem = UnityEngine.EventSystems.EventSystem.current;
        animator = GetComponent<Animator>();
    }

    void Update () {
		if(!selected && eventSystem.currentSelectedGameObject == master) {
            animator.SetTrigger("Highlighted");
            selected = true;
        }
        else if(selected && eventSystem.currentSelectedGameObject != master) {
            animator.SetTrigger("Normal");
            selected = false;
        }
    }
}

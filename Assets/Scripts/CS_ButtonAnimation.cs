using UnityEngine;

public class CS_ButtonAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter()
    {
        animator.SetTrigger("Highlighted"); // Trigger the hover animation
    }

    public void OnPointerExit()
    {
        animator.ResetTrigger("Highlighted"); // Reset the hover animation
    }
}

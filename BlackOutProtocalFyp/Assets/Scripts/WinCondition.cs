using UnityEngine;
using UnityEngine.InputSystem;

public class WinCondition : MonoBehaviour
{

    [SerializeField] private GameObject papers;
    private bool Objective = false;
    public GameObject WinScreen;

    void OnInteract(InputValue value)
    {
        if (value.isPressed && Objective == true)
        {
            WinScreen.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collection")
        {
            Objective = true;
        }
    }
}

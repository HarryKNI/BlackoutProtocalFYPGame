using UnityEngine;
using UnityEngine.InputSystem;

public class MenuButton : MonoBehaviour
{

    [SerializeField] Scenes Scenes;

    void OnMenu(InputValue value)
    {
        if (value.isPressed)
        {
            Scenes.Pause_Menu.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

        }
    }
}

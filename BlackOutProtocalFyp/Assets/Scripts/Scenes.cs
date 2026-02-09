using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{

    [Header("Gameobjects")]
    public GameObject Pause_Menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Pause_Menu != null)
        {
            Pause_Menu.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        Pause_Menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SceneLoader(int SceneNum)
    {
        SceneManager.LoadScene(SceneNum);
    }
}

using UnityEngine.InputSystem;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    private bool closeToDoor = false;
    private GameObject Door;

    void OnInteract(InputValue value)
    {
        if (value.isPressed && closeToDoor == true)
        {
            Door.GetComponent<Animator>().SetBool("OpenDoor", true);
            print(Door);
            
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.tag == "Door")
        {
            closeToDoor = true;
            Door = collision.gameObject;
            print(Door);
        }
    }

    public void OnTriggerExit(Collider collision)
    {

        if (collision.gameObject.tag == "Door")
        {
            closeToDoor = true;
            Door.GetComponent<Animator>().SetBool("OpenDoor", false);
            
        }
    }
}

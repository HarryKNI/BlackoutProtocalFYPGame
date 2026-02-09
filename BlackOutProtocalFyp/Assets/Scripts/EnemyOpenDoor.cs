using UnityEngine;

public class EnemyOpenDoor : MonoBehaviour
{

    private GameObject Door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Door")
        {
            Door = collision.gameObject;
            Door.GetComponent<Animator>().SetBool("OpenDoor", true);
            
        }
    }

    public void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "Door")
        {
            Door.GetComponent<Animator>().SetBool("OpenDoor", false);

        }
    }
}

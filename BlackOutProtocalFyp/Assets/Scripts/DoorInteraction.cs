using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private AudioSource Audio;
    [SerializeField] private float soundRange = 25f;
    public Vector3 SoundPos;
    private bool closeToDoor = false;
    private GameObject Door;

    void OnInteract(InputValue value)
    {
        if (value.isPressed && closeToDoor == true)
        {
            Door.GetComponent<Animator>().SetBool("OpenDoor", true);
            Audio = Door.GetComponent<AudioSource>();
            if (Audio.isPlaying)
            { return; }
            Audio.Play();
            var sound = new Sound(transform.position, soundRange);
            SoundPos = sound.SoundPosition;
            Sounds.MakeSound(sound);
            

            //print($"Sound: with pos {sound.SoundPosition} and range {sound.SoundRange} created!");
            
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.tag == "Door")
        {
            closeToDoor = true;
            Door = collision.gameObject;
            

        }
    }

    public void OnTriggerExit(Collider collision)
    {

        if (collision.gameObject.tag == "Door")
        {
            closeToDoor = false;
            Door.GetComponent<Animator>().SetBool("OpenDoor", false);
            
        }
    }
}

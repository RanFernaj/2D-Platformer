using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    public Manager.DoorKeyColours keyColour;
    GameObject door;
    
    
    // Start is called before the first frame update
    
    void Start()
    {
        door = transform.Find("Door").gameObject; // THis access the child of a parent object
        SpriteRenderer sr = door.GetComponent<SpriteRenderer>();
        
        switch (keyColour)
        {
            case Manager.DoorKeyColours.Red:
                sr.color = Color.red;
                break;
            case Manager.DoorKeyColours.Blue:
                sr.color = Color.blue;
                break;
            case Manager.DoorKeyColours.Yellow:
                sr.color = Color.yellow;
                break;
        }



    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && door != null)
        {
            switch (keyColour)
            {
                case Manager.DoorKeyColours.Red:
                    if (Manager.redKey)
                    {
                        Destroy(door);
                        FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.OpenDoor, transform.position, 1f);
                        Manager.rKey -= 1;
                    }

                    break;
                case Manager.DoorKeyColours.Blue:
                    if (Manager.blueKey)
                    {
                        Destroy(door);
                        FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.OpenDoor, transform.position, 1f);
                        Manager.bKey -= 1;
                    }
                    break;
                case Manager.DoorKeyColours.Yellow:
                    if (Manager.yellowKey)
                    {
                        Destroy(door);
                        FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.OpenDoor, transform.position, 1f);
                        Manager.yKey -= 1;
                    }
                    break;
            }
        }
    }
}

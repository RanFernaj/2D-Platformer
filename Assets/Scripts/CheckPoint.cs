using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    GameObject flag;
    // Start is called before the first frame update
    void Start()
    {
       flag = transform.Find("Flag").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            flag.GetComponent<SpriteRenderer>().color = Color.red;
            Manager.UpdateCheckPoint(gameObject);
            
        }

    }

    public void LowerFlag()
    {
       flag.GetComponent<SpriteRenderer>().color = Color.white;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Manager.AddCoins(coinValue);
            FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.PickUpCoin, transform.position, 1f);
            Destroy(gameObject);

        }
    }
}

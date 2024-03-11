using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public static int coins, lives;
    public enum DoorKeyColours { Red, Blue, Yellow };
    public static bool redKey, blueKey, yellowKey;
    public static int rKey, bKey, yKey;
    public static Vector3 lastCheckPoint;
    public static bool gamePaused;
    static GameUi gameUI;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        gameUI = FindObjectOfType<GameUi>();
        lives = 3;
        coins = 0;
        gameUI.UpdateCoins();
        gameUI.UpdateLives();
    }

    public static void AddCoins(int coinValue)
    {
        coins += coinValue;
        print("Coins = " + coins);
        if(coins >= 100)
        {
            coins -= 100;
            AddLives(1);
        }
        gameUI.UpdateCoins();
    }
    public static void AddLives(int lifeValue)
    {
        lives += lifeValue;
        if (lives == -1)
        {
            FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.Death, Vector3.zero, 1f);
        }
        if (lives < 0)
        {
            gameUI.CheckGameState(GameUi.GameState.GameOver);
        }
        else
        {
            gameUI.UpdateLives();

        }
    }


    public static void KeyPickup(DoorKeyColours keyColour)
    {
        switch (keyColour)
        {
            case DoorKeyColours.Red:
                redKey = true;
                //rKey++;
                break;
            case DoorKeyColours.Blue:
                blueKey = true;
                //bKey++;
                break;
            case DoorKeyColours.Yellow:
                yellowKey = true;
                //yKey++;
                break;
        }
        print("Picked up = " + keyColour + "key");
        print("Yellow = " + yellowKey + " Red= " + redKey + " Blue = " + blueKey);
        gameUI.UpdateKeys(keyColour);
    }

    public static void UpdateCheckPoint(GameObject flag)
    {
        

        lastCheckPoint = flag.transform.position;
        CheckPoint[] allCheckPoints = FindObjectsOfType<CheckPoint>();
        foreach (CheckPoint cp in allCheckPoints)
        {
            if (cp != flag.GetComponent<CheckPoint>())
            {
                cp.LowerFlag();
            }
        }
    }

}

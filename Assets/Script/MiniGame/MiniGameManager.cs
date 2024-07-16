using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject slidingObject;

    public int playerScore = 0;
    public int maxPlayerLives = 3;
    public int playerLives = 0;

    [Header("UI")]

    public GameObject gameHUD;

    public GameObject startMenu;
    public GameObject endMenu;

    public Text scoreText;
    public Text lifeText;

    private void Start()
    {
        gameHUD.SetActive(false);
        startMenu.SetActive(true);
        endMenu.SetActive(false);
    }

    public void spawnSlidingObject()
    {

        objectColor randomColor = GetRandomEnum<objectColor>();

        GameObject slidingObjectRef = Instantiate(slidingObject, spawnPoint);
        slidingObjectRef.GetComponent<SlidingObject>().SetObjectColor(randomColor);
        slidingObjectRef.GetComponent<SlidingObject>()._miniGameManager = this;

    }

    T GetRandomEnum<T>()
    {
        string[] enumNames = System.Enum.GetNames(typeof(T));

        int randomIndex = Random.Range(0, enumNames.Length);

        T randomEnumValue = (T)System.Enum.Parse(typeof(T), enumNames[randomIndex]);

        return randomEnumValue;
    }

    public void SetScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        scoreText.text = "Score: " + playerScore;
        Invoke("spawnSlidingObject", 0.5f);
    }

    public void SetLife()
    {
        playerLives -= 1;

        lifeText.text = "Life: " + playerLives;

        if (playerLives > 0)
        {
            Invoke("spawnSlidingObject", 0.5f);
        }
        else
        {
            GameOver();
        }
    }

    public void StartMiniGame()
    {
        spawnSlidingObject();

        playerLives = maxPlayerLives;
        playerScore = 0;

        scoreText.text = "Score: " + playerScore;
        lifeText.text = "Lives: " + playerLives;

        gameHUD.SetActive(true);
        startMenu.SetActive(false);
        endMenu.SetActive(false);
    }

    public void GameOver()
    {
        gameHUD.SetActive(false);
        endMenu.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class MiniGameManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject slidingObject;

    public int playerScore = 0;

    public float spawnInterval = 3f;
    private float nextSpawn;

    [Header("UI")]

    public GameObject gameHUD;

    public GameObject startMenu;
    public GameObject endMenu;

    public Text scoreText;
    public Text redNumberText;
    public Text blueNumberText;
    public Text yellowNumberText;

    public Button arrivageRandom;
    public Button arrivageRed;
    public Button arrivageBlue;
    public Button arrivageYellow;

    public GameObject command01;
    public GameObject command02;
    public GameObject command03;

    private bool canSpawn = false;

    private bool spawnRandomColor = true;

    private objectColor arrivageType;

    [Header("Stockage")]
    public int numberRed = 0;
    public int numberBlue = 0;
    public int numberYellow = 0;

    private void Start()
    {
        gameHUD.SetActive(false);
        startMenu.SetActive(true);
        endMenu.SetActive(false);

        redNumberText.text = "0";
        yellowNumberText.text = "0";
        blueNumberText.text = "0";

        nextSpawn = Time.time + spawnInterval;
    }


    public void Update()
    {
        if (Time.time >= nextSpawn && canSpawn)
        {
            nextSpawn = Time.time + spawnInterval;

            spawnSlidingObject();
        }
    }


    public void spawnSlidingObject()
    {

        objectColor randomColor = objectColor.red;

        if (spawnRandomColor) 
        { 
           randomColor = GetRandomEnum<objectColor>();
        }
        else
        {
            int randomIndex = Random.Range(0, 4);
            if (randomIndex == 0)
            {
                randomColor = GetRandomEnum<objectColor>();
            }
            else
            {
                randomColor = arrivageType;
            }
        }

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
        scoreText.text = "Money: " + playerScore + "$";
        //Invoke("spawnSlidingObject", 0.5f);
    }

    public void StartMiniGame()
    {
        //spawnSlidingObject();

        canSpawn = true;

        playerScore = 0;

        scoreText.text = "Money: " + playerScore + "$";

        gameHUD.SetActive(true);
        startMenu.SetActive(false);
        endMenu.SetActive(false);
    }

    public void GameOver()
    {
        canSpawn= false;
        gameHUD.SetActive(false);
        endMenu.SetActive(true);
    }

    public void SetSpawnRate(int _valueToAdd)
    {
        spawnInterval += _valueToAdd;
        spawnInterval = Mathf.Clamp(spawnInterval, 1f, 5f);
    }

    public void SelectArrivageType(int selectedColorIndex)
    {
        string[] enumNames = System.Enum.GetNames(typeof(objectColor));
        objectColor selectedColor = (objectColor)System.Enum.Parse(typeof(objectColor), enumNames[selectedColorIndex]);

        arrivageRandom.GetComponent<Image>().color = Color.white;

        switch (selectedColor)
        {
            case objectColor.red:
                arrivageRed.GetComponent<Image>().color = Color.red;
                arrivageBlue.GetComponent<Image>().color = Color.white;
                arrivageYellow.GetComponent<Image>().color = Color.white;
                break;
            case objectColor.yellow:
                arrivageRed.GetComponent<Image>().color = Color.white;
                arrivageBlue.GetComponent<Image>().color = Color.white;
                arrivageYellow.GetComponent<Image>().color = Color.yellow;
                break;
            case objectColor.blue:
                arrivageRed.GetComponent<Image>().color = Color.white;
                arrivageBlue.GetComponent<Image>().color = Color.blue;
                arrivageYellow.GetComponent<Image>().color = Color.white;
                break;
        }
        arrivageType = selectedColor;
        spawnRandomColor = false;
    }

    public void RandomArriveg()
    {
        spawnRandomColor = true;
        arrivageRandom.GetComponent<Image>().color = Color.gray;
        arrivageRed.GetComponent<Image>().color = Color.white;
        arrivageBlue.GetComponent<Image>().color = Color.white;
        arrivageYellow.GetComponent<Image>().color = Color.white;
    }

    public void Slowmotion()
    {

    }

    public void AddStock(objectColor _Color)
    {
        switch (_Color)
        {
            case objectColor.red:
                numberRed += 1;
                redNumberText.text = numberRed.ToString();
                command01.GetComponent<Command>().currentRedNumber = numberRed;
                command02.GetComponent<Command>().currentRedNumber = numberRed;
                command03.GetComponent<Command>().currentRedNumber = numberRed;
                command01.GetComponent<Command>().SetAllText();
                command02.GetComponent<Command>().SetAllText();
                command03.GetComponent<Command>().SetAllText();
                break;
            case objectColor.yellow:
                numberYellow += 1;
                yellowNumberText.text = numberYellow.ToString();
                command01.GetComponent<Command>().currentYellowNumber = numberYellow;
                command02.GetComponent<Command>().currentYellowNumber = numberYellow;
                command03.GetComponent<Command>().currentYellowNumber = numberYellow;
                command01.GetComponent<Command>().SetAllText();
                command02.GetComponent<Command>().SetAllText();
                command03.GetComponent<Command>().SetAllText();
                break;
            case objectColor.blue:
                numberBlue += 1;
                blueNumberText.text = numberBlue.ToString();
                command01.GetComponent<Command>().currentBlueNumber = numberBlue;
                command02.GetComponent<Command>().currentBlueNumber = numberBlue;
                command03.GetComponent<Command>().currentBlueNumber = numberBlue;
                command01.GetComponent<Command>().SetAllText();
                command02.GetComponent<Command>().SetAllText();
                command03.GetComponent<Command>().SetAllText();
                break;
        }
    }

    public void SellCommand(GameObject _Command)
    {

        if (_Command.GetComponent<Command>().CheckCommand())
        {

            Debug.Log("Sell Command");

            _Command.GetComponent<Command>().SetCommand();
            int moneyToAdd = Random.Range(5, 16);

            numberRed -= _Command.GetComponent<Command>().redNumberNeeded;
            numberYellow -= _Command.GetComponent<Command>().yellowNumberNeeded;
            numberBlue -= _Command.GetComponent<Command>().blueNumberNeeded;

            blueNumberText.text = numberBlue.ToString();
            yellowNumberText.text = numberYellow.ToString();
            redNumberText.text = numberRed.ToString();

            playerScore += moneyToAdd;
            scoreText.text = playerScore.ToString();
        }
        else
        {
            Debug.Log("Can't Sell Command");
        }

    }
}

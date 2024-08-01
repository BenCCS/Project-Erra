using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class MiniGameManager : MonoBehaviour
{
    [Header("Camera")]
    public bool canControlTheCam = true;
    public CameraMovement cameraMovement;

    [Header("Player")]
    public int playerScore = 0;
    public int numberRed = 0;
    public int numberBlue = 0;
    public int numberYellow = 0;

    [Header("Sliding Objetcs")]
    public Transform spawnPoint;
    public GameObject slidingObject;
    public float spawnInterval = 3f;
    private float spawnIntervalBase;
    private float nextSpawn;

    [Header("UI")]

    public GameObject gameHUD;
    public GameObject startMenu;
    public GameObject endMenu;

    public Text scoreText;
    public Text redNumberText;
    public Text blueNumberText;
    public Text yellowNumberText;

    public GameObject moneyEarnedText;

    public Image slowMotionPreogressBar;

    public Button arrivageRandom;
    public Button arrivageRed;
    public Button arrivageBlue;
    public Button arrivageYellow;

    public Button multiply1;
    public Button multiply2;
    public Button multiply3;

    private int multiplyIndex;

    public GameObject command01;
    public GameObject command02;
    public GameObject command03;

    [Header("SlowMotion")]
    public float duration = 5;
    private float currentTime = 0;
    private bool isFilling = false;

    private bool canSpawn = false;

    private bool spawnRandomColor = true;

    private objectColor arrivageType;

    private bool isInSlowMotion = false;

    private List<SlidingObject> spawnList = new List<SlidingObject>();

    private bool slowmotionIsActivate = false;

    private float originalTimeScale;

    [Header("Objects")]
    public LandingPad landingPad;
    public Dropship dropship;

    private void Start()
    {
        gameHUD.SetActive(false);
        startMenu.SetActive(true);
        endMenu.SetActive(false);
        moneyEarnedText.SetActive(false);


        originalTimeScale = Time.timeScale;

        spawnIntervalBase = spawnInterval;

        redNumberText.text = "0";
        yellowNumberText.text = "0";
        blueNumberText.text = "0";

        arrivageRandom.GetComponent<Image>().color = Color.gray;
        multiply1.GetComponent<Image>().color = Color.gray;

        nextSpawn = Time.time + spawnInterval;

        cameraMovement.canMove = canControlTheCam;
    }

    public void Update()
    {
        if (Time.time >= nextSpawn && canSpawn)
        {
            nextSpawn = Time.time + spawnInterval;

            spawnSlidingObject();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isInSlowMotion == false)
        {
            Slowmotion();
        }

        if (isFilling)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Clamp01(1 - currentTime / duration);
            slowMotionPreogressBar.fillAmount = progress;

            if (currentTime >= duration)
            {
                isFilling = false;
                currentTime = 0f;
                OnProgressComplete();
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            playerScore += 20;
            scoreText.text = new string("Money: " + playerScore + "$");
        }

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            AddStock(objectColor.red);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            AddStock(objectColor.blue);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            AddStock(objectColor.yellow);
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
        spawnList.Add(slidingObjectRef.GetComponent<SlidingObject>());
        slidingObjectRef.GetComponent<SlidingObject>().SetObjectColor(randomColor);
        slidingObjectRef.GetComponent<SlidingObject>()._miniGameManager = this;

        if (slowmotionIsActivate == true)
        {
            slidingObjectRef.GetComponent<SlidingObject>().SetSpeed(true);
        }
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
        scoreText.text = new string("Money: " + playerScore + "$");
    }

    public void StartMiniGame()
    {
        canSpawn = true;

        playerScore = 0;

        scoreText.text = new string("Money: " + playerScore + "$");

        gameHUD.SetActive(true);
        startMenu.SetActive(false);
        endMenu.SetActive(false);
    }

    public void GameOver()
    {
        canSpawn = false;
        gameHUD.SetActive(false);
        endMenu.SetActive(true);
    }

    public void SetSpawnRate(int _valueToMultiply)
    {
        spawnInterval = spawnIntervalBase / _valueToMultiply;

        multiplyIndex = _valueToMultiply;

        switch (_valueToMultiply)
        {
            case 0:
                break;
            case 1:
                spawnInterval = 1;
                multiplyIndex = 1;
                multiply1.GetComponent<Image>().color = Color.gray;
                multiply2.GetComponent<Image>().color = Color.white;
                multiply3.GetComponent<Image>().color = Color.white;
                break;
            case 2:
                multiply1.GetComponent<Image>().color = Color.white;
                multiply2.GetComponent<Image>().color = Color.gray;
                multiply3.GetComponent<Image>().color = Color.white;
                break;
            case 3:
                multiply1.GetComponent<Image>().color = Color.white;
                multiply2.GetComponent<Image>().color = Color.white;
                multiply3.GetComponent<Image>().color = Color.gray;
                break;
        }
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

        slowMotionPreogressBar.color = Color.gray;

        multiply1.GetComponent<Image>().color = Color.white;
        multiply2.GetComponent<Image>().color = Color.white;
        multiply3.GetComponent<Image>().color = Color.white;

        StartCoroutine(SetSlowmotion(5, duration));
        isFilling = true;
        currentTime = 0f;
    }

    private void OnProgressComplete()
    {
        slowMotionPreogressBar.color = Color.white;

        switch (multiplyIndex)
        {
            case 0:
                break;
            case 1:
                multiply1.GetComponent<Image>().color = Color.gray;
                multiply2.GetComponent<Image>().color = Color.white;
                multiply3.GetComponent<Image>().color = Color.white;
                break;
            case 2:
                multiply1.GetComponent<Image>().color = Color.white;
                multiply2.GetComponent<Image>().color = Color.gray;
                multiply3.GetComponent<Image>().color = Color.white;
                break;
            case 3:
                multiply1.GetComponent<Image>().color = Color.white;
                multiply2.GetComponent<Image>().color = Color.white;
                multiply3.GetComponent<Image>().color = Color.gray;
                break;
        }
    }

    public void AddStock(objectColor _Color)
    {
        switch (_Color)
        {
            case objectColor.red:
                numberRed += 1;
                redNumberText.text = numberRed.ToString();
                break;
            case objectColor.yellow:
                numberYellow += 1;
                yellowNumberText.text = numberYellow.ToString();
                break;
            case objectColor.blue:
                numberBlue += 1;
                blueNumberText.text = numberBlue.ToString();
                break;
        }
    }

    public void SubstractStock(objectColor _oColor)
    {
        switch (_oColor)
        {
            case objectColor.red:
                numberRed -= 1;
                if (numberRed < 0)
                {
                    numberRed = 0;
                }
                redNumberText.text = numberRed.ToString();
                break;
            case objectColor.yellow:
                numberYellow -= 1;
                if (numberYellow < 0)
                {
                    numberYellow = 0;
                }
                yellowNumberText.text = numberYellow.ToString();
                break;
            case objectColor.blue:
                numberBlue -= 1;
                if (numberBlue < 0)
                {
                    numberBlue = 0;
                }
                blueNumberText.text = numberBlue.ToString();
                break;
        }
    }

    public void SellCommand()
    {
        int moneyToAdd = Random.Range(5, 16);

        dropship.Fly();

        playerScore += moneyToAdd;
        StartCoroutine(ShowMoneyEarnedText(1f, moneyToAdd));
        scoreText.text = new string("Money: " + playerScore + "$");
    }

    private IEnumerator SetSlowmotion(float _slowValue, float seconds) 
    {
        isInSlowMotion = true;
        Time.timeScale = 0.5f;

        yield return new WaitForSeconds(seconds);

        Time.timeScale = originalTimeScale;
        isInSlowMotion = false;
    }

    private void SetAllCommandText()
    {
        command01.GetComponent<Command>().currentRedNumber = numberRed;
        command02.GetComponent<Command>().currentRedNumber = numberRed;
        command03.GetComponent<Command>().currentRedNumber = numberRed;

        command01.GetComponent<Command>().currentYellowNumber = numberYellow;
        command02.GetComponent<Command>().currentYellowNumber = numberYellow;
        command03.GetComponent<Command>().currentYellowNumber = numberYellow;

        command01.GetComponent<Command>().currentBlueNumber = numberBlue;
        command02.GetComponent<Command>().currentBlueNumber = numberBlue;
        command03.GetComponent<Command>().currentBlueNumber = numberBlue;

        command01.GetComponent<Command>().SetAllText();
        command02.GetComponent<Command>().SetAllText();
        command03.GetComponent<Command>().SetAllText();
    }

    IEnumerator ShowMoneyEarnedText(float duration, int _moneyToAdd)
    {
        moneyEarnedText.GetComponent<Text>().text = "+" + _moneyToAdd + "$";
        moneyEarnedText.SetActive(true);

        yield return new WaitForSeconds(duration);

        moneyEarnedText.SetActive(false);
    }

    public void AddToShip(objectColor _colorToAdd)
    {
       switch (_colorToAdd)
        {
            case objectColor.red:
                command01.GetComponent<Command>().currentRedNumber += 1;
                command01.GetComponent<Command>().SetAllText();
                break;
            case objectColor.yellow:
                command01.GetComponent<Command>().currentYellowNumber += 1;
                command01.GetComponent<Command>().SetAllText();
                break;
            case objectColor.blue:
                command01.GetComponent<Command>().currentBlueNumber += 1;
                command01.GetComponent<Command>().SetAllText();
                break;
        }
    }
}

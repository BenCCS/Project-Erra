using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
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

    // Phase 2
    [Header("Phase")]
    public bool isInPhase2 = false;
    public Transform phase1CameraTransform;
    public Transform phase2CameraTransform;

    public LandingPad landingPad;

    // New gameplay
    public int spawnNumber = 5;
    public int spawnCount = 0;
    public int error = 0;
    public Text errorText;

    public int MaxCommands = 10;
    public int numberOfCommands = 10;
    public int currentNumberOfCommands = 0;

    public GameObject truckObject;
    public Transform truckSpawnPoint;
    public bool canSpawnTrucks = false;

    public objectColor mouseSelectedColor;

    private void Start()
    {
        gameHUD.SetActive(false);
        startMenu.SetActive(true);
        endMenu.SetActive(false);
        moneyEarnedText.SetActive(false);

        Camera.main.transform.position = phase1CameraTransform.position;

        originalTimeScale = Time.timeScale;

        spawnIntervalBase = spawnInterval;

        redNumberText.text = "0";
        yellowNumberText.text = "0";
        blueNumberText.text = "0";

        arrivageRandom.GetComponent<Image>().color = Color.gray;
        multiply1.GetComponent<Image>().color = Color.gray;

        nextSpawn = Time.time + spawnInterval;

        cameraMovement.canMove = canControlTheCam;

        spawnNumber = Random.Range(5, 16);
        spawnCount = 0;


        Debug.Log(spawnNumber);
    }

    public void Update()
    {
        if (Time.time >= nextSpawn && canSpawn && spawnNumber > spawnCount)
        {
            nextSpawn = Time.time + spawnInterval;

            spawnSlidingObject();
        }

        if (Time.time >= nextSpawn &&  canSpawnTrucks)
        {
            nextSpawn = Time.time + spawnInterval;

            SpawnSpaceTruck();
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            NextPhase();
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

        if (slowmotionIsActivate == false)
        {
            
        }
        else
        {
            slidingObjectRef.GetComponent<SlidingObject>().SetSpeed(true);
        }

        spawnCount += 1;

        if (spawnNumber <= spawnCount)
        {
            NextPhase();
        }

    }

    public void SpawnSpaceTruck()
    {
        GameObject truckObjectRef = Instantiate(truckObject, truckSpawnPoint);
        truckObjectRef.GetComponent<SpaceTruck>().miniGameManager = this;
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
        //spawnSlidingObject();

        canSpawn = true;

        playerScore = 0;

        scoreText.text = new string("Money: " + playerScore + "$");

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

            int moneyToAdd = Random.Range(5, 16);

            numberRed -= _Command.GetComponent<Command>().redNumberNeeded;
            if (numberRed < 0)
            {
                numberRed = 0;
            }
            numberYellow -= _Command.GetComponent<Command>().yellowNumberNeeded;
            if (numberYellow < 0)
            {
                numberYellow = 0;
            }
            numberBlue -= _Command.GetComponent<Command>().blueNumberNeeded;
            if (numberBlue < 0)
            {
                numberBlue = 0;
            }

            blueNumberText.text = numberBlue.ToString();
            yellowNumberText.text = numberYellow.ToString();
            redNumberText.text = numberRed.ToString();

            _Command.GetComponent<Command>().SetCommand();
            _Command.GetComponent<Command>().currentRedNumber = numberRed;
            _Command.GetComponent<Command>().currentBlueNumber = numberBlue;
            _Command.GetComponent<Command>().currentYellowNumber = numberYellow;
            _Command.GetComponent<Command>().SetAllText();

            SetAllCommandText();

            playerScore += moneyToAdd;
            StartCoroutine(ShowMoneyEarnedText(1f, moneyToAdd));
            scoreText.text = new string("Money: " + playerScore + "$");
        }

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



    public void NextPhase()
    {
        if(isInPhase2 == false)
        {
            scoreText.gameObject.SetActive(true);
            errorText.gameObject.SetActive(false);

            spawnInterval = 10;
            isInPhase2 = true;
            canSpawn = false;
            canSpawnTrucks = true;
            CalculateNumberOfCommands();
            Camera.main.transform.position = phase2CameraTransform.position;
            if (landingPad.UIisHide == true)
            {
                landingPad.HideUI();
            }
        }
        else
        {
            isInPhase2 = false;
            canSpawn = true;
            Camera.main.transform.position = phase1CameraTransform.position;
        }
    }

    public void AddError()
    {
        error += 1;
        errorText.text = "Errors : " + error;
    }

    public void CalculateNumberOfCommands()
    {
       
        if (error == spawnCount)
        {
            GameOver();
        }
        else if(error == 0) 
        {
            numberOfCommands = MaxCommands;
        }
        else
        {

            int temp = error * 100;
            int temp2 = temp / spawnNumber;

            int temp3 = temp * MaxCommands;
            int temp4 = temp3 / 100;

            numberOfCommands = MaxCommands - temp4;

            Debug.Log(temp2);
        }
    }
}

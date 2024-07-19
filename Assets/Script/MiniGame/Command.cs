using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class Command : MonoBehaviour
{
    [Header("Resources")]
    public int currentRedNumber = 0;
    public int redNumberNeeded = 0;

    public int currentBlueNumber = 0;
    public int blueNumberNeeded = 0;

    public int currentYellowNumber = 0;
    public int yellowNumberNeeded = 0;

    [Header("UI")]
    public Image redImage;
    public Image blueImage;
    public Image yellowImage;

    public Text redText;
    public Text blueText;
    public Text yellowText;

    void Start()
    {
        SetCommand();
    }

    
    void Update()
    {
        
    }

    public void SetText(Text _text, int currentNumber,int neededNumber)
    {
        _text.text = currentNumber.ToString() + "/" + neededNumber.ToString();
    }

    public void SetCommand()
    {

        currentRedNumber = 0;
        currentBlueNumber = 0 ;
        currentYellowNumber = 0 ;

        redNumberNeeded = Random.Range(3, 6);
        blueNumberNeeded = Random.Range(3, 6);
        yellowNumberNeeded = Random.Range(3, 6);

        SetAllText();
    }

    public bool CheckCommand()
    {

        if (currentBlueNumber >= blueNumberNeeded && currentYellowNumber >= yellowNumberNeeded && currentRedNumber >= redNumberNeeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetAllText()
    {
        SetText(redText, currentRedNumber, redNumberNeeded);
        SetText(blueText, currentBlueNumber, blueNumberNeeded);
        SetText(yellowText, currentYellowNumber, yellowNumberNeeded);
    }
}

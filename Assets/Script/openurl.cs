using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openurl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public void btn_steam_dlc()
    {
        Application.OpenURL("");
    }

    public void btn_reddit()
    {
        Application.OpenURL("");
    }


    // OTHER
    public void openfolder()
    {
        Application.OpenURL("file://MyPictures");
    }


    }

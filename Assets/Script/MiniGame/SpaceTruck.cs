using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTruck : MonoBehaviour
{
    public float startX = 0f;
    public float endX = 10f;

    public float startZ = 0f;
    public float endZ = 10f;

    public float duration = 5f;
    public float slowDuration = 10f;
    private float baseDuration;
    private float startTime;

    public float timePassed;

    public GameObject packageRed;
    public GameObject packageyellow;
    public GameObject packageBlue;
    

    public MiniGameManager miniGameManager;
    void Start()
    {
        packageBlue.SetActive(false);
        packageRed.SetActive(false);
        packageyellow.SetActive(false);

        startTime = Time.time;
        baseDuration = duration;
    }

    void Update()
    {
        timePassed = Time.time - startTime;
        float t = Mathf.Clamp01(timePassed / duration);


        float newX = Mathf.Lerp(startX, endX, t);
        float newZ = Mathf.Lerp(startZ, endZ, t);


        Vector3 newPosition = transform.position;
        newPosition.x = newX;
        newPosition.z = newZ;
        transform.position = newPosition;


        if (transform.position.x == endX && transform.position.z == endZ || t == 1)
        {
            //miniGameManager.SetScore(25);
            Destroy(gameObject);
        }

        //if (Input.GetMouseButtonDown(0))
        //{

        //    {
        //        RaycastHit hit;
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //        if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        //        {
        //            Addpackage(miniGameManager.mouseSelectedColor);
        //        }
        //    }
        //}
    }

    //public void Addpackage(objectColor objectColor)
    //{
    //    switch (objectColor)
    //    {
    //        case objectColor.red:
    //            packageRed.SetActive(true); 
    //            break;
    //        case objectColor.blue: 
    //            packageBlue.SetActive(true); 
    //            break;
    //        case objectColor.yellow:
    //            packageyellow.SetActive(true);
    //            break;
    //    }
    //}
}

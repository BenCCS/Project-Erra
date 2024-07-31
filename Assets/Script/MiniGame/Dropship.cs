using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropship : MonoBehaviour
{
    public GameObject commandUI;

    void Start()
    {
        commandUI.SetActive(false);
    }

     void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                commandUI.SetActive(true);
            }
            else
            {
                commandUI.SetActive(false);
            }
           
        }
    }
}

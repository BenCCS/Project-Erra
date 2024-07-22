using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPad : MonoBehaviour
{
    public GameObject arrivageUI;
    private bool UIisHide = false;

    public GameObject container01;
    public GameObject container02;
    public GameObject container03;
    public GameObject container04;

    public Mesh[] containerMesh;

    void Start()
    {
        HideUI();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                
                if (!UIisHide)
                {
                    UIisHide = true;
                    arrivageUI.SetActive(true);
                }
                //else
                //{
                //    UIisHide = false;
                //    arrivageUI.SetActive(false);
                //}
            }
            else
            {
                if (UIisHide)
                {
                   Invoke("HideUI",1f);
                }
            }
        }

    }

    public void HideUI()
    {
        arrivageUI.SetActive(false);
        UIisHide = false;
    }

    public void setContainerColor(string _color)
    {
        switch(_color)
        {
            case "red":
                container01.GetComponent<MeshFilter>().mesh = containerMesh[0];
                container02.GetComponent<MeshFilter>().mesh = containerMesh[0];
                container03.GetComponent<MeshFilter>().mesh = containerMesh[0];
                container04.GetComponent<MeshFilter>().mesh = containerMesh[3];
                break;
            case "blue":
                container01.GetComponent<MeshFilter>().mesh = containerMesh[1];
                container02.GetComponent<MeshFilter>().mesh = containerMesh[1];
                container03.GetComponent<MeshFilter>().mesh = containerMesh[1];
                container04.GetComponent<MeshFilter>().mesh = containerMesh[3];
                break;
            case "yellow":
                container01.GetComponent<MeshFilter>().mesh = containerMesh[2];
                container02.GetComponent<MeshFilter>().mesh = containerMesh[2];
                container03.GetComponent<MeshFilter>().mesh = containerMesh[2];
                container04.GetComponent<MeshFilter>().mesh = containerMesh[3];
                break;
            case "random":
                container01.GetComponent<MeshFilter>().mesh = containerMesh[0];
                container02.GetComponent<MeshFilter>().mesh = containerMesh[1];
                container03.GetComponent<MeshFilter>().mesh = containerMesh[2];
                container04.GetComponent<MeshFilter>().mesh = containerMesh[3];
                break;
        }
    }
}

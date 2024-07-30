using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock : MonoBehaviour
{

    public objectColor stockColor;
    private int colorIndex = 0;

    public GameObject[] ingredientPrefabs;
    private GameObject currentIngredient;
    private bool isHoldingIngredient = false;

    private GameObject selectedIngredient = null;
    private Camera mainCamera;

    public MiniGameManager miniGameManager;

    private void Start()
    {
        switch (stockColor)
        {
            case objectColor.blue:
                colorIndex = 1;
                break;
            case objectColor.green:
                break;
            case objectColor.red:
                colorIndex = 0;
                break;
            case objectColor.yellow:
                colorIndex = 2;
                break;

        }

        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                {
                    miniGameManager.mouseSelectedColor = stockColor;
                    Debug.Log(miniGameManager.mouseSelectedColor);
                    Debug.Log(this.gameObject.name);
                }
            }
        }

        //void SelectIngredient()
        //{
        //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        foreach (GameObject prefab in ingredientPrefabs)
        //        {
        //            if (hit.collider.gameObject == prefab)
        //            {
        //                selectedIngredient = Instantiate(prefab, hit.point, Quaternion.identity);
        //                break;
        //            }
        //        }
        //    }
        //}

    }

}


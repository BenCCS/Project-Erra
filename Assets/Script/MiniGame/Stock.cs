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

    [Header("Lines")]
    public LineRenderer linerendererRed;
    public LineRenderer linerendererBlue;
    public LineRenderer linerendererYellow;
    public LineRenderer linerendererAll;
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

                switch (stockColor )
                {
                    case objectColor.blue:
                        CheckStockage(miniGameManager.numberBlue, 1, linerendererBlue, stockColor);
                        break;
                    case objectColor.red:
                        CheckStockage(miniGameManager.numberRed, 0, linerendererRed,stockColor);
                        break;
                    case objectColor.yellow:
                        CheckStockage(miniGameManager.numberYellow, 2, linerendererYellow, stockColor);
                        break;
                }

                }
            }
        }

    }
    private void CheckStockage(int minigameStock, int prefabIndex, LineRenderer linerenderer, objectColor _Color)
    {
        if (minigameStock > 0)
        {
            miniGameManager.SubstractStock(_Color);
            GameObject containerRef = Instantiate(ingredientPrefabs[prefabIndex]);
            containerRef.GetComponent<Containers>().lineRenderer = linerenderer;
            containerRef.GetComponent<Containers>().lineRendererAll = linerendererAll;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropship : MonoBehaviour
{
    public GameObject commandUI;

    public Transform landingPosition;
    public Transform endPoistion;
    public Transform startPoistion;

    public GameObject spaceShip;
    void Start()
    {
        commandUI.SetActive(false);
        Land();
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

    public void Fly()
    {
        StartCoroutine(Move(landingPosition.position,endPoistion.position,3f));
        StartCoroutine(Delay(1f));
        Land();
    }

    public void Land()
    {
        StartCoroutine(Move(startPoistion.position,landingPosition.position,2f));
    }

    private IEnumerator Move(Vector3 start, Vector3 end, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            spaceShip.transform.position = Vector3.Lerp(start, end, elapsedTime/time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spaceShip.transform.position = end;
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }
}

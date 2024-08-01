using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Containers : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public LineRenderer lineRendererAll;
    public objectColor selectedColor;

    public MiniGameManager miniGameManager;

    public float speed = 6.0f;

    private int currentIndex = 0;
    private float t = 0.0f; 

    private bool canMove = true;
    private bool endMove = false;

    public GameObject destroyVFX;

    void Start()
    {
        transform.position = lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(0));
    }

    void Update()
    {
        if (canMove)
        {
            Vector3 startPosition = lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(currentIndex));
            Vector3 endPosition = lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(currentIndex + 1));

            // Interpoler entre les deux points
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // Incrémenter t en fonction de la vitesse
            t += Time.deltaTime * speed / Vector3.Distance(startPosition, endPosition);

            // Si t dépasse 1, passer au prochain segment de la ligne
            if (t >= 1.0f)
            {
                t = 0.0f;
                currentIndex++;

                if (currentIndex >= lineRenderer.positionCount - 1)
                {
                    // currentIndex = 0; // Réinitialiser ou arrêter le mouvement
                    canMove = false;
                    if (endMove == false)
                    {
                        SetSecondMovement();
                        endMove = true;
                    }
                    else 
                    {
                        miniGameManager.AddToShip(selectedColor);
                        Instantiate(destroyVFX, transform.position, Quaternion.identity);
                        Destroy(this.gameObject);
                    }
                }
            }

        }
        
    }

    private void SetSecondMovement()
    {
        lineRenderer = lineRendererAll.GetComponent<LineRenderer>();
        transform.position = lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(0));
        t = 0.0f; 
        currentIndex = 0;
        canMove = true;
    }

}

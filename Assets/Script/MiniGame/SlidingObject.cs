using UnityEngine;

public class SlidingObject : MonoBehaviour
{

    public objectColor selectedColor;
    public Mesh[] objectMeshes;
    public MeshFilter _meshFilter;
    public TrailRenderer _trailRenderer;

    private Vector3 offset;
    private bool isDragging = false;
    private float slideSpeed = 0.0f;
    private Vector3 lastMousePosition;

    public StockageArea stockAreaRef;

    
    public float sensitivity = 0.1f;
    public float friction = 0.98f;
    public float leftLimit = -5f; 
    public float rightLimit = 5f; 

    public bool canBeDragged = true;

    public MiniGameManager _miniGameManager;

    private bool canCheckSuccess = false;

    [Header("VFX")]
    public GameObject sucessVFX;
    public GameObject unSucessVFX;


    public float startX = 0f; 
    public float endX = 10f; 
    public float duration = 5f; 
    private float startTime;


    void Start()
    {
        startTime = Time.time;
    }


    public void SetObjectColor(objectColor color)
    {
        selectedColor = color;
        switch (color)
        {
            case objectColor.red:
                _meshFilter.mesh = objectMeshes[0];
                _trailRenderer.startColor = Color.red;
                break;
            case objectColor.yellow:
                _meshFilter.mesh = objectMeshes[1];
                _trailRenderer.startColor = Color.yellow;
                break;
            case objectColor.blue:
                _meshFilter.mesh = objectMeshes[2];
                _trailRenderer.startColor = Color.blue;
                break;
            case objectColor.green:
                _meshFilter.mesh = objectMeshes[3];
                _trailRenderer.startColor = Color.green;
                break;
        }
    }


    void Update()
    {
        //    if (Input.GetMouseButtonDown(0) && canBeDragged)
        //    {
        //        RaycastHit hit;
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //        if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        //        {

        //            isDragging = true;
        //            offset = transform.position - GetMouseWorldPosition();
        //            lastMousePosition = GetMouseWorldPosition();
        //            canCheckSuccess = true;
        //        }
        //    }
        //    else if (Input.GetMouseButtonUp(0))
        //    {
        //        if (isDragging)
        //        {

        //            Vector3 mouseEndPosition = GetMouseWorldPosition();
        //            slideSpeed = (mouseEndPosition - lastMousePosition).x / Time.deltaTime * sensitivity;
        //            isDragging = false;
        //        }
        //    }

        //    if (isDragging && canBeDragged)
        //    {

        //        Vector3 mousePosition = GetMouseWorldPosition() + offset;
        //        float newXPosition = Mathf.Clamp(mousePosition.x, leftLimit, rightLimit); // Limiter la position en X
        //        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
        //        lastMousePosition = GetMouseWorldPosition();
        //    }
        //    else
        //    {
        //        transform.position += new Vector3(slideSpeed, 0, 0) * Time.deltaTime;
        //        slideSpeed *= friction;

        //        if (Mathf.Abs(slideSpeed) < 0.01f)
        //        {
        //            slideSpeed = 0;
        //            if (canCheckSuccess)
        //            {
        //                CheckSuccess();
        //            }
        //        }

        //        if (transform.position.x < leftLimit)
        //        {
        //            transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);
        //        }
        //    }

        float timePassed = Time.time - startTime;
        float t = Mathf.Clamp01(timePassed / duration);

        
        float newX = Mathf.Lerp(startX, endX, t);

        
        Vector3 newPosition = transform.position;
        newPosition.x = newX;
        transform.position = newPosition;

        
        if (t >= 1f)
        {
            if (selectedColor != objectColor.green)
            {

            }
            //Instantiate(unSucessVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {

                //isDragging = true;
                //offset = transform.position - GetMouseWorldPosition();
                //lastMousePosition = GetMouseWorldPosition();
                canCheckSuccess = true;
                CheckSuccess();
            }
        }
    }

    //Vector3 GetMouseWorldPosition()
    //{
    //    Vector3 mousePoint = Input.mousePosition;
    //    mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
    //    return Camera.main.ScreenToWorldPoint(mousePoint);
    //}

    //public void SetCanBeDragged(bool _canBeDragged)
    //{
    //    canBeDragged = _canBeDragged;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StockageArea"))
        {
            stockAreaRef = other.GetComponent<StockageArea>();
        }
        else
        {
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("StockageArea"))
        {
            stockAreaRef = null;
        }
    }

    public void CheckSuccess()
    {
        canCheckSuccess = false;

        if (stockAreaRef != null)
        {
            if (stockAreaRef.requiredColor == selectedColor)
            {
                _miniGameManager.AddStock(selectedColor);
                Instantiate(sucessVFX, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                Instantiate(unSucessVFX, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else
        {
            Instantiate(unSucessVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}



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
    public float slowDuration = 10f;
    private float baseDuration;
    private float startTime;


    void Start()
    {
        startTime = Time.time;
        baseDuration = duration;
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

                canCheckSuccess = true;
                CheckSuccess();
            }
        }
    }

    public void SetSpeed(bool slow)
    {

        if (slow == false)
        {
            duration = baseDuration;
        }
        else
        {
            duration = slowDuration;
        }

    }

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
                _miniGameManager.AddError();
                Instantiate(unSucessVFX, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else
        {
            _miniGameManager.AddError();
            Instantiate(unSucessVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void DestroyGameObject()
    {
        Instantiate(sucessVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}



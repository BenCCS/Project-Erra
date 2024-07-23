using UnityEngine;
using UnityEngine.UI;

public class StockageArea : MonoBehaviour
{
    public objectColor requiredColor;

    public int level = 1;

    public int updatePrice = 20;

    public MiniGameManager miniGameManager;

    public GameObject[] container;

    public GameObject upgradeBtn;
    public Text priceText;
    private bool UIisHide = true;

    private bool canGrabObject = false;
    public float grabInterval = 20f;
    private float nextGrab = 0;

    private void Start()
    {
        for (int i = 0; i < container.Length; i++)
        {
            container[i].SetActive(false);
        }
        HideUI();
    }

    private void Update()
    {

        if (Time.time > nextGrab)
        {
            nextGrab = Time.time + grabInterval;
            canGrabObject = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                priceText.text = updatePrice.ToString();
                UIisHide = false;
                upgradeBtn.SetActive(true);
            }
            else
            {
                if (UIisHide == false)
                {
                    Invoke("HideUI", 1f);
                }
            }
        }
    }

    public void HideUI()
    {
        upgradeBtn.SetActive(false);
        UIisHide = true;
    }

    public void UpdateBat()
    {

        if (miniGameManager.playerScore >= updatePrice)
        {

            miniGameManager.playerScore -= updatePrice;
            miniGameManager.scoreText.text = miniGameManager.playerScore.ToString();

            updatePrice *= 2;
            level += 1;

            if (level >= 2 )
            {
                for (int i = 0; i < container.Length; i++)
                {
                    container[i].SetActive(true);
                }
            }

        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (canGrabObject && level >= 2)
        {
            if (other.gameObject.tag == "Sliding Object")
            {
                if (other.GetComponent<SlidingObject>().selectedColor == requiredColor)
                {
                    other.GetComponent<SlidingObject>().DestroyGameObject();
                    GrabResource();
                }
            }
        }

    }

    private void GrabResource()
    {
        miniGameManager.AddStock(requiredColor);
        Debug.Log("Grab");
        canGrabObject = false;
    }

}

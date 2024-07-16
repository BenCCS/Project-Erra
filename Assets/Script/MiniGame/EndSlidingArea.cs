using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSlidingArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Sliding Object"))
        {
            other.GetComponent<SlidingObject>().SetCanBeDragged(false);
        }
    }
}

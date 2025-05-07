using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertigoTrigger : MonoBehaviour
{

    public GameObject vertigoFeet;
    public GameObject afterImage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && vertigoFeet != null && !vertigoFeet.activeSelf) 
        {
            vertigoFeet.SetActive(true);
        }
        if(collision.CompareTag("Player") &&afterImage!=null && afterImage.activeSelf)
        {
            afterImage.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShowImage : MonoBehaviour
{
    public GameObject uiImage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiImage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiImage.SetActive(false);
        }
    }
}

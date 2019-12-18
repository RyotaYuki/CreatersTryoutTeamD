using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLEnterCheck : MonoBehaviour
{
    private bool enterFlag = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            enterFlag = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enterFlag = false;
        }
    }

    public bool GetEnterFlag()
    {
        return enterFlag;
    }
}
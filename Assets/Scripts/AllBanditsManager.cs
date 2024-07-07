using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllBanditsManager : MonoBehaviour
{

    public void Awake()
    {
        // etc 
    }
    public void BlockInput(bool flag)
    // this should work for laptop, browsers and touchscreens 
    {
        AllCollidersEnabled(!flag);
    }


    public void ActivateBandits(bool flag)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(flag);
        }
    }


    private void AllCollidersEnabled(bool enabled)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            PolygonCollider2D polyCollider = child.GetComponent<PolygonCollider2D>();
            if (polyCollider != null)
            {
                polyCollider.enabled = enabled;
            }
        }
    }

    public void ShuffleBandits()
    {
        //shuffle bandit positions I guess someow 
    }
}







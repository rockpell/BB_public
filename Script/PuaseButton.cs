using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuaseButton : MonoBehaviour {

    public GameObject PauseText;
    private bool PuaseState;

    private void Start()
    {
        PuaseState = false;
    }

    public void OnClickPause()
    {
        if (PuaseState == false)
        {
            PauseText.SetActive(true);
            PuaseState = true;
        }
        else
        {
            PauseText.SetActive(false);
            PuaseState = false;
        }

    }
}

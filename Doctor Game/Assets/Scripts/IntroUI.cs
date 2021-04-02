using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroUI : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject instructions;

    // Start is called before the first frame update
    void Start()
    {
        if (!Stats.PlayedOnce)
        {
            instructions.SetActive(true);
            mainUI.SetActive(false);
            Stats.PlayedOnce = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(instructions.activeSelf && Input.GetKeyDown(KeyCode.Mouse0))
        {
            instructions.SetActive(false);
            mainUI.SetActive(true);
        }
    }
}

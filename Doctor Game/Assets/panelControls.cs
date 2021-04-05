using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panelControls : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panel;
    void Start()
    {
        panel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.Mouse0))
        {
            panel.SetActive(false);
        }
    }
}

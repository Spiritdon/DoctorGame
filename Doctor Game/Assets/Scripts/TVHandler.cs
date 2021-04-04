using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVHandler : MonoBehaviour
{
    public GameObject sofa;
    public GameObject TVCam;
    public GameObject player;
    //public GameObject panel;
    public GameObject mainCam;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        sofa.transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
        player.SetActive(false);
        mainCam.SetActive(false);
        TVCam.SetActive(true);
        Stats.DestressUsed("TV");
    }

    public void Deactivate()
    {
        sofa.transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
        player.SetActive(true);
        TVCam.SetActive(false);
        mainCam.SetActive(true);
    }
}

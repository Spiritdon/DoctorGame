using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerZoom : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject flowerCam;
    public GameObject player;
    public GameObject flowerPanel;
    private bool exit;

    void Start()
    {
        exit = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (InRange())
        {
            CamToggle();
        }
    }

    public void CamToggle()
    {

        if (!exit)
        {
            mainCam.SetActive(false);
            flowerCam.SetActive(true);
            flowerPanel.SetActive(true);
        }
        else
        {
            flowerCam.SetActive(false);
            mainCam.SetActive(true);
            flowerPanel.SetActive(false);
        }
    }

    public bool InRange()
    {
        return gameObject.GetComponent<Collider>().bounds.Contains(player.transform.position);
    }

    public float DistanceToPlayer()
    {
        return Mathf.Sqrt(
            Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) * Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) + // a^2
            Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) * Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) + // b^2
            Mathf.Abs(player.transform.position.z - gameObject.transform.position.z) * Mathf.Abs(player.transform.position.z - gameObject.transform.position.z) // a^2
        );
    }

    public void ExitBttnClicked()
    {
        exit = true;
        CamToggle();
    }   
}

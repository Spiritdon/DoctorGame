using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    private ImgScale img;
    private Transform mainCam;

    void Start()
    {
        mainCam = GameObject.Find("Main Camera").transform;
        img = this.GetComponentInChildren<ImgScale>();
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCam.forward);
    }

    public void SetHealth(int health)
    {
        if (img != null)
        {
            switch (health)
            {
                case 7:
                    img.ChangeScale(new Vector3(1, 1, 1));
                    break;
                case 6:
                    img.ChangeScale(new Vector3(0.9f, 1, 1));
                    break;
                case 5:
                    img.ChangeScale(new Vector3(0.8f, 1, 1));
                    break;
                case 4:
                    img.ChangeScale(new Vector3(0.6f, 1, 1));
                    break;
                case 3:
                    img.ChangeScale(new Vector3(0.4f, 1, 1));
                    break;
                case 2:
                    img.ChangeScale(new Vector3(0.2f, 1, 1));
                    break;
                case 1:
                    img.ChangeScale(new Vector3(0.1f, 1, 1));
                    break;
                case 0:
                    Destroy(this.gameObject);
                    break;
            }
        }
    }
}

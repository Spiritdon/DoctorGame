using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurgeryMouseControl : MonoBehaviour
{
    GameObject held = null;
    Camera cam;
    Vector3 prevMousePos;
    Vector3 mousePos;
    Vector3 center;
    public float heldSeekSpeed = 5;

    public GameObject Held
    {
        get
        {
            return held;
        }
    }

    public Vector3 MousePos
    {
        get
        {
            return mousePos;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        if (Input.GetMouseButtonDown(0) && held == null)
        {
            RaycastHit hit;

            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                held = hit.transform.gameObject;
                center = hit.transform.position;
            }
        }
        else if(Input.GetMouseButtonDown(0))
        {
            held = null;
        }

        UpdateHeldPos();

        prevMousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }

    void UpdateHeldPos()
    {
        if (held != null)
        {
            mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

            Vector3 centerToMouse = mousePos - center;
            //if (mouseVelocity.sqrMagnitude < 1)
            ///    mouseVelocity = Vector3.zero;
            //else
            //mouseVelocity.Normalize();

            Vector3 heldPos = mousePos + centerToMouse / 5 * (held.transform.position - center).magnitude / 2;

            //held.GetComponent<Rigidbody>().AddForce(forceDir * heldSeekSpeed);
            held.transform.position = heldPos;
        }
    }
}

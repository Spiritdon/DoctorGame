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
    LineRenderer lineRenderer;
    int test1;
    int test2;
    Vector2 lastPoint;
    public List<GameObject> lines;
    public GameObject line;

    private void Start()
    {
        test1 = 0;
        test2 = 0;
    }

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
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                CreatingLine();
            }

            Vector2 heldPosition = held.transform.position;
            if (heldPosition != lastPoint)
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    AddNewPoint(heldPosition);
                }

                lastPoint = heldPosition;
            }

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
    //Line Creation
    private void CreatingLine()
    {
        test1++;
        //Debug.Log("Create Line Trigger:" + test1);

        GameObject lineInstance = Instantiate(line);
        lines.Add(lineInstance);
        lineRenderer = lineInstance.GetComponent<LineRenderer>();

        Vector2 heldPosition = held.transform.position;

        lineRenderer.SetPosition(0, heldPosition);
        lineRenderer.SetPosition(1, heldPosition);
    }
    private void AddNewPoint(Vector2 _lastPoint)
    {
        test2++;
        //Debug.Log("Create Points:" + test2);
        lineRenderer.positionCount++;
        int positionIndex = lineRenderer.positionCount - 1;
        lineRenderer.SetPosition(positionIndex, _lastPoint);
    }
}

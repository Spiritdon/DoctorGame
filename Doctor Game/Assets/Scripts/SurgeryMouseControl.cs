using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurgeryMouseControl : MonoBehaviour
{
    public GameObject held;
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
    public GameObject cursor;
    bool canDraw = true;

    public bool CanDraw
    {
        set
        {
            canDraw = value;
        }
    }

    private void Start()
    {
        test1 = 0;
        test2 = 0;
        Cursor.visible = false;
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
        if (Input.GetMouseButtonDown(0))
        {
            if (held.transform.childCount <= 0)
            {
                //center = held.transform.position;

                RaycastHit hit;

                if (Physics.Raycast(cam.transform.position, held.transform.position - cam.transform.position, out hit))
                {
                    hit.transform.parent = held.transform;
                }
            }
            else
            {
                held.transform.GetChild(0).parent = null;
                //center = held.transform.position;
            }
        }

        UpdateHeldPos();

        prevMousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }

    void UpdateHeldPos()
    {
        if (held != null)
        {
            if (canDraw)
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

                
            }

            mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

            Vector3 centerToMouse = mousePos - center;

            Vector3 heldPos = mousePos + centerToMouse / 5 * (held.transform.position - center).magnitude / 2;
            heldPos = new Vector3(Mathf.Clamp(heldPos.x, -10f, 10f), Mathf.Clamp(heldPos.y, -4.75f, 2.5f), heldPos.z);

            held.transform.position = heldPos;
        }
    }

    public void DropHeldAt(Vector3 pos)
    {
        if (held.transform.childCount > 0)
        {
            GameObject heldItem = held.transform.GetChild(0).gameObject;
            heldItem.transform.parent = null;
            held.transform.parent = null;
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

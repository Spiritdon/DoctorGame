using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject brush;
    public Camera mainCamera;
    LineRenderer lineRenderer;
    public List<GameObject> lines;
    public Text currentColorText;
    public BoxCollider2D uiCollider;
    Vector2 lastPos;
    Color selectedColorTool;
    public bool paintingBool;
    void Start()
    {
        selectedColorTool = Color.black;
        currentColorText.text = "Current Color/Tool:Black";
        paintingBool = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))//eraser white
        {
            selectedColorTool = Color.white;
            currentColorText.text = "Current Color/Tool:Erasers/White";
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))//black
        {
            selectedColorTool = Color.black;
            currentColorText.text = "Current Color/Tool:Black";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))//red
        {
            selectedColorTool = Color.red;
            currentColorText.text = "Current Color/Tool:Red";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))//blue
        {
            selectedColorTool = Color.blue;
            currentColorText.text = "Current Color/Tool:Blue";
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))//green
        {
            selectedColorTool = Color.green;
            currentColorText.text = "Current Color/Tool:Green";
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))//yellow
        {
            selectedColorTool = Color.yellow;
            currentColorText.text = "Current Color/Tool:Yellow";
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))//cyan
        {
            selectedColorTool = Color.cyan;
            currentColorText.text = "Current Color/Tool:Cyan";
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))//gray
        {
            selectedColorTool = Color.gray;
            currentColorText.text = "Current Color/Tool:Gray";
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))//magenta
        {
            selectedColorTool = Color.magenta;
            currentColorText.text = "Current Color/Tool:Magenta";
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))//Unique
        {
            selectedColorTool = Color.HSVToRGB(0.5f,0.8f,0.3f);
            currentColorText.text = "Current Color/Tool:Custom Color Dark Teal";
        }
        if (paintingBool)
        {
            Draw();
        }
        else
        {

        }
        
        if (Input.GetKeyDown(KeyCode.Z) && lines.Count > 0)
        {
            Destroy(lines[lines.Count - 1]);
            lines.RemoveAt(lines.Count - 1);
        }
    }
    private void Draw()
    {
        //dont draw
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreatePaintBrush();
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 mousepos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (mousepos != lastPos)
            {
                AddinANewPoint(mousepos);
                lastPos = mousepos;
            }
        }
        else
        {
            lineRenderer = null;
        }

    }
    private void CreatePaintBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        lines.Add(brushInstance);
        lineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        lineRenderer.endColor = selectedColorTool;
        lineRenderer.startColor = selectedColorTool;
        lineRenderer.SetPosition(0,mousePos);
        lineRenderer.SetPosition(1, mousePos);
    }

    private void AddinANewPoint(Vector2 pointPos)
    {
        lineRenderer.positionCount++;
        int positionIndex = lineRenderer.positionCount - 1;
        lineRenderer.SetPosition(positionIndex,pointPos);
    }
}

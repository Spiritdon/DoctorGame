using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brushControler : MonoBehaviour
{
    // Start is called before the first frame update
    private float widthValue;


    void Start()
    {
        widthValue = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        //if button is pressed reset a line
    }
    public void changeEndWidth(float _endwith)
    {
        changeWidth(widthValue);
        GetComponent<LineRenderer>().endWidth = _endwith;
    }
    public void changeStartWidth(float _startwith)
    {
        changeWidth(widthValue);
        GetComponent<LineRenderer>().startWidth = _startwith;
    }
    public void changeWidth(float _width)
    {
        widthValue = _width;
        GetComponent<LineRenderer>().startWidth = _width;
        GetComponent<LineRenderer>().endWidth = _width;
    }
}

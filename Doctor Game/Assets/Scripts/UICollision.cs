using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICollision : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public PaintManager PaintManager;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Debug.Log("Enter");
        PaintManager.paintingBool = false;
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Debug.Log("Exit");
        PaintManager.paintingBool = true;
    }
}

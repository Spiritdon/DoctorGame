using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImgScale : MonoBehaviour
{
    public void ChangeScale(Vector3 inputVector)
    {
        this.GetComponent<RectTransform>().localScale = inputVector;
        Debug.Log(this.transform.localScale);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressBar : MonoBehaviour
{
    public static float stress = 0;
    RectTransform bar;
    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stress != bar.localScale.x)
        {
            bar.localScale = new Vector3(stress, 1,1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressBar : MonoBehaviour
{
    public RectTransform bar;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Stats.Stress != bar.localScale.x)
        {
            bar.localScale = new Vector3(Stats.Stress / 10f, 1, 1);
        }
    }
}

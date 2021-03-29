using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressBar : MonoBehaviour
{
    public RectTransform bar;
    void Start()
    {
        bar.localScale = new Vector3(Stats.Stress / 10f, 1, 1);
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

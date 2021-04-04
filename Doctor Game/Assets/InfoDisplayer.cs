using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoDisplayer : MonoBehaviour
{
    public Text monotony;
    public Text stress;
    public Text sustainability;
    // Start is called before the first frame update
    void Start()
    {
        float varietyPercentage = Stats.Variety / 21f;
        varietyPercentage = 1f - varietyPercentage;

        float dayToDay = Stats.WentHomeStressed / 7f;

        if (dayToDay > 0.5f)
        {
            stress.transform.GetChild(0).gameObject.SetActive(true);
        }
        if (varietyPercentage > 0.5f)
        {
            monotony.transform.GetChild(0).gameObject.SetActive(true);
        }
        monotony.text += "" + Mathf.Clamp(varietyPercentage * 100, 0, 100).ToString("0") + "%";
        stress.text += "" + Mathf.Clamp(dayToDay * 100, 0, 100).ToString("0") + "%";

        sustainability.text += "" + Mathf.Clamp((1 - (dayToDay + varietyPercentage)/2f) * 100, 0, 100).ToString("0") + "%";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

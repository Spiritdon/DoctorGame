using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patients : MonoBehaviour
{
    public GameObject patientGameObject;
    private GameObject healthBar;
    public int health;
    private int cDay;

    void Awake()
    {
        if (patientGameObject.GetComponent<Patients>().healthBar != null)
        {
            Debug.Log("1");
            DontDestroyOnLoad(patientGameObject);
        }
        else
        {
            cDay = 1;
            healthBar = Resources.Load("HealthBar") as GameObject;
            Instantiate(healthBar, patientGameObject.transform, false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Stats.Day != cDay)
        {
            cDay++;
            health--;
            healthBar.GetComponentInChildren<Transform>().localScale =
                new Vector3(healthBar.transform.localScale.x - 0.2f, 1, 1);
            Debug.Log(healthBar.GetComponentInChildren<Transform>().localScale);
        }
    }
}
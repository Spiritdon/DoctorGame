using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patients : MonoBehaviour
{
    public GameObject patientWithBed;
    public GameObject patientObj;
    public GameObject bedObj;
    private GameObject healthBar;
    public int health;
    private int cDay;

    void Awake()
    {
        if (patientWithBed.GetComponent<Patients>().healthBar != null)
        {
            Debug.Log("1");
            DontDestroyOnLoad(patientWithBed);
        }
        else
        {
            cDay = 1;
            healthBar = Resources.Load("HealthBarCanvas") as GameObject;
            healthBar = Instantiate(healthBar, new Vector3(patientWithBed.transform.position.x, patientWithBed.transform.position.y + 10, patientWithBed.transform.position.z + 1.5f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (patientObj != null)
        {
            if (Stats.Day != cDay)
            {
                cDay++;
                health--;

                if (health == 0)
                {
                    Destroy(patientObj);
                    bedObj.GetComponent<Renderer>().material.color = Color.red;
                }
                else if (health >= 7)
                {
                    Destroy(patientObj);
                    bedObj.GetComponent<Renderer>().material.color = Color.blue;
                }
            }

            healthBar.GetComponentInChildren<HealthBar>().SetHealth(health);
        }
    }
}
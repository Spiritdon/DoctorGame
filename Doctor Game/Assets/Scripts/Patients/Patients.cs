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
            healthBar = Resources.Load("HealthBarCanvas") as GameObject;
            healthBar = Instantiate(healthBar, new Vector3(patientGameObject.transform.position.x, patientGameObject.transform.position.y + 10, patientGameObject.transform.position.z + 1.5f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Stats.Day != cDay)
        {
            cDay++;
            health--;

            if (health == 0)
            {
                Destroy(this.gameObject);
            }
        }

        healthBar.GetComponentInChildren<HealthBar>().SetHealth(health);
    }
}
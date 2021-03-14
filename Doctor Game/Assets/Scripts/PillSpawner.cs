using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillSpawner : MonoBehaviour
{
    private GameObject pillObject;
    public GameObject badPill;
    public GameObject goodPill;
    public List<GameObject> pills;
    // Start is called before the first frame update
    void Start()
    {
        pillObject = null;
        pills = new List<GameObject>();
        badPill.name = "bad";
        goodPill.name = "good";
        InvokeRepeating("SpawnPill", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        RemovePill();
    }
    void SpawnPill()
    {
        int pillType = Random.Range(1, 11);
        if (pillType <= 5)
        {
            pillObject = badPill;
        }
        else
        {
            pillObject = goodPill;
        }
        GameObject pillGameObject = Instantiate(pillObject, new Vector3(Random.Range(-4, 6), 10, -8), Quaternion.identity);
        pillGameObject.SetActive(true);
        pills.Add(pillGameObject);
        pillObject = null;
    }
    private void RemovePill()
    {
        GameObject[] temp = pills.ToArray();
        foreach (GameObject pill in temp)
        {

            if (pill.transform.position.y <= 0)
            {
                Debug.Log("Pill Destroyed");
                pills.Remove(pill);
                Destroy(pill);
            }
        }
    }
    public void CollectPill(GameObject pill)
    {
        GameObject[] temp = pills.ToArray();
        foreach (GameObject goodpill in temp)
        {
            if (goodpill.name == pill.name && pill.activeSelf == false)
            {
                var clonePill = goodpill;
                pills.Remove(goodpill);
                Destroy(clonePill);
            }
            break;
        }
    }

}

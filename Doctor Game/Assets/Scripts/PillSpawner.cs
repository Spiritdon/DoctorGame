using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Vector3 gravSpeed = new Vector3(0,-200,0);
        Physics.gravity = gravSpeed;
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
        GameObject pillGameObject = Instantiate(pillObject, new Vector3(Random.Range(660, 1276), 756, -45), Quaternion.identity);
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
        foreach (GameObject goodBadPill in temp)
        {
            if (goodBadPill.name == pill.name && pill.activeSelf == false)
            {
                var clonePill = goodBadPill;
                pills.Remove(goodBadPill);
                Destroy(clonePill);
            }
            break;
        }
    }

}

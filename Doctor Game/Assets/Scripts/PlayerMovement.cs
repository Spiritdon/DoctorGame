using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public ProgressSaver data;

    Camera cam;
    NavMeshAgent agent;
    public GameObject dest;

    [HideInInspector]
    public float stress;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();

        stress = 1.0f;

        gameObject.transform.position = data.playerPos;
        stress = data.stressLevel;
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                dest.transform.position = hit.point;
            }
        }

        if (Input.GetMouseButtonDown(1)) {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.gameObject.GetComponent<DestressorScript>() != null) {
                    hit.transform.gameObject.GetComponent<DestressorScript>().destress();
                }
                if (hit.transform.gameObject.GetComponent<StressorScript>() != null) {
                    hit.transform.gameObject.GetComponent<StressorScript>().stress();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            stress += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            stress -= 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            data.playerPos = transform.position;
            data.stressLevel = stress;
            SceneManager.LoadScene("Minigame1");
        }
    }
}

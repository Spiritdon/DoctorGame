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

        stress = 0;

        gameObject.transform.position = data.playerPos;
        stress = data.stressLevel;
    }

    // Update is called once per frame
    void Update() {

        data.playerPos = transform.position;
        data.stressLevel = stress;

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
            if (stress < 1)
            {
                stress += 0.1f;
                StressBar.stress = stress;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (stress > 0)
            {
                stress -= 0.1f;
                StressBar.stress = stress;
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("Minigame1");
        }
    }
}

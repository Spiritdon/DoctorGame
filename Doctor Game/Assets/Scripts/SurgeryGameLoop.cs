using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum SurgeryState { Incision, Extraction, Replacement };
public class SurgeryGameLoop : MonoBehaviour
{
    SurgeryState gameState;

    float stateTime = 30f;
    bool scalpelClicked = false;

    public GameObject organ1;
    public GameObject organ2;

    public GameObject organSpot1;
    public GameObject organSpot2;

    public GameObject incisionLinePrefab;
    GameObject[] incisionLines;

    // Start is called before the first frame update
    void Start()
    {
        gameState = SurgeryState.Incision;
        incisionLines = new GameObject[2];
        for (int i = 0; i < 2; i++)
        {
            GenerateIncisionMarker(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState == SurgeryState.Incision)
        {
            if (scalpelClicked)
            {
                stateTime -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                float width = Mathf.Abs(incisionLines[0].GetComponent<LineRenderer>().GetPosition(0).x - incisionLines[0].GetComponent<LineRenderer>().GetPosition(1).x);
                float height = Mathf.Abs(incisionLines[0].GetComponent<LineRenderer>().GetPosition(0).y - incisionLines[0].GetComponent<LineRenderer>().GetPosition(1).y);
                organSpot1.transform.localScale = new Vector3(width, height, 1);
                Vector3 point = (incisionLines[0].GetComponent<LineRenderer>().GetPosition(0) + incisionLines[0].GetComponent<LineRenderer>().GetPosition(1)) / 2;
                organSpot1.transform.position = point;

                width = Mathf.Abs(incisionLines[1].GetComponent<LineRenderer>().GetPosition(0).x - incisionLines[1].GetComponent<LineRenderer>().GetPosition(1).x);
                height = Mathf.Abs(incisionLines[1].GetComponent<LineRenderer>().GetPosition(0).y - incisionLines[1].GetComponent<LineRenderer>().GetPosition(1).y);
                organSpot2.transform.localScale = new Vector3(width, height, 1);
                point = (incisionLines[1].GetComponent<LineRenderer>().GetPosition(0) + incisionLines[1].GetComponent<LineRenderer>().GetPosition(1)) / 2;
                organSpot2.transform.position = point;
            }

            if (stateTime <= 0)
            {
                gameState = SurgeryState.Extraction;
                stateTime = 30f;
            }
        }
        else if(gameState == SurgeryState.Extraction)
        {
            stateTime -= Time.deltaTime;

            if (stateTime <= 0)
            {
                gameState = SurgeryState.Replacement;
                stateTime = 30f;
            }
        }
        else if(gameState == SurgeryState.Replacement)
        {
            if (scalpelClicked)
            {
                stateTime -= Time.deltaTime;
            }

            if (stateTime <= 0)
            {
                gameState = SurgeryState.Extraction;
                if (organ1.GetComponent<Collider>().bounds.Intersects(organSpot1.GetComponent<Collider>().bounds)
                    && organ2.GetComponent<Collider>().bounds.Intersects(organSpot2.GetComponent<Collider>().bounds))
                {
                    SurgerySuccesful();
                }
                else
                {
                    SurgeryBotched();
                }
            }
        }
    }

    void SurgeryBotched()
    {
        SceneManager.LoadScene("MainGame");
    }

    void SurgerySuccesful()
    {
        SceneManager.LoadScene("MainGame");
    }

    void GenerateIncisionMarker(int index, float xMinMax = 8f, float yMinMax = 4.75f)
    {
        Vector3 point1 = new Vector3(Random.Range(-xMinMax, xMinMax), Random.Range(-yMinMax, yMinMax), 0);
        Vector3 point2 = Vector3.zero;
        switch (Random.Range(0, 3)) 
        {
            case 0:
                point2 = point1 + new Vector3(Random.Range(1f, 2f), Random.Range(1f, 2f), 0);
                break;

            case 1:
                point2 = point1 + new Vector3(Random.Range(1f, 2f), Random.Range(-1f, -2f), 0);
                break;

            case 2:
                point2 = point1 + new Vector3(Random.Range(-1f, -2f), Random.Range(-1f, -2f), 0);
                break;

            case 3:
                point2 = point1 + new Vector3(Random.Range(-1f, -2f), Random.Range(1f, 2f), 0);
                break;
        }

        incisionLines[index] = Instantiate(incisionLinePrefab);

        incisionLines[index].GetComponent<LineRenderer>().SetPositions(new Vector3[] { point1, point2 });
        incisionLines[index].GetComponent<EdgeCollider2D>().SetPoints(new List<Vector2> { new Vector2(point1.x, point1.y), new Vector2(point2.x, point2.y) });
    }
}

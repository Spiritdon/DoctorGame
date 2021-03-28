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

    GameObject[] incisionGuideLines;
    GameObject[] incisionLines;

    GameObject[] organs;
    GameObject[] organSpots;

    public GameObject incisionLinePrefab;
<<<<<<< Updated upstream
    public GameObject[] organPrefabs;
    public Material healthyMat;
    public Material sickMat;
    
    public SurgeryMouseControl mouseInfo;
=======
    public GameObject[] incisionLines;//needs to be used in surgerymousecontrol
>>>>>>> Stashed changes

    // Start is called before the first frame update
    void Start()
    {
        organs = new GameObject[2];
        organSpots = new GameObject[2];

        gameState = SurgeryState.Incision;
        incisionGuideLines = new GameObject[2];
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
            if (mouseInfo.Held != null && !scalpelClicked)
            {
                scalpelClicked = true;
            }
            else if (scalpelClicked)
            {
                stateTime -= Time.deltaTime;
                if(mouseInfo.Held != null)
                {

                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                for (int i = 0; i < incisionGuideLines.Length; i++)
                {
                    float width = Mathf.Abs(incisionGuideLines[i].GetComponent<LineRenderer>().GetPosition(0).x - incisionGuideLines[i].GetComponent<LineRenderer>().GetPosition(1).x);
                    float height = Mathf.Abs(incisionGuideLines[i].GetComponent<LineRenderer>().GetPosition(0).y - incisionGuideLines[i].GetComponent<LineRenderer>().GetPosition(1).y);
                    organSpots[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    organSpots[i].transform.localScale = new Vector3(width, height, 0.25f);
                    Vector3 point = (incisionGuideLines[i].GetComponent<LineRenderer>().GetPosition(0) + incisionGuideLines[i].GetComponent<LineRenderer>().GetPosition(1)) / 2;
                    point.z = 10f;
                    organSpots[i].transform.position = point;
                    MeshRenderer meshRend = Instantiate(organPrefabs[Random.Range(organPrefabs.Length/2, organPrefabs.Length)]).GetComponent<MeshRenderer>();
                    meshRend.material = sickMat;
                    meshRend.gameObject.transform.eulerAngles = new Vector3(-90f, 0, 0);
                    meshRend.gameObject.transform.position = new Vector3(point.x, point.y, point.z - 10);
                    meshRend.gameObject.transform.localScale = new Vector3(5, 5, 5);
                    incisionGuideLines[i].SetActive(false);
                }

                gameState = SurgeryState.Extraction;

                //width = Mathf.Abs(incisionGuideLines[1].GetComponent<LineRenderer>().GetPosition(0).x - incisionGuideLines[1].GetComponent<LineRenderer>().GetPosition(1).x);
                //height = Mathf.Abs(incisionGuideLines[1].GetComponent<LineRenderer>().GetPosition(0).y - incisionGuideLines[1].GetComponent<LineRenderer>().GetPosition(1).y);
                //organSpot2.transform.localScale = new Vector3(width, height, 1);
                //point = (incisionGuideLines[1].GetComponent<LineRenderer>().GetPosition(0) + incisionGuideLines[1].GetComponent<LineRenderer>().GetPosition(1)) / 2;
                //organSpot2.transform.position = point;
                //meshRend = Instantiate(organPrefabs[Random.Range(0, organPrefabs.Length)]).GetComponent<MeshRenderer>();
                //meshRend.material = sickMat;
                //meshRend.gameObject.transform.eulerAngles = new Vector3(-90f, 0, 0);
                //meshRend.gameObject.transform.position = point;
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
                if (organs[0].GetComponent<Collider>().bounds.Intersects(organSpots[0].GetComponent<Collider>().bounds)
                    && organs[1].GetComponent<Collider>().bounds.Intersects(organSpots[1].GetComponent<Collider>().bounds))
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

    IEnumerator SurgeryBotched()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainGame");
    }

    IEnumerator SurgerySuccesful()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainGame");
    }

    void GenerateIncisionMarker(int index, float xMinMax = 6f, float yMinMax = 3f)
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

        incisionGuideLines[index] = Instantiate(incisionLinePrefab);

        incisionGuideLines[index].GetComponent<LineRenderer>().SetPositions(new Vector3[] { point1, point2 });
        incisionGuideLines[index].GetComponent<EdgeCollider2D>().SetPoints(new List<Vector2> { new Vector2(point1.x, point1.y), new Vector2(point2.x, point2.y) });
    }

    void TrackIncision()
    {
        //figure out which guideline the player is attempting to make an incision on
        Vector3 startPoint = mouseInfo.Held.transform.position;
        EdgeCollider2D targetCollider = incisionGuideLines[0].GetComponent<EdgeCollider2D>();

        float minDist = incisionGuideLines[0].GetComponent<EdgeCollider2D>().Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance;
        if(minDist > incisionGuideLines[1].GetComponent<EdgeCollider2D>().Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance)
        {
            targetCollider = incisionGuideLines[1].GetComponent<EdgeCollider2D>();
        }

        //Make sure the player does not get too far from the guid lines

        if (targetCollider.Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance > 1)
        {
            SurgeryBotched();
        }
    }
}

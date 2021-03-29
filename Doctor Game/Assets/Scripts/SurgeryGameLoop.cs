using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum SurgeryState { Incision, Extraction, Replacement };
public class SurgeryGameLoop : MonoBehaviour
{
    SurgeryState gameState;

    float stateTime = 30f;
    bool scalpelClicked = false;

    GameObject[] incisionGuideLines;
    GameObject[] newOrgans;

    GameObject[] oldOrgans;
    GameObject[] organSpots;

    public GameObject incisionLinePrefab;
    public Transform[] organTraySpots;
    public GameObject organSpotPrefab;
    public GameObject[] organPrefabs;
    public Material healthyMat;
    public Material sickMat;
    public GameObject canvas;
    public GameObject bg;
    public GameObject tray;
    public Collider2D organTray;

    public SurgeryMouseControl mouseInfo;


    // Start is called before the first frame update
    void Start()
    {
        oldOrgans = new GameObject[2];
        newOrgans = new GameObject[2];
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
        if (mouseInfo.Held !=null && Input.GetMouseButton(1))
        {
            TrackIncision();
        }
        if(gameState == SurgeryState.Incision)
        {
            stateTime -= Time.deltaTime;

            if (stateTime <= 0 || Input.GetKeyDown(KeyCode.Q))
            {
                gameState = SurgeryState.Extraction;
                EnterState(0);
                stateTime = 30f;
                mouseInfo.CanDraw = false;
            }
        }
        else if(gameState == SurgeryState.Extraction)
        {
            stateTime -= Time.deltaTime;

            if (stateTime <= 0 || Input.GetKeyDown(KeyCode.Q))
            {
                

                if (organTray.bounds.Contains(oldOrgans[0].transform.position)
                    && organTray.bounds.Contains(oldOrgans[1].transform.position))
                {
                    EnterState(1);
                    gameState = SurgeryState.Replacement;
                    stateTime = 30f;
                }
                else
                {
                    StartCoroutine(SurgeryBotched());
                }
            }
        }
        else if(gameState == SurgeryState.Replacement)
        {
            stateTime -= Time.deltaTime;

            if (stateTime <= 0)
            {
                if ((organSpots[0].GetComponent<Collider2D>().bounds.Contains(newOrgans[0].transform.position)
                    && organSpots[1].GetComponent<Collider2D>().bounds.Contains(newOrgans[1].transform.position))
                    || (organSpots[1].GetComponent<Collider2D>().bounds.Contains(newOrgans[0].transform.position)
                    && organSpots[0].GetComponent<Collider2D>().bounds.Contains(newOrgans[1].transform.position)))
                {
                    StartCoroutine(SurgerySuccesful());
                }
                else
                {
                    StartCoroutine(SurgeryBotched());
                }
            }
        }
        UpdateUI();
    }

    IEnumerator SurgeryBotched()
    {
        bg.transform.position = new Vector3(0,0,5);
        for(int i = 0; i < 6; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }
        canvas.transform.GetChild(6).gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainGame");
    }

    IEnumerator SurgerySuccesful()
    {
        bg.transform.position = new Vector3(0, 0, 5);
        for (int i = 0; i < 6; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }
        canvas.transform.GetChild(7).gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainGame");
    }

    void GenerateIncisionMarker(int index, float xMinMax = 6f, float yMax = 2f, float yMin = -3f)
    {
        Vector3 point1 = organTray.bounds.center;
        while (organTray.bounds.Contains(point1))
        {
            point1 = new Vector3(Random.Range(-xMinMax, xMinMax), Random.Range(yMin, yMax), 0);
        }
        
        Vector3 point2 = organTray.bounds.center;
        while (organTray.bounds.Contains(point2))
        {
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
        }

        incisionGuideLines[index] = Instantiate(incisionLinePrefab);

        incisionGuideLines[index].GetComponent<LineRenderer>().SetPositions(new Vector3[] { point1, point2 });
        incisionGuideLines[index].GetComponent<EdgeCollider2D>().SetPoints(new List<Vector2> { new Vector2(point1.x, point1.y), new Vector2(point2.x, point2.y) });
    }

    void TrackIncision()
    {

        //figure out which guideline the player is attempting to make an incision on
        //Vector3 startPoint = mouseInfo.Held.transform.position;
        EdgeCollider2D targetCollider = incisionGuideLines[0].GetComponent<EdgeCollider2D>();
        Vector3 heldPos = mouseInfo.Held.transform.position;
        bool hasPlayerCutYet = false;

        foreach (GameObject incision in incisionGuideLines)
        {
            if (Input.GetMouseButton(1) && incision.GetComponent<EdgeCollider2D>().bounds.Contains(heldPos))
            {
                Debug.Log("Right Click Down and Inside");
                //sorry tried to get both the box and the circle collider to determine if the player won did not work out
                /*foreach (GameObject line in mouseInfo.lines)
                {
                    Vector3[] scapleCutPostions = new Vector3[line.GetComponent<LineRenderer>().positionCount];
                    line.GetComponent<LineRenderer>().GetPositions(scapleCutPostions);

                    foreach (Vector3 scapleCut in scapleCutPostions)
                    {
                        bool circleHit = false;
                        bool boxHit = false;
                        if (incision.GetComponent<CircleCollider2D>().bounds.Contains(scapleCut))
                        {
                            //Debug.Log("Circle Hit");
                            circleHit = true;
                        }
                        if (incision.GetComponent<BoxCollider2D>().bounds.Contains(scapleCut))
                        {
                            //Debug.Log("Box Hit");
                            boxHit = true;
                        }
                        if (boxHit ==true && circleHit == true)
                        {
                            Debug.Log("You Won :D");
                        }
                    }
                }*/
            }
            else
            {
                Debug.Log("You are either outside of the incision or you let go of right click");
            }
        }
        
        /*if (incisionGuideLines[0].GetComponent<EdgeCollider2D>().bounds.Contains(heldPos))
        {
            //Debug.Log("First: Held is within");
        }
        if (incisionGuideLines[1].GetComponent<EdgeCollider2D>().bounds.Contains(heldPos))
        {
            //Debug.Log("Second: Held is within");
        }
        foreach(GameObject line in mouseInfo.lines)
        {
            Vector3[] scapleCutPostions = new Vector3[line.GetComponent<LineRenderer>().positionCount];
            line.GetComponent<LineRenderer>().GetPositions(scapleCutPostions);

            foreach (Vector3 scapleCut in scapleCutPostions)
            {
                if (incisionGuideLines[0].GetComponent<CircleCollider2D>().bounds.Contains(scapleCut))
                {

                }
            }
        }*/

        float minDist = incisionGuideLines[0].GetComponent<EdgeCollider2D>().Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance;
        if(minDist > incisionGuideLines[1].GetComponent<EdgeCollider2D>().Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance)
        {
            targetCollider = incisionGuideLines[1].GetComponent<EdgeCollider2D>();
        }

        //Make sure the player does not get too far from the guid lines

        if (targetCollider.Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance > 1)
        {
            StartCoroutine(SurgeryBotched());
        }
        
    }

    void UpdateUI()
    {
        Text[] display = canvas.GetComponentsInChildren<Text>();
        display[0].text = stateTime.ToString("0.0");
        display[1].text = gameState.ToString();
    }

    void EnterState(int stage)
    {
        switch (stage)
        {
            case 0:
                for (int i = 0; i < incisionGuideLines.Length; i++)
                {
                    float width = Mathf.Abs(incisionGuideLines[i].GetComponent<LineRenderer>().GetPosition(0).x - incisionGuideLines[i].GetComponent<LineRenderer>().GetPosition(1).x);
                    float height = Mathf.Abs(incisionGuideLines[i].GetComponent<LineRenderer>().GetPosition(0).y - incisionGuideLines[i].GetComponent<LineRenderer>().GetPosition(1).y);
                    organSpots[i] = Instantiate(organSpotPrefab);
                    organSpots[i].transform.localScale = new Vector3(width, height, 0.25f);
                    organSpots[i].GetComponent<BoxCollider2D>().size = new Vector3(width, height, 0.25f);
                    Vector3 point = (incisionGuideLines[i].GetComponent<LineRenderer>().GetPosition(0) + incisionGuideLines[i].GetComponent<LineRenderer>().GetPosition(1)) / 2;
                    point.z = 10f;
                    organSpots[i].transform.position = point;
                    MeshRenderer meshRend = Instantiate(organPrefabs[Random.Range(organPrefabs.Length / 2, organPrefabs.Length)]).GetComponent<MeshRenderer>();
                    meshRend.material = sickMat;
                    meshRend.gameObject.transform.eulerAngles = new Vector3(-90f, 0, 0);
                    meshRend.gameObject.transform.position = new Vector3(point.x, point.y, point.z - 10);
                    meshRend.gameObject.transform.localScale = new Vector3(5, 5, 5);
                    oldOrgans[i] = meshRend.gameObject;
                    incisionGuideLines[i].SetActive(false);
                }

                tray.SetActive(true);
                canvas.transform.GetChild(2).gameObject.SetActive(false);
                canvas.transform.GetChild(3).gameObject.SetActive(true);
                break;

            case 1:
                for (int i = 0; i < organTraySpots.Length; i++)
                {
                    MeshRenderer meshRend = Instantiate(organPrefabs[Random.Range(0, organPrefabs.Length / 2)]).GetComponent<MeshRenderer>();
                    meshRend.material = healthyMat;
                    meshRend.gameObject.transform.eulerAngles = new Vector3(-90f, 0, 0);
                    meshRend.gameObject.transform.position = organTraySpots[i].position - new Vector3(0,0, organTraySpots[i].position.z);
                    meshRend.gameObject.transform.localScale = new Vector3(5, 5, 5);
                    newOrgans[i] = meshRend.gameObject;
                    oldOrgans[i].SetActive(false);
                }

                canvas.transform.GetChild(3).gameObject.SetActive(false);
                canvas.transform.GetChild(4).gameObject.SetActive(true);
                break;
        }
    }
}

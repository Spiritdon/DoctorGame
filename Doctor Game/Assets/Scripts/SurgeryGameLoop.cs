using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum SurgeryState { Incision, Extraction, Replacement };
public class SurgeryGameLoop : MonoBehaviour
{
    SurgeryState gameState;

    float stateTime = 999f;
    bool gameOver = false;

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
    public Collider2D gameBounds;

    public GameObject[] scapleIncisions;

    public SurgeryMouseControl mouseInfo;

    private bool oneCompleteIncision;//because there are only 2 incsisions if we find the player has completed one the next one will always be at index zero
    private int incCompleted;//number of incinsion completed max 2

    
    //bool to check which incision is complete
    bool firstIncCompleted;
    bool secondIncCompleted;

    // Start is called before the first frame update
    void Start()
    {
        firstIncCompleted = false;
        secondIncCompleted = false;
        incCompleted = 0;
        oneCompleteIncision = false;// if thep layer has completed an incision
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
        //Debug.Log(incCompleted);
        stateTime -= Time.deltaTime;
        if (gameState == SurgeryState.Incision)
        {
            //if (Input.GetKeyUp(KeyCode.Mouse1))
            //{
            //    //CheckIncision(mouseInfo.lines[lineCount].GetComponent<LineRenderer>());
            //    //lineCount++;
            //}
            /*if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                lineCount++;
            }*/
            if (Input.GetKey(KeyCode.Mouse1))
            {
                TrackIncision();
            }
            List<GameObject> line1 = mouseInfo.lines;
            if (stateTime <= 0 || incCompleted >= 2)
            {

                gameState = SurgeryState.Extraction;
                EnterState(0);
                stateTime = 30f;
                mouseInfo.CanDraw = false;
                mouseInfo.DeleteLines();
            }
        }
        else if(gameState == SurgeryState.Extraction)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (organTray.bounds.Contains(oldOrgans[0].transform.position)
                        && organTray.bounds.Contains(oldOrgans[1].transform.position))
                {
                    EnterState(1);
                    gameState = SurgeryState.Replacement;
                    stateTime = 30f;
                }
            }

            if (stateTime <= 0)
            {
                if (!organTray.bounds.Contains(oldOrgans[0].transform.position)
                    || !organTray.bounds.Contains(oldOrgans[1].transform.position))
                {
                    StartCoroutine(SurgeryBotched());
                    gameOver = true;
                }
            }
        }
        else if(gameState == SurgeryState.Replacement)
        {
            if ((organSpots[0].transform.GetChild(0).GetComponent<Collider2D>().bounds.Contains(newOrgans[0].transform.position)
                    && organSpots[1].transform.GetChild(0).GetComponent<Collider2D>().bounds.Contains(newOrgans[1].transform.position))
                    || (organSpots[1].transform.GetChild(0).GetComponent<Collider2D>().bounds.Contains(newOrgans[0].transform.position)
                    && organSpots[0].transform.GetChild(0).GetComponent<Collider2D>().bounds.Contains(newOrgans[1].transform.position))
                    && Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(SurgerySuccesful());
                gameOver = true;
            }

            if (stateTime <= 0)
            {
                if ((!organSpots[0].transform.GetChild(0).GetComponent<Collider2D>().bounds.Contains(newOrgans[0].transform.position)
                    || !organSpots[1].transform.GetChild(0).GetComponent<Collider2D>().bounds.Contains(newOrgans[1].transform.position))
                    && (!organSpots[1].transform.GetChild(0).GetComponent<Collider2D>().bounds.Contains(newOrgans[0].transform.position)
                    || !organSpots[0].transform.GetChild(0).GetComponent<Collider2D>().bounds.Contains(newOrgans[1].transform.position)))
                {
                    StartCoroutine(SurgeryBotched());
                    gameOver = true;
                }
            }
        }
        if (!gameOver)
        {
            UpdateUI();
        }
    }

    IEnumerator SurgeryBotched()
    {
        bg.transform.position = new Vector3(0,0,5);
        incisionGuideLines[0].SetActive(false);
        incisionGuideLines[1].SetActive(false);
        for(int i = 0; i < 6; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }
        canvas.transform.GetChild(6).gameObject.SetActive(true);
        mouseInfo.DeleteLines();
        yield return new WaitForSeconds(3);
        Cursor.visible = true;
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
        Cursor.visible = true;
        Stats.Stress -= 3.5f;
        SceneManager.LoadScene("MainGame");
    }

    void GenerateIncisionMarker(int index, float xMinMax = 6f, float yMax = 2f, float yMin = -3f)
    {
        Vector3 point1 = organTray.bounds.center;
        while (organTray.bounds.Contains(point1) || !gameBounds.bounds.Contains(point1))
        {
            point1 = new Vector3(Random.Range(-xMinMax, xMinMax), Random.Range(yMin, yMax), 0);
        }
        
        Vector3 point2 = organTray.bounds.center;
        while (organTray.bounds.Contains(point2) || !gameBounds.bounds.Contains(point2))
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
        //this bool is for determining if the player has drawn any lines 
        bool noLinesDrawn = true;
        //game object for the chosen line
        GameObject chosenLine = new GameObject();
        //componets needed for determining all aspects of the target incision
        EdgeCollider2D targetCollider;
        LineRenderer targetLine;
        GameObject targetObj;

        List<GameObject> tempIncisions = new List<GameObject>();
        
        //this determines if the player has completed an incision in which case one of the lines will be gone leveing only 1
        if (oneCompleteIncision)
        {
            tempIncisions.Add(incisionGuideLines[0]);
            targetCollider = incisionGuideLines[0].GetComponent<EdgeCollider2D>();
            targetLine = incisionGuideLines[0].GetComponent<LineRenderer>();
            targetObj = incisionGuideLines[0];
            targetObj.name = "Inc0";
        }
        else
        {
            tempIncisions.Add(incisionGuideLines[0]);
            tempIncisions.Add(incisionGuideLines[1]);
            targetCollider = incisionGuideLines[0].GetComponent<EdgeCollider2D>();
            targetLine = incisionGuideLines[0].GetComponent<LineRenderer>();
            targetObj = incisionGuideLines[0];
            targetObj.name = "Inc0";

            float minDist = incisionGuideLines[0].GetComponent<EdgeCollider2D>().Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance;
            if (minDist > incisionGuideLines[1].GetComponent<EdgeCollider2D>().Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance)
            {
                targetCollider = incisionGuideLines[1].GetComponent<EdgeCollider2D>();
                targetLine = incisionGuideLines[1].GetComponent<LineRenderer>();
                targetObj = incisionGuideLines[1];
                targetObj.name = "Inc1";
            }
            
        }


        //if the player has attempted to cut outside of the incision guid lines it will end the game
        if (Input.GetMouseButton(1))
        {
            if (!targetCollider.bounds.Contains(mouseInfo.Held.transform.position))
            {
                StartCoroutine(SurgeryBotched());
            }
        }
        //we determine if lines have been drawn if there are none do nothing
        if (mouseInfo.lines.Count == 0)
        {

        }
        else//if a line has been drawn set nolinedrawn to false as we now need to compare the currentline with the guildline incision
        {
            noLinesDrawn = false;
            chosenLine = mouseInfo.lines[mouseInfo.lines.Count - 1];//the chosen line will always be the current line the chosen line must be within the the bounds
        }

        if (!noLinesDrawn)//determines if lines are drawn to prevent null exceptions
        {
            
            //we get the first and the last position of the chosen
            int lastPosition = chosenLine.GetComponent<LineRenderer>().positionCount - 1;

            Vector3 firstPoint = chosenLine.GetComponent<LineRenderer>().GetPosition(0);
            Vector3 lastPoint = chosenLine.GetComponent<LineRenderer>().GetPosition(lastPosition);
            float distanceOfChosen = Vector3.Distance(firstPoint, lastPoint);

            //we get the first and the last position of the targetIncisions
            int lastIncisionPosition = targetLine.GetComponent<LineRenderer>().positionCount - 1;

            Vector3 incisionFirstPoint = targetLine.GetComponent<LineRenderer>().GetPosition(0);
            Vector3 incisionLastPoint = targetLine.GetComponent<LineRenderer>().GetPosition(lastIncisionPosition);
            float distanceOfTarget = Vector3.Distance(incisionFirstPoint, incisionLastPoint);

            float modifiedDistance = distanceOfTarget - 0.5f;

            //we muse continue to check if the player is out of bounds
            if (!targetCollider.bounds.Contains(mouseInfo.Held.transform.position))
            {
                StartCoroutine(SurgeryBotched());
                gameOver = true;
            }
            //if the players incision is of adequate length and still within the bounds than need to determine which incision is that target and set its color to clear
            if (modifiedDistance <= distanceOfChosen)
            {
                
                
                //than we determine what is the target object and change its color to be clear
                if (targetObj == incisionGuideLines[0] && !firstIncCompleted)
                {
                    
                    incisionGuideLines[0].GetComponent<LineRenderer>().endColor = Color.clear;
                    incisionGuideLines[0].GetComponent<LineRenderer>().startColor = Color.clear;
                    incCompleted++;
                    firstIncCompleted = true;
                }
                else if(targetObj == incisionGuideLines[1] && !secondIncCompleted)
                {

                    incisionGuideLines[1].GetComponent<LineRenderer>().endColor = Color.clear;
                    incisionGuideLines[1].GetComponent<LineRenderer>().startColor = Color.clear;
                    incCompleted++;
                    secondIncCompleted = true;
                }
            }


        }//if no lines are drawn do nothing
        else
        {

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
                    organSpots[i].transform.GetChild(0).GetComponent<BoxCollider2D>().size = new Vector3(width, height, 0.25f);
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

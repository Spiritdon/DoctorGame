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

    int lineCount = 0;

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

    private bool oneCompleteIncision;
    private int incCompleted;//number of incinsion completed max 2

    Vector3 IncisionPosition1;
    Vector3 IncisionPosition2;

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log(incCompleted);
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
        IncisionPosition1 = point1;
        IncisionPosition2 = point2;
        incisionGuideLines[index].GetComponent<LineRenderer>().SetPositions(new Vector3[] { point1, point2 });
        incisionGuideLines[index].GetComponent<EdgeCollider2D>().SetPoints(new List<Vector2> { new Vector2(point1.x, point1.y), new Vector2(point2.x, point2.y) });
    }

    //void CheckIncision(LineRenderer incision)
    //{
    //
    //    //figure out which guideline the player is attempting to make an incision on
    //    Vector3 startPoint = incision.GetPosition(0);
    //    EdgeCollider2D targetCollider = incisionGuideLines[0].GetComponent<EdgeCollider2D>();
    //    //Vector3 heldPos = mouseInfo.Held.transform.position;
    //    bool aCollider = false;
    //    bool bCollider = false;
    //
    //    float minDist = incisionGuideLines[0].GetComponent<EdgeCollider2D>().Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance;
    //    if (minDist > incisionGuideLines[1].GetComponent<EdgeCollider2D>().Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance)
    //    {
    //        targetCollider = incisionGuideLines[1].GetComponent<EdgeCollider2D>();
    //    }
    //
    //    //foreach (GameObject incision in incisionGuideLines)
    //    //{
    //    //    if (Input.GetMouseButton(1) && incision.GetComponent<EdgeCollider2D>().bounds.Contains(heldPos))
    //    //    {
    //    //        Debug.Log("Right Click Down and Inside");
    //    //        //sorry tried to get both the box and the circle collider to determine if the player won did not work out
    //    //        /*foreach (GameObject line in mouseInfo.lines)
    //    //        {
    //    //            Vector3[] scapleCutPostions = new Vector3[line.GetComponent<LineRenderer>().positionCount];
    //    //            line.GetComponent<LineRenderer>().GetPositions(scapleCutPostions);
    //    //
    //    //            foreach (Vector3 scapleCut in scapleCutPostions)
    //    //            {
    //    //                bool circleHit = false;
    //    //                bool boxHit = false;
    //    //                if (incision.GetComponent<CircleCollider2D>().bounds.Contains(scapleCut))
    //    //                {
    //    //                    //Debug.Log("Circle Hit");
    //    //                    circleHit = true;
    //    //                }
    //    //                if (incision.GetComponent<BoxCollider2D>().bounds.Contains(scapleCut))
    //    //                {
    //    //                    //Debug.Log("Box Hit");
    //    //                    boxHit = true;
    //    //                }
    //    //                if (boxHit ==true && circleHit == true)
    //    //                {
    //    //                    Debug.Log("You Won :D");
    //    //                }
    //    //            }
    //    //        }*/
    //    //    }
    //    //    else
    //    //    {
    //    //        Debug.Log("You are either outside of the incision or you let go of right click");
    //    //    }
    //    //}
    //    
    //    //Make sure the player does not get too far from the guid line
    //    if (targetCollider.Distance(mouseInfo.Held.GetComponent<EdgeCollider2D>()).distance > 0.5f)
    //    {
    //        StartCoroutine(SurgeryBotched());
    //    }
    //
    //    Vector3[] scapleCutPostions = new Vector3[targetCollider.gameObject.GetComponent<LineRenderer>().positionCount];
    //    targetCollider.GetComponent<LineRenderer>().GetPositions(scapleCutPostions);
    //
    //    foreach (Vector3 scapleCut in scapleCutPostions)
    //    {
    //        if (targetCollider.GetComponent<CircleCollider2D>().bounds.Contains(scapleCut) && !aCollider)
    //        {
    //            aCollider = true;
    //        }
    //        if (targetCollider.GetComponent<BoxCollider2D>().bounds.Contains(scapleCut) && !bCollider)
    //        {
    //            bCollider = true;
    //        }
    //        if (targetCollider.Distance(mouseInfo.Held.GetComponent<EdgeCollider2D>()).distance > 0.5f)
    //        {
    //            StartCoroutine(SurgeryBotched());
    //        }
    //    }
    //
    //    if(aCollider && bCollider)
    //    {
    //
    //    }
    //}

    void TrackIncision()
    {
        bool noLinesDrawn = true;
        GameObject chosenLine = new GameObject();
        Vector3 currPoint;
        EdgeCollider2D targetCollider;
        LineRenderer targetLine;
        GameObject targetObj;

        List<GameObject> tempIncisions = new List<GameObject>();
        
        //this determines if the player has completed an incision in which case one of the lines will be gone leveing only 1
        if (oneCompleteIncision)
        {
            tempIncisions.Add(incisionGuideLines[0]);
            currPoint = mouseInfo.Held.transform.position;
            targetCollider = incisionGuideLines[0].GetComponent<EdgeCollider2D>();
            targetLine = incisionGuideLines[0].GetComponent<LineRenderer>();
            targetObj = incisionGuideLines[0];
        }
        else
        {
            tempIncisions.Add(incisionGuideLines[0]);
            tempIncisions.Add(incisionGuideLines[1]);
            currPoint = mouseInfo.Held.transform.position;
            targetCollider = incisionGuideLines[0].GetComponent<EdgeCollider2D>();
            targetLine = incisionGuideLines[0].GetComponent<LineRenderer>();
            targetObj = incisionGuideLines[0];

            float minDist = incisionGuideLines[0].GetComponent<EdgeCollider2D>().Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance;
            if (minDist > incisionGuideLines[1].GetComponent<EdgeCollider2D>().Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance)
            {
                targetCollider = incisionGuideLines[1].GetComponent<EdgeCollider2D>();
                targetLine = incisionGuideLines[1].GetComponent<LineRenderer>();
                targetObj = incisionGuideLines[1];
            }
        }

        
        //Debug.Log("Right Click Down and Inside");
        
        if (mouseInfo.lines.Count == 0)
        {

        }
        else 
        {
            if (targetCollider.bounds.Contains(mouseInfo.lines[mouseInfo.lines.Count - 1].GetComponent<LineRenderer>().GetPosition(0)))
            {
                noLinesDrawn = false;
                chosenLine = mouseInfo.lines[mouseInfo.lines.Count - 1];//the chosen line will always be the current line the chosen line must be within the the bounds
                //chosenLine = mouseInfo.lines[0];
            }
        }

        if (!noLinesDrawn)//determines if lines are drawn to prevent null exceptions
        {
            /*for (int x = 0; x < mouseInfo.lines.Count; x++)
            {
                GameObject tempLine = mouseInfo.lines[x];
                if (targetCollider.bounds.Contains(tempLine.GetComponent<LineRenderer>().GetPosition(0)))
                {

                    for (int y = 0;y<pastChosen.Count;y++)
                    {
                        if (tempLine == pastChosen[y])
                        {
                            x++;
                            continue;
                        }
                        else
                        {
                            pastChosen.Add(tempLine);
                            chosenLine = tempLine;
                            break;
                        }
                    }
                }
            }*/


            /*foreach (GameObject line in mouseInfo.lines)
            {
                line.GetComponent<LineRenderer>();
                if (targetCollider.bounds.Contains(line.GetComponent<LineRenderer>().GetPosition(0)))
                {
                    chosenLine = line;
                }
            }*/

            int lastPosition = chosenLine.GetComponent<LineRenderer>().positionCount - 1;

            Vector3 firstPoint = chosenLine.GetComponent<LineRenderer>().GetPosition(0);
            Vector3 lastPoint = chosenLine.GetComponent<LineRenderer>().GetPosition(lastPosition);
            float distanceOfChosen = Vector3.Distance(firstPoint, lastPoint);


            int lastIncisionPosition = targetLine.GetComponent<LineRenderer>().positionCount - 1;

            Vector3 incisionFirstPoint = targetLine.GetComponent<LineRenderer>().GetPosition(0);
            Vector3 incisionLastPoint = targetLine.GetComponent<LineRenderer>().GetPosition(lastIncisionPosition);
            float distanceOfTarget = Vector3.Distance(incisionFirstPoint, incisionLastPoint);

            float modifiedDistance = distanceOfTarget - 0.5f;

            //Debug.Log(distanceOfChosen);

            //than we are going to check if every point is within the the bounds if not the player loses
            /*for (int x = 0; x < chosenLine.GetComponent<LineRenderer>().positionCount; x++)//this is possibly going to be expensive
            {
                Vector3[] chosenPoints = new Vector3[10];
                chosenLine.GetComponent<LineRenderer>().GetPositions(chosenPoints);
                if (!targetCollider.bounds.Contains(chosenPoints[x]))
                {
                    StartCoroutine(SurgeryBotched());
                    gameOver = true;
                }
            }*/
            if (!targetCollider.bounds.Contains(mouseInfo.Held.transform.position))
            {
                StartCoroutine(SurgeryBotched());
                gameOver = true;
            }
            if (modifiedDistance <= distanceOfChosen)
            {
                //incCompleted++;

                //onces a incision is completed it will delete it along with all the incisions the uses mad up until that point leaving only the last incision
                //Debug.Log("Victory");
                
                //once all thel ines are removed we must remove the incision as well to prevent confusion 
                //first i will create a temp list that will be a copy of the incisionGuideLines array
                
                //than we determine what is the target object and remove it
                if (targetObj = incisionGuideLines[0])
                {
                    Destroy(incisionGuideLines[0]);
                    tempIncisions.Remove(incisionGuideLines[0]);
                    incCompleted++;
                }
                else if(targetObj = incisionGuideLines[1])
                {
                    Destroy(incisionGuideLines[1]);
                    tempIncisions.Remove(incisionGuideLines[1]);
                    incCompleted++;
                }
                /*for (int x = 0;x<mouseInfo.lines.Count;x++)
                {
                    mouseInfo.lines.Remove(mouseInfo.lines[x]);
                    Destroy(mouseInfo.lines[x]);
                }*/
                /*foreach (GameObject line in mouseInfo.lines)
                {
                    Destroy(line);
                    mouseInfo.lines.Remove(line);
                }*/
                //we than update our old array with the new shorter list
                incisionGuideLines = tempIncisions.ToArray();

                //and than we destroy the object
               
                oneCompleteIncision = true;//there are only 2 incisions 
                
                //we need to destory the current target*/
            }


        }
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

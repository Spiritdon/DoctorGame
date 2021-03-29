using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum SurgeryState { Incision, Extraction, Replacement };
public class SurgeryGameLoop : MonoBehaviour
{
    SurgeryState gameState;

    float stateTime = 10f;
    bool scalpelClicked = false;
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
        stateTime -= Time.deltaTime;
        if (gameState == SurgeryState.Incision)
        {
            //if (Input.GetKeyUp(KeyCode.Mouse1))
            //{
            //    //CheckIncision(mouseInfo.lines[lineCount].GetComponent<LineRenderer>());
            //    //lineCount++;
            //}
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                lineCount++;
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                TrackIncision();
            }

            if (stateTime <= 0 || lineCount >= 2)
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
        Vector3 currPoint = mouseInfo.Held.transform.position;
        EdgeCollider2D targetCollider = incisionGuideLines[0].GetComponent<EdgeCollider2D>();
        
        float minDist = incisionGuideLines[0].GetComponent<EdgeCollider2D>().Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance;
        if (minDist > incisionGuideLines[1].GetComponent<EdgeCollider2D>().Distance(mouseInfo.Held.GetComponent<Collider2D>()).distance)
        {
            targetCollider = incisionGuideLines[1].GetComponent<EdgeCollider2D>();
        }


        
        Debug.Log("Right Click Down and Inside");
        if (!targetCollider.bounds.Contains(mouseInfo.Held.transform.position))
        {
            StartCoroutine(SurgeryBotched());
            gameOver = true;
        }
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

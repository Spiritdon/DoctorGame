using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CapsuleControl : MonoBehaviour
{
    private int pointCounter;
    private int badCounter;
    public PillSpawner pillSpawner;
    public AudioSource pillCollect;

    public Text goodPillCounter;
    public Text badPillCounter;

    // Start is called before the first frame update
    void Start()
    {
        pointCounter = 0;
        badCounter = 0;

    }

    // Update is called once per frame
    void Update()
    {
        goodPillCounter.text = "Number Of Pill Collected:  " + pointCounter.ToString() + "/10";
        badPillCounter.text = "Number Of Bad Pill Collected:  " + badCounter.ToString() + "/3";
        Debug.Log("Point Counter:" + pointCounter);

        if (gameObject.transform.position.x < 1276 && gameObject.transform.position.x > 660)
        {
            transform.position += new Vector3((Input.GetAxis("Horizontal") * Time.deltaTime) * 500, 0, 0);

        }
        if (gameObject.transform.position.x >= 1276)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.position += new Vector3(-3 * Time.deltaTime * 500, 0, 0);
            }
        }
        if (gameObject.transform.position.x <= 660)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.position += new Vector3(3 * Time.deltaTime * 500, 0, 0);
            }
        }

        if (pointCounter == 10)
        {
            SceneManager.LoadScene("MainGame");
            pointCounter = 0;
            Stats.Stress -= 2;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.SetActive(false);
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "badPill")
        {
            pillCollect.Play();
            badCounter++;
            pillSpawner.CollectPill(collision.gameObject);
        }
        if (collision.gameObject.tag == "goodPill")
        {
            pillCollect.Play();
            pointCounter++;

            pillSpawner.CollectPill(collision.gameObject);

        }
        if (badCounter == 3)
        {
            SceneManager.LoadScene("MainGame");
            Stats.Stress += 1;
        }
    }

}

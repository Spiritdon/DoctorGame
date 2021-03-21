using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CapsuleControl : MonoBehaviour
{
    private int pointCounter;
    public PillSpawner pillSpawner;
    // Start is called before the first frame update
    void Start()
    {
        pointCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x<5&& gameObject.transform.position.x > -5)
        {
            transform.position += new Vector3((Input.GetAxis("Horizontal") * Time.deltaTime) * 10, 0, 0);
            
        }
        if (gameObject.transform.position.x >= 5.0f)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.position += new Vector3(-3 * Time.deltaTime * 10, 0, 0);
            }
        }
        if (gameObject.transform.position.x <= -5.0f)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.position += new Vector3(3 * Time.deltaTime * 10, 0, 0);
            }
        }

        if (pointCounter == 5)
        {
            SceneManager.LoadScene("MainGame");
            pointCounter = 0;
            Stats.Stress -= 0.2f;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.SetActive(false);
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "badPill")
        {
            SceneManager.LoadScene("MainGame");
            Stats.Stress += 0.1f;
        }
        if(collision.gameObject.tag == "goodPill")
        {
            pointCounter++;
            Debug.Log("Point Counter:" + pointCounter);
            pillSpawner.CollectPill(collision.gameObject);
        }
    }

}

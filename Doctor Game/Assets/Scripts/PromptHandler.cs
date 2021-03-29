<<<<<<< Updated upstream
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PromptHandler : MonoBehaviour
{
    public RectTransform prompt;
    string targetScene;
    PromptData data;

    public void CreatePrompt(string staminaCost, string stressCost, string timeCost, bool isRelaxing, string stressBuff, string scene)
    {
        prompt.transform.GetChild(2).GetComponent<Text>().text = "Stamina: " + staminaCost;
        prompt.transform.GetChild(3).GetComponent<Text>().text = "Stress: " + stressCost;
        prompt.transform.GetChild(4).GetComponent<Text>().text = "Time: " + timeCost;
        prompt.transform.GetChild(7).GetComponent<Text>().text = "Stress: " + stressBuff;

        data = new PromptData(float.Parse(staminaCost.Substring(1)), int.Parse(stressCost.Substring(1)), float.Parse(timeCost.Substring(1, timeCost.IndexOf('h') - 1)), isRelaxing);

        prompt.gameObject.SetActive(true);
        targetScene = scene;
    }

    public void HidePrompt()
    {
        prompt.gameObject.SetActive(false);
    }

    public void SelectPrompt()
    {
        HidePrompt();
        if (data.isRelaxing)
        {
            if (Stats.StressReleaseLimit <= 5)
            {
                Stats.StressReleaseLimit += data.stressCost;
                Stats.Stress -= data.stressCost;
            }
        }
        else
        {
            Stats.Stress += data.stressCost;
        }

        Stats.UpdateTime((int)(data.timeCost * 60), data.isRelaxing);
        if (targetScene != "")
        {
            SceneManager.LoadScene(targetScene);
        }
        gameObject.SetActive(false);

        if (gameObject.transform.parent.gameObject.GetComponent<StressorScript>() != null)
        {
            gameObject.transform.parent.gameObject.GetComponent<StressorScript>().active = false;
        }
    }
}

struct PromptData
{
    public int stressCost;
    public float staminaCost;
    public float timeCost;
    public bool isRelaxing;

    public PromptData(float staC, int strC, float tC, bool tf)
    {
        stressCost = strC;
        staminaCost = staC;
        timeCost = tC;
        isRelaxing = tf;
    }
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PromptHandler : MonoBehaviour
{
    public RectTransform prompt;
    string targetScene;
    PromptData data;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreatePrompt(string staminaCost, string stressCost, string timeCost, string stressBuff, string scene)
    {
        prompt.transform.GetChild(2).GetComponent<Text>().text = "Stamina: " + staminaCost;
        prompt.transform.GetChild(3).GetComponent<Text>().text = "Stress: " + stressCost;
        prompt.transform.GetChild(4).GetComponent<Text>().text = "Time: " + timeCost;
        prompt.transform.GetChild(7).GetComponent<Text>().text = "Stress: " + stressBuff;

        data = new PromptData(float.Parse(staminaCost.Substring(1)), int.Parse(stressCost.Substring(1)), float.Parse(timeCost.Substring(1, timeCost.IndexOf('h') - 1)));

        prompt.gameObject.SetActive(true);
        targetScene = scene;
    }

    public void HidePrompt()
    {
        prompt.gameObject.SetActive(false);
    }

    public void SelectPrompt()
    {
        HidePrompt();
        Stats.Stress += data.stressCost;
        Debug.Log(data.timeCost);
        Debug.Log(data.timeCost*60);
        Stats.UpdateTime((int)(data.timeCost * 60), false);
        if (targetScene != "")
        {
            SceneManager.LoadScene(targetScene);
        }
        gameObject.SetActive(false);

        if (gameObject.transform.parent.gameObject.GetComponent<StressorScript>() != null) {
            gameObject.transform.parent.gameObject.GetComponent<StressorScript>().active = false;
        }
    }
}

struct PromptData
{
    public int stressCost;
    public float staminaCost;
    public float timeCost;
    public GameObject stressor;

    public PromptData(float staC, int strC, float tC, GameObject stressObj)
    {
        stressCost = strC;
        staminaCost = staC;
        timeCost = tC;
        stressor = stressObj;
    }
}
>>>>>>> Stashed changes

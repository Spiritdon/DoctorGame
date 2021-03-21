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
        if (prompt.gameObject.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition;
            if (mousePos.x >= prompt.rect.xMin && mousePos.x <= prompt.rect.xMax && mousePos.y <= prompt.rect.yMax && mousePos.y >= prompt.rect.yMin && Input.GetMouseButtonDown(1))
            {
                HidePrompt();
                Stats.Stamina -= data.staminaCost;
                Stats.Stress += data.stressCost;
                Stats.UpdateTime(data.timeCost, false);
                SceneManager.LoadScene(targetScene);
            }
        }
    }

    public void CreatePrompt(string staminaCost, string stressCost, string timeCost, string stressBuff, string scene)
    {
        prompt.transform.GetChild(2).GetComponent<Text>().text = "Stamina: " + staminaCost;
        prompt.transform.GetChild(3).GetComponent<Text>().text = "Stress: " + stressCost;
        prompt.transform.GetChild(4).GetComponent<Text>().text = "Time: " + timeCost;
        prompt.transform.GetChild(7).GetComponent<Text>().text = "Stress: " + stressBuff;

        data = new PromptData(float.Parse(staminaCost.Substring(1)), float.Parse(stressCost.Substring(1)), float.Parse(timeCost.Substring(1, timeCost.IndexOf('h') - 1)));

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
        Stats.UpdateTime(data.timeCost, false);
        SceneManager.LoadScene(targetScene);
    }
}

struct PromptData 
{
    public float stressCost;
    public float staminaCost;
    public float timeCost;

    public PromptData(float staC, float strC, float tC)
    {
        stressCost = strC;
        staminaCost = staC;
        timeCost = tC;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MiniGamePrompter : MonoBehaviour
{
    PromptHandler prompter;
    public string staminaCost;
    public string stressCost;
    public string timeCost;
    public string stressBuff;
    public string targetScene;

    // Start is called before the first frame update
    void Start()
    {
        prompter = GameObject.FindGameObjectWithTag("Prompter").GetComponent<PromptHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            prompter.CreatePrompt(staminaCost, stressCost, timeCost, stressBuff, targetScene);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            prompter.HidePrompt();
        }
    }
}

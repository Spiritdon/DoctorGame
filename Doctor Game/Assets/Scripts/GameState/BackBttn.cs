using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackBttn : MonoBehaviour
{
    public void BackBttnClicked()
    {
        SceneManager.LoadScene("StartMenu");
    }
}

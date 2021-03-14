using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StressorScript : MonoBehaviour {

    public GameObject player;

    public void stress() {

        if (distanceToPlayer() < 3.0f) {
            SceneManager.LoadScene("Minigame1");
        }
    }

    public float distanceToPlayer() {

        return Mathf.Sqrt(
            Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) * Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) + // a^2
            Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) * Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) + // b^2
            Mathf.Abs(player.transform.position.z - gameObject.transform.position.z) * Mathf.Abs(player.transform.position.z - gameObject.transform.position.z) // a^2
        );
    }
}

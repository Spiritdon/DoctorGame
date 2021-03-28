using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestressorScript : MonoBehaviour {

    public GameObject player;

    public void destress() {

        if (distanceToPlayer() < 2.0f) {
            if (player.GetComponent<PlayerMovement>().stress > 0.0f) {
                player.GetComponent<PlayerMovement>().stress -= 1.0f;
                if (player.GetComponent<PlayerMovement>().stress < 0.0f) {
                    player.GetComponent<PlayerMovement>().stress = 0.0f;
                }
            }
            Debug.Log(player.GetComponent<PlayerMovement>().stress);
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
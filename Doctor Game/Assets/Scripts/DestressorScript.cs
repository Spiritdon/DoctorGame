using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestressorScript : MonoBehaviour {

    public GameObject player;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    public void destress() {

        if (distanceToPlayer() < 2.0f) {
            if (player.GetComponent<PlayerMovement>().stress > 0.0f) {
                player.GetComponent<PlayerMovement>().stress -= 0.1f;
                if (player.GetComponent<PlayerMovement>().stress < 0.0f) {
                    player.GetComponent<PlayerMovement>().stress = 0.0f;
                }
            }
            Debug.Log(player.GetComponent<PlayerMovement>().stress);
        }
    }

    public float distanceToPlayer() {

        float distance;

        distance = Mathf.Sqrt(
            Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) * Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) + // a^2
            Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) * Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) + // b^2
            Mathf.Abs(player.transform.position.z - gameObject.transform.position.z) * Mathf.Abs(player.transform.position.z - gameObject.transform.position.z) // a^2
        );

        return distance;
    }
}
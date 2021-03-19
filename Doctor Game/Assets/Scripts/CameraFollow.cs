using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    Collider limits;
    float zExtent;
    float xExtent;

    // Start is called before the first frame update
    void Start()
    {
        limits = transform.parent.GetComponent<Collider>();
        zExtent = Camera.main.orthographicSize;
        xExtent = zExtent * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        bool controlled = false;
        float minX = limits.bounds.center.x-limits.bounds.extents.x + xExtent;
        float maxX = limits.bounds.center.x + limits.bounds.extents.x - xExtent;
        float minZ = limits.bounds.center.z - limits.bounds.extents.z + zExtent;
        float maxZ = limits.bounds.center.z + limits.bounds.extents.z - zExtent;

        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, Mathf.Clamp(transform.position.z-5*Time.deltaTime, minZ, maxZ));
            controlled = true;
        }
        else if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, Mathf.Clamp(transform.position.z + 5 * Time.deltaTime, minZ, maxZ));
            controlled = true;
        }
        else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x - 5 * Time.deltaTime, minX, maxX), transform.position.y, Mathf.Clamp(transform.position.z, minZ, maxZ));
            controlled = true;
        }
        else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x + 5 * Time.deltaTime, minX, maxX), transform.position.y, Mathf.Clamp(transform.position.z, minZ, maxZ));
            controlled = true;
        }
        if(!controlled)
        {
            transform.position = new Vector3(Mathf.Clamp(target.position.x, minX, maxX), transform.position.y, Mathf.Clamp(target.position.z, minZ, maxZ));
        }
    }
}

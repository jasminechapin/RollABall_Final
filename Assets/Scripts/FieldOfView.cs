using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public GameObject player;
    GameObject blockingWall;
    List<GameObject> walls;

    int invMask;
    int visMask;

    private void Awake()
    {
        invMask = 8;
        visMask = 9;

        walls = new List<GameObject>();
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            walls.Add(wall);
        }
    }

    void Update()
    {
        // if the wall is blocking the cam, make it invisible
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.CompareTag("Wall"))
            {
                blockingWall = hit.transform.gameObject;
                blockingWall.layer = invMask;
                
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            }
        }
        else
        {
            resetVisibility();
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }

    void resetVisibility()
    {
        foreach (GameObject wall in walls)
        {
            wall.layer = visMask;
        }
    }
}

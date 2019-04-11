using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offsetValue;
    private float turnSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        offsetValue = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offsetValue;
        offsetValue = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offsetValue;
        offsetValue = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.right) * offsetValue;
        transform.position = player.transform.position + offsetValue;
        transform.LookAt(player.transform);
    }
}

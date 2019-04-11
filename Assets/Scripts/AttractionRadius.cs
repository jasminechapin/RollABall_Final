using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionRadius : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.transform.position =
                Vector3.MoveTowards(other.transform.position,
                transform.position, 3f * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
    }
}

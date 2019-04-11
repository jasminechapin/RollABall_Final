﻿using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private int count;
    private int numCollectables = 0;
    public Text countText;
    public Text winText;
    public bool isGrounded = true;
    private bool isLevelOver = false;
    public float jumpForce = 300f;
    float pushPower = 4.0f;

    public bool speedUp;
    public bool jumpHigher;
    public bool attractPickUps;
    public float powerUpTime;
    public GameObject magnetSphere;

    
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        powerUpTime = 0f;
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 0);
        count = 0;
        SetCountText();
        winText.text = "";
        Vector3 startPos = GameObject.Find("Start").GetComponent<Transform>().localPosition;
        startPos.y += 0.9f;
        this.GetComponent<Transform>().localPosition = startPos;
        foreach(GameObject g in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            if (g.CompareTag("Pick Up") && !(g.activeSelf))
            {
                g.SetActive(true);
            }
        }

        numCollectables = GameObject.FindGameObjectsWithTag("Pick Up").Length;
    }


    void FixedUpdate()
    {
        if (!isLevelOver) { 
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            movement = Camera.main.transform.TransformDirection(movement);
            movement.y = 0.0f;

            rb.AddForce(movement * speed);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(0, jumpForce, 0);
            }
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
            SetCountText();
        }

        if (speedUp && powerUpTime > 0f)
        {
            speed = 10f;
        }
        else if (attractPickUps && powerUpTime > 0f)
        {
            magnetSphere.SetActive(true);
        }
        else if (jumpHigher && powerUpTime > 0f)
        {
            jumpForce = 500f;
        }

        if (powerUpTime > 0)
        {
            powerUpTime -= .01f;
        }
        else
        {
            speedUp = false;
            attractPickUps = false;
            jumpHigher = false;

            speed = 5f;
            magnetSphere.SetActive(false);
            jumpForce = 300f;
        }
    }

    void OnTriggerEnter(Collider other)
    {  
        if(other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
        else if(other.gameObject.CompareTag("End"))
        {
            if (count == numCollectables)
            {
                isLevelOver = true;
            }
        }
        else if (other.gameObject.GetComponent<Powerup>() != null)
        {
            ActivatePowerUp(other.gameObject.tag);
            Destroy(other.gameObject);
        }
    }

    void ActivatePowerUp(string type)
    {
        switch (type)
        {
            case "Speed":
                speedUp = true;
                attractPickUps = false;
                jumpHigher = false;
                powerUpTime = 10f;
                break;
            case "Magnet":
                speedUp = false;
                attractPickUps = true;
                jumpHigher = false;
                powerUpTime = 15f;
                break;
            case "Jump":
                speedUp = false;
                attractPickUps = false;
                jumpHigher = true;
                powerUpTime = 10f;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("OOB"))
        {
            Start();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count == numCollectables && isLevelOver)
        {
            winText.text = "You Win!";
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }
}

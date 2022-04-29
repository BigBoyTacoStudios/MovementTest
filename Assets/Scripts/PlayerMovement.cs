//THINGS TO DO make player turn in direct of move then finish dash
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //string
    private string colliderName;
    //gameobjects
    public GameObject Player;
    public GameObject DashPoint;
    //rigidbody
    public Rigidbody rb;
    //vector3
    [Header("Forces")]
    public Vector3 leftForce;
    public Vector3 rightForce;
    public Vector3 jumpForce;
    private Vector3 dashTo;
    //bools
    private bool canJump = true;
    private bool wallCollide = false;
    private bool onWall = false;
    private bool canWall = true;
    private bool sameWall = false;
    private bool canDash = true;
    private bool LR;
    //float
    public float maxDashDistance = 30f;
    public float maxSpeed;
    // Update is called once per frame
    void Update()
    {
        InputCheck();
    }

    //checks input
    void InputCheck()
    {
        if(GetComponent<Rigidbody>().velocity.x >= maxSpeed)
        {
            if(Input.GetButtonDown("jump") && canJump == true)
            {
                rb.AddForce(jumpForce);
            }
            if(Input.GetKeyDown(KeyCode.LeftShift) && canDash == true)
            {
                if(LR == false)
                {
                    dashTo = Player.transform.position;
                    dashTo.x += 5;
                    dashTo.z = 0;
                    Player.transform.position = dashTo;
                }
                else
                {
                    dashTo = Player.transform.position;
                    dashTo.x -= 5;
                    Player.transform.position = dashTo;
                }
                canDash = false;
                Invoke("resetDash", 3f);
            }
            return;
        }
        else if(GetComponent<Rigidbody>().velocity.x <= -maxSpeed)
        {
            if(Input.GetButtonDown("jump") && canJump == true)
            {
                rb.AddForce(jumpForce);
            }
            if(Input.GetKeyDown(KeyCode.LeftShift) && canDash == true)
            {
                if(LR == false)
                {
                    dashTo = Player.transform.position;
                    dashTo.x += 5;
                    dashTo.z = 0;
                    Player.transform.position = dashTo;
                }
                else
                {
                    dashTo = Player.transform.position;
                    dashTo.x -= 5;
                    Player.transform.position = dashTo;
                }
                canDash = false;
                Invoke("resetDash", 3f);
            }
            if(Input.GetButton("right"))
            {
                rb.AddForce(rightForce * Time.deltaTime);
                //Player.transform.rotation = Quaternion.Euler(0, 0, 0);
                LR = false;
            }
            return;
        }
        if(Input.GetButton("left"))
        {
            rb.AddForce(leftForce * Time.deltaTime);
            //Player.transform.rotation = Quaternion.Euler(0, 180, 0);
            LR = true;
        }
        else if(Input.GetButton("right"))
        {
            rb.AddForce(rightForce * Time.deltaTime);
            //Player.transform.rotation = Quaternion.Euler(0, 0, 0);
            LR = false;
        }
        if(Input.GetButtonDown("jump") && canJump == true)
        {
            rb.AddForce(jumpForce);
        }
        //get off wall
        if(Input.GetButtonDown("jump") && onWall == true)
        {
            canWall = false;
            Invoke("resetWall", 0.5f);
            disableWall();
            rb.AddForce(jumpForce);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash == true)
        {
            if(LR == false)
            {
                dashTo = Player.transform.position;
                dashTo.x += 5;
                dashTo.z = 0;
                Player.transform.position = dashTo;
            }
            else
            {
                dashTo = Player.transform.position;
                dashTo.x -= 5;
                Player.transform.position = dashTo;
            }
            canDash = false;
            Invoke("resetDash", 3f);
        }
    }
    //walljump stuff
    void resetWall()
    {
        canWall = true;
    }
    void wallJumpCheck()
    {
        if(wallCollide == true && canWall == true && sameWall == false)
        {
            onWall = true;
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            Invoke("disableWall", 5f);
        }
        else
        {
            disableWall();   
        }
    }
    private void OnCollisionEnter(Collision other) 
    {
        if(other.collider.tag != "walljump")
        {
            sameWall = false;
            colliderName = "";
        }
        if(other.collider.tag == "ground")
        {
            canJump = true;
            sameWall = false;
        }
        else
        {
            canJump = false;
        }
    }
    private void OnCollisionExit(Collision other) 
    {
        canJump = false;
        sameWall = false;
    }
    private void OnCollisionStay(Collision other) 
    {
        if(other.collider.tag == "ground")
        {
            canJump = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "walljump")
        {
            wallCollide = true;
        }
        if(other.tag != "walljump")
        {
            wallCollide = false;
        }
        if(other.name == colliderName)
        {
            sameWall = true;
        }
        else
        {
            sameWall = false;
        }
        colliderName = other.name;
        wallJumpCheck();
    }
    private void OnTriggerExit(Collider other) 
    {
        wallCollide = false;
        if(other.tag == "walljump")
        {
            disableWall();
        }
    }
    public void disableWall()
    {
        onWall = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
    public void resetDash()
    {
        canDash = true;
    }
}
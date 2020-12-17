﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform bulletSpawn;
    public GameObject bullet;
    public int fireRate;


    public BulletManager bulletManager;

    [Header("Movement")]
    public float speed;
    public bool isGrounded;


    public RigidBody3D body;
    public CubeBehaviour cube;
    public Camera playerCam;

    // movement
    private bool wKey, aKey, sKey, dKey;
    private bool upArrow, downArrow, leftArrow, rightArrow;
    private bool spaceBar;

    void start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // _Fire();
        // _Move();
        Movement();
    }

    // moves character
    private void _Move()
    {
        if (isGrounded)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.0f)
            {
                // move right
                body.velocity = playerCam.transform.right * speed * Time.deltaTime;
            }

            if (Input.GetAxisRaw("Horizontal") < 0.0f)
            {
                // move left
                body.velocity = -playerCam.transform.right * speed * Time.deltaTime;
            }

            if (Input.GetAxisRaw("Vertical") > 0.0f)
            {
                // move forward
                body.velocity = playerCam.transform.forward * speed * Time.deltaTime;
            }

            if (Input.GetAxisRaw("Vertical") < 0.0f) 
            {
                // move Back
                body.velocity = -playerCam.transform.forward * speed * Time.deltaTime;
            }

            body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, 0.9f);
            body.velocity = new Vector3(body.velocity.x, 0.0f, body.velocity.z); // remove y
            

            if (Input.GetAxisRaw("Jump") > 0.0f)
            {
                body.velocity = transform.up * speed * 0.1f * Time.deltaTime;
            }

            transform.position += body.velocity;
        }
    }

    // on movement
    public void OnMove(InputAction.CallbackContext context)
    {
        // Value - triggers three times (start of press, full push, release)
        // Button - triggers three times (start of press, full push, release)
        // Pass Through - triggers twice (down and up)
        Debug.Log(context.control.name);

        switch (context.control.name)
        {
                // move forward
            case "w":
                wKey = !wKey;
                break;
            case "upArrow":
                upArrow = !upArrow;
                break;
                
                // move back
            case "s":
                sKey = !sKey;
                break;
            case "downArrow":
                downArrow = !downArrow;
                break;

                // move left
            case "a":
                aKey = !aKey;
                break;
            case "leftArrow":
                leftArrow = !leftArrow;
                break;

                // move right
            case "d":
                dKey = !dKey;
                break;
            case "rightArrow":
                rightArrow = !rightArrow;
                break;

                // jump
            case "spaceBar":
                spaceBar = !spaceBar;
                break;

            default:
                break;
        }

    }

    // movement calculation
    private void Movement()
    {
        // move forward
        if (wKey || upArrow)
            body.velocity = playerCam.transform.forward * speed * Time.deltaTime;

        // move back
        if (sKey || downArrow)
            body.velocity = -playerCam.transform.forward * speed * Time.deltaTime;

        // move left
        if (aKey || leftArrow)
            body.velocity = -playerCam.transform.right * speed * Time.deltaTime;

        // move right
        if (dKey || rightArrow)
            body.velocity = playerCam.transform.right * speed * Time.deltaTime;

        // lerp function
        // body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, 0.9f); // original
        body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, 0.9f);
        body.velocity = new Vector3(body.velocity.x, 0.0f, body.velocity.z); // remove y

        // jump
        if (spaceBar)
            body.velocity = transform.up * speed * 0.1f * Time.deltaTime;

        // velocity
        transform.position += body.velocity;
    }

    // fires bullet
    private void _Fire()
    {
        if (Input.GetAxisRaw("Fire1") > 0.0f)
        {
            // delays firing
            if (Time.frameCount % fireRate == 0)
            {

                var tempBullet = bulletManager.GetBullet(bulletSpawn.position, bulletSpawn.forward);
                tempBullet.transform.SetParent(bulletManager.gameObject.transform);
            }
        }
    }

    void FixedUpdate()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        isGrounded = cube.isGrounded;
    }

}

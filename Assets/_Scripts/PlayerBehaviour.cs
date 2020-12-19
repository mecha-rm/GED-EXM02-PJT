using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform bulletSpawn;
    public GameObject bullet;
    public int fireRate;


    // public BulletManager bulletManager;

    [Header("Movement")]
    public float speed;
    public bool isGrounded;


    public RigidBody3D body;
    public CubeBehaviour cube;
    public Camera playerCam;

    // movement keys
    private bool wKey, aKey, sKey, dKey;
    private bool upArrow, downArrow, leftArrow, rightArrow;

    // jump key
    private bool spaceBar;

    // fire key
    private bool fireBullet;

    // the collision manager
    public CollisionManager colManager;
    private bool resetBlocks = false; // variable for resetting blocks (stops it from being triggered twice)

    // the game state loader
    public GameStateLoader gsLoader;
    private bool saveData, loadData; // save and load variables

    // void start() // original name
    void Start()
    {
        // finds the collision manager in the scene.
        if (colManager == null)
            colManager = FindObjectOfType<CollisionManager>();

        // if the game state loader has not been set, it searches for one.
        if (gsLoader == null)
            gsLoader = FindObjectOfType<GameStateLoader>();

    }

    // Update is called once per frame
    void Update()
    {
        // _Fire();
        // _Move();
        Movement();
        FireBullet();
        // ChangeBullet();
    }

    // moves character (original function)
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
        // Debug.Log(context.control.name);

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

    // fires bullet (original function)
    private void _Fire()
    {
        if (Input.GetAxisRaw("Fire1") > 0.0f)
        {
            // delays firing
            if (Time.frameCount % fireRate == 0)
            {
                // var tempBullet = bulletManager.GetBullet(bulletSpawn.position, bulletSpawn.forward);
                // tempBullet.transform.SetParent(bulletManager.gameObject.transform);

                var tempBullet = BulletManager.GetInstance().GetBullet(bulletSpawn.position, bulletSpawn.forward);
                tempBullet.transform.SetParent(BulletManager.GetInstance().parentObject.transform);
            }
        }
    }

    // called when firing
    public void OnFire(InputAction.CallbackContext context)
    {
        // Debug.Log(context.control.name);

        if (context.control.name == "leftButton")
            fireBullet = !fireBullet;
    }

    // fires the bullet
    private void FireBullet()
    {
        if (fireBullet)
        {
            // delays firing
            if (Time.frameCount % fireRate == 0)
            {

                var tempBullet = BulletManager.GetInstance().GetBullet(bulletSpawn.position, bulletSpawn.forward);
                tempBullet.transform.SetParent(BulletManager.GetInstance().parentObject.transform);
            }
        }
    }

    // original function for changing the bullet
    private void ChangeBullet()
    {
        // this will need to be changed to updated input system
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            BulletManager.GetInstance().SetCurrentBulletType(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            BulletManager.GetInstance().SetCurrentBulletType(1);
        }
    }

    // changes the bullet
    public void OnBulletTypeChange(InputAction.CallbackContext context)
    {
        // Debug.Log("Control Name: " + context.control.name);

        // int bulletInt = context.ReadValue<int>();
        int bulletInt = 0;

        // due to the way this works, it provides a number 1 less than the number selected.
        switch(context.control.name)
        {
            case "1":
            case "numpad1":
                bulletInt = 0;
                break;

            case "2":
            case "numpad2":
                bulletInt = 1;
                break;

            default:
                break;
        }

        // changes type
        BulletManager.GetInstance().SetCurrentBulletType(bulletInt);
    }

    // general function for other actions
    public void OnUnspecificAction(InputAction.CallbackContext context)
    {
        // control
        // this gets triggered twice 
        switch(context.control.name)
        {
            case "r": // resets blocks
                resetBlocks = !resetBlocks;
                break;

            case "i": // import files
                loadData = !loadData;

                if (loadData) // if data can be loaded.
                    gsLoader.LoadContent();
                break;

            case "o": // export files
                saveData = !saveData;

                if (saveData) // if data should be saved.
                    gsLoader.SaveContent();
                break;

            case "c": // clears scene (don't do this?)
                colManager.DestroyCubesInList();
                break;

            default:
                break;
        }

        // resets blocks if triggered.
        if(resetBlocks)
            colManager.ResetCubes();
    }

    // fixed update
    void FixedUpdate()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        isGrounded = cube.isGrounded;
    }
}

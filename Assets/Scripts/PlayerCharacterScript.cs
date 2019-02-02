using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCharacterScript : NetworkBehaviour {

    private FPSViewScript mouseLook;
    private CharacterController player;
    
    private Vector3 move = Vector3.zero;
    private Vector2 moveInput;
    private Vector2 look;
    private bool jumpTick;

    public float fallSpeedMax;
    private Camera fpsCamera;
    public float moveSpeed;
    public float jumpForce;
    public float gravity;
    public float playerHealth;
    public bool dead;

    public static bool paused;
    public static bool deathMenuActive;
    public static bool freeMouse;

    public override void OnStartAuthority()
    {
        player = gameObject.GetComponent<CharacterController>();
        fpsCamera = Camera.main;
        mouseLook = player.GetComponent<FPSViewScript>();
        mouseLook.Initialisation(transform, fpsCamera.transform);

        if (transform.parent == !isLocalPlayer)
        {
            fpsCamera.enabled = false;
        }
        else
        {
            fpsCamera.gameObject.transform.parent = this.transform;
            fpsCamera.transform.position = player.transform.position;
            return;
        }
    }   
         
	
	void Update ()
    {
        CheckAuthority();
    }


    void CheckAuthority()
    {
        if (hasAuthority)
        {
            Movement();
            Rotate();
        }
        else
        {
            return;
        }           
    }





    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveInput = new Vector2(horizontal, vertical);

        if(moveInput.sqrMagnitude > 1)
        {
            moveInput.Normalize();
        }

        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

        move.x = moveDirection.x * moveSpeed;
        move.z = moveDirection.z * moveSpeed;

        if (player.isGrounded)
        {
            move.y = 0;
            jumpTick = false;
        }
        else
        {
            move.y -= gravity * Time.deltaTime;
        }

        if (Input.GetButton("Jump") && jumpTick == false)
        {
            move.y = jumpForce;
            jumpTick = true;
        }

        player.Move(move);
        mouseLook.UpdateCursorLock();
    }

    void Rotate()
    {
        mouseLook.LookRotation(transform, fpsCamera.transform);
    }

    void OnGUI()
    {
        if (hasAuthority)
        {
            GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 5, 5), "");
        }
        
    }
}

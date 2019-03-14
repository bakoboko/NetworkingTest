using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.Networking;

public class PlayerCharacterScript : NetworkBehaviour {

    private FPSViewScript mouseLook;
    private CharacterController player;
    private SpellSystem spellSys;

    private Vector3 move = Vector3.zero;
    private GameObject vrtk;
    private Camera fpsCamera;
    private Vector2 moveInput;
    private Vector2 look;
    private bool jumpTick;
    private KeywordRecognizer recognizer;

    private string[] keywords = new string[] { "fireball", "lightning","shield" };
    public float moveSpeed;
    public float jumpForce;
    public float gravity;
    public float playerHealth;


    public override void OnStartAuthority()
    {
        player = gameObject.GetComponent<CharacterController>();
        spellSys = gameObject.GetComponentInChildren<SpellSystem>();
        vrtk = GameObject.FindGameObjectWithTag("VRKit");
        fpsCamera = Camera.main;

        if (transform.parent != isLocalPlayer)
        {
            fpsCamera.enabled = false;
            vrtk.gameObject.SetActive(false);
        }
        else
        {
            vrtk.gameObject.SetActive(true);
            vrtk.transform.parent = this.transform;
            vrtk.transform.position = player.transform.position;
            recognizer = new KeywordRecognizer(keywords);
            recognizer.OnPhraseRecognized += SpeechRecognition;
            recognizer.Start();
            return;
        }


    }


    void Update()
    {
        CheckAuthority();
    }


    void CheckAuthority()
    {
        if (hasAuthority)
        {
            Movement();

        }
        else
        {
            return;
        }
    }

    void Movement()
    {
       // transform.rotation = fpsCamera.transform.rotation;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveInput = new Vector2(horizontal, vertical);

        if (moveInput.sqrMagnitude > 1)
        {
            moveInput.Normalize();
        }

        Vector3 moveDirection = fpsCamera.transform.TransformDirection(transform.forward * moveInput.y + transform.right * moveInput.x);

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
    }

    void SpeechRecognition(PhraseRecognizedEventArgs args)
    {
        if (transform.parent == isLocalPlayer)
        {
            print(args.text);
            if (args.text == "fireball")
            {
                spellSys.FireMode = true;
                spellSys.LitMode = false;
                spellSys.ShieldMode = false;
            }
            else if (args.text == "lightning")
            {
                spellSys.LitMode = true;
                spellSys.FireMode = false;
                spellSys.ShieldMode = false;
            }
            else if(args.text == "shield")
            {
                spellSys.ShieldMode = true;
                spellSys.FireMode = false;
                spellSys.LitMode = false;
            }
        }


    }




    void OnGUI()
    {
        if (hasAuthority)
        {
            GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 5, 5), "");
        }
        
    }
}

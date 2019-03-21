﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpellSystem : NetworkBehaviour {

    public GameObject pBolt;
    public float LitForce = 100;

    public GameObject pBall;
    public bool OnceInstance = false;

    private Vector3 wandFirePoint = Vector3.zero;
    private Quaternion wandPointRotation;

    public GameObject castPoint;
    public GameObject pShield;
    private bool shieldExpanding;
    private GameObject Shield;
    private Vector3 sEndRange;
    private float sActiveTimer;

    public bool FireMode, LitMode, ShieldMode;


    void Update ()
    {           
        GetInputs();
    }

    private void LightningBolt()
    {
        if (LitMode  && !shieldExpanding)
        {

            CmdSpawnLightning();
            LitMode = false;
        }
    }

    [Command]
    private void CmdSpawnLightning()
    {
        GameObject LitObj = Instantiate(pBolt, castPoint.transform.position, castPoint.transform.localRotation);
        LitObj.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * LitForce);
        NetworkServer.Spawn(LitObj);
    }



    private void FireBall()
    {
        if (!OnceInstance && FireMode && !shieldExpanding)
        {        
            CmdSpawnBall();
            OnceInstance = true;
            FireMode = false;
        }
    }

    [Command]
    private void CmdSpawnBall()
    {
        GameObject Ball = Instantiate(pBall, castPoint.transform.position, castPoint.transform.localRotation);
        int force = Random.Range(2000, 2500);
        Ball.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * force);
        NetworkServer.Spawn(Ball);
    }

    private void ShieldCall()
    {
        sEndRange = new Vector3(castPoint.transform.localPosition.x, castPoint.transform.localPosition.y, castPoint.transform.localPosition.z + 1f);
        if (ShieldMode && !shieldExpanding)
        {
            Shield = Instantiate(pShield, castPoint.transform);
            Shield.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * 150);
            shieldExpanding = true;
            sActiveTimer = Time.time + 5;
            ShieldMode = false;
        }
        if (shieldExpanding)
        {
            if (Shield.transform.position.z >= sEndRange.z)
            {
                Shield.GetComponent<Rigidbody>().Sleep();
                Shield.GetComponent<Rigidbody>().WakeUp();
            }
            if (Shield.transform.localScale.x <= 12)
            {
               Shield.transform.localScale += new Vector3(0.5f, 0.5f, 0);
            }

            if (Time.time >= sActiveTimer)
            {
                shieldExpanding = false;
                Destroy(Shield);
            }
        }
    }

    private void GetInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (FireMode)
            {
                FireBall();
            }

            if (LitMode)
            {

                LightningBolt();
            }
            if (ShieldMode)
            {

                ShieldCall();
            }
        }
    }
}

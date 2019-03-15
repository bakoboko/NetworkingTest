using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpellSystem : NetworkBehaviour {

    public GameObject pBolt;
    public float LitForce = 100;

    public GameObject pBall;
    public static bool OnceInstance = false;

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
            GameObject LitObj = Instantiate(pBolt, castPoint.transform.localPosition, castPoint.transform.localRotation);
            NetworkServer.Spawn(LitObj);
            LitObj.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * LitForce);
            LitMode = false;
        }
    } 

  
    private void FireBall()
    {
        if (!OnceInstance && FireMode && !shieldExpanding)
        {
            GameObject Ball = Instantiate(pBall, castPoint.transform.position, castPoint.transform.localRotation);
            NetworkServer.Spawn(Ball);
            int force = Random.Range(2000, 2500);
            Ball.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * force);
            OnceInstance = true;
            FireMode = false;
        }
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

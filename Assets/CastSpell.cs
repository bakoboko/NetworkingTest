using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CastSpell : NetworkBehaviour {

    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {

            print("Entering");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CastRadius"))
        {
            player.GetComponent<SpellSystem>().shouldCast = true;
            print("hello");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MoveScript : NetworkBehaviour {
    GameObject[] lobbyMenus;

    private void Awake()
    {
        lobbyMenus = GameObject.FindGameObjectsWithTag("characterSelect");
        foreach (GameObject i in lobbyMenus)
            i.SetActive(false);
    }

    void Update ()
    {
        if (isLocalPlayer)
            if (Input.GetKeyDown(KeyCode.A))
                transform.Translate(Vector3.right * 2);
	}
}

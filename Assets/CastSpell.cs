<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CastSpell : MonoBehaviour {

    public GameObject player;
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CastSpell : NetworkBehaviour {

    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {

            print("Entering");
    }
>>>>>>> master

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CastRadius"))
        {
            player.GetComponent<SpellSystem>().shouldCast = true;
<<<<<<< HEAD
        }
    }
}
=======
            print("hello");
        }
    }
}
>>>>>>> master

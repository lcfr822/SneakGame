using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileNPC : NPC
{
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Renderer>().IsVisibleFrom(viewCamera))
        {
            ConfirmPlayerVisibility();
        }
        else
        {
            if (playerInSight)
            {
                playerInSight = false;
                onLoseSight.Invoke(gameObject);
            }
        }
    }
}

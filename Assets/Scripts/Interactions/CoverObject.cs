using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverObject : MonoBehaviour
{
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(player == null) { player = FindObjectOfType<PlayerController>(); }
        if (player.coverObject == null && other.gameObject.tag.Equals("Player"))
        {
            player.coverObject = gameObject;
            player.uiManager.HideCoverGroup();
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (player == null) { player = FindObjectOfType<PlayerController>(); }
        if (player.coverObject != null && other.gameObject.tag.Equals("Player"))
        {
            player.coverObject = null;
            player.uiManager.HideCoverGroup();
        }
    }
}

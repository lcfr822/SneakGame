using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpableObject : CoverObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (player == null) { player = FindObjectOfType<PlayerController>(); }
        if (other.gameObject.tag.Equals("Player") && !player.CanVault)
        {
            player.CanVault = true;
            player.uiManager.ShowVaultGroup();
            base.OnTriggerEnter(other);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (player == null) { player = FindObjectOfType<PlayerController>(); }
        if (other.gameObject.tag.Equals("Player") && player.CanVault)
        {
            player.CanVault = false;
            player.uiManager.HideVaultGroup();
            base.OnTriggerExit(other);
        }
    }
}

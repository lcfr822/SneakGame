using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
    public bool playerInSight = false;
    public GameObject player;
    public Camera viewCamera;
    public enum ColorCodes { Pink, Teal, Orange }
    public ColorCodes colorCode = ColorCodes.Orange;
    public Color debugColorCode = Color.red;
    public UnityGOEvent onPlayerNoticed;
    public UnityGOEvent onLoseSight;

    private void OnValidate()
    {
        if ((colorCode.Equals(ColorCodes.Orange) && debugColorCode.Equals(Color.red)) ||
            (colorCode.Equals(ColorCodes.Pink) && debugColorCode.Equals(Color.magenta)))
        {
            return;
        }

        switch (colorCode)
        {
            case ColorCodes.Orange:
                debugColorCode = Color.red;
                break;
            case ColorCodes.Teal:
                debugColorCode = Color.cyan;
                break;
            case ColorCodes.Pink:
                debugColorCode = Color.magenta;
                break;
            default:
                Debug.LogError("Invalid Color Code for: " + gameObject.name);
                break;
        }
    }

    public void ConfirmPlayerVisibility()
    {
        Vector3 viewCameraPos = viewCamera.transform.position;
        Vector3 viewDirection = (player.transform.position - viewCameraPos).normalized * viewCamera.farClipPlane;

        Debug.DrawRay(viewCameraPos, viewDirection, Color.green);
        if (Physics.Raycast(new Ray(viewCameraPos, viewDirection), out RaycastHit raycastHit))
        {
            if (raycastHit.collider.gameObject.tag.Equals("Player"))
            {
                onPlayerNoticed.Invoke(gameObject);
                playerInSight = true;
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

[System.Serializable]
public class UnityGOEvent : UnityEvent<GameObject> { }

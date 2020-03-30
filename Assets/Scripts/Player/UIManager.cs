using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CanvasGroup coverGroup, vaultGroup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleCanvasGroup(CanvasGroup canvasGroup)
    {
        if (canvasGroup.alpha >= 1)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
    }

    public void ShowCoverGroup() { ToggleCanvasGroup(coverGroup); }
    public void HideCoverGroup() { ToggleCanvasGroup(coverGroup); }

    public void ShowVaultGroup() { ToggleCanvasGroup(vaultGroup); }
    public void HideVaultGroup() { ToggleCanvasGroup(vaultGroup); }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthManager : MonoBehaviour
{
    public List<GameObject> watchers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NoticePlayer(GameObject watcher)
    {
        if (watchers.Contains(watcher)) { return; }
        watchers.Add(watcher);
//        Debug.LogWarning(watcher.name + " has noticed me!");
    }

    public void LosePlayer(GameObject watcher)
    {
        if (!watchers.Contains(watcher)) { return; }
        watchers.Remove(watcher);
//        Debug.LogWarning(watcher.name + " has lost track of me!");
    }
}

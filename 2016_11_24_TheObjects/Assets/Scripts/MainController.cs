using UnityEngine;
using System;

public class MainController : MonoBehaviour {
    Transform eyePlayerTransform;
	void Start()
    {
        eyePlayerTransform = GameObject.Find("Player").transform.FindChild("Camera");
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Insert)) {
            var result = GetComponent<NPCSetupManager>().AddObject(eyePlayerTransform);
            if (!result)
                Debug.Log("Can't to add more");
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            var result = GetComponent<NPCSetupManager>().RemoveAllObjects(eyePlayerTransform);
            Debug.Log(String.Format("Removed {0} objects", result));
        }
	}
}

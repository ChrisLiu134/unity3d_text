using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKey : MonoBehaviour {
    public GameObject pressAnyKey;
    public GameObject startAndeLoad;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey)
        {
            pressAnyKey.SetActive(false);
            startAndeLoad.SetActive(true);
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFllow : MonoBehaviour {

    public Transform target;
    public float backDistance = 5f;
    public float rightDistance = 5f;
    public float height = 3f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target!=null)
        {
            transform.position = target.position + Vector3.back * backDistance + Vector3.up * height+Vector3.right*rightDistance;
            transform.LookAt(target);
        }
        else
        {
            return;
        }
	}
}

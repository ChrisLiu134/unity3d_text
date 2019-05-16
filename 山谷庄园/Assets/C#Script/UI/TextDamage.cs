using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDamage : MonoBehaviour {
    TextMesh textDamage;
    public Transform cameraTransform;
	// Use this for initialization
	void Awake () {
        textDamage = GetComponent<TextMesh>();
        cameraTransform = FindObjectOfType<FollowCamera>().transform;
        transform.rotation = cameraTransform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = cameraTransform.rotation;
        textDamage.GetComponent<Renderer>().material.color -= new Color(0, 0, 0, 1.5f * Time.deltaTime); //该组件的颜色渐渐变淡
        transform.Translate(Vector3.up * 1.5f * Time.deltaTime);    //该组件的位置渐渐上移
        if (textDamage.GetComponent<Renderer>().material.color.a <= 0)  //当该组件消失时， 消耗这个物体
        {
            Destroy(gameObject);
        }
    }

    public void GetInfo(string s,Color c)
    {
        textDamage.text = s;
        textDamage.GetComponent<Renderer>().material.color = c;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回复泉水
/// </summary>
public class RejuvenateSpring : MonoBehaviour {

    /// <summary>
    /// 加血特效
    /// </summary>
    public GameObject healEffect;

    float time;//计时器

    //加血特效
    public AudioClip healClip;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Collider[] colliders  = Physics.OverlapSphere(transform.position, 3f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<PlayerCharater>()!=null)
            {
                time += Time.deltaTime;
                if (time>1)
                {
                    time = 0;
                    colliders[i].GetComponent<PlayerCharater>().player.hp += colliders[i].GetComponent<PlayerCharater>().player.maxHp / 5;
                    colliders[i].GetComponent<PlayerCharater>().player.mp += colliders[i].GetComponent<PlayerCharater>().player.maxMp / 5;
                    AudioSource.PlayClipAtPoint(healClip, colliders[i].transform.position);
                    GameObject obj = Instantiate(healEffect, colliders[i].transform.position , Quaternion.identity, colliders[i].transform);
                    Destroy(obj, 0.6f);
                }
                
            }
        }


    }
}

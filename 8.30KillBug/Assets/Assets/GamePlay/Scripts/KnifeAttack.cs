using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAttack : MonoBehaviour {
    public int attack = 40;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            if (enemy.isAlive==false)
            {
                return;
            }
            enemy.OnHit(attack);
            if (enemy.health <= 0||enemy.transform.position.y<=-10)
            {
                enemy.Die();
            }
           
        }
    }
}

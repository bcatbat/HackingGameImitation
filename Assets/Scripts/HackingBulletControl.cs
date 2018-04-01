using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingBulletControl : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        // myShell, hurt enemy
        if (this.CompareTag("MyShell"))
        {
            if (other.transform.CompareTag("Enemy"))
            {
                var boss = other.gameObject.GetComponent<HackingBossControl>();
                var enemy = other.gameObject.GetComponent<HackingSoldierControl>();
                if (boss != null)
                    boss.Hurt();
                if (enemy != null)
                    enemy.Hurt();
                // boom! sound need..
            }            
        }

        // enemyShell, hurt me
        if (this.CompareTag("EnemyShell"))
        {
            if (other.transform.CompareTag("Player"))
            {
                // boom! sound need..
                var player = other.gameObject.GetComponent<HackingPlayerControl>();
                player.Hurt();
            }
            if (other.transform.CompareTag("EnemyShield"))
            {
                // guard sound need.
                return;
            }
        }        
        Destroy(this.gameObject);
        //Debug.Log(this.tag);
    }
}

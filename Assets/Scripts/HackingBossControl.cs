using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingBossControl : MonoBehaviour {
    public Transform[] gunPos;
    public GameObject bulletPrefab;

    // self
    public int hp = 5;
    private Rigidbody rb;

    // 弹幕参数
    private float atkTick = float.MaxValue;
    private float gcdTime = 0.5f;
    private float bulletVelocity = 5f;
    [SerializeField]private float rotateRate = 120f;
    [SerializeField] private GameObject shield;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        this.tag = "EnemyShield";
    }
    private void Update()
    {
        atkTick += Time.deltaTime;

        // Barrage
        RotatingBarrageFire();
    }

    // 挨打
    public void Hurt()
    {
        if (hp > 1)
        {
            Debug.Log("bingo");
            hp--;
        }
        else
        {
            Debug.Log("boss die");
            Destroy(this.gameObject);
        }
    }

    // 旋转弹幕
    void RotatingBarrageFire()
    {
        // fire
        if (atkTick >= gcdTime)
        {
            foreach (var gp in gunPos)
            {
                // bullet
                GameObject bullet = Instantiate(bulletPrefab, gp.position, new Quaternion());
                bullet.tag = "EnemyShell";
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                bulletRb.velocity = gp.forward * bulletVelocity;
                
                atkTick = 0;
            }
        }
        // rotate
        Quaternion curRot = rb.rotation;
        rb.MoveRotation(curRot * Quaternion.Euler(0, rotateRate * Time.deltaTime, 0));
    }

    public void ShieldDestroy(object sender, EventArgs e)
    {
        Debug.Log("On Shield Destroyed detect.");
        if (HackingGameManager.Instance.LastSolider())
        {
            // shield destroyed.
            Debug.Log("Shield Destroyed!");
            this.tag = "Enemy";
            // shield booooom!! effect need..
            Destroy(this.shield);
        }
    }
}

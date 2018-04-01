using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingPlayerControl : MonoBehaviour {
    public GameObject bulletPrefab;
    public Transform gunPos;

    // self
    private int hp = 5;
    private Rigidbody rb;
    private float move_speed = 7f;
    // atk
    private float atkTick = float.MaxValue;
    private float gcdTime = 0.2f;    
    private float bulletVelocity = 20f;


    // Use this for initialization
    void Awake () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        // global ticking
        atkTick += Time.deltaTime;

        Move();
        Fire();
	}

    void Move()
    {
        // 获取输入
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 移动位置(h,0,v)
        Vector3 curPos = transform.position;
        Vector3 tarPos = curPos + new Vector3(h, 0, v).normalized * move_speed * Time.deltaTime;
        rb.MovePosition(tarPos);

        // 转向
        Rotate();
    }
    void Rotate()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        Vector3 tarPos = hit.point;
        tarPos.y = 0;
        Vector3 curPos = rb.position;

        rb.rotation = Quaternion.LookRotation(tarPos - curPos);
    }

    void Fire()
    {
        if (atkTick >= gcdTime && Input.GetButton("Fire1"))
        {
            //Debug.Log("shot");
            GameObject bullet = Instantiate(bulletPrefab,gunPos.position,new Quaternion());
            bullet.tag = "MyShell";
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();            
            bulletRb.velocity = gunPos.forward * bulletVelocity;            

            atkTick = 0;
        }
    }

    public void Hurt()
    {
        if (hp > 1)
        {
            Debug.Log("ooop!");
            hp--;
        }
        else
        {
            Debug.Log("die...");
            Destroy(this.gameObject);
        }
    }
}

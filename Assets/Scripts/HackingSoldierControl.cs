using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingSoldierControl : MonoBehaviour {
    public Transform gunPos;
    public GameObject bulletPrefab;

    // self
    public int hp = 5;
    private Rigidbody rb;
    // atk
    private float atkTick = float.MaxValue;
    private float gcdTime = 0.3f;
    private float bulletVelocity = 12f;
    private Transform player;
    // move
    private float moveTick = float.MaxValue;
    private float intervalTime = 1f;
    private float moveSpeed = 6f;
    private Vector3 direction;
    private float widthBorder;
    private float heightBorder;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        widthBorder = HackingGameManager.Instance.iWidth *0.8f;
        heightBorder = HackingGameManager.Instance.iHeight * 0.8f;
    }

    private void FixedUpdate()
    {
        // global ticking
        atkTick += Time.deltaTime;
        moveTick += Time.deltaTime;

        // fire
        if(player!=null)
            Shot();

        // move
        RandomMove();
    }

    // random move: 每隔xx秒 随机选择一个方向移动
    void RandomMove()
    {
        if (moveTick >= intervalTime)
        {
            // 随机一个方向.
            float xDirection =UnityEngine.Random.Range(-1.0f, 1.0f);
            float zDirection =UnityEngine.Random.Range(-1.0f, 1.0f);
            direction = new Vector3(xDirection, 0, zDirection);
            //Debug.Log(direction);
            moveTick = 0f;
        }
        Vector3 curPos = rb.position;
        // border-x
        if (curPos.x < -widthBorder / 2)
            direction.x = Mathf.Abs(direction.x);
        else if (curPos.x > widthBorder / 2)
            direction.x = -Mathf.Abs(direction.x);
        // border-y
        if (curPos.z < -heightBorder / 2)
            direction.z = Mathf.Abs(direction.z);
        else if (curPos.z > heightBorder / 2)
            direction.z = -Mathf.Abs(direction.z);
        // move
        Vector3 tarPos = curPos + direction.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(tarPos);
    }

    // aiming shot
    void Shot()
    {
        // aiming at player
        Vector3 curPos = rb.position;
        Vector3 tarPos = player.position;
        rb.rotation = Quaternion.LookRotation(tarPos - curPos);

        // fire
        if (atkTick >= gcdTime)
        {
            //Debug.Log("enemy shot");
            GameObject bullet = Instantiate(bulletPrefab, gunPos.position, new Quaternion());
            bullet.tag = "EnemyShell";
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = gunPos.forward * bulletVelocity;

            atkTick = 0;
        }
    }

    public void Hurt()
    {
        if (hp > 1)
        {
       //     Debug.Log("ooop!");        
            hp--;
        }
        else
        {
            Die();
        }
    }

    public event EventHandler Dead;
    protected virtual void OnDied(EventArgs e)
    {
        if (Dead != null)
            Dead(this, e);
    }
    private void Die()
    {
        Debug.Log("die...");
        EventArgs e = new EventArgs();
        OnDied(e);        
        HackingGameManager.Instance.RemoveSolider(this);
        Destroy(this.gameObject);
    }
}

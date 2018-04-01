using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HackingGameManager : MonoBehaviour {
    // singleton
    private static HackingGameManager instance;
    public static HackingGameManager Instance{ get { return instance; } }

    // prefabs
    public GameObject planePrefab;
    public GameObject wallCubePrefab;    
    public GameObject playerPrefab;
    public GameObject bossPrefab;
    public GameObject soldierPrefab;

    // environment
    public int iWidth;
    public int iHeight;

    // walls
    private GameObject walls;

    //// role list
    //public HackingPlayerControl player;
    //public List<HackingBossControl> bossList;
    private HackingBossControl boss;
    private List<HackingSoldierControl> soldierList;    

    private void Awake()
    {
        instance = this;        
        walls = new GameObject("environment");
        soldierList = new List<HackingSoldierControl>();

        InitEnvironment(iWidth,iHeight);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ResetGame();
        }
    }

    private void InitEnvironment(int width, int height  )
    {
        // plane
        InitPlane(width, height);
        // walls
        InitWalls(width, height);
        // player
        InitPlayer();
        // enemy
        InitSolider();
        // boss
        InitBoss();
    }

    private void InitPlane(int width, int height)
    {
        var p = Instantiate(planePrefab);
        p.transform.localScale = new Vector3(width / 5, 1, height / 5);
        
        Camera.main.transform.position = new Vector3(0, width, 0);
    }

    private void InitWalls(int width, int height)
    {
        float w = width;
        float h = height;
        if(width<=0 || height <= 0)
        {
            Debug.LogError("场景尺寸不能为负");
        }

        for (int i = 0; i < width - 1; ++i)
        {
            Vector3 curPos = new Vector3(-(w - 1) / 2 + i, 0, -(h - 1) / 2);
            GameObject curWallCube = Instantiate(wallCubePrefab, curPos, new Quaternion());
            curWallCube.transform.parent = walls.transform;
        }
        for (int j = 0; j < height - 1; ++j)
        {
            Vector3 curPos = new Vector3((w - 1) / 2, 0, -(h - 1) / 2 + j);
            GameObject curWallCube = Instantiate(wallCubePrefab, curPos, new Quaternion());
            curWallCube.transform.parent = walls.transform;
        }
        for (int i = width - 1; i > 0; --i)
        {
            Vector3 curPos = new Vector3(-(w - 1) / 2 + i, 0, (h - 1) / 2);
            GameObject curWallCube = Instantiate(wallCubePrefab, curPos, new Quaternion());
            curWallCube.transform.parent = walls.transform;
        }
        for (int j = height - 1; j > 0; --j)
        {
            Vector3 curPos = new Vector3(-(w - 1) / 2, 0, -(h - 1) / 2 + j);
            GameObject curWallCube = Instantiate(wallCubePrefab, curPos, new Quaternion());
            curWallCube.transform.parent = walls.transform;
        }
    }

    private void InitPlayer()
    {
        Vector3 spawnPoint = new Vector3(0, 0, -iHeight / 4);
        Instantiate(playerPrefab, spawnPoint, new Quaternion());
    //    player = p.GetComponent<HackingPlayerControl>();
    }

    private void InitBoss()
    {
        Vector3 spawnPoint = new Vector3(0, 0, iHeight / 4);
       var b =Instantiate(bossPrefab, spawnPoint, new Quaternion());
        //  bossList.Add(b.GetComponent<HackingBossControl>());
        boss = b.GetComponent<HackingBossControl>();
      foreach(var s in soldierList)
        {
            s.Dead += boss.ShieldDestroy;
        }
    }

    private void InitSolider()
    {
        Vector3 spawnPoint = new Vector3(-iWidth / 4, 0, iHeight / 4);
       var e1= Instantiate(soldierPrefab, spawnPoint, new Quaternion());
        AddSoldier(e1.GetComponent<HackingSoldierControl>());
        spawnPoint = new Vector3(iWidth / 4, 0, iHeight / 4);
       var e2= Instantiate(soldierPrefab, spawnPoint, new Quaternion());
       AddSoldier(e2.GetComponent<HackingSoldierControl>());
    }

    // 退出 or 胜利.

    // 重开
    private void ResetGame()
    {
        SceneManager.LoadScene("hacking game");
    }


    public void AddSoldier(HackingSoldierControl soldier)
    {
        soldierList.Add(soldier);
    }

    public bool RemoveSolider(HackingSoldierControl enemy)
    {
        enemy.Dead -= boss.ShieldDestroy;
        return soldierList.Remove(enemy);
    }

    public bool LastSolider()
    {
        return soldierList.Count== 1;
    }

}

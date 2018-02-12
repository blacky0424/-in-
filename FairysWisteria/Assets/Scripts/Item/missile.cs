using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : EnemyBase
{ 
    float time = 0f;   //一定時間で消滅
    float minDis = 128;  //マップの高さ
    private GameObject point;  //暫定1位
    public GameObject[] enemys;  //1位候補群
    public Transform target;   //ターゲット
    public float speed;  //速度
    public float missileRotationSmooth = 1.3f;  //旋回速度

    private PlayerController.PlayerNum m_playerNum;

    // Use this for initialization
    void Start()
    {

        enemys = GameObject.FindGameObjectsWithTag("Player");
 //       Vector3 pos = transform.position;
        Vector3 newpos = new Vector3(0, minDis, 0);
        foreach (GameObject enemy in enemys)
        {
            Vector3 epos = enemy.transform.position;
            epos.x = 0;
            Vector3 newepos = epos;
            float dis = Vector3.Distance(newpos, newepos);  //マップの高さ - プレイヤーの高さ
            if (dis < minDis)   
            {
                minDis = dis;
                point = enemy;
            }
        }
        target = point.transform;

        //        target = point.transform.position;
        //        target = GameObject.FindWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.Translate(0, speed + 0f, 0);
        if (time > 5)
        {
            Destroy(gameObject);
        }
        if (speed < 0.15f)
        {
            speed += (0.01f * time / 5);  
        }
        //       missileRotationSmooth = Time.deltaTime * missileRotationSmooth + missileRotationSmooth;
        // プレイヤーの方向に向ける

        Vector3 targetvec = target.position - transform.position;
        float targetangle = Vector2.Angle(targetvec, Vector2.up);
        //        print(targetvec);
        if (targetvec.x > 0)  
        {
            targetangle = -targetangle;  //x軸反転
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetangle), Time.deltaTime * missileRotationSmooth);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(other.gameObject.GetComponent<PlayerController>().GetPlayerNum() != m_playerNum)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetShotPlayerNum(PlayerController.PlayerNum player)
    {
        m_playerNum = player;
    }
}
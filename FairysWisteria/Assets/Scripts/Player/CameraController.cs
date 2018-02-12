using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラの挙動を管理するクラス
/// 作成者　関口駿
/// 作成日　12/11
/// </summary>
public class CameraController : MonoBehaviour
{
    //追従するプレイヤー
    Transform m_target;
    //playerのTransForm
    public GameObject[] m_ObjectsBox;


    //ゴールの高さ
    [SerializeField]
    float m_goalHeight;

    void Start()
    {
        //すべてのプレイヤーを取得
        m_ObjectsBox = GameObject.FindGameObjectsWithTag("Player");
        //プレイヤー１を初期ターゲットに
        m_target = m_ObjectsBox[0].transform;
    }

    void Update()
    {
        for (int i = 0; i < m_ObjectsBox.Length; i++)
        {
            //ターゲットよりy軸が高い位置のプレイヤーを見つける
            if (m_ObjectsBox[i].transform.position.y > m_target.position.y)
            {
                //ターゲットの変更
                m_target = m_ObjectsBox[i].transform;
            }

        }

        //カメラの座標更新
        transform.position = new Vector3(transform.position.x, m_target.position.y, -10);
    }
}
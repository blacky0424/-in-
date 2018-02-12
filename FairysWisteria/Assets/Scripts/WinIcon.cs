using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// WinIconの動きを管理するクラス
/// 作成者　関口駿
/// 作成日　12/14
/// </summary>
public class WinIcon : MonoBehaviour {

    //ローカルスケール
    float m_sizeX;
    float m_sizeY;
    float m_sizeZ;

    //大きさを変更するスイッチ
    bool reduction = false;


    void Start () {
        m_sizeX = transform.localScale.x;
        m_sizeY = transform.localScale.y;
        m_sizeZ = transform.localScale.z;
    }

	void Update () {
        if (reduction)
        {
            m_sizeX -= 0.005f;
            m_sizeY -= 0.005f;
            m_sizeZ -= 0.005f;
        }
        else
        {
            m_sizeX += 0.005f;
            m_sizeY += 0.005f;
            m_sizeZ += 0.005f;
        }

        if(m_sizeX > 0.5f)
        {
            reduction = true;
        }
        else if(m_sizeX < 0.4f)
        {
            reduction = false;
        }
        transform.localScale = new Vector3(m_sizeX, m_sizeY, m_sizeZ);
    }
}

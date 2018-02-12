using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ResultSceneを管理するクラス
/// 作成者　関口駿
/// 作成日　12/14
/// </summary>
public class ResultScene : MonoBehaviour {
    [SerializeField]
    Text m_winnerText;
    public int m_winNum;

    List<Sprite> m_usePlayerIconBox = new List<Sprite>();
    List<SpriteRenderer> m_useIconOcjectBox = new List<SpriteRenderer>();
    [SerializeField]
    Sprite[] m_iconBox = new Sprite[8];
    SpriteRenderer[] m_iconObjectBox = new SpriteRenderer[4];

    void Start () {

        m_winNum = GameManager.winnerNum;
        int playerCount = GameManager.PlayerNum;
		Debug.Log(m_winNum);
		Debug.Log(playerCount);
		//m_winnerText.text = "Win " + m_winNum.ToString() + "P";

        m_iconObjectBox = GameObject.Find("IconBox").GetComponentsInChildren<SpriteRenderer>();

        m_usePlayerIconBox.Add(m_iconBox[(m_winNum) * 2]);
        for (int i = 0; i < playerCount; i++){
            if (i != m_winNum) {
                m_usePlayerIconBox.Add(m_iconBox[(i * 2) + 1]);
            }
        }

        m_useIconOcjectBox.Add(m_iconObjectBox[0]);
        switch (playerCount)
        {
            case 2:
                m_useIconOcjectBox.Add(m_iconObjectBox[2]);
                break;
            case 3:
                m_useIconOcjectBox.Add(m_iconObjectBox[1]);
                m_useIconOcjectBox.Add(m_iconObjectBox[3]);
                break;
            case 4:
                for(int i = 1;i < 4;i++)
                {
                    m_useIconOcjectBox.Add(m_iconObjectBox[i]);
                }
                break;
        }

        for(int i = 0;i < m_usePlayerIconBox.Count; i++)
        {
            m_useIconOcjectBox[i].sprite = m_usePlayerIconBox[i];
        }

	}
}

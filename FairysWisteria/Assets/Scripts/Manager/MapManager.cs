//------------------------------------------------------------------
//スクリプト名：MapManager.cs
//概要：マップ上のオブジェクトを管理する
//作成者:日本電子専門学校＿張越
//作成日:2017/12/11(月)
//------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //System.IO.FileInfo, System.IO.StreamReader, System.IO.StreamWriter
using System;
using System.Text;

public class MapManager : MonoBehaviour 
{
	//----------------------------------------
	//変数宣言
	//----------------------------------------
	[SerializeField]
	private GameObject m_Wisteria;
	private int m_Frame;
	[SerializeField]
	private Transform[] m_WhereCreateWisteria;
	private const int m_MapHight = 128;
	private const int m_MapWeith = 7;
	private const float m_WisteriaSize = 128.0f;
	[SerializeField]
	private int m_PlayerNum;
	[SerializeField]
	private GameObject m_Player;
	[SerializeField]
	private GameObject m_PlayerStamina;
	private int[,] MapData
	= new int[m_MapHight,m_MapWeith];

	[SerializeField]
	private GameObject m_PlayerBox;
	[SerializeField]
	private GameObject m_StaminaBox;


	private void Awake()
	{
		ReadMapData();
		CreateFlower();
		CreatePlayer(GameManager.PlayerNum);
	}
	/// <summary>
	/// 開始後一回実行
	/// </summary>
	private void Start () 
	{

	}
	
	
	/// <summary>
	/// 毎フレーム更新
	/// </summary>
	private void Update () 
	{
		m_Frame++;
	}

	/// <summary>
 	/// 外部から花の生成座標を見る
 	/// </summary>
 	/// <returns>マップマネージャーが持っているトランスフォームの配列</returns>
 	public Transform[] GetCreatPos()
 	{
 		return m_WhereCreateWisteria;
 	}

	/// <summary>
	/// プレイヤーを生成する
	/// </summary>
	private void CreatePlayer(int PlayerNum)
	{
		//Instantiate(m_PlayerBox, this.transform.position, Quaternion.identity);
		GameObject obj = null;
		Vector3 createPos = Vector3.zero;
		switch (PlayerNum)
		{
			case 1:
				Debug.Log("ぼっちはげーむするんじゃねーぞ！");
				break;
			case 2:
				for (int i = 0; i < PlayerNum; i++)
				{
					createPos = this.transform.GetChild(1).GetChild(0).GetChild(i).position;
					obj = Instantiate(m_Player, createPos, Quaternion.identity);
					obj.transform.GetComponent<PlayerController>().SetPlayerNumber((PlayerController.PlayerNum)i);
					obj.transform.parent = m_PlayerBox.transform;
					
					//プレイヤースタミナUI生成
					obj = Instantiate(m_PlayerStamina, createPos, Quaternion.identity);
					obj.transform.parent = m_StaminaBox.transform;
				}
				break;
			case 3:
				for (int i = 0; i < PlayerNum; i++)
				{
					createPos = this.transform.GetChild(1).GetChild(1).GetChild(i).position;
					obj = Instantiate(m_Player, createPos, Quaternion.identity);
					obj.transform.GetComponent<PlayerController>().SetPlayerNumber((PlayerController.PlayerNum)i);
					obj.transform.parent = m_PlayerBox.transform;

					//プレイヤースタミナUI生成
					obj = Instantiate(m_PlayerStamina, createPos, Quaternion.identity);
					obj.transform.parent = m_StaminaBox.transform;
				}
				break;
			case 4:
				for (int i = 0; i < PlayerNum; i++)
				{
					createPos = this.transform.GetChild(1).GetChild(2).GetChild(i).position;
					obj = Instantiate(m_Player, createPos, Quaternion.identity);
					obj.transform.GetComponent<PlayerController>().SetPlayerNumber((PlayerController.PlayerNum)i);
					obj.transform.parent = m_PlayerBox.transform;

					//プレイヤースタミナUI生成
					obj = Instantiate(m_PlayerStamina, createPos, Quaternion.identity);
					obj.transform.parent = m_StaminaBox.transform;
				}
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// 藤の花のつたを生成する
	/// </summary>
	private void CreateFlower()
	{
		float YPos = m_WhereCreateWisteria[0].position.y;

		for (int i = 0; i < m_MapHight; i++)
		{
			for (int j = 0; j < m_MapWeith; j++)
			{
				GameObject obj = null;
				switch (MapData[i, j])
				{
					case 0:
						break;
					case 1:
					case 2:
					case 3:
					case 4:
						obj = Instantiate(m_Wisteria,new Vector3(m_WhereCreateWisteria[j].position.x,YPos,this.transform.position.z), Quaternion.identity);
						obj.transform.parent = this.transform;
						Wisteria wis = obj.transform.GetComponent<Wisteria>();
						wis.SetType((Wisteria.Type)MapData[i,j]);
						break;
					default:
						break;
				}
			}
			YPos += m_WisteriaSize / 100;
		}
	}


	/// <summary>
	/// マップデータを読み込む
	/// </summary>
	void ReadMapData()
	{
		//マップデータをファイルから読み込む
		string txt = "null";
		FileInfo fi = new FileInfo(Application.dataPath + "/" + "Resources" + "/"+ "MapData.txt");
		using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
		{
			txt = sr.ReadToEnd();
		}


		//準備した配列にマップデータを入れる
		int i = 0;
		int j = 0;
		//行分け
		string[] lines = txt.Split('\n');
		foreach (var line in lines)
		{
			if (line == "")
			{
				continue;
			}

			//スペース分け
			string[] words = line.Split();
			foreach (var word in words)
			{
				if (word == "")
				{
					continue;
				}
				MapData[i, j] = (int)float.Parse(word);
				j++;
			}
			j = 0;
			i++;
		}
	}

}

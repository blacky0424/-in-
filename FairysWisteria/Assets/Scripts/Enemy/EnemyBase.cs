//------------------------------------------------------------------
//スクリプト名：EnemyBase.cs
//概要：エネミー共通情報
//作成者:日本電子専門学校＿張越
//作成日:2017/12/13(水)
//------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour 
{
	//----------------------------------------
	//変数宣言
	//----------------------------------------
	[SerializeField]
	private float m_Power;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	/// <summary>
	/// 外部からダメージを取得する
	/// </summary>
	/// <returns>威力</returns>
	public float GetDagame()
	{
		return m_Power;
	}
}

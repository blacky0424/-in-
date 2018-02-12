//------------------------------------------------------------------
//スクリプト名：Wisteria.cs
//概要：藤のオブジェクトを管理するクラス
//		藤のオブジェクト階層
//			親オブジェクト
//				描画用
//				コライダー用（当たり判定）
//作成者:日本電子専門学校＿張越
//作成日:2017/12/11(月)
//変更日:2017/12/12(火)
//変更点:移動処理を無くす
//		 コライダー持つ持たない処理も実装する	
//------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisteria : MonoBehaviour 
{
	//----------------------------------------
	//変数宣言
	//----------------------------------------
	public enum Type							//藤のタイプ種類（仮三タイプ）
	{
		NULL,
		Flower_Medium,							//真ん中の花
		Flower_Down,							//下の花
		Flower_Top,								//上の花
		ivy,									//つた
	}
	[SerializeField]
	private Type m_Type;                        //このゲームオブジェクトのタイプ種類

	[SerializeField]
	private Sprite[] m_sprite;
	private void Start () 
	{
		
	}


	/// <summary>
	/// 毎フレーム更新
	/// </summary>
	private void Update () 
	{

	}

	/// <summary>
	/// 外部からこのゲームオブジェクトのタイプを設定する
	/// </summary>
	/// <param name="n">タイプ種類</param>
	public void SetType(Type ty)
	{
		m_Type = ty;
		switch (m_Type)
		{
			case Type.NULL:
				break;
			case Type.Flower_Top:
				this.transform.GetComponent<SpriteRenderer>().sprite = m_sprite[2];
				break;
			case Type.Flower_Medium:
				this.transform.GetComponent<SpriteRenderer>().sprite = m_sprite[1];
				break;
			case Type.Flower_Down:
				this.transform.GetComponent<SpriteRenderer>().sprite = m_sprite[0];
				break;
			case Type.ivy:
				this.transform.GetComponent<SpriteRenderer>().sprite = m_sprite[3];
				this.transform.GetComponent<BoxCollider2D>().enabled = false;
				break;
			default:
				break;
		}
	}
}

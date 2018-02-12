//------------------------------------------------------------------
//スクリプト名：PlayerIcon.cs
//概要：マルチ画面でプレイヤーのアイコン表示
//作成者:日本電子専門学校＿張越
//作成日:2017/12/14(木)
//------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIcon : MonoBehaviour
{
	//----------------------------------------
	//変数宣言
	//----------------------------------------
	enum MyType
	{
		Player1,
		Player2,
		Player3,
		Player4,
	}

	[SerializeField]
	private bool m_isActive = false;
	[SerializeField]
	private bool m_isMoveOK = true;
	[SerializeField]
	GameObject m_pressB_Button;

	[SerializeField]
	private int frame;

	[SerializeField]
	private bool salvation = true;
	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		// 処理中のフラグをたてとく。
		if (m_isActive && m_isMoveOK)
		{
			iTween.MoveBy(this.gameObject, iTween.Hash("y", -7, "easeType", "easeInOutBack", "loopType", "none", "delay", .1));
			m_isMoveOK = false;
		}
		if (!m_isMoveOK)
		{
			frame++;
		}
		if (frame > 70 && salvation)
		{
			m_pressB_Button.SetActive(true);
			salvation = false;
		}
	}

	public void SetActive()
	{
		m_isActive = true;
	}
}

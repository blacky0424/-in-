//------------------------------------------------------------------
//スクリプト名：BackGroundCloud.cs
//概要：背景の雲をスクロールするスクリプト
//作成者:日本電子専門学校＿張越
//作成日:2017/12/12(火)
//------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundCloud : MonoBehaviour 
{

	//----------------------------------------
	//変数宣言
	//----------------------------------------
	[SerializeField]
	private float ScrollSpeed;

	void Update()
	{
		transform.localPosition += new Vector3(ScrollSpeed * -1.0f, 0.0f, 0.0f);
		if (transform.localPosition.x <= -20.0f * 2.0f)
		{
			transform.localPosition = new Vector3(20.0f, 0.0f, 0.0f);
		}
	}
}

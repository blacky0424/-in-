//------------------------------------------------------------------
//スクリプト名：BackGround.cs
//概要：背景スクロールするクラス
//作成者:日本電子専門学校＿張越
//作成日:2017/12/12(火)
//------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour {

	//***************************************************************************************
	//変数宣言
	//***************************************************************************************
	[SerializeField]
	private Vector2 ScrollRate;        //スクロールをどれだけずらすか
	[SerializeField]
	private float Y_Shift;          //Y座標のズレ
	private Vector2 PrevCameraPos;          //前フレームのカメラ位置
	private Vector2 MoveCameraPos;          //カメラの移動量
	private GameObject CameraObj;           //カメラオブジェクト
	private Vector3 NewPos;                 //移動後の座標

	void Start()
	{
		CameraObj = GameObject.Find("Main Camera");
		PrevCameraPos = CameraObj.transform.position;
		NewPos.z = transform.position.z;
		this.transform.position = new Vector3(this.transform.position.x, CameraObj.transform.position.y + Y_Shift, this.transform.position.z);
	}

	void LateUpdate()
	{
		MoveCameraPos.x = CameraObj.transform.position.x - PrevCameraPos.x;
		MoveCameraPos.y = CameraObj.transform.position.y - PrevCameraPos.y;

		NewPos.x = transform.position.x + MoveCameraPos.x * ScrollRate.x;
		//NewPos.y = CameraObj.transform.position.y + Y_Shift;
		NewPos.y = transform.position.y + MoveCameraPos.y * ScrollRate.y;


		transform.position = NewPos;
		PrevCameraPos = CameraObj.transform.position;
	}
}

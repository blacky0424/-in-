/* スクリプト名：PlayerDeath.cs
 * アニメーション終わったらゲームオブジェクトを削除
 * 作成者：16CU0117_張越
 * 作成日：2017/09/06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAnimeFinish : MonoBehaviour
{
	//----------------------------------------
	//変数宣言
	//----------------------------------------


	/// <summary>
	/// 実行する前の初期化
	/// </summary>
	void Start()
	{
	}

	/// <summary>
	/// 毎フレーム更新
	/// </summary>
	void Update()
	{

	}

	void onDeathAnimeFinish()
	{
		Destroy(this.gameObject);
	}
}

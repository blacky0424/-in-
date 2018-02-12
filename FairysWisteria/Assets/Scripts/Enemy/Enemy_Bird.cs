using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bird : EnemyBase
{
	//座標
	//最小値
	[SerializeField]
	float YMinposition = -2.0f;
	//最大値
	[SerializeField]
	float YMaxposition = 3.40f;

	//スピード-------------------------
	//最小値
	float MinSpeed = 0.01f;
	//最低値
	float MaxSpeed = 0.5f;

	//ランダムの宣言
	int random;




	// Use this for initialization
	void Start()
	{
		//ランダムをかける---------------------------------------
		random = Random.Range(0, 2);

		if (transform.position.x < 0)
		{
			random = 0;
		}

		else
		{
			random = 1;
		}
	}

	// Update is called once per frame
	void Update()
	{


		//鳥の移動------------------------------------
		//右移動
		if (random == 0)
		{
			this.transform.localScale = new Vector3(-1.0f,1.0f,1.0f);
			transform.Translate(Random.Range(MinSpeed, MaxSpeed), 0, 0);
		}
		//左移動
		if (random == 1)
		{
			transform.Translate(Random.Range(-MinSpeed, -MaxSpeed), 0, 0);

		}
	}
}

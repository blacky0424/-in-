using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bee : EnemyBase 
{

	//座標
	//最小値
	float YMinposition = -2.0f;
	//最大値
	float YMaxposition = 2.5f;

	//スピード-------------------------
	//最小値
	float MinSpeed = 0.001f;
	//最低値
	float MaxSpeed = 0.1f;

	//ランダムの宣言
	int random;

	public Vector3 basePos;


	// Use this for initialization
	void Start()
	{

		basePos = transform.position;

		if (transform.position.x < 0)
		{
			random = 0;
		}

		else
		{
			random = 1;
		}
		/*
        //ランダムをかける---------------------------------------
        random = Random.Range(0, 2);

        Debug.Log(random);


        //ポジションセット------------------------
        //右ポジションセット
        if (random == 0)
        {
           basePos = transform.position;
        }
        //左ポジションセット
        if (random == 1)
        {
            basePos = transform.position;
        }
        */
		//basePos = transform.position;
	}



	// Update is called once per frame
	void Update()
	{

		//ゆらゆら動く＋右移動
		if (random == 0)
		{
			this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

			transform.position = new Vector3(transform.position.x,
			Mathf.Sin(Time.frameCount * 2 * Mathf.PI / 60.0f) + basePos.y, transform.position.z);

			transform.Translate(Random.Range(MinSpeed, MaxSpeed), 0, 0);

			//transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);

			if (transform.position.x >= 15)
			{
				Destroy(gameObject);
			}


		}
		//ゆらゆら動く＋左移動
		if (random == 1)
		{
			transform.position = new Vector3(transform.position.x,
			Mathf.Sin(Time.frameCount * 2 * Mathf.PI / 60.0f) + basePos.y, transform.position.z);

			transform.Translate(Random.Range(-MinSpeed, -MaxSpeed), 0, 0);

			if (transform.position.x <= -15)
			{
				Destroy(gameObject);
			}




		}

	}
}

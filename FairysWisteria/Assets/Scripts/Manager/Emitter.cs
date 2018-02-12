//------------------------------------------------------------------
//スクリプト名：Emitter.cs
//概要：エネミーを発射するクラス
//作成者:日本電子専門学校＿張越
//作成日:2017/12/13(水)
//------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
	//----------------------------------------
	//変数宣言
	//----------------------------------------
	[SerializeField]
	private Transform[] m_WhereCreatEnemy;
	[SerializeField]
	private GameObject[] m_Enemy;
	[SerializeField]
	private GameObject[] m_Item;
	[SerializeField]
	private int interval_enemy;
	[SerializeField]
	private int interval_item;
	private int m_frame;
	[SerializeField]
	private LayerMask LayerMaskOfItemCreate;

	[SerializeField]
	private Transform[] m_WhereCreatItem;
	private MapManager m_mapmanager;

	private void Start () 
	{
		m_mapmanager = FindObjectOfType<MapManager>();
	}
	

	private void Update () 
	{
        if (StageScene.gameState == GameState.Game)
        {
            if (m_frame % interval_enemy == 0)
            {
                CreateEnemy();
            }

            if (m_frame % interval_item == 0)
            {
                CreateItem();
            }
            m_frame++;
        }
	}

	private void CreateEnemy()
	{
		int seed = Random.Range(0, 2);
		GameObject obj = null;
		//左か右か
		switch (seed)
		{
			case 0:
				seed = Random.Range(0, 2);
				//蜂かことりか
				switch (seed)
				{
					case 0:
						obj = Instantiate(m_Enemy[0], m_WhereCreatEnemy[0].position, Quaternion.identity);
						break;
					case 1:
						obj = Instantiate(m_Enemy[1], m_WhereCreatEnemy[0].position, Quaternion.identity);
						break;
					default:
						break;
				}
				break;
			case 1:
				seed = Random.Range(0, 2);
				//蜂かことりか
				switch (seed)
				{
					case 0:
						obj = Instantiate(m_Enemy[0], m_WhereCreatEnemy[1].position, Quaternion.identity);
						break;
					case 1:
						obj = Instantiate(m_Enemy[1], m_WhereCreatEnemy[1].position, Quaternion.identity);
						break;
					default:
						break;
				}
				break;
			default:
				break;
		}
	}

	private void CreateItem()
	{

		int seed = Random.Range(0, 3);
		GameObject obj = null;
		int CreateSeed = Random.Range(0, 7);
		
		Vector2 pos = new Vector2(m_WhereCreatItem[CreateSeed].position.x, m_WhereCreatItem[CreateSeed].position.y);

		bool isCreateOK;
			//タイプ
		switch (seed)
		{
			case 0:
				isCreateOK = false;
				Collider2D[] col = Physics2D.OverlapCircleAll(pos, 0.3f, LayerMaskOfItemCreate);
				if (col.Length > 0)
				{
					isCreateOK = true;
				}
				if (!isCreateOK)
				{
					break;
				}
				obj = Instantiate(m_Item[0], pos, Quaternion.identity);
				obj.GetComponent<Item>().SetType(Item.Type.Recovery);
				break;
			case 1:
				isCreateOK = false;
				pos.y -= 6.0f;
				Collider2D[] col2 = Physics2D.OverlapCircleAll(pos, 0.3f, LayerMaskOfItemCreate);
				if (col2.Length > 0)
				{
					isCreateOK = true;
				}
				if (!isCreateOK)
				{
					break;
				}
				obj = Instantiate(m_Item[1], pos, Quaternion.identity);
				obj.GetComponent<Item>().SetType(Item.Type.Invincible);
				break;
			case 2:
				isCreateOK = false;
				pos.y -= 6.0f;
				Collider2D[] col3 = Physics2D.OverlapCircleAll(pos, 0.3f, LayerMaskOfItemCreate);
				if (col3.Length > 0)
				{
					isCreateOK = true;
				}
				if (!isCreateOK)
				{
					break;
				}
				obj = Instantiate(m_Item[2], pos, Quaternion.identity);
				obj.GetComponent<Item>().SetType(Item.Type.Change);
				break;
			default:
				break;
		}

	}
}

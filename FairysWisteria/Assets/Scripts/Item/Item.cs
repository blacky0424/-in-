using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour 
{
	//----------------------------------------
	//変数宣言
	//----------------------------------------
	public enum Type
	{
		Null,
		Recovery,
		Invincible,
		Change,
	}
	const int PlayerLayerNum = 8;

	private Type m_Type = Type.Null;

	[SerializeField]
	private int m_RecoveryNum;


	// Use this for initialization
	private void Start () 
	{
		
	}
	

	private void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == PlayerLayerNum)
		{
			GameObject.Destroy(this.gameObject);
		}
		
	}

	public void SetType(Type tp)
	{
		m_Type = tp;
	}

	public Type GetType()
	{
		return m_Type;
	}

	public int GetRecoveryNum()
	{
		return m_RecoveryNum;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBackGroundColorChange : MonoBehaviour
{
	//----------------------------------------
	//変数宣言
	//----------------------------------------
	private float g_color;                              
	private bool m_ColorFlag;                             

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		ColorChange();
	}

	private void ColorChange()
	{
		this.transform.GetComponent<SpriteRenderer>().color = new Color(1.0f, g_color, 1.0f, 1.0f);

		if (m_ColorFlag)
			g_color -= Time.deltaTime * 0.1f;
		else
			g_color += Time.deltaTime * 0.1f;

		if (g_color < 0.5f)
		{
			g_color = 0.5f;
			m_ColorFlag = false;
		}
		else if (g_color > 1.0f)
		{
			g_color = 1.0f;
			m_ColorFlag = true;
		}
	}
}

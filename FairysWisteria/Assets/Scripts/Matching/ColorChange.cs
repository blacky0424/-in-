using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
	//----------------------------------------
	//変数宣言
	//----------------------------------------
	public static float a_color;
	public static bool m_ColorFlag;

	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		a_ColorChange();
	}

	private void a_ColorChange()
	{
		this.transform.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, a_color);

		if (m_ColorFlag)
			a_color -= Time.deltaTime * 0.1f;
		else
			a_color += Time.deltaTime * 0.1f;

		if (a_color < 0.2f)
		{
			a_color = 0.2f;
			m_ColorFlag = false;
		}
		else if (a_color > 1.0f)
		{
			a_color = 1.0f;
			m_ColorFlag = true;
		}
	}
}

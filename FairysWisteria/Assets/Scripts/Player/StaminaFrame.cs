using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaFrame : MonoBehaviour 
{
	PlayerController.PlayerNum playerType;

	[SerializeField]
	private Sprite[] m_frame;
	Image m_image;
	// Use this for initialization
	void Start () 
	{
		m_image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (true)
		{
			playerType = this.gameObject.transform.parent.transform.parent.GetComponent<PlayerController>().GetPlayerNum();

			switch (playerType)
			{
				case PlayerController.PlayerNum.PlayerOne:
					m_image.sprite = m_frame[0];
					break;
				case PlayerController.PlayerNum.PlayerTwo:
					m_image.sprite = m_frame[1];
					break;
				case PlayerController.PlayerNum.PlayerThree:
					m_image.sprite = m_frame[2];
					break;	
				case PlayerController.PlayerNum.PlayerFour:
					m_image.sprite = m_frame[3];
					break;
				default:
					break;
			}
		}
		
	}
}

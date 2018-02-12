//------------------------------------------------------------------
//スクリプト名：TitleButtonManager.cs
//概要：タイトルボタン管理クラス
//作成者:日本電子専門学校＿張越
//作成日:2017/12/13(水)
//------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class TitleButtonManager : MonoBehaviour 
{
	//----------------------------------------
	//変数宣言
	//----------------------------------------
	public enum ButtonState
	{
		Null,
		GameStart,
		RuleExplanation,
		GameEnd,
	}

	[SerializeField]
	ButtonState m_ButtonState;

	private bool m_isChangeOK = true;

	private Vector2 m_input;
	[SerializeField]
	private GameObject[] m_ButtonObj;
	[SerializeField]
	private float m_scale;
    [SerializeField]
    private float contractLoopTime = 1.0f;

    private GameObject[] howToPlay;


    private float m_sinScale;
    private int ruleCnt;

	void Start () 
	{
        ruleCnt = 0;
        howToPlay = GameObject.FindGameObjectsWithTag("RuleExplanation");
	}


	void Update () 
	{
        m_sinScale = (m_scale - 1) / 2;
		/*
		m_input.x = Input.GetAxis("Horizontal");
		m_input.y = Input.GetAxis("Vertical");
		*/

        if(ruleCnt != 0)
        {
            m_isChangeOK = false;
        }

		m_input = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
		switch (m_ButtonState)
		{
			case ButtonState.Null:
				if (m_isChangeOK)
				{
					if (m_input.y == -1.0f)
					{
						m_ButtonState = ButtonState.GameStart;
						SoundManager.Instance.PlaySE(SoundManager.SElist.SE_CursorChange);
						m_isChangeOK = false;
					}
					else if (m_input.y == 1.0f)
					{
						m_ButtonState = ButtonState.GameStart;
						SoundManager.Instance.PlaySE(SoundManager.SElist.SE_CursorChange);
						m_isChangeOK = false;
					}
				}
				break;
			case ButtonState.GameStart:
				m_ButtonObj[0].transform.localScale = new Vector3(1 + m_sinScale + m_sinScale * Mathf.Sin( Mathf.PI * Time.time/contractLoopTime) , 1 + m_sinScale + m_sinScale * Mathf.Sin(Mathf.PI * Time.time / contractLoopTime), m_scale);
				m_ButtonObj[1].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				m_ButtonObj[2].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); 
				if (m_isChangeOK)
				{
					if (m_input.y  == -1.0f)
					{
						m_ButtonState = ButtonState.RuleExplanation;
						SoundManager.Instance.PlaySE(SoundManager.SElist.SE_CursorChange);
						m_isChangeOK = false;
					}
				}
				if (Input.anyKeyDown)
				{
					GameManager.isGameStart = true;
					SoundManager.Instance.PlaySE(SoundManager.SElist.SE_Choice);
				}
				break;
			case ButtonState.RuleExplanation:
				m_ButtonObj[0].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				m_ButtonObj[1].transform.localScale = new Vector3(1 + m_sinScale + m_sinScale * Mathf.Sin(Mathf.PI * Time.time / contractLoopTime), 1 + m_sinScale + m_sinScale * Mathf.Sin(Mathf.PI * Time.time / contractLoopTime), m_scale);
                m_ButtonObj[2].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); 
				if (m_isChangeOK)
				{
					if (m_input.y == -1.0f)
					{
						m_ButtonState = ButtonState.GameEnd;
						SoundManager.Instance.PlaySE(SoundManager.SElist.SE_CursorChange);
						m_isChangeOK = false;
					}
					else if (m_input.y == 1.0f)
					{
						m_ButtonState = ButtonState.GameStart;
						SoundManager.Instance.PlaySE(SoundManager.SElist.SE_CursorChange);
						m_isChangeOK = false;
					}
                    
				}

                if( GamePad.GetButtonDown(GamePad.Button.A,GamePad.Index.Any) )
                {
                    switch(ruleCnt)
                    {
                        case 0:
                            ruleCnt = howToPlay.Length;
                            foreach(GameObject obj in howToPlay)
                            {
                                obj.GetComponent<HowToPlayMove>().Down();
                            }
                            break;
                        case 1:
                            foreach (GameObject obj in howToPlay)
                            {
                                HowToPlayMove hObj = obj.GetComponent<HowToPlayMove>();
                                if(hObj.GetIndex() == HowToPlayMove.Index.Three)
                                {
                                    hObj.Up();
                                    ruleCnt--;
                                }
                            }
                            break;
                        case 2:
                            foreach (GameObject obj in howToPlay)
                            {
                                HowToPlayMove hObj = obj.GetComponent<HowToPlayMove>();
                                if (hObj.GetIndex() == HowToPlayMove.Index.Two)
                                {
                                    hObj.Up();
                                    ruleCnt--;
                                }
                            }
                            break;
                        case 3:
                            foreach (GameObject obj in howToPlay)
                            {
                                HowToPlayMove hObj = obj.GetComponent<HowToPlayMove>();
                                if (hObj.GetIndex() == HowToPlayMove.Index.One)
                                {
                                    hObj.Up();
                                    ruleCnt--;
                                }
                            }
                            break;
                    }
                }
				break;
			case ButtonState.GameEnd:
				m_ButtonObj[0].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				m_ButtonObj[1].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				m_ButtonObj[2].transform.localScale = new Vector3(1 + m_sinScale + m_sinScale * Mathf.Sin(Mathf.PI * Time.time / contractLoopTime), 1 + m_sinScale + m_sinScale * Mathf.Sin(Mathf.PI * Time.time / contractLoopTime), m_scale);
                if (m_isChangeOK)
				{
					if (m_input.y == 1.0f)
					{
						m_ButtonState = ButtonState.RuleExplanation;
						SoundManager.Instance.PlaySE(SoundManager.SElist.SE_CursorChange);
						m_isChangeOK = false;
					}
				}
				if (Input.anyKeyDown)
				{
					Application.Quit();
				}
				break;
			default:
				break;
		}

		if (m_input.y == 0.0f)
		{
			m_isChangeOK = true;
		}
	}
}

//------------------------------------------------------------------
//スクリプト名：SoundManager.cs
//概要：シーンの切り替えフェイドイン/フェイドアウト用
//作成者:日本電子専門学校＿張越
//作成日:2017/12/12(火)
//------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	//***************************************************************************************
	//変数宣言
	//***************************************************************************************
	public enum SElist
	{
		SE_CursorChange,
		SE_Choice,
		SE_Cansell,
		ButtonConfirmSE,
		Recovery,
		Count_3,
		Count_2,
		Count_1,
		Count_go,
		Enemy_HIT,
		Get_Item,
		Result_SE,
		Enemy_Bird,
		HitGround,
	}
	//SEリスト
	//0:タイトル画面ボタンSE（Start）
	//1:タイトル画面ボタンSE（Rule）
	//2:タイトル画面ボタンSE（End）
	//3:タイトル画面決定音
	//4:回復効果音
	//5:
	//6
	//7
	//8
	//9
	//10
	[SerializeField]
	private AudioClip[] m_SEAudioClips;                           //SE音

	public enum BGMlist
	{
		TitleBGM,
		StageNormalBGM,
		GameClearBGM,
	}
	//BGMリスト
	[SerializeField]
	private AudioClip[] m_BGMAudiClips;                             //BGM音
	[SerializeField]
	public AudioSource m_SE_Player;
	[SerializeField]
	public AudioSource m_BGM_Player;
	private AudioClip m_NextBGM;
	private bool m_ChangeBGM = false;
	private bool m_FadeInFlag = false;
	private bool m_FadeOutFlag = false;
	[SerializeField]
	private float m_ChangeBGM_DelayTime;
	private static bool isCreated = false;                          //生成済みのフラグ

	public static SoundManager Instance;
	/// <summary>
	/// 初期化
	/// </summary>
	void Awake()
	{
		Instance = FindObjectOfType<SoundManager>();

		//シーンの切り替えで削除しないに設定する、すでに存在する場合は削除する
		if (!isCreated)
		{
			DontDestroyOnLoad(this.gameObject);
			isCreated = true;
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// 毎フレーム呼び出す
	/// </summary>
	void Update()
	{
		if (m_ChangeBGM)
		{
			ChangeBackGroundMusicWithFadeInAndFadeOut();
		}
	}

	/// <summary>
	/// SEを再生する
	/// </summary>
	/// <param name="senum">再生するSEの番号</param>
	public void PlaySE(SElist seNum)
	{
		m_SE_Player.PlayOneShot(m_SEAudioClips[(int)seNum], 0.8f);
	}

	/// <summary>
	/// 外部からBGMを変わる用（自動フェードイン/フェードアウト）
	/// </summary>
	/// <param name="bgmNum">再生するBGMの番号</param>
	public void PlayBGM(BGMlist bgmNum)
	{
		m_NextBGM = m_BGMAudiClips[(int)bgmNum];
		m_ChangeBGM = true;
	}

	/// <summary>
	/// BGMフェードイン/フェードアウト処理
	/// 再生している曲ある？　　　Y                                    N
	/// 　　　　　　　　　　　　　↓                                  ↓
	/// 　　　　　　　　　　　フェードアウト　　終わったら→　　フェードイン
	/// </summary>
	private void ChangeBackGroundMusicWithFadeInAndFadeOut()
	{

		if (m_BGM_Player.clip == null)
		{
			m_BGM_Player.clip = m_NextBGM;
			m_BGM_Player.PlayDelayed(0.0f);
			m_BGM_Player.volume = 0.0f;
			m_FadeInFlag = true;
		}
		else
		{
			if (!m_FadeInFlag)
			{
				m_FadeOutFlag = true;
			}
		}

		if (m_FadeInFlag)
		{
			if (m_BGM_Player.volume < 0.4f)
			{
				m_BGM_Player.volume += m_ChangeBGM_DelayTime;
			}
			else
			{
				m_FadeInFlag = false;
				m_ChangeBGM = false;
			}
		}

		if (m_FadeOutFlag)
		{
			if (m_BGM_Player.volume > 0.0f)
			{
				m_BGM_Player.volume -= m_ChangeBGM_DelayTime;
			}
			else
			{
				m_FadeOutFlag = false;
				m_BGM_Player.clip = m_NextBGM;
				m_BGM_Player.PlayDelayed(0.0f);
				m_FadeInFlag = true;
			}
		}
	}
}

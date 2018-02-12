//------------------------------------------------------------------
//スクリプト名：GameManager.cs
//概要：ゲーム進行を管理するスクリプト各シーンにロードする時削除しない
//作成者:日本電子専門学校＿張越
//作成日:2017/12/11(月)
//------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	//----------------------------------------
	//変数宣言
	//----------------------------------------
	private static bool m_isCreated = false;                          //生成済みのフラグ
	public enum GameState                                           //ゲーム進行状態
	{
		Title,
		Matching,
		Stage,
		Result,
	}
	public static GameState GameStatus;                             // 現在のゲーム進行状態
	public static GameManager Gamemanager_instance;                 //実体インスタンス
	private bool m_fadeok = true;                                   //フェイドインフェイドアウト可能
	public FadaScene fade;                                          //フェイドインフェイドアウトクラス
	public static bool isGameClear;                               //ゲームクリアしたかどうか
	public static bool isGameStart;
	public static bool isAdvanceOK;

	public static int PlayerNum;									//プレイヤーの個数
	private MatchingSystem m_MS;

	public static int winnerNum;                                    //勝利したプレイヤーナンバー
	private int WaiteFrame;

	/// <summary>
	/// Startの前で実行する
	/// </summary>
	private void Awake()
	{
		//画面サイズを1280*720、全画面表示しないに設定する
		Screen.SetResolution(1920, 1080, true);
		Application.targetFrameRate = 60;
		UnityEngine.Cursor.visible = false;
		fade = GetComponent<FadaScene>();

		//シーンの切り替えで削除しないに設定する、すでに存在する場合は削除する
		if (!m_isCreated)
		{
			DontDestroyOnLoad(this.gameObject);
			m_isCreated = true;
		}
		else
		{
			Destroy(this.gameObject);
		}
		WaiteFrame = 0;
	}

	/// <summary>
	/// 実行する前の初期化
	/// </summary>
	void Start()
	{
		// 再生中のScene名を取得し、変数に保存する
		switch (SceneManager.GetActiveScene().name)
		{
			case "Title": GameStatus = GameState.Title; break;
			case "Matching": GameStatus = GameState.Matching; break;
			case "Stage": GameStatus = GameState.Stage; break;
			case "Result": GameStatus = GameState.Result; break;
			default: Debug.Log("不正なシーン: " + SceneManager.GetActiveScene().name); break;
		}
	}

	/// <summary>
	/// 毎フレーム更新
	/// </summary>
	void Update()
	{

		switch (GameStatus)
		{
			//タイトル画面
			case GameState.Title:
				//エンターキーが押されたらステージをロードする
				if (isGameStart && WaiteFrame > 180)
				{
					GameStatus = GameState.Matching;
					StartCoroutine(LoadScene(GameState.Matching));
					isGameClear = false;
					WaiteFrame = 0;
				}
				break;
			case GameState.Matching:

				if (SceneManager.GetActiveScene().name == "Matching")
				{
					m_MS = GameObject.Find("MatchingManager").GetComponent<MatchingSystem>();
					PlayerNum = m_MS.GetPlayerNum();
					isAdvanceOK = m_MS.GetGameStart();
				}

				if (isAdvanceOK && WaiteFrame > 180)
				{
					GameStatus = GameState.Stage;
					StartCoroutine(LoadScene(GameState.Stage));
					SoundManager.Instance.PlayBGM(SoundManager.BGMlist.StageNormalBGM);
					WaiteFrame = 0;
				}
                Debug.Log(WaiteFrame);
                break;
			//ステージシーン
			case GameState.Stage:
				if (isGameClear && WaiteFrame > 180)
				{
					GameStatus = GameState.Result;
					StartCoroutine(LoadScene(GameState.Result));
					WaiteFrame = 0;
					SoundManager.Instance.PlayBGM(SoundManager.BGMlist.GameClearBGM);
				}
				break;
			case GameState.Result:
				if (Input.anyKeyDown && WaiteFrame > 180)
				{
					GameStatus = GameState.Title;
					StartCoroutine(LoadScene(GameState.Title));
					SoundManager.Instance.PlayBGM(SoundManager.BGMlist.TitleBGM);
					Reset();
					WaiteFrame = 0;
				}
				break;
			default:
				break;
		}

		WaiteFrame++;
		// 終了処理
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}


	private void Reset()
	{
		PlayerNum = 0;
		isGameClear = false;
		isGameStart = false;
		isAdvanceOK = false;
		BindGamepad.Instance.ClearBind();
		InputManager.Instance.ClearInputManager();

	}

    public void LoadStageScene()
    {
        GameStatus = GameState.Stage;
        StartCoroutine(LoadScene(GameState.Stage));
        SoundManager.Instance.PlayBGM(SoundManager.BGMlist.StageNormalBGM);
        WaiteFrame = 0;
    }

	//シーン読み込み--------------------------------------------------------------------------
	IEnumerator LoadScene(GameState nextstate)
	{
		if (m_fadeok)
		{
			fade.fadeOut();
			m_fadeok = false;
		}

		yield return new WaitForSeconds(1.2f); //一定時間待機
		string s = "NULL";
		switch (nextstate)
		{
			case GameState.Title:
				s = "Title";
				break;
			case GameState.Matching:
				s = "Matching";
			break;
			case GameState.Stage:
				s = "Stage";
				break;
			case GameState.Result:
				s = "Result";
				break;
		}
		GameStatus = nextstate;
		SceneManager.LoadScene(s);
		yield return new WaitForSeconds(0.4f); //一定時間待機
		fade.fadeIn();
		m_fadeok = true;
		if (s == "Result")
		{
			SoundManager.Instance.PlaySE(SoundManager.SElist.Result_SE);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ゲームシーンの状態
/// </summary>
public enum GameState
{
    Cloud,      //ゲーム開始前の雲の演出
    StartCount, //ゲーム開始前のカウント
    Game,       //ゲーム中
    End,        //ゲーム終了
}

/// <summary>
/// ゲームシーン、UI、カメラを管理するクラス
/// 作成者　関口駿
/// 作成日　12/12　
/// </summary>
public class StageScene : MonoBehaviour {
    public static GameState gameState;

    //追従するターゲット
    public static PlayerController m_target;

    PlayerController[] m_playerControllerBox;
    List<RectTransform> m_staminaBox = new List<RectTransform>();
    [HideInInspector]
    public StaminaGage[] m_staminaGageBox;

    //スタートカウント
    [SerializeField]
    Text m_countText;
    int m_count = 3;
    float m_countTimer;
    //ゲームスタート時の座標
    Vector3 m_startPos;
    [SerializeField]
    float m_moveSpeed = 2.0f;

    //目安
    Transform m_criterion;
    float m_criterionPosY;
    //スタミナゲージの高さ
    [SerializeField]
    float m_staminaHeight;
    //カメラの高さ
    [SerializeField]
    float m_cameraHeight;

    //ゴールの高さ
    float m_goalHeight = 1.5f;
    //残ったプレイヤーナンバー
    int winNum = -1;

    void Start () {
        gameState = 0;
        //すべてのプレイヤーを探す
        m_playerControllerBox = GameObject.Find("PlayerBox").GetComponentsInChildren<PlayerController>();
		//m_staminaGageBox = GameObject.Find("StaminaBox").GetComponentsInChildren<StaminaGage>();
		//for (int i = 0;i < m_staminaGageBox.Length;i++){
		//	m_staminaBox.Add(m_staminaGageBox[i].GetComponent<RectTransform>());
		//}

        //テキストの初期化
        m_countText.text = m_count.ToString();
        //目安の取得
        m_criterion = GameObject.Find("Criterion").GetComponent<Transform>();
        //ゴールの高さの取得
        m_goalHeight = GameObject.Find("OverCollider_Top").transform.position.y;
        //プレイヤー１を初期ターゲットに
        m_target = m_playerControllerBox[0];
        m_startPos = new Vector3(transform.position.x, -m_cameraHeight, -10);
        m_criterionPosY = -5.0f;
        m_criterion.position = new Vector2(m_criterion.position.x, m_criterionPosY);
    }
	
	void Update () {
        switch (gameState)
        {
            case GameState.Cloud:
                transform.position = Vector3.Lerp(transform.position, m_startPos, m_moveSpeed * Time.deltaTime);
                if(Vector3.Distance(transform.position, m_startPos) < 0.01f)
                {
                    transform.position = m_startPos;
                    gameState = GameState.StartCount;
                }
                break;
            case GameState.StartCount:
                m_countText.gameObject.SetActive(true);
                m_countTimer += Time.deltaTime;
                if(m_countTimer > 1.0f)
                {
                    m_countTimer = 0;
                    CountDown();
                }
                if (m_count == -1)
                {
                    m_countText.gameObject.SetActive(false);
                    gameState = GameState.Game;
                }
                break;
            case GameState.Game:
                //生存プレイヤーの数
                int playerNum = 0;

                for (int i = 0; i < m_playerControllerBox.Length; i++)
                {
                    //生存しているプレイヤーのみ処理する
                    if (m_playerControllerBox[i].isActiveAndEnabled)
                    {
                        //ターゲットよりy軸が高い位置のプレイヤーを見つける
                        if (m_playerControllerBox[i].transform.position.y > m_target.transform.position.y)
                        {
                            //ターゲットの変更
                            m_target = m_playerControllerBox[i];
                        }
                        playerNum++;
                        winNum = (int)m_playerControllerBox[i].m_playerNum; 
                    }
                    //else
                    //{
                    //    m_staminaGageBox[i].gameObject.SetActive(false);
                    //}
                }

                //プレイヤーが残り1人の時
                if (playerNum <= 1)
                {
					GameManager.winnerNum = winNum;
					GameEnd();
                }else
                {
                    winNum = -1;
                }

                //プレイヤーとゴールの距離の割合
                float distance = m_target.transform.position.y / m_goalHeight;
                //目安の移動
                m_criterionPosY = transform.position.y - 5.0f + distance * 10;

                m_criterion.position = new Vector2(m_criterion.position.x, m_criterionPosY);
                //カメラの座標更新
                transform.position = new Vector3(transform.position.x, m_target.transform.position.y - m_cameraHeight, -10);
                break;
            default:
                break;
        }
    }

	private void LateUpdate()
	{
		//スタミナゲージの移動
		//for (int i = 0; i < m_staminaBox.Count; i++)
		//{
		//	Vector2 staminaPos = new Vector2(m_playerControllerBox[i].transform.position.x, m_playerControllerBox[i].transform.position.y + m_staminaHeight);
		//	m_staminaBox[i].position = RectTransformUtility.WorldToScreenPoint(Camera.main, staminaPos);
		//}
	}

    /// <summary>
    /// カウントダウン
    /// </summary>
    void CountDown()
    {
        m_count--;
        if (m_count <= 0)
        {
            m_countText.text = "START!";
        }
        else
        {
            m_countText.text = m_count.ToString();
        }
		switch (m_count)
		{
			case 3:
				SoundManager.Instance.PlaySE(SoundManager.SElist.Count_3);
				break;
			case 2:
				SoundManager.Instance.PlaySE(SoundManager.SElist.Count_2);
				break;
			case 1:
				SoundManager.Instance.PlaySE(SoundManager.SElist.Count_1);
				break;
			case 0:
				SoundManager.Instance.PlaySE(SoundManager.SElist.Count_go);
				break;
			default:
				break;
		}
	}

    /// <summary>
    /// ゲーム終了
    /// </summary>
    void GameEnd()
    {
		GameManager.isGameClear = true;
    }

}

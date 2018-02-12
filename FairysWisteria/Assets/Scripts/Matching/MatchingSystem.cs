using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


// 参加画面のシステム
// (現時点で)パッドを接続していないと警告もなくゲームに進めないので注意
// editor: Ishibashi Ryuto
// date:    2017/12/13

public class MatchingSystem : MonoBehaviour {
    
    public static Dictionary<GamePad.Index, bool> registFlg = new Dictionary<GamePad.Index, bool>();
    public static Dictionary<PlayerController.PlayerNum, bool> enterFlg = new Dictionary<PlayerController.PlayerNum, bool>();
    public static Dictionary<GamePad.Index, PlayerController.PlayerNum> playerBindPad = new Dictionary<GamePad.Index, PlayerController.PlayerNum>();



    private Dictionary<GamePad.Index,GamepadState> input = new Dictionary<GamePad.Index, GamepadState>();
    private Dictionary<GamePad.Index,GamepadState> oldInput = new Dictionary<GamePad.Index, GamepadState>();

	[SerializeField]
    private bool isStart;
	[SerializeField]
    private int m_playerNum;
    [SerializeField]
    private int MIN_PLAYER_NUM = 2;

	[SerializeField]
	GameObject[] m_playerIcon;
	[SerializeField]
	GameObject m_ReadyToFight;
    
    // Use this for initialization
    void Start () {
        m_playerNum = 0;

        for (GamePad.Index padIndex = GamePad.Index.One; (int)padIndex <= Input.GetJoystickNames().Length; padIndex++)
        {
            if(registFlg.ContainsKey(padIndex) == false)
            {
                registFlg.Add(padIndex, false);
            }
            if(input.ContainsKey(padIndex) == false)
            {
                input.Add(padIndex, GamePad.GetState(padIndex));
            }
            if(oldInput.ContainsKey(padIndex) == false)
            {
                oldInput.Add(padIndex, GamePad.GetState(padIndex));
            }
            registFlg[padIndex] = false;
        }

        playerBindPad.Clear();
        enterFlg.Clear();
    }
	
	// Update is called once per frame
	void Update () {

        //キーボードから強制的にゲーム開始
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.PlayerNum = 3;
            GameManager g = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
            g.LoadStageScene();
        }

        // ゲームを開始するかどうかを判定するフラグ
        isStart = false;

        // ゲームパッドループ
        for (GamePad.Index padIndex = GamePad.Index.One; (int)padIndex <= Input.GetJoystickNames().Length; padIndex++)
        {
            oldInput[padIndex] = input[padIndex];
            input[padIndex] = GamePad.GetState((GamePad.Index)padIndex);

            // 参加処理
            if (input[padIndex].Start == true && oldInput[padIndex].Start == false)
            {
                // 未登録状態ならゲームパッドを現在のプレイヤーにバインドして登録状態に移行
                if (registFlg[padIndex] == false)
                {
                    for (PlayerController.PlayerNum playerNum = 0; (int)playerNum < Input.GetJoystickNames().Length; playerNum++)
                    {
                        if (enterFlg.ContainsKey(playerNum) == false)
                        {
                            BindGamepad.Instance.SetBindPadIndex(playerNum, padIndex);
                            registFlg[padIndex] = true;
                            if (playerBindPad.ContainsKey(padIndex) == false)
                            {
                                playerBindPad.Add(padIndex, playerNum);
                            }
                            else
                            {
                                playerBindPad[padIndex] = playerNum;
                            }
                            enterFlg.Add(playerNum, false);
                            //Debug
                            Debug.Log("Regist:" + (int)playerNum);
							m_playerIcon[(int)playerNum].GetComponent<PlayerIcon>().SetActive();
                            break;
                        }
                    }
                }
            }

            // 準備完了処理
            if(input[padIndex].B == true && oldInput[padIndex].B ==false)
            { 
                // 登録状態か？
                if( registFlg[padIndex] == true )
                {
                    // 準備完了状態でなければ準備完了処理
                    if (enterFlg[playerBindPad[padIndex]] == false)
					{
						enterFlg[playerBindPad[padIndex]] = true;
						m_playerNum++;
						Debug.Log("Enter:" + (int)playerBindPad[padIndex]);
						m_playerIcon[(int)playerBindPad[padIndex]].transform.GetChild(0).gameObject.SetActive(true);
						m_playerIcon[(int)playerBindPad[padIndex]].transform.GetChild(1).gameObject.SetActive(false);
					}
				}
            }

            // 準備完了キャンセル処理
            if(input[padIndex].A == true && oldInput[padIndex].A == false)
            {
                // 準備完了状態か？
                if (playerBindPad.ContainsKey(padIndex) && enterFlg.ContainsKey(playerBindPad[padIndex]))
                {
                    //準備完了状態である
                    if (enterFlg[playerBindPad[padIndex]] == true)
                    {
                        // 準備完了状態から参加状態に戻す
                        enterFlg[playerBindPad[padIndex]] = false;
                        m_playerNum--;
                        Debug.Log("Enter Cancel:" + (int)playerBindPad[padIndex]);
						m_playerIcon[(int)playerBindPad[padIndex]].transform.GetChild(0).gameObject.SetActive(false);
						m_playerIcon[(int)playerBindPad[padIndex]].transform.GetChild(1).gameObject.SetActive(true);
					}
				}
            }
            
        }


        bool tmpFlg = true;

        foreach (var data in enterFlg)
        {
            if (data.Value == false)
            {
                tmpFlg = false;
            }
        }


        // 開始可能な状態かつXボタンが押されたらシーン遷移
        if (tmpFlg == true && m_playerNum >= MIN_PLAYER_NUM)
        {
			m_ReadyToFight.gameObject.SetActive(true);
            // ゲームパッドループ
            for (GamePad.Index padIndex = GamePad.Index.One; (int)padIndex <= Input.GetJoystickNames().Length; padIndex++)
            {
                if( input.ContainsKey(padIndex) && input[padIndex].Start)
                {
                    isStart = true;
					SoundManager.Instance.PlaySE(SoundManager.SElist.SE_Choice);
                }
            }
		}
		else
		{
			m_ReadyToFight.gameObject.SetActive(false);
		}

    }

	/// <summary>
	/// プレイヤー個数を取得する
	/// </summary>
	public int GetPlayerNum()
	{
		return m_playerNum;
	}

	public bool GetGameStart()
	{
		return isStart;
	}
}

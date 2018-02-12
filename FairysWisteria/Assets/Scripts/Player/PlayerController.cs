using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

/// <summary>
/// プレイヤーの挙動を管理するクラス
/// 作成者　関口駿
/// 作成日　12/11
/// </summary>
public class PlayerController : MonoBehaviour
{

    /// <summary>
    /// プレイヤー番号
    /// </summary>
    public enum PlayerNum
    {
        PlayerOne,
        PlayerTwo,
        PlayerThree,
        PlayerFour,
    }
    public PlayerNum m_playerNum;

    //スティック横移動の取得名
    string Horizontal;
    //スティック縦移動の取得名
    string Vertical;
    //ダッシュボタンの取得名
    string DashButton = "DashButton";
    //フジのタグ
    const string Wisteria = "Wisteria";

    //エネミーのレイヤー番号
    const int EnemyNum = 9;
    //ダメージ数値
    float m_damageNum;

    //アイテムのレイヤー番号
    const int ItemNum = 12;

    //ゲームオーバー、クリア判定になるオブジェクト名
    const string OverCollider_Top = "OverCollider_Top";
    const string OverCollider_Down = "OverCollider_Down";
    //エネミーのタグ名
    const string Bee = "Bee";
    const string Bird = "Bird";

    //スタミナゲージ
    StaminaGage m_staminaGage;
    //スタミナ復帰の回数
    [SerializeField]
    int m_reviveCount = 10;
    //現在の復帰ボタンを押した回数
    int m_currentReviveCount;

    //スタミナ復帰時の回復量
    [SerializeField]
    float m_reviveNum = 50.0f;

    //フジがある位置のリスト
    public List<float> m_posXBox = new List<float>();
    //現在の位置番号
    public int m_posNum;
    //ジャンプができるかのスイッチ
    bool m_jumpSwitch;

    //アニメーション
    Animator m_anim;
    const string JumpStateName = "Jump";
    const string SpeedXName = "SpeedY";
    int m_jumpState; //-1:左,0:静止,1:右
	float m_speedState; //静止:0,動:1

    //フジに飛び移る速さ
    [SerializeField]
    float m_jumpSpeed;
    //スタミナ減少値
    [SerializeField]
    float m_jumpDecrease;
    //現在の登る速さ
    float m_climbSpeed = 1;
    //通常の登る速さ
    [SerializeField]
    float m_climbNormalSpeed;
    //通常時回復値
    [SerializeField]
    float m_restIncrease;
    //ダッシュ時の登る速さ
    [SerializeField]
    float m_climbDashSpeed;
    //ダッシュ時スタミナ減少値
    [SerializeField]
    float m_dashDecrease;
    //落ちる速さ
    float m_gravity;

    //フジとフジの距離
    float m_wisteriaDistance;
	//プレイヤーとフジの距離
	[SerializeField]
	float m_distance;
    //フジを調べるときの高さ
    [SerializeField]
    float m_serchHeight;

	// オブジェクトに当たったかどうか
	bool m_isHitBird;
	public bool m_isHitChange;
	public Vector2 changepos;

	//止まる時間
	[SerializeField]
    float m_waitTime;
    float m_damageTimer;

	//移動可能チェックレイヤー
	[SerializeField]
	private LayerMask isMoveOK_Layer;

    //パーティクル
    [SerializeField]
    GameObject m_particlePrefab;

	// ゲームパッドの入力情報
	GamepadState input = new GamepadState();
    GamepadState oldInput = new GamepadState();

	//無敵時間
	private int m_Invincible;
	private bool m_isInvincible;
	[SerializeField]
	private GameObject m_invincibleEffect;

	void Start()
    {
        m_anim = GetComponent<Animator>();
        //スタミナゲージの取得
        m_staminaGage = this.transform.GetChild(2).transform.GetChild(1).GetComponent<StaminaGage>();
        MapManager m = FindObjectOfType<MapManager>();
        //移動するツタの座標を取得
        Transform[] pos = m.GetCreatPos();
        for (int i = 0; i < pos.Length; i++)
        {
            m_posXBox.Add(pos[i].position.x);
        }
        //落下速度の取得
        m_wisteriaDistance = Mathf.Abs(m_posXBox[0] - m_posXBox[1]);

        //ゲームパッドの共通な部分だけで操作する場合の処理
        //プレイヤーの番号に応じて初期化
        switch (m_playerNum)
		{
			case PlayerNum.PlayerOne:
				Vertical = "Vertical";
				Horizontal = "Horizontal";
				m_anim.SetLayerWeight(0, 1.0f);
				m_anim.SetLayerWeight(1, 0.0f);
				m_anim.SetLayerWeight(2, 0.0f);
				m_anim.SetLayerWeight(3, 0.0f);
				break;
			case PlayerNum.PlayerTwo:
				Vertical = "Vertical2";
				Horizontal = "Horizontal2";
				m_anim.SetLayerWeight(0, 0.0f);
				m_anim.SetLayerWeight(1, 1.0f);
				m_anim.SetLayerWeight(2, 0.0f);
				m_anim.SetLayerWeight(3, 0.0f);
				break;
			case PlayerNum.PlayerThree:
				Vertical = "Vertical3";
				Horizontal = "Horizontal3";
				m_anim.SetLayerWeight(0, 0.0f);
				m_anim.SetLayerWeight(1, 0.0f);
				m_anim.SetLayerWeight(2, 1.0f);
				m_anim.SetLayerWeight(3, 0.0f);
				break;
			case PlayerNum.PlayerFour:
				Vertical = "Vertical4";
				Horizontal = "Horizontal4";
				m_anim.SetLayerWeight(0, 0.0f);
				m_anim.SetLayerWeight(1, 0.0f);
				m_anim.SetLayerWeight(2, 0.0f);
				m_anim.SetLayerWeight(3, 1.0f);
				break;
		}

		// 入力の初期化処理
		InputManager.Instance.SetInput(m_playerNum);

		m_isHitChange = false;
		m_currentReviveCount = 0;
        //初期位置へ
        transform.position = new Vector2(m_posXBox[m_posNum], 0);
    }

    void Update()
    {
		if (GameManager.isGameClear == true)
		{
			return;
		}
		if (StageScene.gameState == GameState.Game)
		{

			// バインドされたパッドの入力情報取得
			oldInput = input;
			input = InputManager.Instance.GetInputState(m_playerNum);


			if (input != null)
			{
				//float x = Input.GetAxis(Horizontal);
				float x = input.LeftStickAxis.x;
				float y = 0;

				//ダメージ処理中じゃない
				if (m_damageTimer == 0)
				{
					//ツタとプレイヤーのx座標の距離
					float distance = Mathf.Abs(transform.position.x - m_posXBox[m_posNum]);
					//距離が近ければ
					if (distance < m_distance)
					{
						m_jumpState = 0;
						transform.position = new Vector2(m_posXBox[m_posNum], transform.position.y);
						//y座標の移動ができる 

						//y = Input.GetAxis(Vertical);
						y = input.LeftStickAxis.y;

						//隣のツタに飛び移る
						if (m_jumpSwitch)
						{
							//右に
							if (x > 0.5f)
							{
								HasWisteria(Vector2.right);
								m_jumpSwitch = false;
							}
							//左に
							else if (x < -0.5f)
							{
								HasWisteria(Vector2.left);
								m_jumpSwitch = false;
							}
						}
						//スティックの位置を戻すと移動可能に(長倒しによる連続移動ができないように)
						if (x == 0)
						{
							m_jumpSwitch = true;
						}
					}

					//if (Input.GetButton(DashButton) && y > 0)
					if (input.A)
					{
						if (y > 0)
						{
							//ダッシュ
							m_climbSpeed = m_climbDashSpeed;
							if (!m_isInvincible)
							{
								m_staminaGage.ChangeStaminaNum(m_dashDecrease);
							}
						}
					}
					else
					{
						//初期値に戻す
						m_climbSpeed = m_climbNormalSpeed;
						if (m_staminaGage.staminaNum > 0)
						{
							m_staminaGage.ChangeStaminaNum(m_restIncrease);
						}
					}
				}

				//m_posXBoxの範囲外に出た時に修正
				if (m_posNum < 0)
				{
					m_posNum++;
				}
				else if (m_posNum == m_posXBox.Count)
				{
					m_posNum--;
				}

				//上下制限
				bool isMoveOK = false;
				if (y > 0)
				{
					Collider2D[] col = Physics2D.OverlapPointAll(this.transform.GetChild(0).transform.position, isMoveOK_Layer);
					for (int i = 0; i < col.Length; i++)
					{
						if (col[i] != null)
						{
							isMoveOK = true;
						}
					}
				}
				else
				{
					Collider2D[] col = Physics2D.OverlapPointAll(this.transform.GetChild(1).transform.position, isMoveOK_Layer);
					for (int i = 0; i < col.Length; i++)
					{
						if (col[i] != null)
						{
							isMoveOK = true;
						}
					}
				}

				//ダメージ関連初期化
				if (m_damageTimer > m_waitTime)
				{
					m_isHitBird = false;
					m_damageTimer = 0;
					m_damageNum = 0;
				}

				//バードダメージ処理中
				if (m_isHitBird)
				{
					m_damageTimer += Time.deltaTime;
					//落下
					Vector2 down = new Vector2(m_posXBox[m_posNum], transform.position.y - m_damageNum);
					transform.position = Vector2.Lerp(transform.position, down, m_jumpSpeed * Time.deltaTime);
					m_staminaGage.ChangeStaminaNum(-m_restIncrease);
				}

				//スタミナが0ではない時
				if (m_staminaGage.staminaNum > 0)
				{
					if (y == 0)
					{
						//静止時スタミナ回復
						m_staminaGage.ChangeStaminaNum(m_restIncrease);
						m_speedState = 0;
					}
					else
					{
						m_speedState = 1.0f;
					}
				}
				else
				{
					y = 0;
					m_speedState = 0;
					reviveStamina();
				}

				//移動
				Vector2 pos = new Vector2(m_posXBox[m_posNum], isMoveOK ? transform.position.y + y * m_climbSpeed : this.transform.position.y);
				
				if (m_isHitChange)
				{
					pos = changepos;
					//ツタとプレイヤーのx座標の距離
					float distance = Mathf.Abs(transform.position.x - m_posXBox[m_posNum]);
					//距離が近ければ
					if (distance < m_distance)
					{
						m_isHitChange = false;
					}
				}
				transform.position = Vector2.Lerp(transform.position, pos, m_jumpSpeed * Time.deltaTime);


				if (m_Invincible > 0 && m_isInvincible == true)
				{
					m_invincibleEffect.SetActive(true);
					m_Invincible--;
				}
				else
				{
					m_isInvincible = false;
					m_invincibleEffect.SetActive(false);
				}

				//アニメーション
				m_anim.SetFloat(SpeedXName, m_speedState);
				m_anim.SetInteger(JumpStateName, m_jumpState);
			}
		}
	}

    /// <summary>
    /// スタミナ0からの復帰
    /// </summary>
    void reviveStamina()
    {
        //if (Input.GetButtonDown(ReviveButton))
        if(input.A == true && oldInput.A == false)
		{
            m_currentReviveCount++;
            Debug.Log(m_currentReviveCount);
        }

        if(m_currentReviveCount >= m_reviveCount)
        {
            m_currentReviveCount = 0;
            m_staminaGage.ChangeStaminaNum(m_reviveNum);
        }
    }

    /// <summary>
    /// 移動Particleの生成
    /// </summary>
    void MoveParticleinstantiate()
    {
        //位置修正
        Vector2 pos = new Vector2(transform.position.x - 0.3f, transform.position.y);
        //パーティクル生成
        Instantiate(m_particlePrefab, pos, Quaternion.identity);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 衝突したゲームオブジェクトのタグを調べる
		if (collision.gameObject.name == OverCollider_Top)
		{
            GameManager.winnerNum = (int)m_playerNum;
			GameManager.isGameClear = true;
		}
		//ゲームオーバー処理
		if (collision.gameObject.name == OverCollider_Down)
		{
			SoundManager.Instance.PlaySE(SoundManager.SElist.HitGround);
			this.gameObject.SetActive(false);
		}

		//アイテムとの当たり判定
		if (collision.gameObject.layer == ItemNum)
		{
			switch (collision.gameObject.GetComponent<Item>().GetType())
			{
				case Item.Type.Null:
					break;
				case Item.Type.Recovery:
					m_staminaGage.ChangeStaminaNum(collision.gameObject.GetComponent<Item>().GetRecoveryNum());
					break;
				case Item.Type.Invincible:
					m_isInvincible = true;
					m_Invincible = 180;
					break;
				case Item.Type.Change:
					ChangePos();
					break;
				default:
					break;
			}
			SoundManager.Instance.PlaySE(SoundManager.SElist.Get_Item);
		}

		//エネミーとの当たり判定
		if (collision.gameObject.layer == EnemyNum)
		{
			m_damageNum = collision.gameObject.GetComponent<EnemyBase>().GetDagame();
			switch (collision.gameObject.tag)
			{
				case Bee:
					if (!m_isInvincible)
					{
						m_staminaGage.ChangeStaminaNum(m_damageNum);
					}
					break;
				case Bird:
					m_isHitBird = true;
					break;
				default:
					break;
			}
			SoundManager.Instance.PlaySE(SoundManager.SElist.Enemy_HIT);
		}
	}

	/// <summary>
	/// チェンジアイテム取得時の処理
	/// </summary>
	void ChangePos()
	{
		if (StageScene.m_target != this)
		{
			m_isHitChange = true;
			PlayerController target = StageScene.m_target;
			int pos1 = target.m_posNum;
			int pos2 = m_posNum;
			m_posNum = pos1;
			changepos = new Vector2(m_posXBox[m_posNum], target.transform.position.y);
			target.m_isHitChange = true;
			target.m_posNum = pos2;
			target.changepos = new Vector2(m_posXBox[pos2], transform.position.y);
		}
	}

	/// <summary>
	/// 隣にフジがあるかの確認
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	void HasWisteria(Vector2 direction)
    {
        //放つ位置のの修正
        Vector2 pos = new Vector2(transform.position.x + (m_wisteriaDistance / 2  * direction.x), transform.position.y + m_serchHeight);

        RaycastHit2D hit = Physics2D.Raycast(pos, direction, m_wisteriaDistance);

        if (hit)
        {
            //ヒットしたオブジェクトのタグがWisteriaなら
            if (hit.collider.gameObject.tag == Wisteria)
            {
                if (m_staminaGage.staminaNum > 0)
                {
					if (!m_isInvincible)
					{
						m_staminaGage.ChangeStaminaNum(m_jumpDecrease);

					}
					int intDir = (int)direction.x;
                    m_posNum += intDir;
                    m_jumpState = intDir;
                }
            }
        }
    }

	/// <summary>
	/// プレイヤー番号設定
	/// </summary>
	/// <param name="pn"></param>
	public void SetPlayerNumber(PlayerNum pn)
	{
		//プレイヤー番号 0~3
		m_playerNum = pn;
		MapManager m = FindObjectOfType<MapManager>();
		//プレイヤーの数
		int playerCounr = (int)GameManager.PlayerNum;
		//フジの数
		int wisteriaNum = m.GetCreatPos().Length;
		//配置する番号決定
		int num = (wisteriaNum - 1) / playerCounr * (int)pn;
		if (num >= wisteriaNum)
		{
			num = wisteriaNum - 1;
		}
		m_posNum = num;
	}

	public PlayerNum GetPlayerNum()
	{
		return m_playerNum;
	}
}

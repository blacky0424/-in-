using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


public class BindGamepad : MonoBehaviour
{

    // バインドデータ
    // プレイヤー情報をキーに、ゲームパッドのインデックスを保存
    private Dictionary<PlayerController.PlayerNum, GamePad.Index> data = new Dictionary<PlayerController.PlayerNum, GamePad.Index>();

    // シングルトン設計
    private static BindGamepad m_inst = null;

    private BindGamepad()
    {
    }

    public static BindGamepad Instance
    {
        get
        {
            if (m_inst == null)
            {
                GameObject go = new GameObject("BindGamepad");
                // オブジェクト削除の禁止
                DontDestroyOnLoad(go);
                m_inst = go.AddComponent<BindGamepad>();
            }

            return m_inst;
        }
    }

    // ゲームパッドのバインド情報を初期化する
    public void Init()
    {
        data.Clear();
    }

    // ゲームパッドのインデックスを取得する
    public GamePad.Index GetBindPadIndex(PlayerController.PlayerNum player)
    {
        if( data.ContainsKey(player) )
        {
            return data[player];
        }

        return GamePad.Index.Any;
    }

    // 指定したプレイヤーにパッド情報をバインドする
    public void SetBindPadIndex(PlayerController.PlayerNum player, GamePad.Index padIndex)
    {
        if(!data.ContainsKey(player))
        {
            data.Add(player, padIndex);
        }
        else
        {
            data[player] = padIndex;
        }
    }

    // 指定したプレイヤーのバインド情報を削除する
    public void DeleteBindPadIndex(PlayerController.PlayerNum player)
    {
        data.Remove(player);
    }

	// バインド情報をすべて削除する
	public void ClearBind()
	{
		data.Clear();
	}

    /// <summary>
    /// ゲームパッドがバインドされているプレイヤー数を調べる
    /// </summary>
    public int GetBindingPlayerNum()
    {
        return data.Count;
    }
}
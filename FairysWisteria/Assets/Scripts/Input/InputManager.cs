using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class InputManager : MonoBehaviour
{
    // ゲームパッドのステートをプレイヤーと関連付けて保存するディクショナリ
    private Dictionary<PlayerController.PlayerNum, GamepadState> data = new Dictionary<PlayerController.PlayerNum, GamepadState>();


    // シングルトン設計
    private static InputManager m_inst = null;

    private InputManager()
    {
    }

    public static InputManager Instance
    {
        get
        {
            if (m_inst == null)
            {
                GameObject go = new GameObject("InputManager");
                // オブジェクト削除の禁止
                DontDestroyOnLoad(go);
                m_inst = go.AddComponent<InputManager>();
            }
            return m_inst;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // LateUpdate is called once per frame
    void Update()
    {
        // 入力のアップデート
        List<PlayerController.PlayerNum> keyList = new List<PlayerController.PlayerNum>(data.Keys);
        foreach (var keyData in keyList)
        {
            GamePad.Index index;
            index = BindGamepad.Instance.GetBindPadIndex(keyData);
            data[keyData] = GamePad.GetState(index);
        }
    }

    // 入力のステートを取得する
    public GamepadState GetInputState(PlayerController.PlayerNum player = PlayerController.PlayerNum.PlayerOne)
    {
        if(data.ContainsKey(player))
        {
            return data[player];
        }

        return null;
    }

    public void SetInput(PlayerController.PlayerNum player)
    {
        
        data.Add(player, GamePad.GetState(BindGamepad.Instance.GetBindPadIndex(player)));
    }

	public void ClearInputManager()
	{
		data.Clear();
	}
}
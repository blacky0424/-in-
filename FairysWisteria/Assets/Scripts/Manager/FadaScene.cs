//------------------------------------------------------------------
//スクリプト名：FadaScene.cs
//概要：シーンの切り替えフェイドイン/フェイドアウト用
//作成者:日本電子専門学校＿張越
//作成日:2017/12/12(火)
//------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadaScene : MonoBehaviour 
{
	//----------------------------------------
	//変数宣言
	//----------------------------------------
	public Texture2D fadeOutTexture;					//フェイドインフェイドアウトテクスチャー
    public float fadeSpeed = 0.4f;
    int drawDepth = -1000;
    private float alpha = 1.0f;
    private float fadeDir = -1f;
    Rect rect = new Rect(0, 0, Screen.width, Screen.height);
    Color color;

    void Start()
    {
        alpha = 1;
        color = new Color(1.0f, 1.0f, 1.0f, alpha);
        fadeIn();
    }
    //アルファ値更新
    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        color.a = alpha;
        GUI.color = color;
        GUI.depth = drawDepth;
        GUI.DrawTexture(rect, fadeOutTexture);
    }

    public void fadeIn()
    {
        fadeDir = -1;
    }

    public void fadeOut()
    {
        fadeDir = 1;
    }

  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaGage : MonoBehaviour {

    Image staminaImage;
    //現在のスタミナ
    //
    [HideInInspector]
    public float staminaNum;
    //
    //スタミナマックス値
    const float maxNum = 100.0f;
	void Start () {
        staminaImage = GetComponent<Image>();
        staminaNum = maxNum;
	}
	
	void Update () {

        //上限を超えないように
        if (staminaNum > maxNum)
        {
            staminaNum = maxNum;
        }else if (staminaNum < 0)
        {
            staminaNum = 0;
        }

        //スタミナゲージのfillAmount更新
        staminaImage.fillAmount = staminaNum / maxNum;
    }

    /// <summary>
    /// スタミナの変化
    /// </summary>
    public void ChangeStaminaNum(float num)
    {
        staminaNum += num;
    }
}

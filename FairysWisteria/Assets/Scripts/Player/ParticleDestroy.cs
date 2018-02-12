using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成されたパーティクルをDestroyするクラス
/// 作成者　関口　駿
/// 作成日  12/14
/// </summary>
public class ParticleDestroy : MonoBehaviour {
    //消滅するまでの時間
    [SerializeField]
    float destroyTime;
    float timer;
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer > destroyTime)
        {
            Destroy(gameObject);
        }
	}
}

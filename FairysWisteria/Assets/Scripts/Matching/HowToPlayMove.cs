using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HowToPlayMove : MonoBehaviour {

    public enum Index
    {
        One,
        Two,
        Three
    };

    [SerializeField]
    private float delayTime = 0.0f;
    [SerializeField]
    private float downTime = 1.0f;
    [SerializeField]
    private float upTime = 1.0f;
    [SerializeField]
    Index index = Index.One;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Down()
    {
        
        iTween.MoveTo(this.gameObject, iTween.Hash("y", 0, "time", downTime,"delay",delayTime));
    }

    public void Up()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("y", 12, "time", upTime));
    }

    public Index GetIndex()
    {
        return index;
    }
}

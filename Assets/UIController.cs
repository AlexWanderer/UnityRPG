using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public static UIController instance;

	// Use this for initialization
	void Start () {
        if (!instance) instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

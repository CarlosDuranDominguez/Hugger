using UnityEngine;
using System;

[Serializable]
public class Player : Hugger {

    public float angularVelocity;

	// Use this for initialization
	private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
        //if(Input.GetMouseButton(0))
          //  RotateAppends(Camera.main.ScreenToWorldPoint(Input.mousePosition), angularVelocity);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerAppend : Append {

    private void OnMouseDrag()
    {
        MoveTowards(Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position, ((Player)ParentHugger).angularVelocity);
    }

    private void OnMouseDown()
    {

    }
    private void OnMouseUp()
    {

    }
}

using UnityEngine;
using System;

[Serializable]
public class Hugger : MonoBehaviour {

    public Append[] Appends
    {
        get
        {
            return appends;
        }
        set
        {
            appends = value;
        }
    }

    [HideInInspector]
    [SerializeField]
    private Append[] appends;

    public bool ThisAppendBelongsToIt(Append append)
    {
        int i = 0;
        while (i < appends.Length && appends[i] != append)
            i++;
        return i < appends.Length;
    }

    public void RotateAppend(int index, float angle, float interpolationFactor)
    {
        RotateAppend(appends[index], angle, interpolationFactor);
    }

    public void RotateAppend(Append append, float angle, float interpolationFactor)
    {
        append.LimitedGyre.Current = Mathf.LerpAngle(append.LimitedGyre.Current, angle, interpolationFactor);
    }

    public void RotateAppend(Append append, Vector2 position, float interpolationFactor)
    {
        RotateAppend(append, Vector2.SignedAngle(Vector2.up, position), interpolationFactor);
    }

    public void RotateAppends(float angle, float interpolationFactor)
    {
        for (int i = 0; i < Appends.Length; i++)
        {
            if (appends[i].LimitedGyre.Minimum < angle && appends[i].LimitedGyre.Maximum >= angle)
            {
                appends[i].LimitedGyre.Current = Mathf.LerpAngle(appends[i].LimitedGyre.Current, angle, interpolationFactor);
            }
        }
    }

    public void RotateAppends(Vector2 position, float interpolationFactor)
    {
        RotateAppends(Vector2.SignedAngle(Vector2.up, position), interpolationFactor);
    }

}

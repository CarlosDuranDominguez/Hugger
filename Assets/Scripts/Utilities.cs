using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LimitedAngle
{
    const float MINIMUM = -180f;
    const float MAXIMUM = 180f;

    [SerializeField]
    private float minimum;
    [SerializeField]
    private float maximum;
    [SerializeField]
    private float current;

    virtual public float Minimum
    {
        get
        {
            return minimum;
        }
        set
        {
            while (value > MAXIMUM)
                value -= 360f;
            while (value < MINIMUM)
                value += 360f;
            minimum = value;
        }
    }
    virtual public float Maximum
    {
        get
        {
            return maximum;
        }
        set
        {
            while (value > MAXIMUM)
                value -= 360f;
            while (value < MINIMUM)
                value += 360f;
            maximum = value;
        }
    }
    virtual public float Current
    {
        get
        {
            return current;
        }
        set
        {
            while (value > MAXIMUM)
                value -= 360f;
            while (value < MINIMUM)
                value += 360f;
            if(minimum-maximum<0f)
            {
                if (value < minimum)
                {
                    current = minimum;
                }
                else if (value > maximum)
                {
                    current = maximum;
                }
                else
                    current = value;
            }
            else
            {

                if (value < maximum || value > minimum)
                {
                    current = value;
                }
                else if (2f*current>(minimum+maximum))
                {
                    current = minimum;
                }
                else
                {
                    current = maximum;
                }
                    
            }
        }
    }

    public LimitedAngle(float min, float max, float cur)
    {
        Minimum = min;
        Maximum = max;
        Current = cur;
    }

    #if UNITY_EDITOR

    public Vector2 MinimumVector()
    {
        return new Vector2(Mathf.Sin(Mathf.PI / 180f * minimum), -Mathf.Cos(Mathf.PI / 180f * minimum));
    }
    public Vector2 MaximumVector()
    {
        return new Vector2(Mathf.Sin(Mathf.PI / 180f * maximum), -Mathf.Cos(Mathf.PI / 180f * maximum));
    }
    public Vector2 CurrentVector()
    {
        return new Vector2(Mathf.Sin(Mathf.PI / 180f * current), -Mathf.Cos(Mathf.PI / 180f * current));
    }

#endif
}

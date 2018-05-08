using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Append : MonoBehaviour {

    [Serializable]
    public class LimitedRotation : LimitedAngle
    {
        [HideInInspector]
        [SerializeField]
        Transform transform;

        public override float Current
        {
            get
            {
                return base.Current;
            }

            set
            {
                base.Current = value;
                if(transform!=null)
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f + base.Current);
            }
        }
        public LimitedRotation(float min, float max, float cur, Transform trans) : base(min, max, cur)
        {
            transform = trans;
        }
    }

    public LimitedRotation LimitedGyre
    {
        get
        {
            return limitedGyre;
        }
        set
        {
            limitedGyre = value;
        }
    }

    public Hugger ParentHugger
    {
        get
        {
            return parentHugger;
        }
        set
        {
            parentHugger = value;
        }
    }

    [SerializeField]
    private LimitedRotation limitedGyre;

    [HideInInspector]
    [SerializeField]
    private Hugger parentHugger;

    [HideInInspector]
    [SerializeField]
    private List<Hugger> touchedHugger;

    private void Start()
    {
        
    }

#if UNITY_EDITOR
    public void InitiateList()
    {
        touchedHugger = new List<Hugger>();
    }
#endif

    public bool TouchOtherHugger()
    {
        return touchedHugger.Count != 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Append aux;
        if ((aux = collision.GetComponent<Append>()) != null && aux.parentHugger!=parentHugger && !touchedHugger.Contains(aux.parentHugger))
            touchedHugger.Add(aux.parentHugger);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Append aux;
        if ((aux = collision.GetComponent<Append>()) != null && touchedHugger.Contains(aux.parentHugger))
            touchedHugger.Remove(aux.parentHugger);
    }

    public void MoveTowards(Vector2 position, float interpolationFactor)
    {
        limitedGyre.Current = Mathf.LerpAngle(limitedGyre.Current, Vector2.SignedAngle(Vector2.down,
            position), interpolationFactor);
    }

}

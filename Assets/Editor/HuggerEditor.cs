using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Hugger))]
[CanEditMultipleObjects]
public class HuggerEditor : Editor{

    private void OnEnable()
    {
        Hugger hg = (Hugger)target;
        Append[] apds = hg.GetComponentsInChildren<Append>();
        if(apds.Length==0)
        {
            apds = new Append[1]
            {
                new GameObject("Append0",typeof(Append)).GetComponent<Append>()
            };
            apds[0].transform.parent = hg.transform;
            apds[0].LimitedGyre = new Append.LimitedRotation(0f, 360f, 180f, apds[0].transform);

        }
        else
        {
            /* for(int i = 0; i<apds.Length; i++)
             {
                 apds[i].transform.parent = hg.transform;
                 apds[i].LimitedGyre = new Append.LimitedRotation(Mathf.Round(360f / apds.Length * i), Mathf.Round(360f /apds.Length * (i+1)), Mathf.Round(360f / apds.Length * (2*i + 1)/2f), apds[i].transform);
                 apds[i].transform.rotation = Quaternion.Euler(0f, 0f, 180f + apds[i].LimitedGyre.Current);
             }*/
            for (int i = 0; i < apds.Length; i++)
                apds[i].transform.rotation = Quaternion.Euler(0f, 0f, 180f + apds[i].LimitedGyre.Current);
        }
        hg.Appends = apds;
        for (int i = 0; i < apds.Length; i++)
        {
            hg.Appends[i].ParentHugger = hg;
            hg.Appends[i].InitiateList();
            EditorUtility.SetDirty(hg.Appends[i]);
        }
        EditorUtility.SetDirty(hg);
    }
}

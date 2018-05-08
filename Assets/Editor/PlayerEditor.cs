using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
[CanEditMultipleObjects]
public class PlayerEditor : Editor
{

    private void OnEnable()
    {
        Player hg = (Player)target;
        PlayerAppend[] apds = hg.GetComponentsInChildren<PlayerAppend>();
        if (apds.Length == 0)
        {
            GameObject a = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerAppend"));
            apds = new PlayerAppend[1]
            {
                a.GetComponent<PlayerAppend>()
            };
            apds[0].transform.parent = hg.transform;
            apds[0].LimitedGyre = new Append.LimitedRotation(0f, 360f, 180f, apds[0].transform);
        }
        else
        {
            /*for (int i = 0; i < apds.Length; i++)
            {
                apds[i].transform.parent = hg.transform;
                apds[i].LimitedGyre = new Append.LimitedRotation(Mathf.Round(360f / apds.Length * i), Mathf.Round(360f / apds.Length * (i + 1)), Mathf.Round(360f / apds.Length * (2 * i + 1) / 2f), apds[i].transform);
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

    public override void OnInspectorGUI()
    {
        Player hg = (Player)target;
        GUILayout.Label("Angular Velocity");
        hg.angularVelocity = GUILayout.HorizontalSlider(hg.angularVelocity, 0f, 1f );
        for(int i =0; i<hg.Appends.Length; i++)
        {
            GUILayout.Label(hg.Appends[i].name);
            GUILayout.Label("Minimum");
            hg.Appends[i].LimitedGyre.Minimum = EditorGUILayout.FloatField(hg.Appends[i].LimitedGyre.Minimum, GUILayout.MaxWidth(70f));
            GUILayout.Label("Maximum");
            hg.Appends[i].LimitedGyre.Maximum = EditorGUILayout.FloatField(hg.Appends[i].LimitedGyre.Maximum, GUILayout.MaxWidth(70f));
            GUILayout.Label("Current");
            hg.Appends[i].LimitedGyre.Current = EditorGUILayout.FloatField(hg.Appends[i].LimitedGyre.Current, GUILayout.MaxWidth(70f));
        }
        
    }

    private void OnSceneGUI()
    {
        Player hg = (Player)target;
        PlayerAppend[] apds = hg.GetComponentsInChildren<PlayerAppend>();
        LimitedAngle[] limits = new LimitedAngle[apds.Length];

        for(int i = 0; i<apds.Length; i++)
        {
            limits[i] = new LimitedAngle(apds[i].LimitedGyre.Minimum, apds[i].LimitedGyre.Maximum, apds[i].LimitedGyre.Current);
            float size = HandleUtility.GetHandleSize(apds[i].transform.position);
            Handles.color = Color.red;
            Handles.DrawWireArc(apds[i].transform.position, Vector3.forward, apds[i].LimitedGyre.MinimumVector(), apds[i].LimitedGyre.Maximum - apds[i].LimitedGyre.Minimum>0f? apds[i].LimitedGyre.Maximum - apds[i].LimitedGyre.Minimum: 360f + apds[i].LimitedGyre.Maximum - apds[i].LimitedGyre.Minimum, size/2f);
            Handles.DrawLine(apds[i].transform.position, apds[i].transform.position + (Vector3)apds[i].LimitedGyre.MinimumVector()*size*1.2f);
            Handles.DrawLine(apds[i].transform.position, apds[i].transform.position + (Vector3)apds[i].LimitedGyre.MaximumVector()*size * 1.2f);
            Handles.color = Color.blue;
            Handles.DrawLine(apds[i].transform.position, apds[i].transform.position + (Vector3)apds[i].LimitedGyre.CurrentVector() * size*1.5f);
            EditorGUI.BeginChangeCheck();
            limits[i].Minimum = (Vector2.SignedAngle(Vector2.up,(Vector2) ((Handles.FreeMoveHandle(i * 5, apds[i].transform.position + (Vector3)apds[i].LimitedGyre.MinimumVector() * size * 1.2f, Quaternion.identity, size * 0.1f, Vector3.one*0.1f, DrawCustomizeHandle)) - apds[i].transform.position))+180f);
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(apds[i], "Move Angle");
                EditorUtility.SetDirty(apds[i]);
                apds[i].LimitedGyre.Minimum = limits[i].Minimum;
            }
            EditorGUI.BeginChangeCheck();
            limits[i].Maximum = (Vector2.SignedAngle(Vector2.up, (Vector2)((Handles.FreeMoveHandle(i * 5 + 1, apds[i].transform.position + (Vector3)apds[i].LimitedGyre.MaximumVector() * size * 1.2f, Quaternion.identity, size * 0.1f, Vector3.one * 0.1f, DrawCustomizeHandle)) - apds[i].transform.position)) + 180f);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(apds[i], "Move Angle");
                EditorUtility.SetDirty(apds[i]);
                apds[i].LimitedGyre.Maximum = limits[i].Maximum;
            }
            EditorGUI.BeginChangeCheck();
            limits[i].Current = (Vector2.SignedAngle(Vector2.up, (Vector2)((Handles.FreeMoveHandle(i * 5 + 2, apds[i].transform.position + (Vector3)apds[i].LimitedGyre.CurrentVector() * size*1.5f, Quaternion.identity, size * 0.1f, Vector3.one * 0.1f, DrawCustomizeHandle)) - apds[i].transform.position)) + 180f);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(apds[i], "Move Angle");
                EditorUtility.SetDirty(apds[i]);
                apds[i].LimitedGyre.Current = limits[i].Current;
            }
            Vector3 position = apds[i].transform.position;
            EditorGUI.BeginChangeCheck();
            position = Handles.DoPositionHandle(position, apds[i].transform.rotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(apds[i], "Move Angle");
                EditorUtility.SetDirty(apds[i]);
                apds[i].transform.position = position;
            }
        }
    }

    void DrawCustomizeHandle(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
        Handles.CircleHandleCap(controlID, position, rotation, size, eventType);
    }
}

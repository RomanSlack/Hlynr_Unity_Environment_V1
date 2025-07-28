using UnityEngine;

public class HitAlert : MonoBehaviour
{
    void Awake() => Destroy(gameObject, 1f);
    void OnGUI()
    {
        var style = new GUIStyle(GUI.skin.label){fontSize=26,alignment=TextAnchor.LowerLeft};
        GUI.color = Color.red;
        GUI.Label(new Rect(0,0,Screen.width,Screen.height), "THREAT NEUTRALIZED", style);
    }
}

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EditorGridSnap : MonoBehaviour
{
    private float unitSize = 1f;
    Vector3 lastPos;

    void Update()
    {
        if (EditorApplication.isPlaying) return;

        if (transform.localPosition != lastPos)
        {
            float roundX = Mathf.Round(transform.localPosition.x) / unitSize;
            float roundY = Mathf.Round(transform.localPosition.y) / unitSize;

            transform.localPosition = new Vector3(roundX, roundY, transform.localPosition.z);
            lastPos = transform.localPosition;
        }
    }
}
#endif
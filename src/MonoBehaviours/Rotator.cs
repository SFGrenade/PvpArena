using JetBrains.Annotations;
using UnityEngine;

namespace PvpArena.MonoBehaviours;

[UsedImplicitly]
public class Rotator : MonoBehaviour
{
    public int divider = 4;
    public bool rotating = false;
    public Transform[] transformsToRotate = new Transform[0];

    private void FixedUpdate()
    {
        if (!rotating) return;
        foreach (var t in transformsToRotate)
        {
            t.localEulerAngles = new Vector3(0, 0, (Mathf.Round(t.localEulerAngles.z * (float) divider) / (float) divider) - (1f / (float) divider));
        }
    }
}
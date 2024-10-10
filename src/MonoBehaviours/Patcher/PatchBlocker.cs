using JetBrains.Annotations;
using UnityEngine;

namespace PvpArena.MonoBehaviours.Patcher;

[UsedImplicitly]
class PatchBlocker : MonoBehaviour
{
    public enum Type
    {
        WALL,
        FLOOR
    }

    public Type type;
    public string pdBool;

    public void Start()
    {
    }
}
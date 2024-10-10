using JetBrains.Annotations;
using UnityEngine;

namespace PvpArena.MonoBehaviours.Patcher;

[RequireComponent(typeof(BoxCollider2D))]
[UsedImplicitly]
class PatchInspectRegion : MonoBehaviour
{
    public string GameTextConvo = "";
    public bool HeroAlwaysLeft = false;
    public bool HeroAlwaysRight = false;

    public void Start()
    {
    }
}
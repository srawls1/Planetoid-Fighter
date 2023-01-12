using UnityEngine;

public abstract class ScreenTransitioner : MonoBehaviour
{
    public abstract Coroutine HideScreen();
    public abstract Coroutine RevealScreen();
}

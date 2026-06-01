using System;
using UnityEngine;

public abstract class Usable : MonoBehaviour
{
    public event Action OnUseStartAction;
    public event Action OnUseStopAction;

    public virtual void UseStart()
    {
        OnUseStartAction?.Invoke();
    }

    public virtual void UseStop()
    {
        OnUseStopAction?.Invoke();
    }
}

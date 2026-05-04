using System;
using UnityEngine;

public static class Bus<T> where T : IEvent
{
    public delegate void Event(T @event);

    public static event Event OnEvent;

    public static void Raise(T @event)
    {
        OnEvent?.Invoke(@event);
    }
}

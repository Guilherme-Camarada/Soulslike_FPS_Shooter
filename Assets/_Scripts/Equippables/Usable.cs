using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Usable : MonoBehaviour
{
    public abstract void UseStart();
    public abstract void UseStop();
}
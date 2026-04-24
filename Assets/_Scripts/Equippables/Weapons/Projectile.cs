using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public abstract void Execute(Collision collision);

    private void OnCollisionEnter(Collision collision)
    {
        Execute(collision);
    }
}

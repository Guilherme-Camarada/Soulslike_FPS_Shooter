using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected SoundData _soundData;
    public abstract void Execute(Collision collision);

    private void OnCollisionEnter(Collision collision)
    {
        Execute(collision);
    }
}

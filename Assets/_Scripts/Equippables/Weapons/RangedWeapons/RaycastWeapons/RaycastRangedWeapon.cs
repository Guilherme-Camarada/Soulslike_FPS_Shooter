using System.Collections;
using UnityEngine;

public abstract class RaycastRangedWeapon : RangedWeapon
{
    [SerializeField] protected TrailRenderer _trailRenderer;
    [SerializeField] protected Transform _muzzlePoint;

    [SerializeField] protected float _trailSpeed = 100f;

    protected IEnumerator SpawnTrail(TrailRenderer trail, Vector3 destination, Vector3 hitNormal, bool hitSomething, ParticleSystem particleSystem)
    {
        while (Vector3.Distance(trail.transform.position, destination) > 0.05f)
        {
            trail.transform.position = Vector3.MoveTowards(trail.transform.position, destination, _trailSpeed * Time.deltaTime);

            yield return null;
        }

        trail.transform.position = destination;

        if (hitSomething && particleSystem != null)
        {
            Instantiate(particleSystem, destination, Quaternion.LookRotation(hitNormal));
        }

        Destroy(trail.gameObject, trail.time);
    }
}

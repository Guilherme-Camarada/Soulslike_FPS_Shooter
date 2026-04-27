using System.Collections;
using UnityEngine;

public class RaycastRangedWeapon : RangedWeapon
{
    [SerializeField] protected TrailRenderer _trailRenderer;
    [SerializeField] protected Transform _muzzlePoint;

    protected IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit targetPoint, ParticleSystem particleSystem)
    {
        Vector3 startPosition = trail.transform.position;
        float time = 0f;

        while (time < 1f)
        {
            trail.transform.position = Vector3.Lerp(startPosition, targetPoint.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = targetPoint.point;
        Instantiate(particleSystem, targetPoint.point, Quaternion.LookRotation(targetPoint.normal));

        Destroy(trail.gameObject, trail.time);
    }
}

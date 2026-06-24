using UnityEngine;

public class UILookAtPlayer : MonoBehaviour
{

    [SerializeField] private Transform _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt( _camera );
    }
}

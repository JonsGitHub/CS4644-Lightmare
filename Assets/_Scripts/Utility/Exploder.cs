using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _epicenter;
    [SerializeField] private float _minForce;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _radius;
    [SerializeField] private bool _explodeOnEnable = false;

    private void Awake()
    {
        _container.gameObject.SetActive(false);
    }

    public void Explode()
    {
        _container.gameObject.SetActive(true);

        Rigidbody rigidbody = null;
        foreach (Transform child in _container)
        {
            if (child.TryGetComponent(out rigidbody))
            {
                rigidbody.useGravity = true;
                rigidbody.isKinematic = false;
                rigidbody.AddExplosionForce(Random.Range(_minForce, _maxForce), _epicenter.position, _radius);
            }
        }
    }

    private void OnEnable()
    {
        if (_explodeOnEnable)
            Explode();
    }
}

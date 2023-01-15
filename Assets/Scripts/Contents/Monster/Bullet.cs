using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum HitCount
{
    One,
    Infinity
}

public abstract class Bullet : MonoBehaviour
{
    [Header("Bullet Info")]
    public int _damage;
    public Transform _target;
    public HitCount _hitCount;

    public NavMeshAgent nav;

    protected virtual void Awake()
    {
        _target = GameObject.FindWithTag("Player").transform;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 닿음");
            if (_hitCount == HitCount.One)
            {
                Destroy(gameObject, 1);
            }
            else
            {

            }
        }
    }
}

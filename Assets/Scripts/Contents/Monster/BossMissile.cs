using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet
{
    public ParticleSystem _exp;
    private bool _on = false;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    StartCoroutine(Destroy());
    //}

    public void Start()
    {

        nav = gameObject.GetOrAddComponent<NavMeshAgent>();
        StartCoroutine(Destroy());
    }

    private void Update()
    {
        if(nav != null)
            nav.SetDestination(_target.position);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ÇÃ·¹ÀÌ¾î ´êÀ½");
            if (_hitCount == HitCount.One)
            {
                StopAllCoroutines();
                _exp.Play();
                Destroy(gameObject, 0.2f);
            }
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(4.8f);
        _exp.Play();
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);

        Debug.Log(gameObject.name+"ÆÄ±«");
    }
}

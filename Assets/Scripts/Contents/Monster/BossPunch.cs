using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPunch : MonoBehaviour
{
    BossController _boss;

    private void Start()
    {
        _boss = gameObject.GetComponentInParent<BossController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _boss._inPunchRange = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _boss._inPunchRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _boss._inPunchRange = false;
        }
    }
}

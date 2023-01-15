using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Bullet
{
    Rigidbody _rb;
    float _scaleValue = 0.1f;
    float _speed = 0.5f;
    bool _isShoot;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }

    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(5f);
        _isShoot = true;
    }

    IEnumerator GainPower()
    {
        while(!_isShoot)
        {
            _scaleValue += 0.005f;
            _speed += 0.1f;
            transform.localScale = Vector3.one * _scaleValue;
            _rb.AddForce(transform.right * _speed, ForceMode.Acceleration);
            yield return null;
        }
    }
}

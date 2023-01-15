using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FollowWorld : MonoBehaviour
{
    [SerializeField]
    private Transform _lookAt;
    [SerializeField]
    private Vector3 _offset;


    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        Vector3 _pos = _cam.WorldToScreenPoint(_lookAt.position + _offset);
        if (transform.position != _pos)
            transform.position = _pos;
    }
}

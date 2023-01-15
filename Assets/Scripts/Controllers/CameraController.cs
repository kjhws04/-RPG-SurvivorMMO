using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    Vector3 _delta = new Vector3();

    [SerializeField]
    GameObject _player = null;
    [SerializeField]
    GameObject _shopPos;

    public void SetPlayer(GameObject player) { _player = player; }

    void LateUpdate()
    { 
        if (_mode == Define.CameraMode.QuarterView)
        {

            if(_player.IsValid() == false)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, 1 << (int)Define.Layer.Block))
            {
                float dist = (hit.point - _player.transform.position).magnitude* 0.8f;
                transform.position = _player.transform.position + new Vector3(0,1,0) + _delta.normalized * dist;
            }
            else
            {
				transform.position = _player.transform.position + _delta;
				transform.LookAt(_player.transform);
			}
		}
        if(_mode == Define.CameraMode.FrontView)
        {
            Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("Player"));

            //Vector3 dir = _shopPos.transform.position - transform.position;
            //Quaternion quat = Quaternion.LookRotation(dir);
            //transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
            

            //transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = _shopPos.transform.rotation;
            transform.position = _shopPos.transform.position;
        }
    }
    
    
    


    public void SetQuarterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }

    public void ShopPos(GameObject _pos)
    {
        _shopPos = _pos;
    }
}

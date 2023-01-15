using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcChatBox : MonoBehaviour
{
    public string[] _text; //NPC ´ë»ç ¹è¿­
    public GameObject _ChatPos; //¸»Ç³¼± À§Ä¡
    public GameObject _textBoxPrefab; //¸»Ç³¼± ÇÁ¸®Æé
    public GameObject _textgo;
    public bool _isTalk = false;

    public void TalkNpc()
    {
        _textgo = Instantiate(_textBoxPrefab);
        _textgo.GetComponent<ChatSystem>().Ondialogue(_text, _ChatPos);
    }
    
    public void StopTalkNPC()
    {
        Destroy(_textgo);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 dir = other.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.G) && !_isTalk)
            {
                _isTalk = true;
                TalkNpc();
                GameObject _shopPos = Util.FindChild(gameObject, "FrontPos");

                CameraController _cam = Camera.main.GetComponent<CameraController>();
                _cam.ShopPos(_shopPos);
                _cam._mode = Define.CameraMode.FrontView;

                Managers.UI.CloseAllPopupUI();
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                _isTalk = false;
                StopTalkNPC();
                CameraController _cam = Camera.main.GetComponent<CameraController>();
                _cam._mode = Define.CameraMode.QuarterView;
                Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Player");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType
{
    Equ,
    Amr,
    Sho,
    Pot
}

public class ShopMolang : MonoBehaviour
{
    [SerializeField]
    private ShotType _shopType;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                GameObject _shopPos = Util.FindChild(gameObject, "ShopPos");
                CameraController _cam = Camera.main.GetComponent<CameraController>();
                _cam.ShopPos(_shopPos);
                _cam._mode = Define.CameraMode.FrontView;
                
                Managers.UI.CloseAllPopupUI();

                switch (_shopType)
                {
                    case ShotType.Equ:
                        Managers.UI.ShowPopupUI<UI_Shop>();
                        break;
                    case ShotType.Amr:
                        Managers.UI.ShowPopupUI<UI_Shop_Amr>();
                        break;
                    case ShotType.Sho:
                        Managers.UI.ShowPopupUI<UI_Shop_Sho>();
                        break;
                    case ShotType.Pot:
                        Managers.UI.ShowPopupUI<UI_Shop_Pot>();
                        break;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item _item;

    public void Start()
    {
        Managers.UI.MakeWorldSpaceUI<UI_ItemPickUp>(transform); //�����ۿ� �޾��� �ڵ�
    }
}

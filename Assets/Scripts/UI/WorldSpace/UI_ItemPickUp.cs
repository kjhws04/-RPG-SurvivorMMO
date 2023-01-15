using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ItemPickUp : UI_Base
{
    enum Texts
    {
        ItemName,
    }

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        //아이템의 정보를 받아와야 함

        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y)
                                             - Vector3.back * (parent.GetComponent<Collider>().bounds.size.z)/2; //위치
        GetTextMeshProUGUI((int)Texts.ItemName).text = parent.transform.GetComponent<ItemPickUp>()._item._itemName;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]

public class Item : ScriptableObject //게임오브잭트에 붙일 필요가 없다.
{
    public Define.Item _itemType; //아이템 유형() {무기,방어구,신발,포션,재료}
    public string _itemName; //아이템의 이름
    public Sprite _itemImage; //아이템의 이미지
    public GameObject _itemPrefab; //아이템의 프리펩 (Drop용)
    public GameObject _itemUIPrefab; //아이템의 장착 프리펩

    public Define.WeaponType _itemADAPSPType; //무기일 경우 물리/마법 으로 구별 {AD,AP}
    public Define.HpMpType _potionType; //포션의 회복 타입
    public int _potionCoolTime; //포션의 재사용 대기시간
    public int _itemEffect; //무기일경우 AD, AP 방어구일경우 AD, AP 신발일 경우 Sp
    public float _itemRange; //무기일 경우 사거리

    public string _itemDesc; //설멍
    public int _itemPrice; //판매 아이템 가격

    public Define.WeaponAnim _weaponAnim; //아이템의 공격 에니메이션
    public Define.Hand _itemHand; //아이템의 장착 손
}

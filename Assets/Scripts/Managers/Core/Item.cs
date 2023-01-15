using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]

public class Item : ScriptableObject //���ӿ�����Ʈ�� ���� �ʿ䰡 ����.
{
    public Define.Item _itemType; //������ ����() {����,��,�Ź�,����,���}
    public string _itemName; //�������� �̸�
    public Sprite _itemImage; //�������� �̹���
    public GameObject _itemPrefab; //�������� ������ (Drop��)
    public GameObject _itemUIPrefab; //�������� ���� ������

    public Define.WeaponType _itemADAPSPType; //������ ��� ����/���� ���� ���� {AD,AP}
    public Define.HpMpType _potionType; //������ ȸ�� Ÿ��
    public int _potionCoolTime; //������ ���� ���ð�
    public int _itemEffect; //�����ϰ�� AD, AP ���ϰ�� AD, AP �Ź��� ��� Sp
    public float _itemRange; //������ ��� ��Ÿ�

    public string _itemDesc; //����
    public int _itemPrice; //�Ǹ� ������ ����

    public Define.WeaponAnim _weaponAnim; //�������� ���� ���ϸ��̼�
    public Define.Hand _itemHand; //�������� ���� ��
}

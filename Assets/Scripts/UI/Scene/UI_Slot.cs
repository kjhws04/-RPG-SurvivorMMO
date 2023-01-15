using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Slot : UI_Base, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    public Define.SlotType _slotType = Define.SlotType.Normal;
    [SerializeField]
    public Define.Item _quickSlotType;
    [SerializeField]
    private TextMeshProUGUI _textCount; //slot�� ���� ǥ�� text
    [SerializeField]
    private GameObject _goCountImage; //slot�� ���� ǥ�� UI
    public UI_Inventory _inventory;
    public PlayerStat _stat;

    public Vector3 _orgPos;
    public Item _item; //ȹ���� ������
    public Item _orgItem; //���� ������ ����
    public int _itemCount; //�������� �� ����
    public Image _itemImage; //������ �̹���
    public Image _baseImage; //������ �������� �̹���

    #region NeedStudy
    //private void Start()
    //{

    //    _itemImage = gameObject.transform.Find("ItemImage").GetComponent<Image>();
    //    _itemImage = Util.FindChild<Image>(gameObject, "ItemImage");

    //    ��Ȱ��ȭ�� �Ǿ� �ִ� �ڽ� ������Ʈ�� ĳ���ϴ� ����� ���� ���ΰ� �ʿ���.
    //     �ӽ÷� �巹�� ����� ����ϸ� Slot Prefab�� �������.

    //    _goCountImage = Util.FindChild<GameObject>(_itemImage.gameObject, "ItemCountBase");
    //    _textCount = Util.FindChild<TextMeshProUGUI>(_goCountImage.gameObject, "ItemCountText");
    //}
    #endregion

    public override void Init()
    {
        _inventory = FindObjectOfType<UI_Inventory>();
        _stat = FindObjectOfType<PlayerStat>();
        _orgPos = transform.position;
        _baseImage = gameObject.GetComponent<Image>();
    }

    private void SetColor(float _alp) //�̹����� ������ ���� 1=100%, 0=0% //�ּ�[1]
    {
        Color color = _itemImage.color;
        color.a = _alp;
        _itemImage.color = color;
    }

    public void AddItem(Item _getItem, int _getCount = 1) //�������� ȹ���ϴ� �Լ�
    {
        _item = _getItem;
        _itemCount = _getCount;
        _itemImage.sprite = _item._itemImage; //�����ۿ��� sprite�� �߱� ������ 

        if (_item._itemType != Define.Item.Equipment && //������ Ÿ���� ����, ��, �Ź��� ���
            _item._itemType != Define.Item.Armor &&
            _item._itemType != Define.Item.Shoes)
        {
            _goCountImage.SetActive(true); //�������� ���ڸ� ǥ���ϴ� UI�� Ȱ��ȭ
            _textCount.text = _itemCount.ToString(); 
        }
        else
        {
            _textCount.text = "0"; 
            _goCountImage.SetActive(false); //�������� ���ڸ� ǥ���ϴ� UI�� ��Ȱ��ȭ
        }

        SetColor(1); //�������� ������ ������ 100���� �ø� //�ּ�[1]
    }

    public void SetSlotCount(int _count) //�������� ����ϴ� �Լ�
    {
        _itemCount += _count; 
        _textCount.text = _itemCount.ToString();

        if (_itemCount <= 0) //�������� ������ 0���̸�
            ClearSlot();
    }

    public void ClearSlot() //�������� ���� ������ �ʱ�ȭ�ϴ� �Լ�
    {
        _item = null;
        _itemCount = 0;
        _itemImage.sprite = null;
        SetColor(0);

        _textCount.text = "0";
        _goCountImage.SetActive(false);
        _inventory.RestItemExp();
    }

    #region EventHandler
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) //�κ��丮���� ��Ŭ���� �� ���
        {
            if (_item != null)
            {
                UI_InvRigthClick _rc = Managers.UI.ShowPopupUI<UI_InvRigthClick>(); //��Ŭ�� �˾� ����
                _rc.ItemSave(this);
            }
            else
            {
                _inventory.RestItemExp();
            }
        }

        if (eventData.button == PointerEventData.InputButton.Left) //�κ��丮���� ��Ŭ���� �� ���
        {
            if (_item != null) //�������� ������
            {
                _inventory.ShowItemExp(_item, _orgPos); //������ ���� ����
            }
            else
                _inventory.RestItemExp(); //������ ���� �����
        }
    }

    public void OnBeginDrag(PointerEventData eventData) //Ŭ���� ��
    {
        if (_item != null) //UI_DragSlot�� Base�� �ڽİ�ü�� �ִ� Slot�� �̵��Ҷ�, UI�� ������ �����Ƿ�, �������� DragSlot gameObject�� ������ ������ �����ؼ� �巡�׽� �̹����� ���̵��� ��
        {
            UI_DragSlot._instance._dragSlot = this;
            UI_DragSlot._instance.DragSetImage(_itemImage);
            UI_DragSlot._instance.transform.position = eventData.position;

            Debug.Log($"������ �̸� {_item}");
        }
    }

    public void OnDrag(PointerEventData eventData) //Ŭ�� �� => �巹�� �� ��
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_item != null)
            {
                UI_DragSlot._instance.transform.position = eventData.position;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) //�巹�� �� => ��𿣰� ������ 
    {
        _orgItem = _item;
         UI_DragSlot._instance.SetColor(0);
         UI_DragSlot._instance._dragSlot = null; //_dragSlot�� ���� �ߴ� Slot(this)�� null������ �ٲ�
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (UI_DragSlot._instance._dragSlot != null) //�󽽷� -> �󽽷� �̵��� ���� ���� �ڵ�
            {   
                if (_slotType == Define.SlotType.Normal && UI_DragSlot._instance._dragSlot._slotType == Define.SlotType.Normal) //�Ϲ� ���� => �Ϲ� ����
                    //if(UI_DragSlot._instance._dragSlot._item._itemType == Define.Item.Potion)
                    //{
                    //    if (_item == null)
                    //        ChangeSlot();
                    //    else
                    //    {
                    //        if (UI_DragSlot._instance._dragSlot._item._itemName == _item._itemName)
                    //        {
                    //            _itemCount += UI_DragSlot._instance._dragSlot._itemCount;
                    //            Debug.Log(_itemCount); //4
                    //            Debug.Log(UI_DragSlot._instance._dragSlot._itemCount); //2
                    //        }
                    //        else
                    //        {
                    //            ChangeSlot();
                    //        }
                    //    }
                    //}
                    //else ������ ��ġ�� �ڵ� TODO
                        ChangeSlot(); //���Թٲٱ�
                else if (_slotType == Define.SlotType.Normal && UI_DragSlot._instance._dragSlot._slotType == Define.SlotType.Quick) //�� ���� => �Ϲ� ���� 
                {
                    if (_item == null) //�� => �Ϲ� (������ ����ִ� nullĭ�� ���� ��,)
                    {
                        switch (UI_DragSlot._instance._dragSlot._item._itemType) //������ Ÿ�Կ� ���� ����/��� �߰��� �ٲ���
                        {
                            case Define.Item.Equipment: //�����϶�
                                if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //�����̸�
                                    _stat.Attack -= UI_DragSlot._instance._dragSlot._item._itemEffect; //���� �϶�
                                else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //�����̸�
                                    _stat.MAttack -= UI_DragSlot._instance._dragSlot._item._itemEffect; //���� �϶�
                                _inventory.WeaponAnim("Wait"); //�⺻ �ִϸ��̼� ���
                                _inventory.DestroyInvSocket(); //inv�� �ִ� ����Ƽ¯�� ���Ͽ� �ִ� ���� ����
                                _inventory.DestroyHandSocket(); //game�� �ִ� ����Ƽ¯�� ���Ͽ� �ִ� ���� ����
                                _stat.AttackType = Define.WeaponType.Null;
                                break;
                            case Define.Item.Armor: //���϶�
                                if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //�����̸�
                                    _stat.Defense -= UI_DragSlot._instance._dragSlot._item._itemEffect; //���� �϶�
                                else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //�����̸�
                                    _stat.MDefense -= UI_DragSlot._instance._dragSlot._item._itemEffect; //���� �϶�
                                break;
                            case Define.Item.Shoes: //�Ź��̸�
                                _stat.MoveSpeed -= UI_DragSlot._instance._dragSlot._item._itemEffect; //�̼� �϶�
                                break;
                            case Define.Item.Potion: //�����϶�,
                                _stat.PotionCoolTime = 0;
                                break;
                        }
                    }
                    else //�� => �Ϲ� (������ ĭ�� ���� ��,)
                    {
                        if (UI_DragSlot._instance._dragSlot._item._itemType != _item._itemType)
                            return; //���� ������ Ÿ���� �κ��� Ÿ�԰� �ٸ��� ����
                        else
                        {
                            switch (UI_DragSlot._instance._dragSlot._item._itemType) 
                            {
                                case Define.Item.Equipment: //�����϶�
                                    if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //�� �������� AD��
                                        _stat.Attack -= UI_DragSlot._instance._dragSlot._item._itemEffect; //���� - ������ AD
                                    else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //�� �������� AP��
                                        _stat.MAttack -= UI_DragSlot._instance._dragSlot._item._itemEffect; //���� - ������ AP
                                    if (_item._itemADAPSPType == Define.WeaponType.AD) //���� �������� AD��
                                        _stat.Attack += _item._itemEffect; //���� + ������ AD
                                    else if (_item._itemADAPSPType == Define.WeaponType.AD)//���� �������� AP��
                                        _stat.Attack += _item._itemEffect; //���� + ������ AP
                                    _inventory.WeaponAnim(_item._weaponAnim.ToString()); //������ ������ �ִϸ��̼� ���
                                    _inventory.DestroyInvSocket(); //inv�� �ִ� ����Ƽ¯�� ���Ͽ� �ִ� ���� ����
                                    _inventory.DestroyHandSocket(); //game�� �ִ� ����Ƽ¯�� ���Ͽ� �ִ� ���� ����
                                    if (_item._itemHand == Define.Hand.Right) //���� ������ ������ ���� ���� �������̶��
                                        _inventory.InvWeaponChange(Define.Hand.Right, _item._itemUIPrefab.name); //������ ���
                                    else if (_item._itemHand == Define.Hand.Left) //������ ���� ���� �޼��̶��
                                        _inventory.InvWeaponChange(Define.Hand.Left, _item._itemUIPrefab.name); //�޼� ���
                                        _stat.AttackType = _item._itemADAPSPType; //������ ������ ���� Ÿ�� 
                                    break;
                                case Define.Item.Armor: //���϶�
                                    if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //�����̸�
                                        _stat.Defense -= UI_DragSlot._instance._dragSlot._item._itemEffect; //���� �϶�
                                    else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //�����̸�
                                        _stat.MDefense -= UI_DragSlot._instance._dragSlot._item._itemEffect; //���� �϶�
                                    if (_item._itemADAPSPType == Define.WeaponType.AD) //���� �������� �����̸�
                                        _stat.Defense += _item._itemEffect; //���� ���
                                    else if (_item._itemADAPSPType == Define.WeaponType.AD)//���� �������� �����̸�
                                        _stat.MDefense += _item._itemEffect; //���� ���
                                    break;
                                case Define.Item.Shoes: //�Ź��̸�
                                    _stat.MoveSpeed -= UI_DragSlot._instance._dragSlot._item._itemEffect; //�̼� �϶�
                                    _stat.MoveSpeed += _item._itemEffect;
                                    break;
                                case Define.Item.Potion: //�����϶�, ���� ��ȣ TODO
                                    _stat.PotionCoolTime = 0;
                                    break;
                            }
                        }
                    }
                    ChangeSlot(); //���� �ٲٱ�
                }
                else if (_slotType == Define.SlotType.Quick && UI_DragSlot._instance._dragSlot._slotType == Define.SlotType.Normal)//�Ϲ� ���� => �� ����
                {
                    if (UI_DragSlot._instance._dragSlot._item._itemType == _quickSlotType) //�巹���� ������ ������ �������� ������ ������ ������ Ȯ��
                    {
                        switch (UI_DragSlot._instance._dragSlot._item._itemType) //�巹���� ������ Ÿ����
                        {
                            case Define.Item.Equipment: //������ ��,
                                if(_item == null) //������ �������� ����ִٸ�
                                {
                                    if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //�������� AD��
                                    {
                                        _stat.Attack += UI_DragSlot._instance._dragSlot._item._itemEffect; //AD���ݷ� �߰�
                                        _stat.AttackType = UI_DragSlot._instance._dragSlot._item._itemADAPSPType; //AD ���� ���
                                    }
                                    else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //�������� AP��
                                    {
                                        _stat.MAttack += UI_DragSlot._instance._dragSlot._item._itemEffect; //AP���ݷ� �߰�
                                        _stat.AttackType = UI_DragSlot._instance._dragSlot._item._itemADAPSPType; //AP ���� ���
                                    }
                                }
                                else //������ �����Կ� �̹� ���Ⱑ �ִٸ�
                                {
                                    if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //�������� AD��
                                    {
                                        _stat.Attack -= _item._itemEffect; //���� ���� AD�� ����,
                                        _stat.Attack += UI_DragSlot._instance._dragSlot._item._itemEffect; //AD���ݷ� �߰�
                                        _stat.AttackType = UI_DragSlot._instance._dragSlot._item._itemADAPSPType; //AD ���� ���
                                    }
                                    else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //�������� AP��
                                    {
                                        _stat.Attack -= _item._itemEffect; //���� ���� AP�� ����,
                                        _stat.MAttack += UI_DragSlot._instance._dragSlot._item._itemEffect; //AP���ݷ� �߰�
                                        _stat.AttackType = UI_DragSlot._instance._dragSlot._item._itemADAPSPType; //AP ���ݸ��
                                    }
                                }
                                if (UI_DragSlot._instance._dragSlot._item._itemHand == Define.Hand.Right) //������ ���� ���� �������̶��
                                    _inventory.InvWeaponChange(Define.Hand.Right, UI_DragSlot._instance._dragSlot._item._itemUIPrefab.name); //������ ���
                                else if (UI_DragSlot._instance._dragSlot._item._itemHand == Define.Hand.Left) //������ ���� ���� �޼��̶��
                                    _inventory.InvWeaponChange(Define.Hand.Left, UI_DragSlot._instance._dragSlot._item._itemUIPrefab.name); //�޼� ���
                                _inventory.WeaponAnim(UI_DragSlot._instance._dragSlot._item._weaponAnim.ToString()); //���⿡ �´� ���ϸ��̼� ���
                                break;
                            case Define.Item.Armor: //�����
                                if (_item == null)
                                {
                                    if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //������ ��
                                        _stat.Defense += UI_DragSlot._instance._dragSlot._item._itemEffect; //���� ����
                                    else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //������ ��
                                        _stat.MDefense += UI_DragSlot._instance._dragSlot._item._itemEffect; //���� ����
                                }
                                else
                                {
                                    if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD)
                                    {
                                        _stat.Defense -= _item._itemEffect;
                                        _stat.Defense += UI_DragSlot._instance._dragSlot._item._itemEffect;
                                    }
                                    else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP)
                                    {
                                        _stat.MDefense -= _item._itemEffect;
                                        _stat.MDefense += UI_DragSlot._instance._dragSlot._item._itemEffect;
                                    }
                                }
                                break;
                            case Define.Item.Shoes:
                                if (_item == null)
                                    _stat.MoveSpeed += UI_DragSlot._instance._dragSlot._item._itemEffect;
                                else
                                {
                                    _stat.MoveSpeed -= _item._itemEffect;
                                    _stat.MoveSpeed += UI_DragSlot._instance._dragSlot._item._itemEffect;
                                }
                                break;
                            case Define.Item.Potion:
                                _stat.PotionCoolTime = UI_DragSlot._instance._dragSlot._item._potionCoolTime;
                                break;
                        }
                        ChangeSlot();
                    }
                    return; //�巹���� ������ ������ �������� ������ ������ �ٸ��� ����
                }
            }
        }
    } //��𿣰� ������ => ������ ���� ��

    private void ChangeSlot() //�巹�׸� ������ ������ A, ��� �Ϸ��� ���Կ� �ִ� ������ B
    {
        Item _tempItem = _item; //B���� ����
        int _tempItemCount = _itemCount; //B�� ���� ���� 

        AddItem(UI_DragSlot._instance._dragSlot._item, UI_DragSlot._instance._dragSlot._itemCount); //������ Bĭ�� A�� ����(UI_DragSlot�� A�� ������ ������ ����)

        //A�ڸ��� B�� �־��ִ� �ڵ�
        if(_tempItem != null) //���࿡ �ʱ� B���Կ� �����̶� �־��ٸ�
            UI_DragSlot._instance._dragSlot.AddItem(_tempItem, _tempItemCount); //�����س��� B������ A���Կ� ����
        else
            UI_DragSlot._instance._dragSlot.ClearSlot(); //������ �ʱ�ȭ ��Ŵ
    }
    #endregion
}

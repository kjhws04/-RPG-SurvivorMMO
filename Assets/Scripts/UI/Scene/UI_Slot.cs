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
    private TextMeshProUGUI _textCount; //slot의 개수 표기 text
    [SerializeField]
    private GameObject _goCountImage; //slot의 개수 표기 UI
    public UI_Inventory _inventory;
    public PlayerStat _stat;

    public Vector3 _orgPos;
    public Item _item; //획득한 아이템
    public Item _orgItem; //원래 아이템 저장
    public int _itemCount; //아이템의 총 개수
    public Image _itemImage; //아이템 이미지
    public Image _baseImage; //아이템 보여지는 이미지

    #region NeedStudy
    //private void Start()
    //{

    //    _itemImage = gameObject.transform.Find("ItemImage").GetComponent<Image>();
    //    _itemImage = Util.FindChild<Image>(gameObject, "ItemImage");

    //    비활성화가 되어 있는 자식 오브잭트를 캐싱하는 방법에 대한 공부가 필요함.
    //     임시로 드레그 드랍을 사용하며 Slot Prefab을 만들었음.

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

    private void SetColor(float _alp) //이미지의 투명도를 조절 1=100%, 0=0% //주석[1]
    {
        Color color = _itemImage.color;
        color.a = _alp;
        _itemImage.color = color;
    }

    public void AddItem(Item _getItem, int _getCount = 1) //아이템을 획득하는 함수
    {
        _item = _getItem;
        _itemCount = _getCount;
        _itemImage.sprite = _item._itemImage; //아이템에서 sprite로 했기 때문에 

        if (_item._itemType != Define.Item.Equipment && //아이템 타입이 무기, 방어구, 신발일 경우
            _item._itemType != Define.Item.Armor &&
            _item._itemType != Define.Item.Shoes)
        {
            _goCountImage.SetActive(true); //아이템의 숫자를 표시하는 UI를 활성화
            _textCount.text = _itemCount.ToString(); 
        }
        else
        {
            _textCount.text = "0"; 
            _goCountImage.SetActive(false); //아이템의 숫자를 표시하는 UI를 비활성화
        }

        SetColor(1); //아이템이 들어오면 투명도를 100으로 올림 //주석[1]
    }

    public void SetSlotCount(int _count) //아이템을 사용하는 함수
    {
        _itemCount += _count; 
        _textCount.text = _itemCount.ToString();

        if (_itemCount <= 0) //아이템의 개수가 0개이면
            ClearSlot();
    }

    public void ClearSlot() //아이템이 없는 슬롯을 초기화하는 함수
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
        if (eventData.button == PointerEventData.InputButton.Right) //인벤토리에서 우클릭을 할 경우
        {
            if (_item != null)
            {
                UI_InvRigthClick _rc = Managers.UI.ShowPopupUI<UI_InvRigthClick>(); //우클릭 팝업 생성
                _rc.ItemSave(this);
            }
            else
            {
                _inventory.RestItemExp();
            }
        }

        if (eventData.button == PointerEventData.InputButton.Left) //인벤토리에서 좌클릭을 할 경우
        {
            if (_item != null) //아이템이 있으면
            {
                _inventory.ShowItemExp(_item, _orgPos); //아이템 설명 띄우기
            }
            else
                _inventory.RestItemExp(); //아이템 설명 지우기
        }
    }

    public void OnBeginDrag(PointerEventData eventData) //클릭할 때
    {
        if (_item != null) //UI_DragSlot은 Base에 자식개체로 있는 Slot이 이동할때, UI상에 보이지 않으므로, 상위에다 DragSlot gameObject를 만을어 내용을 복사해서 드래그시 이미지가 보이도록 함
        {
            UI_DragSlot._instance._dragSlot = this;
            UI_DragSlot._instance.DragSetImage(_itemImage);
            UI_DragSlot._instance.transform.position = eventData.position;

            Debug.Log($"아이템 이름 {_item}");
        }
    }

    public void OnDrag(PointerEventData eventData) //클릭 후 => 드레그 할 때
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_item != null)
            {
                UI_DragSlot._instance.transform.position = eventData.position;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) //드레그 후 => 어디엔가 놓기전 
    {
        _orgItem = _item;
         UI_DragSlot._instance.SetColor(0);
         UI_DragSlot._instance._dragSlot = null; //_dragSlot에 복사 했던 Slot(this)를 null값으로 바꿈
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (UI_DragSlot._instance._dragSlot != null) //빈슬롯 -> 빈슬롯 이동을 막기 위한 코드
            {   
                if (_slotType == Define.SlotType.Normal && UI_DragSlot._instance._dragSlot._slotType == Define.SlotType.Normal) //일반 슬롯 => 일반 슬롯
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
                    //else 아이템 합치는 코드 TODO
                        ChangeSlot(); //슬롯바꾸기
                else if (_slotType == Define.SlotType.Normal && UI_DragSlot._instance._dragSlot._slotType == Define.SlotType.Quick) //퀵 슬롯 => 일반 슬롯 
                {
                    if (_item == null) //퀵 => 일반 (퀵에서 비어있는 null칸에 놓을 때,)
                    {
                        switch (UI_DragSlot._instance._dragSlot._item._itemType) //아이템 타입에 따라 공격/방어 추가를 바꿔줌
                        {
                            case Define.Item.Equipment: //무기일때
                                if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //물공이면
                                    _stat.Attack -= UI_DragSlot._instance._dragSlot._item._itemEffect; //물공 하락
                                else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //마공이면
                                    _stat.MAttack -= UI_DragSlot._instance._dragSlot._item._itemEffect; //마공 하락
                                _inventory.WeaponAnim("Wait"); //기본 애니매이션 재생
                                _inventory.DestroyInvSocket(); //inv에 있는 유니티짱의 소켓에 있는 무기 삭제
                                _inventory.DestroyHandSocket(); //game에 있는 유니티짱의 소켓에 있는 무기 삭제
                                _stat.AttackType = Define.WeaponType.Null;
                                break;
                            case Define.Item.Armor: //방어구일때
                                if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //물방이면
                                    _stat.Defense -= UI_DragSlot._instance._dragSlot._item._itemEffect; //물방 하락
                                else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //마방이면
                                    _stat.MDefense -= UI_DragSlot._instance._dragSlot._item._itemEffect; //마방 하락
                                break;
                            case Define.Item.Shoes: //신발이면
                                _stat.MoveSpeed -= UI_DragSlot._instance._dragSlot._item._itemEffect; //이속 하락
                                break;
                            case Define.Item.Potion: //포션일때,
                                _stat.PotionCoolTime = 0;
                                break;
                        }
                    }
                    else //퀵 => 일반 (퀵에서 칸에 놓을 때,)
                    {
                        if (UI_DragSlot._instance._dragSlot._item._itemType != _item._itemType)
                            return; //퀵의 아이템 타입이 인벤의 타입과 다르면 리턴
                        else
                        {
                            switch (UI_DragSlot._instance._dragSlot._item._itemType) 
                            {
                                case Define.Item.Equipment: //무기일때
                                    if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //뺄 아이템이 AD면
                                        _stat.Attack -= UI_DragSlot._instance._dragSlot._item._itemEffect; //물공 - 퀵슬롯 AD
                                    else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //뺄 아이템이 AP면
                                        _stat.MAttack -= UI_DragSlot._instance._dragSlot._item._itemEffect; //물공 - 퀵슬롯 AP
                                    if (_item._itemADAPSPType == Define.WeaponType.AD) //넣을 아이템이 AD면
                                        _stat.Attack += _item._itemEffect; //물공 + 퀵슬롯 AD
                                    else if (_item._itemADAPSPType == Define.WeaponType.AD)//넣을 아이템이 AP면
                                        _stat.Attack += _item._itemEffect; //물공 + 퀵슬롯 AP
                                    _inventory.WeaponAnim(_item._weaponAnim.ToString()); //장착할 아이템 애니메이션 재생
                                    _inventory.DestroyInvSocket(); //inv에 있는 유니티짱의 소켓에 있는 무기 삭제
                                    _inventory.DestroyHandSocket(); //game에 있는 유니티짱의 소켓에 있는 무기 삭제
                                    if (_item._itemHand == Define.Hand.Right) //새로 장착할 무기의 장착 손이 오른손이라면
                                        _inventory.InvWeaponChange(Define.Hand.Right, _item._itemUIPrefab.name); //오른손 장비
                                    else if (_item._itemHand == Define.Hand.Left) //무기의 장착 손이 왼손이라면
                                        _inventory.InvWeaponChange(Define.Hand.Left, _item._itemUIPrefab.name); //왼손 장비
                                        _stat.AttackType = _item._itemADAPSPType; //장착할 아이템 공격 타입 
                                    break;
                                case Define.Item.Armor: //방어구일때
                                    if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //물방이면
                                        _stat.Defense -= UI_DragSlot._instance._dragSlot._item._itemEffect; //물방 하락
                                    else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //마방이면
                                        _stat.MDefense -= UI_DragSlot._instance._dragSlot._item._itemEffect; //마방 하락
                                    if (_item._itemADAPSPType == Define.WeaponType.AD) //넣을 아이템이 물방이면
                                        _stat.Defense += _item._itemEffect; //물방 상승
                                    else if (_item._itemADAPSPType == Define.WeaponType.AD)//넣을 아이템이 마방이면
                                        _stat.MDefense += _item._itemEffect; //마방 상승
                                    break;
                                case Define.Item.Shoes: //신발이면
                                    _stat.MoveSpeed -= UI_DragSlot._instance._dragSlot._item._itemEffect; //이속 하락
                                    _stat.MoveSpeed += _item._itemEffect;
                                    break;
                                case Define.Item.Potion: //포션일때, 지정 번호 TODO
                                    _stat.PotionCoolTime = 0;
                                    break;
                            }
                        }
                    }
                    ChangeSlot(); //슬롯 바꾸기
                }
                else if (_slotType == Define.SlotType.Quick && UI_DragSlot._instance._dragSlot._slotType == Define.SlotType.Normal)//일반 슬롯 => 퀵 슬롯
                {
                    if (UI_DragSlot._instance._dragSlot._item._itemType == _quickSlotType) //드레그한 아이템 종류가 퀵슬롯의 아이템 종류와 같은지 확인
                    {
                        switch (UI_DragSlot._instance._dragSlot._item._itemType) //드레그한 아이템 타입이
                        {
                            case Define.Item.Equipment: //무기일 때,
                                if(_item == null) //기존의 퀵슬롯이 비어있다면
                                {
                                    if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //아이템이 AD면
                                    {
                                        _stat.Attack += UI_DragSlot._instance._dragSlot._item._itemEffect; //AD공격력 추가
                                        _stat.AttackType = UI_DragSlot._instance._dragSlot._item._itemADAPSPType; //AD 공격 모드
                                    }
                                    else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //아이템이 AP면
                                    {
                                        _stat.MAttack += UI_DragSlot._instance._dragSlot._item._itemEffect; //AP공격력 추가
                                        _stat.AttackType = UI_DragSlot._instance._dragSlot._item._itemADAPSPType; //AP 공격 모드
                                    }
                                }
                                else //기존의 퀵슬롯에 이미 무기가 있다면
                                {
                                    if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //아이템이 AD면
                                    {
                                        _stat.Attack -= _item._itemEffect; //기존 무기 AD를 빼고,
                                        _stat.Attack += UI_DragSlot._instance._dragSlot._item._itemEffect; //AD공격력 추가
                                        _stat.AttackType = UI_DragSlot._instance._dragSlot._item._itemADAPSPType; //AD 공격 모드
                                    }
                                    else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //아이템이 AP면
                                    {
                                        _stat.Attack -= _item._itemEffect; //기존 무기 AP를 빼고,
                                        _stat.MAttack += UI_DragSlot._instance._dragSlot._item._itemEffect; //AP공격력 추가
                                        _stat.AttackType = UI_DragSlot._instance._dragSlot._item._itemADAPSPType; //AP 공격모드
                                    }
                                }
                                if (UI_DragSlot._instance._dragSlot._item._itemHand == Define.Hand.Right) //무기의 장착 손이 오른손이라면
                                    _inventory.InvWeaponChange(Define.Hand.Right, UI_DragSlot._instance._dragSlot._item._itemUIPrefab.name); //오른손 장비
                                else if (UI_DragSlot._instance._dragSlot._item._itemHand == Define.Hand.Left) //무기의 장착 손이 왼손이라면
                                    _inventory.InvWeaponChange(Define.Hand.Left, UI_DragSlot._instance._dragSlot._item._itemUIPrefab.name); //왼손 장비
                                _inventory.WeaponAnim(UI_DragSlot._instance._dragSlot._item._weaponAnim.ToString()); //무기에 맞는 에니메이션 재생
                                break;
                            case Define.Item.Armor: //방어구라면
                                if (_item == null)
                                {
                                    if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AD) //물방일 때
                                        _stat.Defense += UI_DragSlot._instance._dragSlot._item._itemEffect; //물방 증가
                                    else if (UI_DragSlot._instance._dragSlot._item._itemADAPSPType == Define.WeaponType.AP) //마방일 때
                                        _stat.MDefense += UI_DragSlot._instance._dragSlot._item._itemEffect; //마방 증가
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
                    return; //드레그한 아이템 종류가 퀵슬롯의 아이템 종류와 다르면 리턴
                }
            }
        }
    } //어디엔가 놓기전 => 완전히 놓을 때

    private void ChangeSlot() //드레그를 시작한 아이템 A, 드랍 하려는 슬롯에 있는 아이템 B
    {
        Item _tempItem = _item; //B정보 저장
        int _tempItemCount = _itemCount; //B의 개수 저장 

        AddItem(UI_DragSlot._instance._dragSlot._item, UI_DragSlot._instance._dragSlot._itemCount); //원래의 B칸에 A를 넣음(UI_DragSlot은 A의 정보를 가지고 있음)

        //A자리에 B를 넣어주는 코드
        if(_tempItem != null) //만약에 초기 B슬롯에 무엇이라도 있었다면
            UI_DragSlot._instance._dragSlot.AddItem(_tempItem, _tempItemCount); //저장해놓은 B정보를 A슬롯에 넣음
        else
            UI_DragSlot._instance._dragSlot.ClearSlot(); //슬롯을 초기화 시킴
    }
    #endregion
}

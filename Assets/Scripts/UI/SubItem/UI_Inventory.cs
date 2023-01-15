using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_Popup
{
    UI_Profile _UIProfile;

    [SerializeField]
    private GameObject _inventoryBase;
    [SerializeField]
    private GameObject _slotsParent;
    [SerializeField]
    private PlayerStat _stat;

    public GameObject LefthandSocket;
    public GameObject RighthandSocket;
    public GameObject InvLefthandSocket;
    public GameObject InvRighthandSocket;

    private UI_Slot[] _slots;

    [SerializeField]
    private Animator _anim;

    [SerializeField]
    private bool _rHandHave = false; //인벤토리 캐릭터 손
    [SerializeField]
    private bool _lHandHave = false;
    [SerializeField]
    private bool _rGHandHave = false; //게임 캐릭터 손
    [SerializeField]
    private bool _lGHandHave = false;

    #region Enum
    enum Buttons
    {
        CloseInventory
    }

    enum Texts
    {
        ItemName,
        ItemWeaponType,
        ItemExpanation,
        ItemDamage,
        ItemType,
        GoldText
    }

    enum GameObjects
    {
        PlayerAnim,
        InventoryBase,
        GridSetting,
        ExpLine,
        SelectSlot
    }

    enum Images
    {
        BlockBase,
        ExplanationBase,
    }
    #endregion

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetObject((int)GameObjects.ExpLine).SetActive(false);
        GetObject((int)GameObjects.SelectSlot).SetActive(false);
    }

    private void LateUpdate()
    {
        GetTextMeshProUGUI((int)Texts.GoldText).text = $"Gold : {_stat.Gold}";
    }

    public override void Init()
    {
        #region Bind
        _stat = gameObject.transform.parent.GetComponent<PlayerStat>();
        _inventoryBase = Util.FindChild(gameObject, "InventoryBase"); //인벤토리 베이스, 그리드 세팅, 슬롯들 매핑
        _slotsParent = Util.FindChild(_inventoryBase, "GridSetting");
        _anim = Util.FindChild(gameObject, "PlayerAnim").GetComponent<Animator>();
        _slots = _slotsParent.GetComponentsInChildren<UI_Slot>();
        #endregion
        CameraSetting();
    }

    public override void ClosePopupUI() //버튼을 이용해 인벤토리를 닫는 함수
    {
        Transform _profile = GameObject.FindWithTag("Player").transform.Find("UI_Profile");
        _UIProfile = _profile.GetComponent<UI_Profile>();
        _UIProfile.OffInventory();

        base.ClosePopupUI();

        //유니티짱의 프로필 on
    }

    public void CameraSetting() //유니티짱의 3D 모델을 볼 수 있게 하는 카메라 세팅
    {
        GameObject ExCam = GameObject.Find("UI_Camera");

        if (ExCam == null) //UI_Camera 존재 여부 확인
        {
            GameObject UI_Camera = Managers.Resource.Instantiate("Camera/UI_Camera"); //캐릭터의 3d표시를 위해 UI용 카메라를 받아옴
            Camera _uiCamera = UI_Camera.GetComponent<Camera>(); //UI_Camera의 <Camera> 컴포넌트를 받아옴
            Canvas inv = GetComponent<Canvas>(); //인벤토리의 <Canvas> 컴포넌트를 받아옴
            inv.worldCamera = _uiCamera; //Canvas.RenderCamera에 UI_Camera<Camera>를 넣어줌
            DontDestroyOnLoad(UI_Camera);
        }
        else //있으면
        {
            Camera _uiCamera = ExCam.GetComponent<Camera>(); //UI_Camera의 <Camera> 컴포넌트를 받아옴
            Canvas inv = GetComponent<Canvas>(); //인벤토리의 <Canvas> 컴포넌트를 받아옴
            inv.worldCamera = _uiCamera; //Canvas.RenderCamera에 UI_Camera<Camera>를 넣어줌
        }
    }

    public void AcquireItem(Item _item, int _count = 1) //아이템을 slot에 받아오는 함수
    {
        if(Define.Item.Equipment != _item._itemType &&
           Define.Item.Armor != _item._itemType &&
           Define.Item.Shoes != _item._itemType) //장비는 1칸을 차지하기 때문에 
        {

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i]._item != null)
                {
                    if (_slots[i]._item._itemName == _item._itemName) //슬롯 안에 아이템이 이미 있는 경우
                    {
                        _slots[i].SetSlotCount(_count); //i번째의 슬롯을 찾아서 _count 만큼 개수를 증가
                        return;
                    }
                }
            } //만약에 여기까지 for문을 돌렸음에도 중복되는 아이템이 없다면,
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i]._item == null) //빈칸인 i번째의 슬롯을 찾아서
            {
                _slots[i].AddItem(_item, _count); //_item과 _count만큼 개수를 증가
                return;
            }
        }
    }

    public void ShowItemExp(Item _item, Vector3 _slotPos)
    {
        GetTextMeshProUGUI((int)Texts.ItemName).text = $"{_item._itemName}"; //아이템 이름
        GetTextMeshProUGUI((int)Texts.ItemType).text = $"{_item._itemType}"; //아이템 종류
        GetTextMeshProUGUI((int)Texts.ItemExpanation).text = $"{_item._itemDesc}"; //아이템 설명

        GetObject((int)GameObjects.ExpLine).SetActive(true);
        GetObject((int)GameObjects.SelectSlot).SetActive(true);
        GetObject((int)GameObjects.SelectSlot).transform.position = _slotPos;

        switch (_item._itemType)
        {
            case Define.Item.Equipment:
                GetTextMeshProUGUI((int)Texts.ItemWeaponType).text = $"{_item._itemADAPSPType} :";
                GetTextMeshProUGUI((int)Texts.ItemDamage).text = $"{_item._itemEffect}";
                break;
            case Define.Item.Armor:
                GetTextMeshProUGUI((int)Texts.ItemWeaponType).text = $"{_item._itemADAPSPType} :";
                GetTextMeshProUGUI((int)Texts.ItemDamage).text = $"{_item._itemEffect}";
                break;
            case Define.Item.Shoes:
                GetTextMeshProUGUI((int)Texts.ItemWeaponType).text = $"{_item._itemADAPSPType} :";
                GetTextMeshProUGUI((int)Texts.ItemDamage).text = $"{_item._itemEffect}";
                break;
            case Define.Item.Potion:
                GetTextMeshProUGUI((int)Texts.ItemWeaponType).text = $"{_item._potionType}";
                GetTextMeshProUGUI((int)Texts.ItemDamage).text = $"{_item._itemEffect}";
                break;
            case Define.Item.Ingredient:
                GetTextMeshProUGUI((int)Texts.ItemWeaponType).text = $"";
                GetTextMeshProUGUI((int)Texts.ItemDamage).text = $"";
                break;
        }
    }

    public void RestItemExp()
    {
        GetTextMeshProUGUI((int)Texts.ItemName).text = $"";
        GetTextMeshProUGUI((int)Texts.ItemType).text = $"";
        GetTextMeshProUGUI((int)Texts.ItemWeaponType).text = $"";
        GetTextMeshProUGUI((int)Texts.ItemExpanation).text = $"";
        GetTextMeshProUGUI((int)Texts.ItemDamage).text = $"";

        GetObject((int)GameObjects.ExpLine).SetActive(false);
        GetObject((int)GameObjects.SelectSlot).SetActive(false);
    }

    public void WeaponAnim(string animName)
    {
        _anim.Play(animName);
    } //무기별 애니메이션 재생 (slot)

    public void InvWeaponChange(Define.Hand _hand ,string _weaponName)
    {
        GameObject goIvn = Managers.Resource.Instantiate($"Item/Equipment/UI_Inventory/{_weaponName}");
        GameObject goGame = Managers.Resource.Instantiate($"Item/Equipment/UI_Inventory/{_weaponName}");
        DestroyInvSocket();
        DestroyHandSocket();

        if (_hand == Define.Hand.Right)
        {
            goIvn.transform.SetParent(InvRighthandSocket.transform, false);
            _rHandHave = true;
        }
        else if (_hand == Define.Hand.Left)
        {
            goIvn.transform.SetParent(InvLefthandSocket.transform, false);
            _lHandHave = true;
        }

        if (_hand == Define.Hand.Right)
        {
            goGame.transform.SetParent(RighthandSocket.transform, false);
            _rGHandHave = true;
        }
        else if (_hand == Define.Hand.Left)
        {
            goGame.transform.SetParent(LefthandSocket.transform, false);
            _lGHandHave = true;
        }


    } //무기모양 변경

    public void DestroyInvSocket()
    {
        if (!_rHandHave && !_lHandHave)
            return;
        if (_rHandHave && !_lHandHave)
        {
            Destroy(InvRighthandSocket.transform.GetChild(0).gameObject);
            _rHandHave = false;
            return;
        }
        if (!_rHandHave && _lHandHave)
        {
            Destroy(InvLefthandSocket.transform.GetChild(0).gameObject);
            _lHandHave = false;
            return;
        }
    } //인벤토리 플레이어의 착용 무기 파괴 함수

    public void DestroyHandSocket()
    {
        if (!_rGHandHave && !_lGHandHave)
            return;
        if (_rGHandHave && !_lGHandHave)
        {
            Destroy(RighthandSocket.transform.GetChild(0).gameObject);
            _rGHandHave = false;
            return;
        }
        if (!_rGHandHave && _lGHandHave)
        {
            Destroy(LefthandSocket.transform.GetChild(0).gameObject);
            _lGHandHave = false;
            return;
        }
    } //필드 플레이어의 착용 무기 파괴 함수
}

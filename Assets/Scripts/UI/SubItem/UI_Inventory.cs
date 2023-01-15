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
    private bool _rHandHave = false; //�κ��丮 ĳ���� ��
    [SerializeField]
    private bool _lHandHave = false;
    [SerializeField]
    private bool _rGHandHave = false; //���� ĳ���� ��
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
        _inventoryBase = Util.FindChild(gameObject, "InventoryBase"); //�κ��丮 ���̽�, �׸��� ����, ���Ե� ����
        _slotsParent = Util.FindChild(_inventoryBase, "GridSetting");
        _anim = Util.FindChild(gameObject, "PlayerAnim").GetComponent<Animator>();
        _slots = _slotsParent.GetComponentsInChildren<UI_Slot>();
        #endregion
        CameraSetting();
    }

    public override void ClosePopupUI() //��ư�� �̿��� �κ��丮�� �ݴ� �Լ�
    {
        Transform _profile = GameObject.FindWithTag("Player").transform.Find("UI_Profile");
        _UIProfile = _profile.GetComponent<UI_Profile>();
        _UIProfile.OffInventory();

        base.ClosePopupUI();

        //����Ƽ¯�� ������ on
    }

    public void CameraSetting() //����Ƽ¯�� 3D ���� �� �� �ְ� �ϴ� ī�޶� ����
    {
        GameObject ExCam = GameObject.Find("UI_Camera");

        if (ExCam == null) //UI_Camera ���� ���� Ȯ��
        {
            GameObject UI_Camera = Managers.Resource.Instantiate("Camera/UI_Camera"); //ĳ������ 3dǥ�ø� ���� UI�� ī�޶� �޾ƿ�
            Camera _uiCamera = UI_Camera.GetComponent<Camera>(); //UI_Camera�� <Camera> ������Ʈ�� �޾ƿ�
            Canvas inv = GetComponent<Canvas>(); //�κ��丮�� <Canvas> ������Ʈ�� �޾ƿ�
            inv.worldCamera = _uiCamera; //Canvas.RenderCamera�� UI_Camera<Camera>�� �־���
            DontDestroyOnLoad(UI_Camera);
        }
        else //������
        {
            Camera _uiCamera = ExCam.GetComponent<Camera>(); //UI_Camera�� <Camera> ������Ʈ�� �޾ƿ�
            Canvas inv = GetComponent<Canvas>(); //�κ��丮�� <Canvas> ������Ʈ�� �޾ƿ�
            inv.worldCamera = _uiCamera; //Canvas.RenderCamera�� UI_Camera<Camera>�� �־���
        }
    }

    public void AcquireItem(Item _item, int _count = 1) //�������� slot�� �޾ƿ��� �Լ�
    {
        if(Define.Item.Equipment != _item._itemType &&
           Define.Item.Armor != _item._itemType &&
           Define.Item.Shoes != _item._itemType) //���� 1ĭ�� �����ϱ� ������ 
        {

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i]._item != null)
                {
                    if (_slots[i]._item._itemName == _item._itemName) //���� �ȿ� �������� �̹� �ִ� ���
                    {
                        _slots[i].SetSlotCount(_count); //i��°�� ������ ã�Ƽ� _count ��ŭ ������ ����
                        return;
                    }
                }
            } //���࿡ ������� for���� ���������� �ߺ��Ǵ� �������� ���ٸ�,
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i]._item == null) //��ĭ�� i��°�� ������ ã�Ƽ�
            {
                _slots[i].AddItem(_item, _count); //_item�� _count��ŭ ������ ����
                return;
            }
        }
    }

    public void ShowItemExp(Item _item, Vector3 _slotPos)
    {
        GetTextMeshProUGUI((int)Texts.ItemName).text = $"{_item._itemName}"; //������ �̸�
        GetTextMeshProUGUI((int)Texts.ItemType).text = $"{_item._itemType}"; //������ ����
        GetTextMeshProUGUI((int)Texts.ItemExpanation).text = $"{_item._itemDesc}"; //������ ����

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
    } //���⺰ �ִϸ��̼� ��� (slot)

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


    } //������ ����

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
    } //�κ��丮 �÷��̾��� ���� ���� �ı� �Լ�

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
    } //�ʵ� �÷��̾��� ���� ���� �ı� �Լ�
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InvRigthClick : UI_Popup
{
    UI_Slot slot;
    public Item item;
    public int itemCount;

    GameObject _player;

    enum Gameobjects
    {
        RegBlock
    }

    enum Texts
    {
        RegisterText
    }

    enum Buttons
    {
        RegisterButton,
        ThrowButton,
        CancelButton
    }

    public override void Init() //�ʰ�ȭ [2��° ����]
    {
        Bind<GameObject>(typeof(Gameobjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        _player = GameObject.FindWithTag("Player");

        GetObject((int)Gameobjects.RegBlock).SetActive(false); //������ �ʱ�ȭ
        Color _orgColor = GetTextMeshProUGUI((int)Texts.RegisterText).color; //�ؽ�Ʈ�� �ʰ�ȭ
        GetTextMeshProUGUI((int)Texts.RegisterText).color = _orgColor;

        CameraSetting(); //UI_Camera ����
        MovePos(); 

        switch (item._itemType)
        {
            case Define.Item.Equipment:
                break;
            case Define.Item.Armor:
                break;
            case Define.Item.Shoes:
                break;
            case Define.Item.Potion:
                break;
            case Define.Item.Ingredient: //����,��,�Ź�,������ �ƴϸ� ������ ���� �Ұ���
                GetObject((int)Gameobjects.RegBlock).SetActive(true);
                GetTextMeshProUGUI((int)Texts.RegisterText).color = Color.gray;
                break;
            default:
                break;
        }
    }

    private void CameraSetting()
    {
        GameObject ExCam = GameObject.Find("UI_Camera");
        Camera _uiCamera = ExCam.GetComponent<Camera>(); //UI_Camera�� <Camera> ������Ʈ�� �޾ƿ�
        Canvas inv = GetComponent<Canvas>(); //�κ��丮�� <Canvas> ������Ʈ�� �޾ƿ�
        inv.worldCamera = _uiCamera; //Canvas.RenderCamera�� UI_Camera<Camera>�� �־���
    }

    private void MovePos()
    {
        transform.GetChild(0).position = slot._orgPos;
        transform.GetChild(0).position += new Vector3(15f, -3.3f, 0f);
    }


    public void ItemSave(UI_Slot _slot) //Slot�� ������ ���� ���� [1��° ����]
    {
        item = _slot._item;
        itemCount = _slot._itemCount;
        slot = _slot;
    }

    public void OnRegisterButton()
    {
        Debug.Log("�����Կ� ������ ���");
        if (item._itemType == Define.Item.Equipment)

        ClosePopupUI();
    }

    public void OnDropButton()
    {
        switch (item._itemType)
        {
            case Define.Item.Equipment:
                DropItemSpawnAndClearSlot();
                break;
            case Define.Item.Armor:
                DropItemSpawnAndClearSlot();
                break;
            case Define.Item.Shoes:
                DropItemSpawnAndClearSlot();
                break;
            case Define.Item.Potion:
                CheckNumItemDrop(itemCount);
                break;
            case Define.Item.Ingredient:
                CheckNumItemDrop(itemCount);
                break;
            default:
                break;
        }
    }

    //public void CheckNumderedItem()
    //{
    //    UI_InputNum _Input = Managers.UI.ShowPopupUI<UI_InputNum>();
    //    _Input.Call(this);
    //} 8�ð��� ��� ���� �ϴ��� ����..
    // ã�ƺ��� �� ���뿡 ���� �޸�
    // Tmp�� �Է¹��� ���� �и��� string������, int.Parse/TryParse�� int ������ ��ȯ�� �Ұ����ϴ�..
    // ���� string�ε� ��.. �׷���..

    public void CheckNumItemDrop(int _num)
    {
        //StartCoroutine(DropItemCoroutine(_num));

        for (int i = 0; i < _num; i++)
        {
            Instantiate(item._itemPrefab, _player.transform.position, item._itemPrefab.transform.rotation);
        }
        slot.ClearSlot(); //���� ����
        ClosePopupUI();
    }

    private void DropItemSpawnAndClearSlot()
    {
        //���� ������� �ǵ�����
        switch (slot._item._itemType)
        {
            case Define.Item.Equipment:
                if (slot._item._itemADAPSPType == Define.WeaponType.AD)
                    slot._stat.Attack -= item._itemEffect;
                else if (slot._item._itemADAPSPType == Define.WeaponType.AP)
                    slot._stat.MAttack -= item._itemEffect;
                break;
            case Define.Item.Armor:
                if (slot._item._itemADAPSPType == Define.WeaponType.AD)
                    slot._stat.Defense -= item._itemEffect;
                else if (slot._item._itemADAPSPType == Define.WeaponType.AP)
                    slot._stat.MDefense -= item._itemEffect;
                break;
            case Define.Item.Shoes:
                slot._stat.MoveSpeed -= item._itemEffect;
                break;
        }
        slot._inventory.WeaponAnim("Wait"); //������ ������ �ִϸ��̼� ���
        slot._inventory.DestroyInvSocket(); //inv�� �ִ� ����Ƽ¯�� ���Ͽ� �ִ� ���� ����
        slot._inventory.DestroyHandSocket(); //game�� �ִ� ����Ƽ¯�� ���Ͽ� �ִ� ���� ����
        GameObject go = Instantiate(item._itemPrefab, _player.transform.position, item._itemPrefab.transform.rotation); //������ ������ ������
        go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 0.1f, go.transform.position.z);
        slot.ClearSlot(); //���� ����
        ClosePopupUI(); //�˾� �ݱ�
    }

    //�ϴ� ����� ���� ������ ������� ��� �ڷ�ƾ
    IEnumerator DropItemCoroutine(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            Instantiate(item._itemPrefab, _player.transform.position, item._itemPrefab.transform.rotation);
            yield return new WaitForSeconds(0.05f);
        }
        slot.ClearSlot(); //���� ����
        ClosePopupUI();
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }
}

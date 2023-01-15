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

    public override void Init() //초가화 [2번째 실행]
    {
        Bind<GameObject>(typeof(Gameobjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        _player = GameObject.FindWithTag("Player");

        GetObject((int)Gameobjects.RegBlock).SetActive(false); //가림막 초기화
        Color _orgColor = GetTextMeshProUGUI((int)Texts.RegisterText).color; //텍스트색 초가화
        GetTextMeshProUGUI((int)Texts.RegisterText).color = _orgColor;

        CameraSetting(); //UI_Camera 세팅
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
            case Define.Item.Ingredient: //무기,방어구,신발,포션이 아니면 퀵슬롯 장착 불가능
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
        Camera _uiCamera = ExCam.GetComponent<Camera>(); //UI_Camera의 <Camera> 컴포넌트를 받아옴
        Canvas inv = GetComponent<Canvas>(); //인벤토리의 <Canvas> 컴포넌트를 받아옴
        inv.worldCamera = _uiCamera; //Canvas.RenderCamera에 UI_Camera<Camera>를 넣어줌
    }

    private void MovePos()
    {
        transform.GetChild(0).position = slot._orgPos;
        transform.GetChild(0).position += new Vector3(15f, -3.3f, 0f);
    }


    public void ItemSave(UI_Slot _slot) //Slot의 아이템 정보 저장 [1번째 실행]
    {
        item = _slot._item;
        itemCount = _slot._itemCount;
        slot = _slot;
    }

    public void OnRegisterButton()
    {
        Debug.Log("퀵슬롯에 아이템 등록");
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
    //} 8시간의 장고 끝에 일단은 유기..
    // 찾아봐야 할 내용에 대한 메모
    // Tmp로 입력받은 값은 분명히 string이지만, int.Parse/TryParse로 int 형으로 변환이 불가능하다..
    // 같은 string인데 왜.. 그렇지..

    public void CheckNumItemDrop(int _num)
    {
        //StartCoroutine(DropItemCoroutine(_num));

        for (int i = 0; i < _num; i++)
        {
            Instantiate(item._itemPrefab, _player.transform.position, item._itemPrefab.transform.rotation);
        }
        slot.ClearSlot(); //슬롯 비우기
        ClosePopupUI();
    }

    private void DropItemSpawnAndClearSlot()
    {
        //스텟 원래대로 되돌리기
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
        slot._inventory.WeaponAnim("Wait"); //장착할 아이템 애니메이션 재생
        slot._inventory.DestroyInvSocket(); //inv에 있는 유니티짱의 소켓에 있는 무기 삭제
        slot._inventory.DestroyHandSocket(); //game에 있는 유니티짱의 소켓에 있는 무기 삭제
        GameObject go = Instantiate(item._itemPrefab, _player.transform.position, item._itemPrefab.transform.rotation); //삭제할 아이템 떨구기
        go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 0.1f, go.transform.position.z);
        slot.ClearSlot(); //슬롯 비우기
        ClosePopupUI(); //팝업 닫기
    }

    //일단 만든어 놓은 아이템 순서대로 뱉기 코루틴
    IEnumerator DropItemCoroutine(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            Instantiate(item._itemPrefab, _player.transform.position, item._itemPrefab.transform.rotation);
            yield return new WaitForSeconds(0.05f);
        }
        slot.ClearSlot(); //슬롯 비우기
        ClosePopupUI();
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }
}

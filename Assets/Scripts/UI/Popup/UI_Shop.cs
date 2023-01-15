using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : UI_Popup
{
    GameObject _player;
    PlayerStat _stat;
    [SerializeField]
    Item _firstItem;
    Item _currentItem;
    private bool _isFirst = true;

    //모든 팝업 닫기
    #region Enum
    enum Buttons
    {
        CloseButton,
        ItemBuyButton, //오 아이템 구입
    }

    enum Texts
    {
        ItemName, //우 아이템 이름
        ItemDesc, //우 아이템 설명
        ItemPrice, //우 아이템 가격
        ItemType, //우 아이템 장비 타입
        HaveGold, //우상 가진 골드
    }

    enum GameObjects
    {
        GridSlot //그리드 슬롯
    }

    enum Images
    {
        ItemImage, //우 아이템 이미지
    }
    #endregion

    private void Start()
    {
        _player = Managers.Game.GetPlayer();
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetImage((int)Images.ItemImage).sprite = _firstItem._itemImage;//초기 아이템 이미지
        GetTextMeshProUGUI((int)Texts.ItemName).text = $"{_firstItem._itemName}"; //초기 아이템 이름
        GetTextMeshProUGUI((int)Texts.ItemDesc).text = $"{_firstItem._itemDesc}"; //초기 아이템 설명
        GetTextMeshProUGUI((int)Texts.ItemType).text = $"{_firstItem._itemType}"; //초기 아이템 종류
        GetTextMeshProUGUI((int)Texts.ItemPrice).text = $"Gold : {_firstItem._itemPrice}"; //초기 아이템 가격

        _stat = _player.GetComponent<PlayerStat>();
    }

    private void LateUpdate()
    {
        GetTextMeshProUGUI((int)Texts.HaveGold).text = $"Gold : {_stat.Gold}";
    }

    public override void Init()
    {
    }

    public void ShowItemExp(Item _item, Vector3 _slotPos)
    {
        _currentItem = _item;
        GetImage((int)Images.ItemImage).sprite = _item._itemImage;//아이템 이미지
        GetTextMeshProUGUI((int)Texts.ItemName).text = $"{_item._itemName}"; //아이템 이름
        GetTextMeshProUGUI((int)Texts.ItemDesc).text = $"{_item._itemDesc}"; //아이템 설명
        GetTextMeshProUGUI((int)Texts.ItemType).text = $"{_item._itemType}"; //아이템 종류
        GetTextMeshProUGUI((int)Texts.ItemPrice).text = $"Gold : {_item._itemPrice}"; //아이템 가격
        GetObject((int)GameObjects.GridSlot).SetActive(true);
        GetObject((int)GameObjects.GridSlot).transform.position = _slotPos;
        _isFirst = false;
    }

    public void OnBuyButton()
    {
        if (_isFirst)
        {
            if (_firstItem._itemPrice > _stat.Gold)
                return;
            else
            {
                _stat.Gold -= _firstItem._itemPrice; //골드를 깎고
                GameObject go = Instantiate(_firstItem._itemPrefab, _player.transform.position, _firstItem._itemPrefab.transform.rotation);
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 0.1f, go.transform.position.z);
            }
        }
        else
        {
            if (_currentItem._itemPrice > _stat.Gold)
                return;
            else
            {
                _stat.Gold -= _currentItem._itemPrice; //골드를 깎고
                GameObject go = Instantiate(_currentItem._itemPrefab, _player.transform.position, _currentItem._itemPrefab.transform.rotation);
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 0.1f, go.transform.position.z);
            }
        }
    }

    public override void ClosePopupUI()
    {
        CameraController _cam = Camera.main.GetComponent<CameraController>();
        _cam._mode = Define.CameraMode.QuarterView;
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Player");
        _isFirst = true;
        base.ClosePopupUI();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ShopSlot : UI_Base, IPointerClickHandler
{
    [SerializeField]
    private ShotType _shopType;
    //필요한 컴포넌트
    private UI_Shop _shopItem;
    private UI_Shop_Amr _shopItemAmr;
    private UI_Shop_Sho _shopItemSho;
    private UI_Shop_Pot _shopItemPot;


    [SerializeField]
    private Item _item; //아이템 정보
    [SerializeField]
    private Image _itemImage; //아이템 이미지
    [SerializeField]
    public int _stock; //아이템 재고
    [SerializeField]
    private int _price; //아이템 가격
    [SerializeField]
    private TextMeshProUGUI _stockText; //남은 재고 텍스트
    [SerializeField]
    private TextMeshProUGUI _priceText; //아이템 가격 텍스트

    public override void Init()
    {
    }

    private void Start()
    {
        if (_item == null)
        {
            SetColor(0);
            _stockText.text = $"";
            _priceText.text = $"";
            return;
        }

        switch (_shopType) //진짜 멍청한 코드.. 수정 필수
        {
            case ShotType.Equ:
                _shopItem = FindObjectOfType<UI_Shop>();
                break;
            case ShotType.Amr:
                _shopItemAmr = FindObjectOfType<UI_Shop_Amr>();
                break;
            case ShotType.Sho:
                _shopItemSho = FindObjectOfType<UI_Shop_Sho>();
                break;
            case ShotType.Pot:
                _shopItemPot = FindObjectOfType<UI_Shop_Pot>();
                break;
        }
        _priceText.text = $"{_price}";
        _itemImage.sprite = _item._itemImage;
    }

    private void LateUpdate()
    {
        if (_item != null)
            _stockText.text = $"{_stock}"; //남은 재고 표시
    }

    public void SetColor(float _alpha)
    {
        Color color = _itemImage.color;
        color.a = _alpha;
        _itemImage.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) //인벤토리에서 좌클릭을 할 경우
        {
            if (_item != null) //아이템이 있으면
            {
                switch (_shopType) //진짜 멍청한 코드.. 수정 필수
                {
                    case ShotType.Equ:
                        _shopItem.ShowItemExp(_item, transform.position);
                        break;
                    case ShotType.Amr:
                        _shopItemAmr.ShowItemExp(_item, transform.position);
                        break;
                    case ShotType.Sho:
                        _shopItemSho.ShowItemExp(_item, transform.position);
                        break;
                    case ShotType.Pot:
                        _shopItemPot.ShowItemExp(_item, transform.position);
                        break;
                }
            }
        }
    }
}

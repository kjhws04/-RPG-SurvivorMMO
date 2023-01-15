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
    //�ʿ��� ������Ʈ
    private UI_Shop _shopItem;
    private UI_Shop_Amr _shopItemAmr;
    private UI_Shop_Sho _shopItemSho;
    private UI_Shop_Pot _shopItemPot;


    [SerializeField]
    private Item _item; //������ ����
    [SerializeField]
    private Image _itemImage; //������ �̹���
    [SerializeField]
    public int _stock; //������ ���
    [SerializeField]
    private int _price; //������ ����
    [SerializeField]
    private TextMeshProUGUI _stockText; //���� ��� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI _priceText; //������ ���� �ؽ�Ʈ

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

        switch (_shopType) //��¥ ��û�� �ڵ�.. ���� �ʼ�
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
            _stockText.text = $"{_stock}"; //���� ��� ǥ��
    }

    public void SetColor(float _alpha)
    {
        Color color = _itemImage.color;
        color.a = _alpha;
        _itemImage.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) //�κ��丮���� ��Ŭ���� �� ���
        {
            if (_item != null) //�������� ������
            {
                switch (_shopType) //��¥ ��û�� �ڵ�.. ���� �ʼ�
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

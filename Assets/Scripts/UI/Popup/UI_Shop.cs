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

    //��� �˾� �ݱ�
    #region Enum
    enum Buttons
    {
        CloseButton,
        ItemBuyButton, //�� ������ ����
    }

    enum Texts
    {
        ItemName, //�� ������ �̸�
        ItemDesc, //�� ������ ����
        ItemPrice, //�� ������ ����
        ItemType, //�� ������ ��� Ÿ��
        HaveGold, //��� ���� ���
    }

    enum GameObjects
    {
        GridSlot //�׸��� ����
    }

    enum Images
    {
        ItemImage, //�� ������ �̹���
    }
    #endregion

    private void Start()
    {
        _player = Managers.Game.GetPlayer();
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetImage((int)Images.ItemImage).sprite = _firstItem._itemImage;//�ʱ� ������ �̹���
        GetTextMeshProUGUI((int)Texts.ItemName).text = $"{_firstItem._itemName}"; //�ʱ� ������ �̸�
        GetTextMeshProUGUI((int)Texts.ItemDesc).text = $"{_firstItem._itemDesc}"; //�ʱ� ������ ����
        GetTextMeshProUGUI((int)Texts.ItemType).text = $"{_firstItem._itemType}"; //�ʱ� ������ ����
        GetTextMeshProUGUI((int)Texts.ItemPrice).text = $"Gold : {_firstItem._itemPrice}"; //�ʱ� ������ ����

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
        GetImage((int)Images.ItemImage).sprite = _item._itemImage;//������ �̹���
        GetTextMeshProUGUI((int)Texts.ItemName).text = $"{_item._itemName}"; //������ �̸�
        GetTextMeshProUGUI((int)Texts.ItemDesc).text = $"{_item._itemDesc}"; //������ ����
        GetTextMeshProUGUI((int)Texts.ItemType).text = $"{_item._itemType}"; //������ ����
        GetTextMeshProUGUI((int)Texts.ItemPrice).text = $"Gold : {_item._itemPrice}"; //������ ����
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
                _stat.Gold -= _firstItem._itemPrice; //��带 ���
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
                _stat.Gold -= _currentItem._itemPrice; //��带 ���
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

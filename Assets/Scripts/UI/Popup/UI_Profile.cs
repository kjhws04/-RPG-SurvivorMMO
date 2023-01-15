using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Profile : UI_Popup
{
    PlayerStat _stat;
    UI_Inventory _inventory;

    private bool _isCoolTime = false;

    enum Buttons
    {
        Inventory,
        SkillTree,
        StatusInfo
    }

    enum Texts
    {
        PlayerLv,
        CurrentHpText,
        CurrentMpText,
        PotionCountText,
        CoolTimeText
    }
    
    enum GameObjects
    {
        HpBarBase,
        CurrentHp,
        MpBarBase,
        PotionSlot,
        CurrentMp,
        ItemCountBase
    }
    
    enum Images
    {
        ProfileBase,
        PlayerBreastShot,
        PotionImage,
        CoolTime
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        //_stat�� �θ�� �����ϱ� ���� UI_root�� �ִ� ���� �÷��̾� ������ �̵��ؾ���
        _stat = transform.parent.GetComponent<PlayerStat>();
        _inventory = Util.FindChild<UI_Inventory>(_stat.gameObject, "UI_Inventory");
        _inventory.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetObject((int)GameObjects.PotionSlot).SetActive(false);
        GetTextMeshProUGUI((int)Texts.CoolTimeText).gameObject.SetActive(false);
    }

    private void Update()
    {
        GetTextMeshProUGUI((int)Texts.PlayerLv).text = $"Lv.{_stat.Level}";

        //ü�� ����
        GetTextMeshProUGUI((int)Texts.CurrentHpText).text = $"{_stat.Hp} / {_stat.MaxHp}";
        float ratioHp = _stat.Hp / (float)_stat.MaxHp;
        SetHPBar(ratioHp);

        ////���� ����
        float ratioMp = _stat.Mp / (float)_stat.MaxMp;
        SetMPBar(ratioMp);
        GetTextMeshProUGUI((int)Texts.CurrentMpText).text = $"{_stat.Mp} / {_stat.MaxMp}";

        SetPotionSlot(); //���� UI
    }

    #region Current HP/MP Bar & Current Potion Count
    public void SetHPBar(float ratioHp)
    {
        GetObject((int)GameObjects.CurrentHp).GetComponent<Image>().fillAmount = ratioHp;
    }

    public void SetMPBar(float ratioMp)
    {
        GetObject((int)GameObjects.CurrentMp).GetComponent<Image>().fillAmount = ratioMp;
    }

    private void CoolTimeCalc()
    {
        if(_isCoolTime)
        {
            _stat.PotionCoolTime -= Time.deltaTime;
            if (_stat.PotionCoolTime <= 0)
                _isCoolTime = false;
        }
    } //��Ÿ���� �����ϴ� �Լ�

    public void SetPotionSlot()
    {
        if (_stat._stat._potion._item != null) //������(����)�� ������ �ִٸ�,
        {
            GetObject((int)GameObjects.PotionSlot).SetActive(true); //���ǽ��� on
            GetImage((int)Images.PotionImage).sprite = _stat._stat._potion._item._itemImage; //�̹����� ������ ���� �̹�����
            GetTextMeshProUGUI((int)Texts.PotionCountText).text = $"{_stat._stat._potion._itemCount}"; //���� ���� ǥ��

            if (_stat.PotionCoolTime == _stat._stat._potion._item._potionCoolTime) //��Ÿ���� ���� �ʾ�����
            {
                SetColor(0);
                GetTextMeshProUGUI((int)Texts.CoolTimeText).gameObject.SetActive(false);
            }
            else
            {
                SetColor(0.5f);
                GetImage((int)Images.CoolTime).fillAmount = _stat.PotionCoolTime / _stat._stat._potion._item._potionCoolTime;
                GetTextMeshProUGUI((int)Texts.CoolTimeText).gameObject.SetActive(true);
                GetTextMeshProUGUI((int)Texts.CoolTimeText).text = $"{(int)_stat.PotionCoolTime + 1}";
            }
        }
        else
            GetObject((int)GameObjects.PotionSlot).SetActive(false); //���ǽ��� off
    }
    #endregion

    private void SetColor(float _alp) //�̹����� ������ ���� 1=100%, 0=0% //�ּ�[1]
    {
        Color color = GetImage((int)Images.CoolTime).color;
        color.a = _alp;
        GetImage((int)Images.CoolTime).color = color;
    }

    #region Button Actived
    public void OnInventory() //�κ� ���� 
    {
        Time.timeScale = 0.0f;
        Managers.UI.CloseAllPopupUI();

        _inventory.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OffInventory()
    {
        _inventory.RestItemExp();
        _inventory.gameObject.SetActive(false);
        gameObject.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void OnSkillTree() //��ų Ʈ�� �˾�
    {
        Managers.UI.ShowPopupUI<UI_SkillTree>(); 
    }

    public void OnStatusInfo() //���� ���� �˾�
    {
        Managers.UI.ShowPopupUI<UI_Status>();
    }
    #endregion

}

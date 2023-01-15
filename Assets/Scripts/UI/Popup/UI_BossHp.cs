using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHp : UI_Popup
{
    Stat _bossStat;
    BossController _bossCont;

    enum Texts
    {
        AKAText,
        NameText,
        CurrentHpText
    }

    enum GameObjects
    {
        CurrentHp
    }


    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        GetTextMeshProUGUI((int)Texts.AKAText).text = _bossCont._aka;
        GetTextMeshProUGUI((int)Texts.NameText).text = _bossCont._name;
    }

    public void keep(Stat _stat, BossController _con)
    {
        _bossStat = _stat;
        _bossCont = _con;
    }

    private void LateUpdate()
    {
        GetTextMeshProUGUI((int)Texts.CurrentHpText).text = $"{_bossStat.Hp} / {_bossStat.MaxHp}";
        float ratioHp = _bossStat.Hp / (float)_bossStat.MaxHp;
        SetHPBar(ratioHp);
    }

    public void SetHPBar(float ratioHp)
    {
        GetObject((int)GameObjects.CurrentHp).GetComponent<Image>().fillAmount = ratioHp;
    }
}

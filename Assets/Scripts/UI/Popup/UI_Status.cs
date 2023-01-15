using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : UI_Popup
{
    PlayerStat _stat;

    enum Buttons
    {
        CloseButton
    }

    enum Texts
    {
        StatusText,
        NowStatus
    }

    enum Images
    {
        StatusBase
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        _stat = FindObjectOfType<PlayerStat>();
    }

    private void Update()
    {
        GetTextMeshProUGUI((int)Texts.NowStatus).text =
            $"Level : {_stat.Level}\n" +
            $"MaxHp : {_stat.MaxHp}\n" +
            $"MaxMp : {_stat.MaxMp}\n" +
            $"AD : {_stat.Attack}\n" +
            $"AP : {_stat.MAttack}\n" +
            $"Armor : {_stat.Defense}\n" +
            $"MR : {_stat.MDefense}\n" +
            $"MoveSpeed :  {_stat.MoveSpeed.ToString("F1")}";
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTree : UI_Popup
{
    PlayerStat _stat;
    public int _maxStat = 10;

    enum Buttons
    {
        CloseButton,
        ResetButton,
        PlusButton1,
        PlusButton2,
        PlusButton3,
        PlusButton4,
        PlusButton5
    }

    enum Texts
    {
        SkillTreeText,
        SkillPointText,
    }

    enum GameObjects
    {
        CurrentSkillBar1,
        CurrentSkillBar2,
        CurrentSkillBar3,
        CurrentSkillBar4,
        CurrentSkillBar5
    }

    enum Images
    {
        SkillTreeBase,
        SkillPointBase,
        SkillIcon1,
        SkillIcon2,
        SkillIcon3,
        SkillIcon4,
        SkillIcon5,
        SkillBar1,
        SkillBar2,
        SkillBar3,
        SkillBar4,
        SkillBar5
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        _stat = FindObjectOfType<PlayerStat>();
        //GameObject _player = Managers.Game.GetPlayer();
        //_stat = _player.GetComponent<PlayerStat>();

        CheckCurrentSkill();
    }

    private void Update()
    {
        //현재 남은 SP양 표시
        GetTextMeshProUGUI((int)Texts.SkillPointText).text = $"SP : {_stat.SkillTreePoint}";
    }

    // init()할 때, 저장된 막대 그래프를 표시 하게 해주는 코드
    private void CheckCurrentSkill()
    {
        int _1division10 = 10;

        GetObject((int)GameObjects.CurrentSkillBar5).GetComponent<Image>().fillAmount = (float)_stat.Vitality / _1division10;
        GetObject((int)GameObjects.CurrentSkillBar4).GetComponent<Image>().fillAmount = (float)_stat.Mentality / _1division10;
        GetObject((int)GameObjects.CurrentSkillBar3).GetComponent<Image>().fillAmount = (float)_stat.Strength / _1division10;
        GetObject((int)GameObjects.CurrentSkillBar2).GetComponent<Image>().fillAmount = (float)_stat.Intellect / _1division10;
        GetObject((int)GameObjects.CurrentSkillBar1).GetComponent<Image>().fillAmount = (float)_stat.Ability / _1division10;

    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }

    #region Use SP To Add Skill Tree & SP Reset
    public void AddVitality()
    {
        if (_stat.SkillTreePoint == 0)
            return;

        if (_stat.Vitality >= _maxStat)
            return;

        _stat.SkillTreePoint--;
        _stat.Vitality++;
        _stat.MaxHp += 20;
        GetObject((int)GameObjects.CurrentSkillBar5).GetComponent<Image>().fillAmount += 0.1f;
    }

    public void AddMentality()
    {
        if (_stat.SkillTreePoint == 0)
            return;

        if (_stat.Mentality >= _maxStat)
            return;

        _stat.SkillTreePoint--;
        _stat.Mentality++;
        _stat.MaxMp += 20;
        GetObject((int)GameObjects.CurrentSkillBar4).GetComponent<Image>().fillAmount += 0.1f;
    }

    public void AddStrength()
    {
        if (_stat.SkillTreePoint == 0)
            return;

        if (_stat.Strength >= _maxStat)
            return;

        _stat.SkillTreePoint--;
        _stat.Strength++;
        _stat.Attack += 20;
        GetObject((int)GameObjects.CurrentSkillBar3).GetComponent<Image>().fillAmount += 0.1f;
    }

    public void AddIntellect()
    {
        if (_stat.SkillTreePoint == 0)
            return;

        if (_stat.Intellect >= _maxStat)
            return;

        _stat.SkillTreePoint--;
        _stat.Intellect++;
        _stat.MAttack += 20;
        GetObject((int)GameObjects.CurrentSkillBar2).GetComponent<Image>().fillAmount += 0.1f;
    }

    public void AddAbility()
    {
        if (_stat.SkillTreePoint == 0)
            return;

        if (_stat.Ability >= _maxStat)
            return;

        _stat.SkillTreePoint--;
        _stat.Ability++;
        _stat.MoveSpeed += 0.2f;
        GetObject((int)GameObjects.CurrentSkillBar1).GetComponent<Image>().fillAmount += 0.1f;
    }

    public void ResetSkillPoint()
    {
        int _useSP = _stat.Vitality + _stat.Mentality + _stat.Strength + _stat.Intellect + _stat.Ability; //투자한 스킬 값

        _stat.SkillTreePoint += _useSP; //투자한 스킬값을 

        _stat.Vitality = 0;
        _stat.Mentality = 0;
        _stat.Strength = 0;
        _stat.Intellect = 0;
        _stat.Ability = 0;
        GetObject((int)GameObjects.CurrentSkillBar5).GetComponent<Image>().fillAmount = 0.0f;
        GetObject((int)GameObjects.CurrentSkillBar4).GetComponent<Image>().fillAmount = 0.0f;
        GetObject((int)GameObjects.CurrentSkillBar3).GetComponent<Image>().fillAmount = 0.0f;
        GetObject((int)GameObjects.CurrentSkillBar2).GetComponent<Image>().fillAmount = 0.0f;
        GetObject((int)GameObjects.CurrentSkillBar1).GetComponent<Image>().fillAmount = 0.0f;

        _stat.ResetStat(_stat.Level);
    }
    #endregion
}

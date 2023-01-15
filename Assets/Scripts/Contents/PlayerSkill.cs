using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/Skill")]

public class PlayerSkill : ScriptableObject
{
    public Define.SkillName _skillName; //스킬 이름
    public Define.SkillType _skilType; //스킬 타입
    public float _skillDuration; //스킬 지속시간
    public float _skillCoolTime; //스킬 쿨타임
    public Define.WeaponType _skillADAPType; //스킬 공격 계수
    public float _skillEffect; //스킬 효과

    public GameObject _skillPrefab; //스킬 프리펩

}

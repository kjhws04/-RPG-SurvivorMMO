using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/Skill")]

public class PlayerSkill : ScriptableObject
{
    public Define.SkillName _skillName; //��ų �̸�
    public Define.SkillType _skilType; //��ų Ÿ��
    public float _skillDuration; //��ų ���ӽð�
    public float _skillCoolTime; //��ų ��Ÿ��
    public Define.WeaponType _skillADAPType; //��ų ���� ���
    public float _skillEffect; //��ų ȿ��

    public GameObject _skillPrefab; //��ų ������

}

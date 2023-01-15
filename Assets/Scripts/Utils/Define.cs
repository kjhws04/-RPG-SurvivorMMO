using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    //https://drive.google.com/drive/folders/1Ub3YToqwMM3yqmlL0lNje_BADDfgUIUI
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }

    public enum EntityType
    {
        Player,
        CommonMonster,
        Boss
    }

    public enum State
	{
		Die,
		Moving,
		Idle,
		Skill,
        Spawn
	}

    public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
        Boss,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum CameraMode
    {
        QuarterView,
        FrontView
    }

    public enum Item
    {
        Equipment,
        Armor,
        Shoes,
        Potion,
        Ingredient
    }

    public enum OpenInv
    {
        On,
        Off,
    }

    public enum SlotType
    {
        Normal,
        Quick
    }

    public enum WeaponType
    {
        AD,
        AP,
        Sp,
        Null
    }

    public enum WeaponAnim
    {
        Null,
        OneHandSword,
        Bow,
        Magic
    }

    public enum Hand
    {
        Right,
        Left
    }

    public enum HpMpType
    {
        Null,
        Hp,
        Mp
    }

    public enum SkillName
    {
        HallFlame,
        HealingFactor,
        Berserk
    }

    public enum SkillType
    {
        Active,
        Buff,
        DeBuff,
        Heal
    }

    public enum MonsterName
    {
        Ghoul,
        GhoulScavenger,
        GhoulGrotesque,
        GhoulFestering
    }
}
#region

using UnityEngine;

#endregion

public class ActorSpell
{
    public ActorSpell(ActorSpellName spellName, ActorType actorType)
    {
        this.ActorSpellType = ActorSpellType.PassiveSpell;
        this.ActorSpellName = spellName;
        this.ReleaseInterval = 0;
        this.AttackRange = 0;
        this.DamageBonusPercent = 0f;
        this.DamageBonusPercentProbability = 0;
        this.IncreaseFriendlyForcesArmor = 0;
        this.Resurrection = false;
        this.StunDuration = 0;
        this.StunProbability = 0;
        this.AdditionalDamage = 0;
        this.AdditionalDamageProbability = 0;
        this.DirectDamage = 0;
        this.DirectDamageProbability = 0;
        this.SplashPercent = 0;
        this.EvasiveProbability = 0;

        switch (actorType)
        {
            case ActorType.Supporter:
                {
                    this.EvasiveProbability = 15;
                    break;
                }
            case ActorType.Sniper:
                {
                    this.DamageBonusPercent = 1.5f;
                    this.DamageBonusPercentProbability = 10;
                    break;
                }
            case ActorType.Marksman:
                {
                    this.DamageBonusPercent = 3;
                    this.DamageBonusPercentProbability = 40;
                    break;
                }
            case ActorType.HeavyGunner:
                {
                    this.AttackRange = 18;
                    this.SplashPercent = 50;
                    break;
                }
            case ActorType.MortarTeam:
                {
                    this.AttackRange = 40;
                    this.DirectDamage = 21;
                    this.ReleaseInterval = 3;
                    this.ActorSpellType = ActorSpellType.ActiveSpell;
                    break;
                }
            case ActorType.Warlock:
                {
                    this.AttackRange = 50;
                    this.DirectDamage = 27;
                    this.ReleaseInterval = 5;
                    this.ActorSpellType = ActorSpellType.ActiveSpell;
                    break;
                }
            case ActorType.GryphonRider:
                {
                    if (spellName == ActorSpellName.Bash)
                    {
                        this.StunDuration = 2;
                        this.StunProbability = 25;
                        this.AdditionalDamage = 15;
                    }
                    else if (spellName == ActorSpellName.ChainLightning)
                    {
                        this.DirectDamage = 150;
                        this.DirectDamageProbability = 10;
                    }
                    break;
                }
            case ActorType.SeniorGryphonRider:
                {
                    if (spellName == ActorSpellName.Bash)
                    {
                        this.StunDuration = 2;
                        this.StunProbability = 30;
                        this.AdditionalDamage = 25;
                    }
                    else if (spellName == ActorSpellName.ChainLightning)
                    {
                        this.DirectDamage = 200;
                        this.DirectDamageProbability = 25;
                    }
                    break;
                }
            case ActorType.Crusader:
                {
                    if (spellName == ActorSpellName.Bash)
                    {
                        this.StunDuration = 2;
                        this.StunProbability = 20;
                        this.AdditionalDamage = 25;
                    }
                    else if (spellName == ActorSpellName.DivineBlessing)
                    {
                        this.AttackRange = 40;
                        this.IncreaseFriendlyForcesArmor = 6;
                        this.ActorSpellType = ActorSpellType.BuffSpell;
                    }
                    break;
                }
            case ActorType.TemplarWarrior:
                {
                    if (spellName == ActorSpellName.Bash)
                    {
                        this.StunDuration = 2;
                        this.StunProbability = 20;
                        this.AdditionalDamage = 25;
                    }
                    else if (spellName == ActorSpellName.DivineBlessing)
                    {
                        this.AttackRange = 40;
                        this.IncreaseFriendlyForcesArmor = 9;
                        this.ActorSpellType = ActorSpellType.BuffSpell;
                    }
                    else if (spellName == ActorSpellName.Zap)
                    {
                        this.AttackRange = 100;
                        this.Resurrection = true;
                        this.ReleaseInterval = 60;
                        this.ActorSpellType = ActorSpellType.ActiveSpell;
                    }
                    break;
                }
        }
    }

    #region Fields

    public ActorSpellType ActorSpellType { get; set; } //技能类型

    public ActorSpellName ActorSpellName { get; set; } //技能名称

    public int ReleaseInterval { get; set; } //释放间隔

    public GameObject AffectedObjects { get; set; } //受影响物体

    public int AttackRange { get; set; } //攻击范围

    public float DamageBonusPercent { get; set; } //伤害加成

    public int DamageBonusPercentProbability { get; set; } //伤害加成概率

    public int IncreaseFriendlyForcesArmor { get; set; } //增加友军护甲

    //public int IncreaseFriendlyForcesArmorDistance { get; set; } //增加友军护甲的影响范围

    public bool Resurrection { get; set; } //使目标单位复活

    public int StunDuration { get; set; } //眩晕持续时长

    public int StunProbability { get; set; } //眩晕几率

    public int AdditionalDamage { get; set; } //追加伤害

    public int AdditionalDamageProbability { get; set; } //追加伤害概率

    public int DirectDamage { get; set; } //直接伤害

    public int DirectDamageProbability { get; set; } //直接伤害几率

    public int SplashPercent { get; set; } //溅射伤害百分比

    public int EvasiveProbability { get; set; } //躲避几率

    #endregion
}
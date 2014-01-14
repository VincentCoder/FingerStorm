#region

using UnityEngine;

#endregion

public class ActorSpell
{
    public ActorSpell(ActorSpellType spellType)
    {
        this.ActorSpellType = spellType;
        this.AttackRange = 0;
        this.Dps = 0;
        this.DamageBonus = 0f;
        this.DamageBonusProbability = 0;
        this.IncreaseFriendlyForcesArmor = 0;
        this.IncreaseFriendlyForcesArmorDistance = 0;
        this.Resurrection = false;
        this.StunDuration = 0;
        this.StunProbability = 0;
        this.EvasiveProbability = 0;

        switch (spellType)
        {
            case ActorSpellType.None:
                {
                    break;
                }
            case ActorSpellType.Dodge:
                {
                    this.EvasiveProbability = 15;
                    break;
                }
            case ActorSpellType.CirticalStrike:
                {
                    this.DamageBonus = 1.5f;
                    this.DamageBonusProbability = 10;
                    break;
                }
            case ActorSpellType.HeadShot:
                {
                    this.DamageBonus = 3;
                    this.DamageBonusProbability = 40;
                    break;
                }
                
        }
    }

    #region Fields

    public ActorSpellType ActorSpellType { get; set; } //技能类型

    public GameObject AffectedObjects { get; set; } //受影响物体

    public int AttackRange { get; set; } //攻击范围

    public int Dps { get; set; } //每次攻击的伤害

    public float DamageBonus { get; set; } //伤害加成

    public int DamageBonusProbability { get; set; } //伤害加成概率

    public int IncreaseFriendlyForcesArmor { get; set; } //增加友军护甲

    public int IncreaseFriendlyForcesArmorDistance { get; set; } //增加友军护甲的影响范围

    public bool Resurrection { get; set; } //使目标单位复活

    public int StunDuration { get; set; } //眩晕持续时长

    public int StunProbability { get; set; } //眩晕几率

    public int EvasiveProbability { get; set; } //躲避几率

    #endregion
}
#region

using System.Collections.Generic;

#endregion

public static class TerranActorInfantryConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.HeavyArmor, 4);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Normal, 0, 18);

    public static ActorType ActorType = ActorType.Infantry;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = false;

    public static int Hp = 250;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.PassiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .Parry,
                                new ActorSpell
                                (
                                ActorSpellName
                                .Parry,
                                ActorType
                                .Infantry)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorSupporterConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.HeavyArmor, 7);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Normal, 0, 38);

    public static ActorType ActorType = ActorType.Supporter;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = false;

    public static int Hp = 575;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.PassiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .Dodge,
                                new ActorSpell
                                (
                                ActorSpellName
                                .Dodge,
                                ActorType
                                .Supporter)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorSniperConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.LightArmor, 0);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Pierce, 50, 22);

    public static ActorType ActorType = ActorType.Sniper;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = true;

    public static int Hp = 270;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.PassiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .CirticalStrike,
                                new ActorSpell
                                (
                                ActorSpellName
                                .CirticalStrike,
                                ActorType
                                .Sniper)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorMarksmanConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.LightArmor, 1);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Pierce, 60, 61);

    public static ActorType ActorType = ActorType.Marksman;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = true;

    public static int Hp = 450;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.PassiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .HeadShot,
                                new ActorSpell
                                (
                                ActorSpellName
                                .HeadShot,
                                ActorType
                                .Marksman)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorHeavyGunnerConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.LightArmor, 3);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Siege, 55, 53);

    public static ActorType ActorType = ActorType.HeavyGunner;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = false;

    public static int Hp = 500;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.PassiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .SplashDamage,
                                new ActorSpell
                                (
                                ActorSpellName
                                .SplashDamage,
                                ActorType
                                .HeavyGunner)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorMortarTeamConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.LightArmor, 0);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Pierce, 100, 21);

    public static ActorType ActorType = ActorType.MortarTeam;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = false;

    public static int Hp = 260;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.ActiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .MortarAttack,
                                new ActorSpell
                                (
                                ActorSpellName
                                .MortarAttack,
                                ActorType
                                .MortarTeam)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorWarlockConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.LightArmor, 1);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Confuse, 20, 17);

    public static ActorType ActorType = ActorType.Warlock;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = true;

    public static int Hp = 320;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.ActiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .ArcaneExplosion,
                                new ActorSpell
                                (
                                ActorSpellName
                                .ArcaneExplosion,
                                ActorType
                                .Warlock)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorGryphonRiderConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.LightArmor, 2);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Magic, 45, 23);

    public static ActorType ActorType = ActorType.GryphonRider;

    public static bool IsAirForce = true;

    public static bool AttackAirForce = true;

    public static int Hp = 500;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.PassiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .Bleed,
                                new ActorSpell
                                (
                                ActorSpellName
                                .Bleed,
                                ActorType
                                .GryphonRider)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorSeniorGryphonRiderConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.LightArmor, 5);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Magic, 50, 30);

    public static ActorType ActorType = ActorType.SeniorGryphonRider;

    public static bool IsAirForce = true;

    public static bool AttackAirForce = true;

    public static int Hp = 650;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.PassiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .Bleed,
                                new ActorSpell
                                (
                                ActorSpellName
                                .Bleed,
                                ActorType
                                .SeniorGryphonRider)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorCrusaderConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.HeavyArmor, 6);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Normal, 0, 42);

    public static ActorType ActorType = ActorType.Crusader;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = false;

    public static int Hp = 600;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.PassiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .Bash,
                                new ActorSpell
                                (
                                ActorSpellName
                                .Bash,
                                ActorType
                                .Crusader)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorTemplarWarriorConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.HeavyArmor, 9);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.HeroAttack, 0, 67);

    public static ActorType ActorType = ActorType.TemplarWarrior;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = false;

    public static int Hp = 850;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.PassiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .Bash,
                                new ActorSpell
                                (
                                ActorSpellName
                                .Bash,
                                ActorType
                                .TemplarWarrior)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorPriestConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.LightArmor, 1);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Magic, 50, 20);

    public static ActorType ActorType = ActorType.Priest;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = true;

    public static int Hp = 550;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.ActiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .Zap,
                                new ActorSpell
                                (
                                ActorSpellName
                                .Zap,
                                ActorType
                                .Priest)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorOracleConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.LightArmor, 3);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Magic, 60, 45);

    public static ActorType ActorType = ActorType.Oracle;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = true;

    public static int Hp = 600;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.PassiveSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .ChainLightning,
                                new ActorSpell
                                (
                                ActorSpellName
                                .ChainLightning,
                                ActorType
                                .Oracle)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorKnightConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.HeavyArmor, 7);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.Normal, 0, 50);

    public static ActorType ActorType = ActorType.Knight;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = false;

    public static int Hp = 600;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.BuffSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .DivineBlessing,
                                new ActorSpell
                                (
                                ActorSpellName
                                .DivineBlessing,
                                ActorType
                                .Knight)
                            }
                        }
                }
            };

    #endregion
}

public static class TerranActorPaladinConfig
{
    #region Static Fields

    public static ActorArmor ActorArmor = new ActorArmor(ActorArmorType.HeavyArmor, 10);

    public static ActorAttack ActorAttack = new ActorAttack(ActorAttackType.HeroAttack, 0, 70);

    public static ActorType ActorType = ActorType.Paladin;

    public static bool IsAirForce = false;

    public static bool AttackAirForce = false;

    public static int Hp = 800;

    public static Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>> SpellDictionary =
        new Dictionary<ActorSpellType, Dictionary<ActorSpellName, ActorSpell>>
            {
                {
                    ActorSpellType.BuffSpell,
                    new Dictionary
                    <ActorSpellName, ActorSpell>
                        {
                            {
                                ActorSpellName
                                .DivineBlessing,
                                new ActorSpell
                                (
                                ActorSpellName
                                .DivineBlessing,
                                ActorType
                                .Paladin)
                            }
                        }
                }
            };

    #endregion
}
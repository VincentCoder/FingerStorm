#region

using System.Collections.Generic;

#endregion

public class Actor
{
    #region Constructors and Destructors

    public Actor(int actorId, ActorType type, FactionType faction)
    {
        this.ActorId = actorId;
        this.ActorType = type;
        this.ActorArmor = this.LoadArmorFromConfig();
        this.ActorAttack = this.LoadActorAttackFromConfig();
        this.FactionType = faction;
        this.Hp = this.LoadHpFromConfig();
        this.SpellDictionary = this.LoadSpellsFromConfig();
    }

    #endregion

    #region Public Properties

    public int ActorId { get; set; }

    public ActorArmor ActorArmor { get; set; }

    public ActorAttack ActorAttack { get; set; }

    public ActorType ActorType { get; set; }

    public FactionType FactionType { get; set; }

    public float Hp { get; set; }

    public Dictionary<ActorSpellType, ActorSpell> SpellDictionary { get; set; }

    #endregion

    #region Methods

    private ActorAttack LoadActorAttackFromConfig()
    {
        ActorAttackType attackType;
        string attackTypeStr;
        int attackRange;
        int dps;
        switch (this.ActorType)
        {
            case ActorType.Infantry:
                {
                    attackTypeStr = GlobalConfig.TerranActorInfantryAttackType;
                    attackRange = GlobalConfig.TerranActorInfantryAttackRange;
                    dps = GlobalConfig.TerranActorInfantryDps;
                    break;
                }
            case ActorType.Supporter:
                {
                    attackTypeStr = GlobalConfig.TerranActorSupporterAttackType;
                    attackRange = GlobalConfig.TerranActorSupporterAttackRange;
                    dps = GlobalConfig.TerranActorSupporterDps;
                    break;
                }
            case ActorType.Sniper:
                {
                    attackTypeStr = GlobalConfig.TerranActorSniperAttackType;
                    attackRange = GlobalConfig.TerranActorSniperAttackRange;
                    dps = GlobalConfig.TerranActorSniperDps;
                    break;
                }
            case ActorType.Marksman:
                {
                    attackTypeStr = GlobalConfig.TerranActorMarksmanAttackType;
                    attackRange = GlobalConfig.TerranActorMarksmanAttackRange;
                    dps = GlobalConfig.TerranActorMarksmanDps;
                    break;
                }
            case ActorType.HeavyGunner:
                {
                    attackTypeStr = GlobalConfig.TerranActorHeavyGunnerAttackType;
                    attackRange = GlobalConfig.TerranActorHeavyGunnerAttackRange;
                    dps = GlobalConfig.TerranActorHeavyGunnerDps;
                    break;
                }
            case ActorType.MortarTeam:
                {
                    attackTypeStr = GlobalConfig.TerranActorMortarTeamAttackType;
                    attackRange = GlobalConfig.TerranActorMortarTeamAttackRange;
                    dps = GlobalConfig.TerranActorMortarTeamDps;
                    break;
                }
            case ActorType.Warlock:
                {
                    attackTypeStr = GlobalConfig.TerranActorWarlockAttackType;
                    attackRange = GlobalConfig.TerranActorWarlockAttackRange;
                    dps = GlobalConfig.TerranActorWarlockDps;
                    break;
                }
            case ActorType.GryphonRider:
                {
                    attackTypeStr = GlobalConfig.TerranActorGryphonRiderAttackType;
                    attackRange = GlobalConfig.TerranActorGryphonRiderAttackRange;
                    dps = GlobalConfig.TerranActorGryphonRiderDps;
                    break;
                }
            case ActorType.SeniorGryphonRider:
                {
                    attackTypeStr = GlobalConfig.TerranActorSeniorGryphonRiderAttackType;
                    attackRange = GlobalConfig.TerranActorSeniorGryphonRiderAttackRange;
                    dps = GlobalConfig.TerranActorSeniorGryphonRiderDps;
                    break;
                }
            case ActorType.Crusader:
                {
                    attackTypeStr = GlobalConfig.TerranActorCrusaderAttackType;
                    attackRange = GlobalConfig.TerranActorCrusaderAttackRange;
                    dps = GlobalConfig.TerranActorCrusaderDps;
                    break;
                }
            case ActorType.TemplarWarrior:
                {
                    attackTypeStr = GlobalConfig.TerranActorTemplarWarriorAttackType;
                    attackRange = GlobalConfig.TerranActorTemplarWarriorAttackRange;
                    dps = GlobalConfig.TerranActorTemplarWarriorDps;
                    break;
                }
            default:
                {
                    attackTypeStr = "";
                    attackRange = 0;
                    dps = 0;
                    break;
                }
        }
        switch (attackTypeStr)
        {
            case "Normal":
                attackType = ActorAttackType.Normal;
                break;
            case "Pierce":
                attackType = ActorAttackType.Pierce;
                break;
            case "Siege":
                attackType = ActorAttackType.Siege;
                break;
            case "Confuse":
                attackType = ActorAttackType.Confuse;
                break;
            case "Magic":
                attackType = ActorAttackType.Magic;
                break;
            case "HeroAttack":
                attackType = ActorAttackType.HeroAttack;
                break;
            default:
                attackType = ActorAttackType.Normal;
                break;
        }
        return new ActorAttack(attackType, attackRange, dps);
    }

    private ActorArmor LoadArmorFromConfig()
    {
        ActorArmorType armorType;
        string armorTypeStr;
        int armorAmount;
        switch (this.ActorType)
        {
            case ActorType.Infantry:
                {
                    armorTypeStr = GlobalConfig.TerranActorInfantryArmorType;
                    armorAmount = GlobalConfig.TerranActorInfantryDef;
                    break;
                }
            case ActorType.Supporter:
                {
                    armorTypeStr = GlobalConfig.TerranActorSupporterArmorType;
                    armorAmount = GlobalConfig.TerranActorSupporterDef;
                    break;
                }
            case ActorType.Sniper:
                {
                    armorTypeStr = GlobalConfig.TerranActorSniperArmorType;
                    armorAmount = GlobalConfig.TerranActorSniperDef;
                    break;
                }
            case ActorType.Marksman:
                {
                    armorTypeStr = GlobalConfig.TerranActorMarksmanArmorType;
                    armorAmount = GlobalConfig.TerranActorMarksmanDef;
                    break;
                }
            case ActorType.HeavyGunner:
                {
                    armorTypeStr = GlobalConfig.TerranActorHeavyGunnerArmorType;
                    armorAmount = GlobalConfig.TerranActorHeavyGunnerDef;
                    break;
                }
            case ActorType.MortarTeam:
                {
                    armorTypeStr = GlobalConfig.TerranActorMortarTeamArmorType;
                    armorAmount = GlobalConfig.TerranActorMortarTeamDef;
                    break;
                }
            case ActorType.Warlock:
                {
                    armorTypeStr = GlobalConfig.TerranActorWarlockArmorType;
                    armorAmount = GlobalConfig.TerranActorWarlockDef;
                    break;
                }
            case ActorType.GryphonRider:
                {
                    armorTypeStr = GlobalConfig.TerranActorGryphonRiderArmorType;
                    armorAmount = GlobalConfig.TerranActorGryphonRiderDef;
                    break;
                }
            case ActorType.SeniorGryphonRider:
                {
                    armorTypeStr = GlobalConfig.TerranActorSeniorGryphonRiderArmorType;
                    armorAmount = GlobalConfig.TerranActorSeniorGryphonRiderDef;
                    break;
                }
            case ActorType.Crusader:
                {
                    armorTypeStr = GlobalConfig.TerranActorCrusaderArmorType;
                    armorAmount = GlobalConfig.TerranActorCrusaderDef;
                    break;
                }
            case ActorType.TemplarWarrior:
                {
                    armorTypeStr = GlobalConfig.TerranActorTemplarWarriorArmorType;
                    armorAmount = GlobalConfig.TerranActorTemplarWarriorDef;
                    break;
                }
            default:
                {
                    armorTypeStr = "";
                    armorAmount = 0;
                    break;
                }
        }
        switch (armorTypeStr)
        {
            case "LightArmor":
                armorType = ActorArmorType.LightArmor;
                break;
            case "MediumArmor":
                armorType = ActorArmorType.MediumArmor;
                break;
            case "HeavyArmor":
                armorType = ActorArmorType.HeavyArmor;
                break;
            default:
                armorType = ActorArmorType.None;
                break;
        }
        return new ActorArmor(armorType, armorAmount);
    }

    private float LoadHpFromConfig()
    {
        switch (this.ActorType)
        {
            case ActorType.Infantry:
                {
                    return GlobalConfig.TerranActorInfantryHp;
                }
            case ActorType.Supporter:
                {
                    return GlobalConfig.TerranActorSupporterHp;
                }
            case ActorType.Sniper:
                {
                    return GlobalConfig.TerranActorSniperHp;
                }
            case ActorType.Marksman:
                {
                    return GlobalConfig.TerranActorMarksmanHp;
                }
            case ActorType.HeavyGunner:
                {
                    return GlobalConfig.TerranActorHeavyGunnerHp;
                }
            case ActorType.MortarTeam:
                {
                    return GlobalConfig.TerranActorMortarTeamHp;
                }
            case ActorType.Warlock:
                {
                    return GlobalConfig.TerranActorWarlockHp;
                }
            case ActorType.GryphonRider:
                {
                    return GlobalConfig.TerranActorGryphonRiderHp;
                }
            case ActorType.SeniorGryphonRider:
                {
                    return GlobalConfig.TerranActorSeniorGryphonRiderHp;
                }
            case ActorType.Crusader:
                {
                    return GlobalConfig.TerranActorCrusaderHp;
                }
            case ActorType.TemplarWarrior:
                {
                    return GlobalConfig.TerranActorTemplarWarriorHp;
                }
            default:
                {
                    return 0;
                }
        }
    }

    private Dictionary<ActorSpellType, ActorSpell> LoadSpellsFromConfig()
    {
        Dictionary<ActorSpellType, ActorSpell> dictionary = new Dictionary<ActorSpellType, ActorSpell>();
        string spellStr;
        switch (this.ActorType)
        {
            case ActorType.Infantry:
                {
                    spellStr = GlobalConfig.TerranActorInfantrySpell;
                    break;
                }
            case ActorType.Supporter:
                {
                    spellStr = GlobalConfig.TerranActorSupporterSpell;
                    break;
                }
            case ActorType.Sniper:
                {
                    spellStr = GlobalConfig.TerranActorSniperSpell;
                    break;
                }
            case ActorType.Marksman:
                {
                    spellStr = GlobalConfig.TerranActorMarksmanSpell;
                    break;
                }
            case ActorType.HeavyGunner:
                {
                    spellStr = GlobalConfig.TerranActorHeavyGunnerSpell;
                    break;
                }
            case ActorType.MortarTeam:
                {
                    spellStr = GlobalConfig.TerranActorMortarTeamSpell;
                    break;
                }
            case ActorType.Warlock:
                {
                    spellStr = GlobalConfig.TerranActorWarlockSpell;
                    break;
                }
            case ActorType.GryphonRider:
                {
                    spellStr = GlobalConfig.TerranActorGryphonRiderSpell;
                    break;
                }
            case ActorType.SeniorGryphonRider:
                {
                    spellStr = GlobalConfig.TerranActorSeniorGryphonRiderSpell;
                    break;
                }
            case ActorType.Crusader:
                {
                    spellStr = GlobalConfig.TerranActorCrusaderSpell;
                    break;
                }
            case ActorType.TemplarWarrior:
                {
                    spellStr = GlobalConfig.TerranActorTemplarWarriorSpell;
                    break;
                }
            default:
                {
                    spellStr = string.Empty;
                    break;
                }
        }
        string[] spellArr = spellStr.Split('/');
        int length = spellArr.Length;
        for (int i = 0; i < length; i ++)
        {
            string spell = spellArr[i];
            switch (spell)
            {
                case "None":
                    dictionary.Add(ActorSpellType.None, new ActorSpell(ActorSpellType.None));
                    break;
                case "Dodge":
                    dictionary.Add(ActorSpellType.Dodge, new ActorSpell(ActorSpellType.Dodge));
                    break;
                case "CirticalStrike":
                    dictionary.Add(ActorSpellType.CirticalStrike, new ActorSpell(ActorSpellType.CirticalStrike));
                    break;
                case "HeadShot":
                    dictionary.Add(ActorSpellType.HeadShot, new ActorSpell(ActorSpellType.HeadShot));
                    break;
                case "SplashDamage":
                    dictionary.Add(ActorSpellType.SplashDamage, new ActorSpell(ActorSpellType.SplashDamage));
                    break;
                case "MortarAttack":
                    dictionary.Add(ActorSpellType.MortarAttack, new ActorSpell(ActorSpellType.MortarAttack));
                    break;
                case "ArcaneExplosion":
                    dictionary.Add(ActorSpellType.ArcaneExplosion, new ActorSpell(ActorSpellType.ArcaneExplosion));
                    break;
                case "Bash":
                    dictionary.Add(ActorSpellType.Bash, new ActorSpell(ActorSpellType.Bash));
                    break;
                case "ChainLightning":
                    dictionary.Add(ActorSpellType.ChainLightning, new ActorSpell(ActorSpellType.ChainLightning));
                    break;
                case "DivineBlessing":
                    dictionary.Add(ActorSpellType.DivineBlessing, new ActorSpell(ActorSpellType.DivineBlessing));
                    break;
                case "Zap":
                    dictionary.Add(ActorSpellType.Zap, new ActorSpell(ActorSpellType.Zap));
                    break;
                default:
                    dictionary.Add(ActorSpellType.None, new ActorSpell(ActorSpellType.None));
                    break;
            }
        }
        return dictionary;
    }

    #endregion
}
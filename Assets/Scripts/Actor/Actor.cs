#region

using System.Collections.Generic;

using UnityEngine;

#endregion

public class Actor
{
    #region Constructors and Destructors

    public Actor(ActorType type, FactionType faction)
    {
        this.ActorType = type;
        switch (type)
        {
            case ActorType.INFANTRY:
                
        }
    }

    #endregion

    #region Public Properties

    public ActorArmor ActorArmor { get; set; }

    public ActorAttack ActorAttack { get; set; }

    public ActorType ActorType { get; set; }

    public FactionType FactionType { get; set; }

    public int Hp { get; set; }

    public Dictionary<ActorSpellType, ActorSpell> SpellDictionary { get; set; }

    #endregion
}
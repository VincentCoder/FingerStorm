public class ActorArmor
{
    #region Constructors and Destructors

    public ActorArmor(ActorArmorType armorType, int armorAmount)
    {
        this.ActorArmorType = armorType;
        this.ArmorAmount = armorAmount;
    }

    #endregion

    #region Public Properties

    public ActorArmorType ActorArmorType { get; set; }

    public int ArmorAmount { get; set; }

    #endregion
}
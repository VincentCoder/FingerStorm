
public class Building
{
    /// <summary>
    /// Gets or sets the building id.
    /// </summary>
    public int BuildingId { get; set; }

    /// <summary>
    /// Gets or sets the is base.
    /// </summary>
    public bool IsBase { get; set; }

    /// <summary>
    /// Gets or sets the race.
    /// </summary>
    public RaceType Race { get; set; }

    /// <summary>
    /// Gets or sets the building type.
    /// </summary>
    public BuildingType BuildingType { get; set; }

    /// <summary>
    /// Gets or sets the actor type.
    /// </summary>
    public ActorType ActorType { get; set; }

    /// <summary>
    /// Gets or sets the faction type.
    /// </summary>
    public FactionType FactionType { get; set; }
}

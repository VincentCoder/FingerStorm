
using UnityEngine;

/// <summary>
/// The game scene controller.
/// </summary>
public class GameSceneController : MonoBehaviour
{
    /// <summary>
    /// The battle field map.
    /// </summary>
    private GameObject battleFieldMap;

    /// <summary>
    /// The awake.
    /// </summary>
    private void Awake()
    {
        this.CreateBattleFieldMap();
    }

    /// <summary>
    /// The create battle field map.
    /// </summary>
    private void CreateBattleFieldMap()
    {
        this.battleFieldMap = (GameObject)Instantiate(Resources.Load("Map/BattleFieldMap"));
        this.battleFieldMap.name = "BattleFieldMap";
        this.battleFieldMap.transform.parent = GameObject.Find("tkAnchor").transform;
        this.battleFieldMap.transform.localScale = new Vector3(1, 1, 1);
        this.battleFieldMap.transform.localPosition = new Vector3(0, 0, 0);
    }
}

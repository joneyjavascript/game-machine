using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxstudio/GameMachine/Level")]
[System.Serializable]
public class Level : ScriptableObject
{
    public string levelTitle = "All is Awesome";
    public List<LevelMachinePieceConfig> machinePieces = new List<LevelMachinePieceConfig>();
}

[System.Serializable]
public class LevelMachinePieceConfig
{
    public MachinePieceType type;
    public float initPerformancePercent = 100f;
    public float initTemperature = 20f;
    public float wearDownDecrement = 20f;
    public float autoRecoveryIncrement = 0f;
}

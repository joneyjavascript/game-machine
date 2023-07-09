using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxstudio/GameMachine/PercentRanges")]
[System.Serializable]
public class PercentRangeData : ScriptableObject
{
    public static Color DEFAULT_COLOR = Color.white;
    public List<PercentRangeDataItem> items = new List<PercentRangeDataItem>();

    public Color GetColor(float percent)
    {
        foreach (PercentRangeDataItem item in items)
        {
            float floorPercent = Mathf.Floor(percent);
            if (floorPercent > item.start && floorPercent <= item.end)
            {
                return item.color;
            }
        }

        return PercentRangeData.DEFAULT_COLOR;
    }

    public PercentRangeDataItem GetPercentRangeDataItem(float percent)
    {
        foreach (PercentRangeDataItem item in items)
        {
            float floorPercent = Mathf.Clamp(Mathf.Floor(percent), 0, 100);
            if (floorPercent >= item.start && floorPercent <= item.end)
            {
                return item;
            }
        }

        return null;
    }
}

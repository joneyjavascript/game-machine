using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PercentRangeDataItem
{
    public string title = "Good";
    public float start = 0;
    public float end = 100;
    public Color color = Color.blue;
    public List<PercentRangeDataItemSideEffect> sideEffects =
        new List<PercentRangeDataItemSideEffect>();
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class Bar : MonoBehaviour
{
    public Sprite iconSprite;
    public float percent = 0;
    public float maxHeight = 100;
    public string labelSuffix = "°C";
    public PercentRangeData percentRanges;

    private Image fillImage;
    private RectTransform fillRectTransform;
    private Icon icon;
    private TextMeshProUGUI label;

    void Start()
    {
        GameObject goFill = transform.Find("Fill").gameObject;
        if (goFill != null)
        {
            fillImage = goFill.GetComponent<Image>();
            fillRectTransform = goFill.GetComponent<RectTransform>();
        }

        GameObject goIcon = transform.Find("Icon").gameObject;
        if (goIcon != null)
        {
            icon = goIcon.GetComponent<Icon>();
        }

        GameObject goLabel = transform.Find("Label").gameObject;
        if (goLabel != null)
        {
            label = goLabel.GetComponent<TextMeshProUGUI>();
        }
    }

    void OnValidate()
    {
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    void ConstraintValues()
    {
        percent = Mathf.Clamp(percent, 0, 100);
    }

    void Refresh()
    {
        ConstraintValues();

        if (
            fillImage == null
            || iconSprite == null
            || fillRectTransform == null
            || icon == null
            || label == null
        )
        {
            return;
        }

        fillImage.color = percentRanges.GetColor(percent);
        fillRectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            GetHeightByPercent(percent)
        );
        icon.iconSprite = iconSprite;
        label.text = Math.Floor(percent) + labelSuffix;
    }

    float GetHeightByPercent(float percent)
    {
        return Mathf.Lerp(0, maxHeight, percent / 100f);
    }
}

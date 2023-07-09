using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class MachinePiece : MonoBehaviour
{
    public MachinePieceType type = MachinePieceType.None;
    public Vector3 initLocalPosition;
    public Sprite iconSprite;
    public string labelText;
    public float performancePercent = 100;
    public float temperaturePercent = 100;
    public float workIncrement = 15f;
    public float workIncrementVelocityDecayFactor = .8f;
    public float workIncrementVelocity = 0f;
    public float wearDownDecrement = 10f;
    public float autoRecoveryIncrement = 0f;

    private Icon mainIcon;
    private Bar performanceBar;
    private Bar temperatureBar;
    private TextMeshProUGUI label;
    private Button workButton;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initLocalPosition = new Vector3(-5, 95, 0);
        workButton = GetInChild<Button>("WorkButton");
        workButton.onClick.AddListener(() =>
        {
            Work();
        });

        GameObject goMainIcon = transform.Find("MainIcon").gameObject;
        if (goMainIcon != null)
        {
            mainIcon = goMainIcon.GetComponent<Icon>();
        }

        GameObject goLabel = transform.Find("Label").gameObject;
        if (goLabel != null)
        {
            label = goLabel.GetComponent<TextMeshProUGUI>();
        }

        GameObject goPerformanceBar = transform.Find("PerformanceBar").gameObject;
        if (goPerformanceBar != null)
        {
            performanceBar = goPerformanceBar.GetComponent<Bar>();
        }

        GameObject goTemperatureBar = transform.Find("TemperatureBar").gameObject;
        if (goTemperatureBar != null)
        {
            temperatureBar = goTemperatureBar.GetComponent<Bar>();
        }
    }

    void Work()
    {
        GameManager.instance.PlaySoundEffect("Work");
        workIncrementVelocity += workIncrement;
    }

    void OnValidate()
    {
        Refresh();
    }

    void Update()
    {
        Refresh();
        if (
            Application.isPlaying
            && LevelManager.instance != null
            && !LevelManager.instance.GameIsPaused()
        )
        {
            AutoRecovery();
            WearDown();
        }

        ConstraintValues();
    }

    public PercentRangeDataItem GetCurrentPerformanceRange()
    {
        if (performanceBar == null)
        {
            Debug.Log("performanceBar is null");
            return null;
        }

        PercentRangeDataItem item = performanceBar.percentRanges.GetPercentRangeDataItem(
            performancePercent
        );

        return item;
    }

    public PercentRangeDataItem GetCurrentTemperatureRange()
    {
        if (temperatureBar == null)
        {
            Debug.Log("temperatureBar is null");
            return null;
        }

        PercentRangeDataItem item = temperatureBar.percentRanges.GetPercentRangeDataItem(
            temperaturePercent
        );

        return item;
    }

    void ConstraintValues()
    {
        performancePercent = Mathf.Clamp(performancePercent, 0, 100);
        temperaturePercent = Mathf.Clamp(temperaturePercent, 0, 100);
    }

    void AutoRecovery()
    {
        performancePercent += autoRecoveryIncrement * Time.deltaTime;
    }

    void WearDown()
    {
        performancePercent -= wearDownDecrement * Time.deltaTime;
    }

    void Refresh()
    {
        if (mainIcon == null || performanceBar == null || temperatureBar == null || label == null)
        {
            return;
        }

        workIncrementVelocity *= workIncrementVelocityDecayFactor;
        performancePercent += workIncrementVelocity;

        performanceBar.percent = performancePercent;
        temperatureBar.percent = temperaturePercent;
        mainIcon.iconSprite = iconSprite;
        label.text = labelText;
    }

    T GetInChild<T>(string childName)
    {
        // Debug.Log("Get In Child: " + childName);
        GameObject go = transform.Find(childName).gameObject;
        return go.GetComponent<T>();
    }
}

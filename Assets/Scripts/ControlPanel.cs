using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class ControlPanel : MonoBehaviour
{
    public MachinePiece memory;
    public MachinePiece video;
    public MachinePiece processor;
    public MachinePiece cooler;

    public Menu menu;
    private Image background;
    private TextMeshProUGUI frameRate;

    void Start()
    {
        memory = GetInChild<MachinePiece>("Memory");
        video = GetInChild<MachinePiece>("Video");
        processor = GetInChild<MachinePiece>("Processor");
        cooler = GetInChild<MachinePiece>("Cooler");
        menu = GetInChild<Menu>("ActionIcons");
        background = GetComponent<Image>();
        frameRate = GetInChild<TextMeshProUGUI>("FrameRate");

        if (Application.isPlaying)
        {
            HideAll();
            AddListeners();
        }
    }

    public void Show(MachinePiece piece)
    {
        HideAll();
        ShowPiece(piece);
    }

    void HideAll()
    {
        HidePiece(memory);
        HidePiece(video);
        HidePiece(processor);
        HidePiece(cooler);
        background.enabled = false;
    }

    void ShowPiece(MachinePiece piece)
    {
        RectTransform rectTransform = piece.gameObject.GetComponent<RectTransform>();
        rectTransform.SetLocalPositionAndRotation(piece.initLocalPosition, Quaternion.identity);
        background.enabled = true;
    }

    void HidePiece(MachinePiece piece)
    {
        Vector3 hidedPosition = new Vector3(
            500,
            piece.initLocalPosition.y,
            piece.initLocalPosition.z
        );
        RectTransform rectTransform = piece.gameObject.GetComponent<RectTransform>();
        rectTransform.SetLocalPositionAndRotation(hidedPosition, Quaternion.identity);
    }

    void AddListeners()
    {
        if (!Already())
        {
            return;
        }

        menu.onChange.AddListener(
            (ChangeMachinePieceEvent eventData) =>
            {
                if (eventData.type == MachinePieceType.None)
                {
                    HideAll();
                    return;
                }

                MachinePiece piece = GetPieceFromType(eventData.type);
                Show(piece);
            }
        );
    }

    public float CalculateFrameRate()
    {
        float totalPontos = 0;

        foreach (MachinePiece piece in GetAllMachinePieces())
        {
            totalPontos += piece.performancePercent + (100 - piece.temperaturePercent);
        }

        float generalPercent = totalPontos / 8f / 100f;
        return Mathf.Floor(Mathf.Lerp(0, 120, generalPercent));
    }

    public bool IsCritical()
    {
        return CalculateFrameRate() <= 10 && TemperatureIsCritial();
    }

    public bool IsWarning()
    {
        return CalculateFrameRate() <= 90 && PerformancePoints() < 350;
    }

    public float PerformancePoints()
    {
        float totalPontos = 0;

        foreach (MachinePiece piece in GetAllMachinePieces())
        {
            totalPontos += piece.performancePercent;
        }

        return totalPontos;
    }

    public float TemperaturePoints()
    {
        float totalPontos = 0;

        foreach (MachinePiece piece in GetAllMachinePieces())
        {
            totalPontos += piece.temperaturePercent;
        }

        return totalPontos;
    }

    public bool TemperatureIsCritial()
    {
        return TemperaturePoints() > (300);
    }

    MachinePiece GetPieceFromType(MachinePieceType type)
    {
        switch (type)
        {
            case MachinePieceType.Cooler:
                return cooler;
            case MachinePieceType.Memory:
                return memory;
            case MachinePieceType.Processor:
                return processor;
            case MachinePieceType.Video:
                return video;
        }

        return null;
    }

    void Refresh()
    {
        if (
            !Application.isPlaying
            || !Already()
            || LevelManager.instance == null
            || LevelManager.instance.GameIsPaused()
        )
        {
            return;
        }

        //Debug.LogWarning("CheckSideEffectsForCooler");

        foreach (MachinePiece piece in GetAllMachinePieces())
        {
            CheckSideEffects(piece.type);
            Icon icon = menu.GetIconByMachinePieceType(piece.type);
            PercentRangeDataItem performanceItem = piece.GetCurrentPerformanceRange();
            icon.SetColor(performanceItem.color);
        }

        Color color = IsWarning() ? Color.yellow : Color.white;
        menu.expandAndContractIcon.SetColor(color);

        frameRate.text = CalculateFrameRate() + " FPS";
    }

    void CheckSideEffects(MachinePieceType type)
    {
        MachinePiece piece = GetPieceFromType(type);

        PercentRangeDataItem performanceItem = piece.GetCurrentPerformanceRange();
        if (performanceItem == null)
        {
            Debug.Log("Piece " + piece.type + " Has no PercentRangeDataItem");
            return;
        }

        PercentRangeDataItem temperatureItem = piece.GetCurrentTemperatureRange();
        if (temperatureItem == null)
        {
            Debug.Log("Piece " + piece.type + " Has no PercentRangeDataItem");
            return;
        }

        List<PercentRangeDataItemSideEffect> allSideEffects =
            new List<PercentRangeDataItemSideEffect>();
        allSideEffects.AddRange(performanceItem.sideEffects);
        allSideEffects.AddRange(temperatureItem.sideEffects);

        List<PercentRangeDataItemSideEffect> sideEffects = allSideEffects.FindAll(sideEffect =>
        {
            return sideEffect.type == MachinePieceType.All || sideEffect.type == piece.type;
        });

        if (sideEffects.Count <= 0)
        {
            //Debug.Log("Cooler Has no SideEffects for value: " + cooler.performancePercent);
            return;
        }

        foreach (PercentRangeDataItemSideEffect sideEffect in sideEffects)
        {
            //Debug.Log(piece.type + "  Side Effect: " + sideEffect.sideEffect);

            if (sideEffect.sideEffect == SideEffects.WarmAllOthers)
            {
                List<MachinePiece> others = GetOtherMachinePieces(piece.type);
                foreach (MachinePiece other in others)
                {
                    other.temperaturePercent += sideEffect.value * Time.deltaTime;
                }
            }

            if (sideEffect.sideEffect == SideEffects.freshAllOther)
            {
                List<MachinePiece> others = GetOtherMachinePieces(piece.type);
                foreach (MachinePiece other in others)
                {
                    other.temperaturePercent -= sideEffect.value * Time.deltaTime;
                }
            }

            if (sideEffect.sideEffect == SideEffects.selfPerformanceUp)
            {
                piece.performancePercent += sideEffect.value * Time.deltaTime;
            }

            if (sideEffect.sideEffect == SideEffects.selfPerformanceDown)
            {
                piece.performancePercent -= sideEffect.value * Time.deltaTime;
            }
        }
    }

    List<MachinePiece> GetOtherMachinePieces(MachinePieceType type)
    {
        List<MachinePiece> all = GetAllMachinePieces();
        List<MachinePiece> list = new List<MachinePiece>();
        foreach (MachinePiece item in all)
        {
            if (item.type != type)
            {
                list.Add(item);
            }
        }
        return list;
    }

    List<MachinePiece> GetAllMachinePieces()
    {
        List<MachinePiece> list = new List<MachinePiece>();
        list.Add(memory);
        list.Add(video);
        list.Add(processor);
        list.Add(cooler);
        return list;
    }

    bool Already()
    {
        return hasAll(new Object[4] { memory, video, processor, cooler });
    }

    T GetInChild<T>(string childName)
    {
        //Debug.Log("Get In Child: " + childName);
        GameObject go = transform.Find(childName).gameObject;
        return go.GetComponent<T>();
    }

    bool hasAll(Object[] objs)
    {
        foreach (Object obj in objs)
        {
            if (obj == null)
            {
                return false;
            }
        }

        return true;
    }

    void OnValidate()
    {
        Refresh();
    }

    void Update()
    {
        Refresh();
    }
}

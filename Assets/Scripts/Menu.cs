using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Sprite expandIconSprite;
    public Sprite contractIconSprite;

    public UnityEvent<ChangeMachinePieceEvent> onChange = new UnityEvent<ChangeMachinePieceEvent>();

    private bool expanded = true;
    public Icon expandAndContractIcon;
    private Icon firstIcon;
    private Icon secondIcon;
    private Icon thirdIcon;
    private Icon fourthIcon;
    private MachinePieceType lastMachinePieceType = MachinePieceType.None;
    private Image background;

    void Start()
    {
        expandAndContractIcon = GetInChild<Icon>("ExpandAndContractIcon");
        firstIcon = GetInChild<Icon>("FirstIcon");
        secondIcon = GetInChild<Icon>("SecondIcon");
        thirdIcon = GetInChild<Icon>("ThirdIcon");
        fourthIcon = GetInChild<Icon>("FourthIcon");
        background = GetComponent<Image>();

        ToggleMenu();
        AddListeners();
    }

    void AddListeners()
    {
        if (!Already())
        {
            return;
        }

        expandAndContractIcon.onClick.AddListener(() =>
        {
            GameManager.instance.PlaySoundEffect("Expand");
            ToggleMenu();
        });

        firstIcon.onClick.AddListener(() =>
        {
            GameManager.instance.PlaySoundEffect("MachinePartClick");
            DispatchChangeEvent(MachinePieceType.Cooler);
        });

        secondIcon.onClick.AddListener(() =>
        {
            GameManager.instance.PlaySoundEffect("MachinePartClick");
            DispatchChangeEvent(MachinePieceType.Memory);
        });

        thirdIcon.onClick.AddListener(() =>
        {
            GameManager.instance.PlaySoundEffect("MachinePartClick");
            DispatchChangeEvent(MachinePieceType.Video);
        });

        fourthIcon.onClick.AddListener(() =>
        {
            GameManager.instance.PlaySoundEffect("MachinePartClick");
            DispatchChangeEvent(MachinePieceType.Processor);
        });
    }

    public Icon GetIconByMachinePieceType(MachinePieceType type)
    {
        switch (type)
        {
            case MachinePieceType.Cooler:
                return firstIcon;
            case MachinePieceType.Memory:
                return secondIcon;
            case MachinePieceType.Video:
                return thirdIcon;
            case MachinePieceType.Processor:
                return fourthIcon;
        }

        return null;
    }

    void DispatchChangeEvent(MachinePieceType type)
    {
        background.enabled = true;
        if (type == MachinePieceType.None)
        {
            background.enabled = false;
        }

        lastMachinePieceType = type;
        ChangeMachinePieceEvent eventData = new ChangeMachinePieceEvent(type);
        // Debug.Log("eventData: " + eventData);
        onChange.Invoke(eventData);
    }

    void Refresh()
    {
        if (!Already())
        {
            return;
        }
    }

    bool Already()
    {
        return hasAll(
            new Object[5] { expandAndContractIcon, firstIcon, secondIcon, thirdIcon, fourthIcon }
        );
    }

    public void ToggleMenu()
    {
        if (expanded)
        {
            ContractMenu();
            expanded = false;
            DispatchChangeEvent(MachinePieceType.None);
            return;
        }

        ExpandMenu();
        expanded = true;
        DispatchChangeEvent(lastMachinePieceType);
    }

    public void ContractMenu()
    {
        expandAndContractIcon.iconSprite = expandIconSprite;

        firstIcon.gameObject.SetActive(false);
        secondIcon.gameObject.SetActive(false);
        thirdIcon.gameObject.SetActive(false);
        fourthIcon.gameObject.SetActive(false);
    }

    public void ExpandMenu()
    {
        expandAndContractIcon.iconSprite = contractIconSprite;
        firstIcon.gameObject.SetActive(true);
        secondIcon.gameObject.SetActive(true);
        thirdIcon.gameObject.SetActive(true);
        fourthIcon.gameObject.SetActive(true);
    }

    T GetInChild<T>(string childName)
    {
        // Debug.Log("Get In Child: " + childName);
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

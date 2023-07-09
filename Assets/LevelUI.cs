using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    private TextMeshProUGUI label;
    private TextMeshProUGUI title;
    private TextMeshProUGUI callToAction;

    void Awake()
    {
        label = GetInChild<TextMeshProUGUI>("Label");
        title = GetInChild<TextMeshProUGUI>("Title");
        callToAction = GetInChild<TextMeshProUGUI>("CallToAction");
    }

    // Start is called before the first frame update
    void Start()
    {
        label = GetInChild<TextMeshProUGUI>("Label");
        title = GetInChild<TextMeshProUGUI>("Title");
        callToAction = GetInChild<TextMeshProUGUI>("CallToAction");
    }

    // Update is called once per frame
    void Update() { }

    public void SetLabel(string newLabel)
    {
        label.text = newLabel;
    }

    public void SetTitle(string newTitle)
    {
        title.text = newTitle;
    }

    public void SetCallToAction(string callToActionText)
    {
        callToAction.text = callToActionText;
    }

    T GetInChild<T>(string childName)
    {
        //Debug.Log("Get In Child: " + childName);
        GameObject go = transform.Find(childName).gameObject;
        return go.GetComponent<T>();
    }
}

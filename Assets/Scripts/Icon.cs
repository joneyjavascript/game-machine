using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class Icon : MonoBehaviour, IPointerClickHandler
{
    public Sprite iconSprite;
    private Image image;

    public KeyCode keyboardShortcut = KeyCode.None;

    public UnityEvent onClick = new UnityEvent();

    void Start()
    {
        GameObject go = transform.Find("Image").gameObject;
        if (go != null)
        {
            image = go.GetComponent<Image>();
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

    public void SetColor(Color color)
    {
        image.color = color;
    }

    void Refresh()
    {
        if (image == null || iconSprite == null)
        {
            return;
        }

        if (Input.GetKeyDown(keyboardShortcut))
        {
            this.onClick.Invoke();
        }

        image.sprite = iconSprite;
    }

    public void OnPointerClick(PointerEventData data)
    {
        this.onClick.Invoke();
    }
}

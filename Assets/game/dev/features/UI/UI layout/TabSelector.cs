using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TabGroup tabGroup;

    [HideInInspector] public Image background;

    public UnityEvent onTabSelected;

    public UnityEvent onTabDeselected;

    private void Awake()
    {
        background = GetComponent<Image>();
        tabGroup.Subscribe (this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    public void Select()
    {
        onTabSelected?.Invoke();
    }

    public void Deselect()
    {
        onTabDeselected?.Invoke();
    }
}

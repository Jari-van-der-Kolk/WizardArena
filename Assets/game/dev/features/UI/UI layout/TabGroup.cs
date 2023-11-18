using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{

    public List<TabSelector> tabButtons;
    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;
    public TabSelector selectedTab;

    public List<GameObject> objectToSwap;

    public PanelGroup panelGroup;


    // hier zou je in de problemen kunnen komen met het niet gebreuken van de StartActiveTab method
    public void StartActiveTab(int tabIndex)
    {
        if (selectedTab == null)
        {
            SetActive(tabIndex);            
        }
    }
    
    private void SetActive(TabSelector tabButton)
    {
        foreach (TabSelector t in tabButtons)
        {
            t.tabGroup.ResetTabs();
        }

        selectedTab = tabButton;
        selectedTab.tabGroup.OnTabSelected(tabButton);

        if (panelGroup != null)
        {
            panelGroup.SetPageIndex(tabButton.transform.GetSiblingIndex());
        }
    }

    public void SetActive(int siblingIndex)
    {
        foreach (var t in tabButtons)
        {
            if (t.transform.GetSiblingIndex() == siblingIndex)    
            {
                SetActive(t);
                return;
            }
        }
    }

    public void Subscribe(TabSelector button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabSelector>();
        }
        
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabSelector button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.color = tabHover;
        }
        else if(button == selectedTab)
        {
            button.background.color = tabHover;
        }

    }
    public void OnTabExit(TabSelector button)
    {
        ResetTabs();
        if (button == selectedTab)
        {
            button.background.color = tabActive;
        }
    }

    public void OnTabSelected(TabSelector button)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }
        selectedTab = button;
        
        selectedTab.Select();
        
        ResetTabs();
        button.background.color = tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectToSwap.Count; i++)
        {
            if (i == index)
            {
                objectToSwap[i].SetActive(true);
            }
            else
            {
                if(!objectToSwap[i].gameObject.activeInHierarchy) continue;
                objectToSwap[i].SetActive(false);
            }
        }
    }
    public void ResetTabs()
    {
        foreach (TabSelector button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab) continue;
            button.background.color = tabIdle;
        }
    }
    
}

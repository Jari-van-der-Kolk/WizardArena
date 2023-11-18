using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField] private bool _dontDestroyOnLoad;

    [Space]
    [SerializeField] private List<MenuData> menus;
    private Dictionary<string, MenuData> menusDict;
    private List<MenuData> _selectedMenus = new List<MenuData>();


    #region singleton

    private static UserInterfaceManager instance;
    public static UserInterfaceManager Instance
    {
        get
        {
            return instance;
        }

        private set
        {
            if (instance)
            {
                Destroy(value);
                Debug.LogError("We have more than one UImanager!!!");
                return;
            }

        }
    }


    private void Awake()
    {
        instance = this;
    }

    #endregion

    #region standard unity functions

    private void Start()
    {
        AddLowestPriority();
        DeactivateAll();
        ActivateMenusOnStart();
        if (_dontDestroyOnLoad) DontDestroyOnLoad(this);
    }

    private void Update()
    {
        foreach (var m in menus)
        {
            if (Input.GetKeyDown(m.button))
            {
                HandleMenu(m);
            }
        }
    }

    #endregion

    #region UserInterface manager functions
    private void HandleMenu(MenuData menuData)
    {
        //hier check je of de node dat is aangedrukt al actief is of niet.
        if (menuData.activated == true)
        {
            //checked of er een menuNode in selectedNodes niet al op stopActivity staat.
            //checked of de menuNode waar op gedrukt is niet zelf op stopActivity staat.
            if (HoldDeactivation() == false || menuData.holdActivity)
            {
                //als dit zo is dan zet hij hem op inactive
                Deactivate(menuData);
            }
        }
        else if (menuData.activated == false && menuData.closeAllLowPriorityMenuActivity)
        {
            CloseAllLowerPriorityMenus(menuData);

            ActivateMenu(menuData);

        }
        else if (menuData.activated == false)
        {
            //als de aangedrukte node niet actief is dan zet je hem op active met deze functie.
            ActivateMenu(menuData);
        }
      
    }

    public void HandleSpecificMenu(string ID)
    {
         var menuData = GetLocalMenu(ID);

        //hier check je of de node dat is aangedrukt al actief is of niet.
        if (menuData.activated == true)
        {
            //checked of er een menuNode in selectedNodes niet al op stopActivity staat.
            //checked of de menuNode waar op gedrukt is niet zelf op stopActivity staat.
            if (HoldDeactivation() == false || menuData.holdActivity)
            {
                //als dit zo is dan zet hij hem op inactive
                Deactivate(menuData);
            }
        }
        else if (menuData.activated == false && menuData.closeAllLowPriorityMenuActivity)
        {
            CloseAllLowerPriorityMenus(menuData);

            ActivateMenu(menuData);

        }
        else if (menuData.activated == false)
        {
            //als de aangedrukte node niet actief is dan zet je hem op active met deze functie.
            ActivateMenu(menuData);
        }

    }

    private bool HoldDeactivation()
    {
        //hij gaat door alles heen dat in de selectedMenus staat
        foreach (var s in _selectedMenus)
        {
            //als er een van de selectedMenus een boolean heeft waar de stopActivity true op staat
            //zet hij de functie StopActivity op true
            if (s.holdActivity)
            {
                return true;
            }
        }
        return false;
    }

    private void CloseAllLowerPriorityMenus(MenuData menuData)
    {
        var getLowerPrio = GetLowerPriorityMenus(menuData);
        foreach (var l in getLowerPrio)
        {
            if(l.stopActivity)
                Deactivate(l);
        }

    }

    private bool CheckForHigherPriority(MenuData menuData)
    {
        for (int i = 0; i < _selectedMenus.Count; i++)
        {
            if (menuData.panelOrder < _selectedMenus[i].panelOrder)
            {
                return true;
            }
        }
        return false;
    }

    private List<MenuData> GetLowerPriorityMenus(MenuData menuData)
    {
        var menus = new List<MenuData>();
        for (int i = 0; i < _selectedMenus.Count; i++)
        {
            if (menuData.panelOrder > _selectedMenus[i].panelOrder)
            {
                menus.Add(_selectedMenus[i]);
            }
        }
        return menus;
    }


    private void ActivateMenu(MenuData menuData)
    {
        if (CheckForHigherPriority(menuData) == false && _selectedMenus.Contains(menuData) == false)
        {
            menuData.Activate();
            _selectedMenus.Add(menuData);
        }
    }

    private void Deactivate(MenuData menuData)
    {
        _selectedMenus.Remove(menuData);
        menuData.Deactivate();    
    }

    public void DeactivateAll()
    {
        //pas op in welke volgorde je de UnityEvents gebruikt. 
        foreach (var m in menus)
        {
            Deactivate(m);
        }
    }

    public void ActivateSpecific(string ID)
    {
        var data = GetLocalMenu(ID);
        if (GetLocalMenu(ID) == null)
            return;

        ActivateMenu(data);
    }

    public void ActivateMenusOnStart()
    {
        foreach (var m in menus)
        {
            if (m.activateOnStart)
            {
                ActivateMenu(m);
            }
        }
    }

    public MenuData GetLocalMenu(string ID)
    {
        MenuData menu = menus.FirstOrDefault(m => m.ID == ID);

        if (menu == null)
        {
            Debug.LogWarning("Manager could not find given ID for menuData search: " + ID);
        }

        return menu;
    }

    public void ChangeTimeScale(float timeScaleSpeed)
    {
        Time.timeScale = timeScaleSpeed;
    }

    private void AddLowestPriority()
    {
        MenuData lowestPriority = new MenuData();
        lowestPriority.ID = "PlaceHolder";
        lowestPriority.stopActivity = false;
        lowestPriority.panelOrder = int.MinValue;
        _selectedMenus.Add(lowestPriority);
    }

    public void ToggleObjectActivityState(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }

    public void NextScene(string name) => UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    public void QuitApplication() => Application.Quit();



   
    #endregion

    #region Observer methods



    #endregion
}

[System.Serializable]
public class MenuData
{
    public string ID;

    public KeyCode button;
    public GameObject panel;
    public int panelOrder;
    [Tooltip("The activated menu will close all other lower priority menus with a activated stopActivity")]
    public bool closeAllLowPriorityMenuActivity; 
    [Tooltip("When another panel is open with higher priority the called panel wont open")]
    public bool holdActivity;
    [Tooltip("The activated menu will close all other lower priority menus")]
    public bool stopActivity;
    [HideInInspector] public bool activated;
    public bool activateOnStart;

    public UnityEvent activateEvent;
    public UnityEvent deactivateEvent;

    public void Activate()
    {
        activateEvent?.Invoke();
        panel.SetActive(true);
        activated = true;
    }

    public void Deactivate()
    {
        deactivateEvent?.Invoke();
        panel.SetActive(false);
        activated = false;
    }
}
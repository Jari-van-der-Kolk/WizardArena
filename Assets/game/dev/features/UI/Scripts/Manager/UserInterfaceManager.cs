using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField] private bool _dontDestroyOnLoad;

    [Space]
    [SerializeField] private List<MenuData> menus;
    [SerializeField] private List<MenuData> _selectedMenus;
    private MenuData _lowestPriority;


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

    #region unity_functions

    private void Start()
    {
        AddLowestPriority();
        DeactivateAll();
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

    #region UIMenuHandler
    private void HandleMenu(MenuData menuNode)
    {
        //hier check je of de node dat is aangedrukt al actief is of niet.
        if (menuNode.activated == true)
        {
            //checked of er een menuNode in selectedNodes niet al op stopActivity staat.
            //checked of de menuNode waar op gedrukt is niet zelf op stopActivity staat.
            if (StopActivity() == false || menuNode.stopActivity)
            {
                //als dit zo is dan zet hij hem op inactive
                Deactivate(menuNode);
            }
        }
        else if (menuNode.activated == false)
        {
            //als de aangedrukte node niet actief is dan zet je hem op active met deze functie.
            ActivateMenu(menuNode);
        }
    }

    private bool StopActivity()
    {
        //hij gaat door alles heen dat in de selectedMenus staat
        foreach (var s in _selectedMenus)
        {
            //als er een van de selectedMenus een boolean heeft waar de stopActivity true op staat
            //zet hij de functie StopActivity op true
            if (s.stopActivity)
            {
                return true;
            }
        }
        return false;
    }


    private bool CheckPriority(MenuData menuNode)
    {
        for (int i = 0; i < _selectedMenus.Count; i++)
        {
            if (menuNode.priority < _selectedMenus[i].priority)
            {
                return true;
            }
        }
        return false;
    }

    private void ActivateMenu(MenuData menuNode)
    {
        if (CheckPriority(menuNode) == false)
        {
            menuNode.Activate();
            _selectedMenus.Add(menuNode);

        }
    }

    private void Deactivate(MenuData menuNode)
    {
        menuNode.Deactivate();
        _selectedMenus.Remove(menuNode);
    }

    public void DeactivateAll()
    {
        //pas op in welke volgorde je de UnityEvents gebruikt. 
        foreach (var m in menus)
        {
            Deactivate(m);
        }
    }

    public void ChangeTimeScale(float timeScaleSpeed)
    {
        Time.timeScale = timeScaleSpeed;
    }

    private void AddLowestPriority()
    {
        MenuData lowestPriority = new MenuData();
        lowestPriority.priority = int.MinValue;
        _selectedMenus.Add(lowestPriority);
    }

    public void ToggleObjectActivityState(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }

    public void NextScene(string name) => UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    public void QuitApplication() => Application.Quit();


    public MenuData GetMenuNode(string ID)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].ID == ID)
            {
                return menus[i];
            }
        }

        return null;
    }
    #endregion

}

[System.Serializable]
public class MenuData
{
    public string ID;

    public KeyCode button;
    public GameObject panel;
    public int priority;
    public bool stopActivity;
    [HideInInspector] public bool activated;

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
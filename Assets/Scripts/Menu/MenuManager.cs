using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public MenuType pCurrentMenu 
    { 
        get => currentMenu; 
    }
    private MenuType currentMenu;
    public static MenuManager pInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MenuManager>();
            }
            return instance;
        }
    }
    private static MenuManager instance;

    [SerializeField] private Menu[] menuObjects;
    private Dictionary<MenuType, Menu> menus;

    public void OpenMenu(int _menuType)
    {
        MenuType t = (MenuType)_menuType;


        if (currentMenu == t || menus[t] == null)
        {
            return;
        }
        CloseMenu();
        menus[t].Open();
        currentMenu = t;
    }
    public void OpenMenu(MenuType _menuType)
    {

        if (currentMenu == _menuType || !menus.ContainsKey(_menuType))
        {
            Debug.Log("menu is already open?");
            return;
        }
        CloseMenu();
        menus[_menuType].Open();
        currentMenu = _menuType;
    }

    public void CloseMenu()
    {

        if (!menus.ContainsKey(currentMenu))
        {
            return;
        }
        if (menus[currentMenu].isActiveAndEnabled)
        {
            menus[currentMenu].Close();
        }

    }

    public void GotoScene(int _scene) 
    { 
        SceneManager.LoadScene(_scene);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        menus = new Dictionary<MenuType, Menu>();
        foreach (Menu m in menuObjects)
        {
            menus.Add(m.menuType, m);
        }
        //OpenMenu(MenuType.Main);
    }

    
}

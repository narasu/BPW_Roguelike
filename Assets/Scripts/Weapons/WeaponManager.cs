using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { SEMI, AUTO }
public class WeaponManager : MonoBehaviour
{
    public Weapon weapon;
    //private WeaponType currentWeapon = WeaponType.AUTO;
    private int currentWeapon = 0;
    private int pCurrentWeapon
    {
        get => currentWeapon;
        set
        {
            if (value != currentWeapon)
            {
                weaponDecorators[currentWeapon].Disable();
                weapon.RemoveDecorator();

                currentWeapon = value;

                weapon.Decorate(weaponDecorators[currentWeapon]);
                weaponDecorators[currentWeapon].Init(weapon);
            }
        }
    }
    [SerializeField] private List<WeaponDecorator> weaponDecorators;


    private void Start()
    {
        for(int i = 0; i < weaponDecorators.Count; i++)
        {
            weaponDecorators[i] = Instantiate(weaponDecorators[i]);
            
        }
        weaponDecorators[1].Init(weapon);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pCurrentWeapon + 1 < weaponDecorators.Count)
            {
                pCurrentWeapon++;
            }
            else
            {
                pCurrentWeapon = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (pCurrentWeapon - 1 >= 0)
            {
                pCurrentWeapon--;
            }
            else
            {
                pCurrentWeapon = weaponDecorators.Count-1;
            }
        }
    }

}

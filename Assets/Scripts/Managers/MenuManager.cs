using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject shopMenu;
    [SerializeField] GameObject statsMenu;
    [SerializeField] GameObject tutorialMenu;
    [SerializeField] Canvas _canvas;

    public void SpawnShopMenu()
    {

    }

    public void SpawnStatsMenu()
    {
        GameObject menu = Instantiate(statsMenu, transform.position, Quaternion.identity, _canvas.transform);
        menu.transform.localPosition = Vector3.zero;
    }

    public void SpawnTutorialMenu()
    {

    }
}

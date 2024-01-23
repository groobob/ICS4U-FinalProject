/*
 * Class to manage all things related to the menu and changing views and tabs.
 * 
 * @author Richard
 * @version January 21
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject statsMenu;
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private Canvas _canvas;

    /*
     * Spawns shop menu for the game.
     */
    public void SpawnShopMenu()
    {

    }
    /*
     * Generates the stats menu
     */
    public void SpawnStatsMenu()
    {
        GameObject menu = Instantiate(statsMenu, transform.position, Quaternion.identity, _canvas.transform);
        menu.transform.localPosition = Vector3.zero;
    }
    /*
     * Generates the tutorial 
     */
    public void SpawnTutorialMenu()
    {

    }
    /*
     * Plays music for the titlescreen.
     */
    public void Start()
    {
        SoundManager.Instance.PlayMusic(0);
    }
}

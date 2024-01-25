/*
 * Class to manage an individual room within the room grid
 * 
 * @author Richard
 * @version January 5
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    // Identifier variables
    GridManager.room type;
    bool accessable;
    SpriteRenderer _sprite;
    Vector2Int index;

    /*
     * A method that sets up the room based on the given inputs from GridManager
     * 
     * @param type An enum that represents what type the room is
     * @param accessable A boolean variable for whether or not the player can access the room or not
     * @param x The x coordinate of the room within the grid
     * @param y The y coordinate of the room within the grid
     * @return void
     */
    public void Setup(GridManager.room type, bool accessable, int x, int y)
    {
        _sprite = GetComponent<SpriteRenderer>();

        this.type = type;
        this.accessable = accessable;
        index = new Vector2Int(x, y);

        if (accessable)
        {
            switch (type)
            {
                case GridManager.room.normal:
                    _sprite.color = Color.white;
                    break;
                case GridManager.room.end:
                    _sprite.color = Color.white;
                    break;
                case GridManager.room.item:
                    _sprite.color = Color.yellow;
                    break;
                case GridManager.room.money:
                    _sprite.color = Color.green;
                    break;
                case GridManager.room.challenge:
                    _sprite.color = Color.red;
                    break;
            }
        }
        else if (type == GridManager.room.explored) _sprite.color = Color.gray;
        else if (type == GridManager.room.current) _sprite.color = Color.cyan;
        else if (type == GridManager.room.empty) _sprite.color = new Color(1f, 1f, 1f, 0f);
        else _sprite.color = Color.black;
    }

    /*
     * Method that gets called whenever the user clicks on this object
     * 
     * @return void
     */
    private void OnMouseDown()
    {
        if (accessable)
        {
            if (type == GridManager.room.end)
            {
                Info.Instance.SetData(DataManager.Instance.upgradesObtained, DataManager.Instance.GetSpeedrunTimerTime());
                UpgradeManager.Instance.ResetUpgrades();
                SceneLoader.Instance.LoadEndScene();
            }
            else
            {
                GridManager.Instance.DestroyGrid();
                MapManager.Instance.GenerateMap(type);
                GridManager.Instance.Move(index.x, index.y);
            }
        }
    }

}

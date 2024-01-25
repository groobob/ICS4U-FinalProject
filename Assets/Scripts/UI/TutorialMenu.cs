/*
 * A menu for the tutorial
 * 
 * @author Richard
 * @version January 24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenu : MonoBehaviour
{
    /*
     * Closes the tutorial menu
     */
    public void Close()
    {
        Destroy(gameObject);
    }
}

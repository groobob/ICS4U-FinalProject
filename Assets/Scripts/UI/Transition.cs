/*
 * Script for transitioning the title screen.
 * 
 * @author Richard
 * @version January 23
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    // References
    [Header("References")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private List<GameObject> buttons;

    // Values
    [Header("Values")]
    [SerializeField] private float timeBetweenTitleAndButton;
    [SerializeField] private float transitionTime;
    [SerializeField] private float titleFadeInTime;
    [SerializeField] private float offset;
    [SerializeField] private float timeUntilButtonEnabled;
    [SerializeField] private Color textColour;

    private List<float> positionsY = new List<float>();
    private float timeElapsed;
    private bool titleOpaque = false;
    private bool flag = false;

    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            positionsY.Add(buttons[i].transform.localPosition.y);
        }
        foreach(GameObject text in buttons)
        {
            text.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 0f);
        }
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > titleFadeInTime && !flag)
        {
            titleOpaque = true;
            flag = true;
            timeElapsed = 0f;
        }
        else if (!titleOpaque)
        {
            titleText.color = new Color(1f, 1f, 1f, timeElapsed / titleFadeInTime);
        }
        else if (titleOpaque)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (timeElapsed > timeBetweenTitleAndButton + (offset * i))
                {
                    if (!buttons[i].GetComponent<Button>().enabled && timeElapsed > timeUntilButtonEnabled + timeBetweenTitleAndButton + (offset * i)) buttons[i].GetComponent<Button>().enabled = true;
                    if (timeElapsed < transitionTime + timeBetweenTitleAndButton + (offset * i))
                    {
                        buttons[i].GetComponent<RectTransform>().localPosition = new Vector3(0f, 200 / (timeElapsed - timeBetweenTitleAndButton - (offset * i)) + positionsY[i], 0f);
                        buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(textColour.r, textColour.g, textColour.b, timeElapsed - timeBetweenTitleAndButton - offset * i / transitionTime);
                        continue;
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    // References
    [Header("References")]
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] List<GameObject> buttons;

    // Values
    [Header("Values")]
    [SerializeField] float timeBetweenTitleAndButton;
    [SerializeField] float transitionTime;
    [SerializeField] float titleFadeInTime;
    [SerializeField] float offset;
    [SerializeField] float timeUntilButtonEnabled;

    List<float> positionsY = new List<float>();
    float timeElapsed;
    bool titleOpaque = false;
    bool flag = false;

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

    void Update()
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
                        buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, timeElapsed - timeBetweenTitleAndButton - offset * i / transitionTime);
                        continue;
                    }
                }
            }
        }
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class LevelSelection : MonoBehaviour
{
    public Transform levelButtonTemplate;
    public EventSystem eventSystem;

    private List<Transform> levelButtons = new List<Transform>();

    void Start()
    {
        RenderButtons();
    }

    private void RenderButtons()
    {

        // Create level buttons
        for (int i = 1; i <= 5; i++)
        {
            Transform button = Instantiate(levelButtonTemplate, transform);

            if (i == 1)
            {
                eventSystem.SetSelectedGameObject(button.gameObject);
            }

            button.GetComponentInChildren<TMP_Text>().text = "Level " + i;
            levelButtons.Add(button);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Disable all buttons
        foreach (var button in levelButtons)
        {
            button.gameObject.SetActive(false);
        }

        float position = levelButtonTemplate.transform.position.y;
        var selectedButton = eventSystem.currentSelectedGameObject;
        var indexSelected = levelButtons.FindIndex(button => button.gameObject == selectedButton);

        if (indexSelected >= 1)
        {
            levelButtons[indexSelected - 1].gameObject.SetActive(true);
            levelButtons[indexSelected - 1].transform.position = new Vector3(levelButtonTemplate.transform.position.x, position, levelButtonTemplate.transform.position.z);
        }

        position -= 75f;
        levelButtons[indexSelected].gameObject.SetActive(true);
        levelButtons[indexSelected].transform.position = new Vector3(levelButtonTemplate.transform.position.x, position, levelButtonTemplate.transform.position.z);

        if(indexSelected + 1 < levelButtons.Count)
        {
            position -= 75f;
            levelButtons[indexSelected + 1].gameObject.SetActive(true);
            levelButtons[indexSelected + 1].transform.position = new Vector3(levelButtonTemplate.transform.position.x, position, levelButtonTemplate.transform.position.z);
        }
    }
}

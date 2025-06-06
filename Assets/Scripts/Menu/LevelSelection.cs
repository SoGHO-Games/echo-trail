using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public Transform levelButtonTemplate;
    public EventSystem eventSystem;

    private List<Transform> levelButtons = new List<Transform>();
    private InputAction cancelAction;

    void Start()
    {
        CreateLevelButtons();
    }

    private void OnEnable()
    {
        cancelAction = InputSystem.actions.FindAction("Cancel");
        cancelAction.performed += CancelAction_Performed;
    }

    private void CancelAction_Performed(InputAction.CallbackContext context)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }

    void OnDisable()
    {
        cancelAction.performed -= CancelAction_Performed;
    }

    private void CreateLevelButtons()
    {
        Debug.Log("Creating level buttons...");
        bool alreadySelected = false;

        foreach (var levelData in GameManager.Instance.LevelDatas)
        {
            Transform button = Instantiate(levelButtonTemplate, transform);
            if (!alreadySelected)
            {
                eventSystem.SetSelectedGameObject(button.gameObject);
                alreadySelected = true;
            }
            button.GetComponentInChildren<TMP_Text>().text = levelData.GetDisplayName();
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(levelData.LevelName);
            });
            levelButtons.Add(button);
        }

        if (levelButtons.Count > GameManager.Instance.ActiveLevelIndex)
        {
            eventSystem.SetSelectedGameObject(levelButtons[GameManager.Instance.ActiveLevelIndex].gameObject);
        }
        else
        {
            eventSystem.SetSelectedGameObject(levelButtons.Last().gameObject);
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

        if (indexSelected >= 1 && GameManager.Instance.ActiveLevelIndex >= indexSelected - 1)
        {
            levelButtons[indexSelected - 1].gameObject.SetActive(true);
            levelButtons[indexSelected - 1].transform.position = new Vector3(levelButtonTemplate.transform.position.x, position, levelButtonTemplate.transform.position.z);
        }

        if(GameManager.Instance.ActiveLevelIndex >= indexSelected)
        {
            position -= 75f;
            levelButtons[indexSelected].gameObject.SetActive(true);
            levelButtons[indexSelected].transform.position = new Vector3(levelButtonTemplate.transform.position.x, position, levelButtonTemplate.transform.position.z);
        }

        if(indexSelected + 1 < levelButtons.Count && GameManager.Instance.ActiveLevelIndex >= indexSelected + 1)
        {
            position -= 75f;
            levelButtons[indexSelected + 1].gameObject.SetActive(true);
            levelButtons[indexSelected + 1].transform.position = new Vector3(levelButtonTemplate.transform.position.x, position, levelButtonTemplate.transform.position.z);
        }
    }
}

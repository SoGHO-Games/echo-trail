using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public Transform levelButtonTemplate;
    public EventSystem eventSystem;

    private List<Transform> levelButtons = new List<Transform>();

    void Start()
    {
        CreateLevelButtons();
    }

    private void CreateLevelButtons()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        bool alreadySelected = false;
        for (int i = 0; i < sceneCount; i++)
        {
            var sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            if(sceneName.StartsWith("Level "))
            {
                Transform button = Instantiate(levelButtonTemplate, transform);

                if (!alreadySelected)
                {
                    eventSystem.SetSelectedGameObject(button.gameObject);
                    alreadySelected = true;
                }
                
                button.GetComponentInChildren<TMP_Text>().text = sceneName;
                button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                });
                levelButtons.Add(button);
            };
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

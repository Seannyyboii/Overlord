using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject startPanel;
    public Button playBtn;
    public Button customizationBtn;
    public Button exitBtn;

    public GameObject customizationPanel;
    public Button backBtn;

    public string[] customizationParts;
    public Transform customizationGroupContainer;
    public GameObject customizationGroupPrefab;

    public Transform weapon;

    void Start()
    {
        playBtn.onClick.AddListener(() => SceneManager.LoadScene("FPS"));
        customizationBtn.onClick.AddListener(ShowCustomizationPanel);
        exitBtn.onClick.AddListener(() => Application.Quit());

        backBtn.onClick.AddListener(ShowStartPanel);

        foreach(string customizationPart in customizationParts)
        {
            GameObject customizationGroup = Instantiate(customizationGroupPrefab);
            customizationGroup.transform.Find("Label").GetComponent<TMP_Text>().text = customizationPart;

            bool isAttached = PlayerPrefs.GetInt(customizationPart, 0) == 0 ? false : true;

            GameObject targetMesh = weapon.Find(customizationPart).gameObject;
            targetMesh.SetActive(isAttached);

            Button button = customizationGroup.transform.Find("Button").GetComponent<Button>();
            button.GetComponentInChildren<TMP_Text>().text = isAttached ? "Detach" : "Attach";
            button.onClick.AddListener(() =>
            {
                bool wasAttached = PlayerPrefs.GetInt(customizationPart, 0) == 0 ? false : true;

                if (wasAttached)
                {
                    PlayerPrefs.SetInt(customizationPart, 0);
                    button.GetComponentInChildren<TMP_Text>().text = "Attach";
                    targetMesh.SetActive(false);
                }
                else
                {
                    PlayerPrefs.SetInt(customizationPart, 1);
                    button.GetComponentInChildren<TMP_Text>().text = "Detach";
                    targetMesh.SetActive(true);
                }
            });

            customizationGroup.transform.SetParent(customizationGroupContainer, false);
        }
    }

    void ShowCustomizationPanel()
    {
        startPanel.SetActive(false);
        customizationPanel.SetActive(true);
    }

    void ShowStartPanel()
    {
        startPanel.SetActive(true);
        customizationPanel.SetActive(false);
    }
}

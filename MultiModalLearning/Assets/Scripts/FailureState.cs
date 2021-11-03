using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FailureState : MonoBehaviour
{
    public static FailureState Instance;
    public CanvasGroup canGroup;
    public TextMeshProUGUI failureReasonText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        canGroup = GetComponent<CanvasGroup>();
        canGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    

    public void SystemFailure(string errorMessage)
    {
        Debug.Log("Failure: " + errorMessage);
        gameObject.SetActive(true);
        failureReasonText.text = errorMessage;
        canGroup.alpha = 1f;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TurnOffScreenDebug()
    {
        canGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

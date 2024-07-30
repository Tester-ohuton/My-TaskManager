// Assets/Scripts/NotificationManager.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }
    public GameObject notificationPanel;
    public Text notificationText;
    public Button closeButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        closeButton.onClick.AddListener(CloseNotification);
    }

    public void Initialize()
    {
        // Initialize the notification panel as hidden
        notificationPanel.SetActive(false);
    }

    // Send a notification for a task deadline
    public void SendTaskDeadlineNotification(string taskDescription, MyDateTime deadline)
    {
        StartCoroutine(DisplayNotification($"Task '{taskDescription}' is due on {deadline}"));
    }

    // Send a notification for project progress
    public void SendProjectProgressNotification(string projectName)
    {
        StartCoroutine(DisplayNotification($"Project '{projectName}' has been updated"));
    }

    // Coroutine to display the notification message for 5 seconds
    private IEnumerator DisplayNotification(string message)
    {
        notificationText.text = message;
        notificationPanel.SetActive(true);
        yield return new WaitForSeconds(5); // Display for 5 seconds
        notificationPanel.SetActive(false);
    }

    // Close the notification panel
    private void CloseNotification()
    {
        notificationPanel.SetActive(false);
    }
}

// Assets/Scripts/UI/NotificationUIController.cs
using UnityEngine;
using UnityEngine.UI;

public class NotificationUIController : MonoBehaviour
{
    public GameObject notificationPanel;
    public Text notificationText;
    public Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(CloseNotification);
        NotificationManager.Instance.notificationPanel = notificationPanel;
        NotificationManager.Instance.notificationText = notificationText;
        NotificationManager.Instance.closeButton = closeButton;
        NotificationManager.Instance.Initialize();
    }

    private void CloseNotification()
    {
        notificationPanel.SetActive(false);
    }
}

// Assets/Scripts/GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        // �e�}�l�[�W���[��������
        TaskManager.Instance.Initialize();
        ProjectManager.Instance.Initialize();
        NotificationManager.Instance.Initialize();
    }
}

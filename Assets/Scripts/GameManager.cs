// Assets/Scripts/GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        // 各マネージャーを初期化
        TaskManager.Instance.Initialize();
        ProjectManager.Instance.Initialize();
        NotificationManager.Instance.Initialize();
    }
}

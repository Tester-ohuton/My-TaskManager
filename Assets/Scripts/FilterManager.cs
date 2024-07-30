using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterManager : MonoBehaviour
{
    public static FilterManager Instance { get; private set; }

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
    }

    public void Initialize()
    {
        // Initialize filter related functionalities
    }

    public List<TaskModels> FilterTasksByPriority(int priority)
    {
        // Logic to filter tasks by priority
        return TaskManager.Instance.GetTasks().FindAll(t => t.Priority == priority);
    }

    public List<TaskModels> SearchTasks(string searchTerm)
    {
        // Logic to search tasks
        return TaskManager.Instance.GetTasks().FindAll(t => t.Description.Contains(searchTerm));
    }
}

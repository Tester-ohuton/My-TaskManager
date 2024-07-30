using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }
    private List<TaskModels> tasks = new List<TaskModels>();

    private string filePath;

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

        filePath = Path.Combine(Application.persistentDataPath, "tasks.json");
    }

    private void OnApplicationQuit()
    {
        SaveTasks();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveTasks();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveTasks();
        }
    }

    public void Initialize()
    {
        LoadTasks();
        StartCoroutine(AutoSave());
    }

    public void AddTask(string description, MyDateTime deadline, int priority, bool isCompleted, int projectId)
    {
        TaskModels newTask = new TaskModels(description, deadline, priority,isCompleted, projectId);
        tasks.Add(newTask);
        SaveTasks();
    }

    public void EditTask(int taskId, string newDescription, MyDateTime newDeadline, int newPriority, int newProjectId)
    {
        TaskModels task = tasks.Find(t => t.Id == taskId);
        if (task != null)
        {
            task.Description = newDescription;
            task.Deadline = newDeadline;
            task.Priority = newPriority;
            task.ProjectId = newProjectId;
            SaveTasks();
        }
    }

    public void DeleteTask(int taskId)
    {
        tasks.RemoveAll(t => t.Id == taskId);
        SaveTasks();
    }

    public List<TaskModels> GetTasks()
    {
        return tasks;
    }

    public List<TaskModels> GetTasksByProject(int projectId)
    {
        return tasks.FindAll(t => t.ProjectId == projectId);
    }

    private void SaveTasks()
    {
        string json = JsonUtility.ToJson(new TaskListWrapper { TaskModels = tasks });
        File.WriteAllText(filePath, json);
    }

    private void LoadTasks()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            TaskListWrapper taskListWrapper = JsonUtility.FromJson<TaskListWrapper>(json);
            tasks = taskListWrapper.TaskModels;
        }
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(60); // 60•b‚²‚Æ‚É•Û‘¶
            SaveTasks();
        }
    }

    [System.Serializable]
    private class TaskListWrapper
    {
        public List<TaskModels> TaskModels;
    }
}

[System.Serializable]
public class TaskModels
{
    public int nextId = 0;

    public int Id;
    public string Description;
    public MyDateTime Deadline;
    public int Priority;
    public bool IsCompleted;
    public int ProjectId;

    public TaskModels(string description, MyDateTime deadline, int priority, bool isCompleted,int projectId)
    {
        Id = nextId++;
        Description = description;
        Deadline = deadline;
        Priority = priority;
        IsCompleted = isCompleted;
        ProjectId = projectId;
    }
}

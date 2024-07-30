using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProjectManager : MonoBehaviour
{
    public static ProjectManager Instance { get; private set; }
    private List<ProjectModels> projects = new List<ProjectModels>();

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

        filePath = Path.Combine(Application.persistentDataPath, "projects.json");
    }

    // アプリケーション開始時にプロジェクトデータをロードする関数
    public void Initialize()
    {
        LoadProjects();
    }

    // プロジェクトデータを追加する関数
    public void AddProject(string name)
    {
        ProjectModels newProject = new ProjectModels(name);
        projects.Add(newProject);
        SaveProjects();
    }

    // プロジェクトデータを編集する関数
    public void EditProject(int projectId, string newName)
    {
        ProjectModels project = projects.Find(p => p.Id == projectId);
        if (project != null)
        {
            project.Name = newName;
            SaveProjects();
        }
    }

    // プロジェクトデータを削除する関数
    public void DeleteProject(int projectId)
    {
        projects.RemoveAll(p => p.Id == projectId);
        SaveProjects();
    }

    // プロジェクトデータを取得する関数
    public List<ProjectModels> GetProjects()
    {
        return projects;
    }

    // 指定した名前のプロジェクトが存在するか確認する関数
    public bool ProjectNameExists(string name)
    {
        return projects.Exists(p => p.Name == name);
    }

    // プロジェクトデータを保存する関数
    private void SaveProjects()
    {
        string json = JsonUtility.ToJson(new ProjectListWrapper { ProjectModels = projects });
        File.WriteAllText(filePath, json);
    }

    // プロジェクトデータをロードする関数
    private void LoadProjects()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ProjectListWrapper projectListWrapper = JsonUtility.FromJson<ProjectListWrapper>(json);
            projects = projectListWrapper.ProjectModels;
        }
    }

    // アプリケーション終了時にプロジェクトデータをセーブする関数
    private void OnApplicationQuit()
    {
        SaveProjects();
    }

    // アプリケーションがポーズ状態になったときにプロジェクトデータをセーブする関数
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveProjects();
        }
    }

    // アプリケーションがフォーカスを失ったときにプロジェクトデータをセーブする関数
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveProjects();
        }
    }

    [System.Serializable]
    private class ProjectListWrapper
    {
        public List<ProjectModels> ProjectModels;
    }
}

[System.Serializable]
public class ProjectModels
{
    public int nextId = 0;

    public int Id;
    public string Name;

    public ProjectModels(string name)
    {
        Id = nextId++;
        Name = name;
    }
}

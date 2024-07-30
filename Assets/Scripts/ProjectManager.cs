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

    // �A�v���P�[�V�����J�n���Ƀv���W�F�N�g�f�[�^�����[�h����֐�
    public void Initialize()
    {
        LoadProjects();
    }

    // �v���W�F�N�g�f�[�^��ǉ�����֐�
    public void AddProject(string name)
    {
        ProjectModels newProject = new ProjectModels(name);
        projects.Add(newProject);
        SaveProjects();
    }

    // �v���W�F�N�g�f�[�^��ҏW����֐�
    public void EditProject(int projectId, string newName)
    {
        ProjectModels project = projects.Find(p => p.Id == projectId);
        if (project != null)
        {
            project.Name = newName;
            SaveProjects();
        }
    }

    // �v���W�F�N�g�f�[�^���폜����֐�
    public void DeleteProject(int projectId)
    {
        projects.RemoveAll(p => p.Id == projectId);
        SaveProjects();
    }

    // �v���W�F�N�g�f�[�^���擾����֐�
    public List<ProjectModels> GetProjects()
    {
        return projects;
    }

    // �w�肵�����O�̃v���W�F�N�g�����݂��邩�m�F����֐�
    public bool ProjectNameExists(string name)
    {
        return projects.Exists(p => p.Name == name);
    }

    // �v���W�F�N�g�f�[�^��ۑ�����֐�
    private void SaveProjects()
    {
        string json = JsonUtility.ToJson(new ProjectListWrapper { ProjectModels = projects });
        File.WriteAllText(filePath, json);
    }

    // �v���W�F�N�g�f�[�^�����[�h����֐�
    private void LoadProjects()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ProjectListWrapper projectListWrapper = JsonUtility.FromJson<ProjectListWrapper>(json);
            projects = projectListWrapper.ProjectModels;
        }
    }

    // �A�v���P�[�V�����I�����Ƀv���W�F�N�g�f�[�^���Z�[�u����֐�
    private void OnApplicationQuit()
    {
        SaveProjects();
    }

    // �A�v���P�[�V�������|�[�Y��ԂɂȂ����Ƃ��Ƀv���W�F�N�g�f�[�^���Z�[�u����֐�
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveProjects();
        }
    }

    // �A�v���P�[�V�������t�H�[�J�X���������Ƃ��Ƀv���W�F�N�g�f�[�^���Z�[�u����֐�
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

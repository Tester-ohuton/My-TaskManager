using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProjectUIController : MonoBehaviour
{
    public GameObject projectCreatePanel; // �v���W�F�N�g�쐬�p�l��
    public GameObject projectEditPanel; // �v���W�F�N�g�ҏW�p�l��
    public InputField projectNameInput1;
    public InputField projectNameInput2;
    public Button addProjectButton;
    public Button editProjectButton;
    public Button deleteProjectButton; // �v���W�F�N�g�폜�{�^��
    public GameObject projectListContent;
    public GameObject projectListItemPrefab;

    public GameObject errorPanel; // �G���[���b�Z�[�W�\���p�̃p�l��
    public Text errorText; // �G���[���b�Z�[�W��\������e�L�X�g
    public Button errorCloseButton; // �G���[�p�l�������{�^��

    private ProjectModels selectedProject;

    private void Start()
    {
        addProjectButton.onClick.AddListener(OnAddProject);
        editProjectButton.onClick.AddListener(OnEditProject);
        deleteProjectButton.onClick.AddListener(OnDeleteProject); // �v���W�F�N�g�폜�{�^���̃��X�i�[
        errorCloseButton.onClick.AddListener(OnErrorClose);
        DisplayProjects(); // ���[�h�����f�[�^��UI�ɔ��f
    }

    private void ShowProjectCreatePanel()
    {
        projectCreatePanel.SetActive(true);
        projectEditPanel.SetActive(false);
    }

    private void ShowProjectEditPanel()
    {
        projectCreatePanel.SetActive(false);
        projectEditPanel.SetActive(true);
    }

    private void OnAddProject()
    {
        string name = projectNameInput1.text;
        if (string.IsNullOrEmpty(name))
        {
            ShowError("�v���W�F�N�g������͂��Ă��������B");
            return;
        }

        if (ProjectManager.Instance.ProjectNameExists(name))
        {
            ShowError("���̃v���W�F�N�g���͊��ɑ��݂��܂��B�ʂ̖��O����͂��Ă��������B");
            return;
        }

        ProjectManager.Instance.AddProject(name);
        DisplayProjects(); // �v���W�F�N�g�ǉ����UI���X�V
        ShowProjectEditPanel();
    }

    private void OnEditProject()
    {
        string newName = projectNameInput2.text;
        if (string.IsNullOrEmpty(newName))
        {
            ShowError("�v���W�F�N�g������͂��Ă��������B");
            return;
        }

        if (ProjectManager.Instance.ProjectNameExists(newName))
        {
            ShowError("���̃v���W�F�N�g���͊��ɑ��݂��܂��B�ʂ̖��O����͂��Ă��������B");
            return;
        }

        if (selectedProject == null)
        {
            ShowError("�ҏW�Ώۂ̃v���W�F�N�g���I������Ă��܂���B");
            return;
        }

        ProjectManager.Instance.EditProject(selectedProject.Id, newName);
        DisplayProjects(); // �v���W�F�N�g�ҏW���UI���X�V
    }

    private void OnDeleteProject()
    {
        if (selectedProject == null)
        {
            ShowError("�폜����v���W�F�N�g���I������Ă��܂���B");
            return;
        }

        ProjectManager.Instance.DeleteProject(selectedProject.Id);
        selectedProject = null;
        projectNameInput2.text = string.Empty; // �v���W�F�N�g�����̓t�B�[���h���N���A
        DisplayProjects(); // �v���W�F�N�g�폜���UI���X�V

        if (ProjectManager.Instance.GetProjects().Count == 0)
        {
            ShowProjectCreatePanel();
        }
        else
        {
            ShowProjectEditPanel();
        }
    }

    // ���[�h�����f�[�^��UI�ɔ��f����֐�
    private void DisplayProjects()
    {
        foreach (Transform child in projectListContent.transform)
        {
            Destroy(child.gameObject);
        }

        var projects = ProjectManager.Instance.GetProjects();
        foreach (var project in projects)
        {
            GameObject projectItem = Instantiate(projectListItemPrefab, projectListContent.transform);
            

            Text nametext = projectItem.transform.Find("Image/Text").GetComponent<Text>();
            //nametext.text = project.Name;
            nametext.text = string.Join("\n", projects.ConvertAll(p => p.Name));

            // Add button click listener to show tasks of this project
            Button projectButton = projectItem.transform.Find("Frame/ProjectButton").GetComponent<Button>();
            projectButton.onClick.AddListener(() => OnProjectSelected(project));

            Button deleteButton = projectItem.transform.Find("Frame/DeleteButton").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => Destroy(projectItem));
        }
    }

    private void OnProjectSelected(ProjectModels project)
    {
        selectedProject = project;
        projectNameInput2.text = project.Name;

        ShowProjectEditPanel();
    }

    private void ShowError(string message)
    {
        Debug.LogWarning(message);
        errorText.text = message;
        errorPanel.SetActive(true); // �G���[�p�l����\��
    }

    private void OnErrorClose()
    {
        errorPanel.SetActive(false); // �G���[�p�l�������
    }
}

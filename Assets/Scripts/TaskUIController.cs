using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class TaskUIController : MonoBehaviour
{
    public InputField taskDescriptionInput;
    public InputField taskDeadlineInput;
    public Dropdown taskPriorityDropdown;
    public Dropdown projectDropdown;
    public Button addTaskButton;
    public Button editTaskButton;
    public GameObject taskListContent;
    public GameObject taskListItemPrefab;

    public GameObject errorPanel; // �G���[���b�Z�[�W�\���p�̃p�l��
    public Text errorText; // �G���[���b�Z�[�W��\������e�L�X�g
    public Button errorCloseButton; // �G���[�p�l�������{�^��
    public Button sortPriorityButton; // �D��x���ɕ��ёւ���{�^��

    private TaskModels selectedTask;
    private bool sortDescending = true; // �\�[�g���̃t���O

    private void Start()
    {
        addTaskButton.onClick.AddListener(OnAddTask);
        editTaskButton.onClick.AddListener(OnEditTask);
        errorCloseButton.onClick.AddListener(OnErrorClose);
        projectDropdown.onValueChanged.AddListener(OnProjectDropdownChanged); // �h���b�v�_�E���Ƀ��X�i�[��ǉ�
        sortPriorityButton.onClick.AddListener(OnSortByPriority); // ���ёւ��{�^���Ƀ��X�i�[��ǉ�
        DisplayProjectsInDropdown();
        DisplayTasks(); // �����\��
    }

    private void OnAddTask()
    {
        string description = taskDescriptionInput.text;
        MyDateTime deadline;
        if (!MyDateTime.TryParse(taskDeadlineInput.text, out deadline))
        {
            ShowError("���t�̌`���������ł��BYYYY/MM/DD HH:MM:SS�`���œ��͂��Ă��������B");
            return;
        }
        int priority = taskPriorityDropdown.value;
        int projectId = projectDropdown.value;

        TaskManager.Instance.AddTask(description, deadline, priority, true, projectId);
        DisplayTasks(); // �^�X�N�ǉ���ɍX�V
    }

    private void OnEditTask()
    {
        if (selectedTask == null)
        {
            ShowError("�ҏW����^�X�N���I������Ă��܂���B");
            return;
        }

        string newDescription = taskDescriptionInput.text;
        MyDateTime newDeadline;
        if (!MyDateTime.TryParse(taskDeadlineInput.text, out newDeadline))
        {
            ShowError("���t�̌`���������ł��BYYYY/MM/DD HH:MM:SS�`���œ��͂��Ă��������B");
            return;
        }
        int newPriority = taskPriorityDropdown.value;
        int newProjectId = projectDropdown.value;

        TaskManager.Instance.EditTask(selectedTask.Id, newDescription, newDeadline, newPriority, newProjectId);
        DisplayTasks(); // �^�X�N�ҏW��ɍX�V
    }

    private void DisplayTasks()
    {
        foreach (Transform child in taskListContent.transform)
        {
            Destroy(child.gameObject);
        }

        int selectedProjectId = projectDropdown.value;
        var tasks = TaskManager.Instance.GetTasksByProject(selectedProjectId);

        if (sortDescending)
        {
            tasks = tasks.OrderByDescending(task => task.Priority).ToList();
        }
        else
        {
            tasks = tasks.OrderBy(task => task.Priority).ToList();
        }

        foreach (var task in tasks)
        {
            GameObject taskItem = Instantiate(taskListItemPrefab, taskListContent.transform);
            
            Text nameText = taskItem.transform.Find("Frame/Text").GetComponent<Text>();
            nameText.text = $"{task.Description} - {task.Deadline}";

            // �^�X�N�I���{�^����ݒ�
            Button taskButton = taskItem.transform.Find("Frame/SelectButton").GetComponent<Button>();
            taskButton.onClick.AddListener(() => OnTaskSelected(task));

            // �^�X�N�폜�{�^����ݒ�
            Button deleteButton = taskItem.transform.Find("Frame/DeleteButton").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => OnDeleteTask(task));
        }
    }

    private void DisplayProjectsInDropdown()
    {
        projectDropdown.ClearOptions();
        var projectOptions = new List<string>();

        foreach (var project in ProjectManager.Instance.GetProjects())
        {
            projectOptions.Add(project.Name);
        }

        projectDropdown.AddOptions(projectOptions);
        OnProjectDropdownChanged(0); // �����v���W�F�N�g�̃^�X�N�\��
    }

    private void OnProjectDropdownChanged(int projectIndex)
    {
        DisplayTasks(); // �v���W�F�N�g�I�����Ƀ^�X�N��\��
    }

    private void OnTaskSelected(TaskModels task)
    {
        selectedTask = task;
        taskDescriptionInput.text = task.Description;
        taskDeadlineInput.text = task.Deadline.ToString();
        taskPriorityDropdown.value = task.Priority;
        projectDropdown.value = task.ProjectId;
    }

    private void OnDeleteTask(TaskModels task)
    {
        TaskManager.Instance.DeleteTask(task.Id);
        DisplayTasks(); // �^�X�N�폜��ɍX�V
    }

    private void OnSortByPriority()
    {
        sortDescending = !sortDescending; // �\�[�g����؂�ւ���
        DisplayTasks(); // �^�X�N���ĕ\�����ă\�[�g�𔽉f
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

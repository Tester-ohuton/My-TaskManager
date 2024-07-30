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

    public GameObject errorPanel; // エラーメッセージ表示用のパネル
    public Text errorText; // エラーメッセージを表示するテキスト
    public Button errorCloseButton; // エラーパネルを閉じるボタン
    public Button sortPriorityButton; // 優先度順に並び替えるボタン

    private TaskModels selectedTask;
    private bool sortDescending = true; // ソート順のフラグ

    private void Start()
    {
        addTaskButton.onClick.AddListener(OnAddTask);
        editTaskButton.onClick.AddListener(OnEditTask);
        errorCloseButton.onClick.AddListener(OnErrorClose);
        projectDropdown.onValueChanged.AddListener(OnProjectDropdownChanged); // ドロップダウンにリスナーを追加
        sortPriorityButton.onClick.AddListener(OnSortByPriority); // 並び替えボタンにリスナーを追加
        DisplayProjectsInDropdown();
        DisplayTasks(); // 初期表示
    }

    private void OnAddTask()
    {
        string description = taskDescriptionInput.text;
        MyDateTime deadline;
        if (!MyDateTime.TryParse(taskDeadlineInput.text, out deadline))
        {
            ShowError("日付の形式が無効です。YYYY/MM/DD HH:MM:SS形式で入力してください。");
            return;
        }
        int priority = taskPriorityDropdown.value;
        int projectId = projectDropdown.value;

        TaskManager.Instance.AddTask(description, deadline, priority, true, projectId);
        DisplayTasks(); // タスク追加後に更新
    }

    private void OnEditTask()
    {
        if (selectedTask == null)
        {
            ShowError("編集するタスクが選択されていません。");
            return;
        }

        string newDescription = taskDescriptionInput.text;
        MyDateTime newDeadline;
        if (!MyDateTime.TryParse(taskDeadlineInput.text, out newDeadline))
        {
            ShowError("日付の形式が無効です。YYYY/MM/DD HH:MM:SS形式で入力してください。");
            return;
        }
        int newPriority = taskPriorityDropdown.value;
        int newProjectId = projectDropdown.value;

        TaskManager.Instance.EditTask(selectedTask.Id, newDescription, newDeadline, newPriority, newProjectId);
        DisplayTasks(); // タスク編集後に更新
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

            // タスク選択ボタンを設定
            Button taskButton = taskItem.transform.Find("Frame/SelectButton").GetComponent<Button>();
            taskButton.onClick.AddListener(() => OnTaskSelected(task));

            // タスク削除ボタンを設定
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
        OnProjectDropdownChanged(0); // 初期プロジェクトのタスク表示
    }

    private void OnProjectDropdownChanged(int projectIndex)
    {
        DisplayTasks(); // プロジェクト選択時にタスクを表示
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
        DisplayTasks(); // タスク削除後に更新
    }

    private void OnSortByPriority()
    {
        sortDescending = !sortDescending; // ソート順を切り替える
        DisplayTasks(); // タスクを再表示してソートを反映
    }

    private void ShowError(string message)
    {
        Debug.LogWarning(message);
        errorText.text = message;
        errorPanel.SetActive(true); // エラーパネルを表示
    }

    private void OnErrorClose()
    {
        errorPanel.SetActive(false); // エラーパネルを閉じる
    }
}

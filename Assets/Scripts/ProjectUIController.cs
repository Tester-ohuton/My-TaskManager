using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProjectUIController : MonoBehaviour
{
    public GameObject projectCreatePanel; // プロジェクト作成パネル
    public GameObject projectEditPanel; // プロジェクト編集パネル
    public InputField projectNameInput1;
    public Button addProjectButton;
    public GameObject projectListContent;
    public GameObject projectListItemPrefab;

    public GameObject errorPanel; // エラーメッセージ表示用のパネル
    public Text errorText; // エラーメッセージを表示するテキスト
    public Button errorCloseButton; // エラーパネルを閉じるボタン

    [Header("0. Text 1.Open  2.Delete")]
    public string[] hierarchyName;

    private ProjectModels selectedProject;

    private void Start()
    {
        ChangeCount(projectNameInput1.text);

        addProjectButton.onClick.AddListener(OnAddProject);
        errorCloseButton.onClick.AddListener(OnErrorClose);
        DisplayProjects(); // ロードしたデータをUIに反映
    }

    private void Update()
    {
        ChangeCount(projectNameInput1.text);
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
            ShowError("プロジェクト名を入力してください。");
            return;
        }

        if (ProjectManager.Instance.ProjectNameExists(name))
        {
            ShowError("このプロジェクト名は既に存在します。別の名前を入力してください。");
            projectNameInput1.text = string.Empty; // プロジェクト名入力フィールドをクリア
            return;
        }

        if (limit <= name.Length)
        {
            ShowError("プロジェクト名が文字制限を超えています");
            return;
        }

        ProjectManager.Instance.AddProject(name);
        DisplayProjects(); // プロジェクト追加後にUIを更新
        ShowProjectEditPanel();
    }

    private void OnEditProject()
    {
        string newName = projectNameInput1.text;
        if (string.IsNullOrEmpty(newName))
        {
            ShowError("プロジェクト名を入力してください。");
            return;
        }

        if (selectedProject == null)
        {
            ShowError("編集対象のプロジェクトが選択されていません。");
            return;
        }

        if (ProjectManager.Instance.ProjectNameExists(newName))
        {
            ShowError("このプロジェクト名は既に存在します。別の名前を入力してください。");
            projectNameInput1.text = string.Empty; // プロジェクト名入力フィールドをクリア
            return;
        }

        if (limit <= newName.Length)
        {
            ShowError("プロジェクト名が文字制限を超えています");
            return;
        }

        ProjectManager.Instance.EditProject(selectedProject.Id, newName);
        DisplayProjects(); // プロジェクト編集後にUIを更新
    }

    private void OnDeleteProject()
    {
        if (selectedProject == null)
        {
            ShowError("削除するプロジェクトが選択されていません。");
            return;
        }

        ProjectManager.Instance.DeleteProject(selectedProject.Id);
        selectedProject = null;
        projectNameInput1.text = string.Empty; // プロジェクト名入力フィールドをクリア
        DisplayProjects(); // プロジェクト削除後にUIを更新

        if (ProjectManager.Instance.GetProjects().Count == 0)
        {
            ShowProjectCreatePanel();
        }
        else
        {
            ShowProjectEditPanel();
        }
    }

    // ロードしたデータをUIに反映する関数
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
            

            Text nametext = projectItem.transform.Find(hierarchyName[0]).GetComponent<Text>();
            nametext.text = project.Name;

            // Add button click listener to show tasks of this project
            Button projectButton = projectItem.transform.Find(hierarchyName[1]).GetComponent<Button>();
            projectButton.onClick.AddListener(() => OnProjectSelected(project));

            Button deleteButton = projectItem.transform.Find(hierarchyName[2]).GetComponent<Button>();
            deleteButton.onClick.AddListener(() => Destroy(projectItem));
        }
    }

    private void OnProjectSelected(ProjectModels project)
    {
        selectedProject = project;
        projectNameInput1.text = project.Name;

        ShowProjectEditPanel();
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

    private void GetProjects()
    {
        ProjectManager.Instance.GetProjects();
    }

    [SerializeField] Text countText;
    Color countColor = Color.white;
    Color countLastColor = Color.red;
    int limit = 50;

    public void ChangeCount(string comment)
    {
        int leftNum = limit - comment.Length;
        if (leftNum < 10)
            countText.color = countLastColor;
        else
            countText.color = countColor;

        countText.text = leftNum.ToString();
    }
}

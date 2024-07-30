using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject StartPanel;
    public GameObject ProjectSelectPanel;
    public GameObject ProjectCreatePanel;

    private void StartPanelActive(bool isOpen)
    {
        StartPanel.SetActive(isOpen);
    }

    public void CreateProjectButton()
    {
        StartPanelActive(false);
        ProjectCreatePanel.SetActive(true);
    }
}

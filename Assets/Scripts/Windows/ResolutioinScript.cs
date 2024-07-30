using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResolutionScript : MonoBehaviour
{
    [SerializeField]
    Dropdown dropdown;

    [SerializeField]
    int minWidth = 800; // 最小幅
    [SerializeField]
    int minHeight = 600; // 最小高さ

    List<Resolution> resolutions = new();

    void Start()
    {
        SetDropdown();
    }

    public void SetDropdown()
    {
        resolutions.Clear();
        dropdown.ClearOptions();

        int currentIndex = 0;
        List<string> options = new();
        Resolution resolution;

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width < minWidth || Screen.resolutions[i].height < minHeight)
            {
                continue; // 小さすぎる解像度をスキップ
            }

            if (Screen.resolutions[i].width == Screen.width && Screen.resolutions[i].height == Screen.height)
            {
                currentIndex = resolutions.Count;
            }

            resolution = new Resolution
            {
                width = Screen.resolutions[i].width,
                height = Screen.resolutions[i].height
            };
            resolutions.Add(resolution);

            options.Add(Screen.resolutions[i].width.ToString() + "x" + Screen.resolutions[i].height.ToString());
        }

        dropdown.AddOptions(options);
        dropdown.value = currentIndex;
        dropdown.onValueChanged.AddListener((x) => SetResolution(resolutions[x]));
    }

    void SetResolution(Resolution resolution)
    {
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    sealed class Resolution
    {
        public int width;
        public int height;
    }
}

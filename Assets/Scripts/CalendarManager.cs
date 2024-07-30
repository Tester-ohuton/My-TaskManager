using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarManager : MonoBehaviour
{
    public static CalendarManager Instance { get; private set; }

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
        // Initialize calendar related functionalities
    }

    public void DisplayTasksOnCalendar()
    {
        // Logic to display tasks on the calendar
    }

    public void SetReminder(int taskId, MyDateTime reminderTime)
    {
        // Logic to set reminders for tasks
    }
}

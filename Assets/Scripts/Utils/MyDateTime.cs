// Assets/Scripts/Utils/MyDateTime.cs
using System;

[Serializable]
public class MyDateTime
{
    public int Year;
    public int Month;
    public int Day;
    public int Hour;
    public int Minute;
    public int Second;

    public MyDateTime(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
    {
        Year = year;
        Month = month;
        Day = day;
        Hour = hour;
        Minute = minute;
        Second = second;
    }

    public static bool TryParse(string dateTimeString, out MyDateTime myDateTime)
    {
        myDateTime = null;
        try
        {
            string[] dateAndTime = dateTimeString.Split(' ');
            string[] dateParts = dateAndTime[0].Split('/');
            string[] timeParts = dateAndTime[1].Split(':');

            int year = int.Parse(dateParts[0]);
            int month = int.Parse(dateParts[1]);
            int day = int.Parse(dateParts[2]);
            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);
            int second = int.Parse(timeParts[2]);

            myDateTime = new MyDateTime(year, month, day, hour, minute, second);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public override string ToString()
    {
        return $"{Year:D4}/{Month:D2}/{Day:D2} {Hour:D2}:{Minute:D2}:{Second:D2}";
    }
}

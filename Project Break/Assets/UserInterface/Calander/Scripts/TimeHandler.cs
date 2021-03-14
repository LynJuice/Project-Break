using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    int currentDay = 13;
    int currentMonth = 7;
    int Daystill;

    [SerializeField] Text CurrentDate;
    [SerializeField] Text DaysTill;
    [SerializeField] Image Moon;
    private void Start()
    {
        CurrentDate.text = currentMonth + "/" + currentDay;
        SetEventDate(20);
        DaysTill.text = Daystill + "/";
    }
    public void NextDay()
    {
        currentDay++;
        Daystill--;
        if (currentDay == 31)
        {
            currentMonth++;
            currentDay = 1;
        }

        CurrentDate.text = currentMonth + "/" + currentDay;
    }
    public void SkipTill(int Month,int Day)
    {
        currentDay = Day;
        currentMonth = Month;
    }
    public bool DateMatch(int Day, int Month)
    {
        if (Day == currentDay && currentMonth == Month)
            return true;
        else
            return false;
    }
    public void SetEventDate(int Day)
    {
        Daystill = currentDay - Day;
    }
}

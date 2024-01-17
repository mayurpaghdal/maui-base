namespace MauiBase.Models;

public class BaseModel : ObservableObject
{
    protected string GetHours(DateTime startTime, DateTime endTime)
    {
        var minutes = (endTime - startTime);
        return string.Format("{0:00}:{1:00}", (int)minutes.TotalHours, minutes.Minutes);
    }

    public string GetMoment(DateTime date)
    {
        var minutes = (DateTime.Now.ToUniversalTime() - date).TotalMinutes;
        if (date.Year == 1)
        {
            return "--/--";
        }
        else if (minutes <= 5)
        {
            return "just now";
        }
        else if (minutes > 5 && minutes <= 30)
        {
            return "few mins ago";
        }
        else if (minutes > 30 && minutes <= 55)
        {
            return "half an hour ago";
        }
        else if (minutes > 55 && minutes <= 85)
        {
            return "an hour ago";
        }
        else if (minutes > 85 && minutes <= 300)
        {
            return "few hours ago";
        }
        else if (date.Date == DateTime.Now.Date)
        {
            return "today";
        }
        else if (date.Date == DateTime.Now.Date.AddDays(-1))
        {
            return "yesterday";
        }
        else
            return date.Date.ToString("MMM dd");
    }
}

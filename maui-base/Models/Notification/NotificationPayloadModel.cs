using Newtonsoft.Json;

namespace MauiAppDemo.Models;

public class NotificationPayloadModel
{
    public NotificationPayloadModel()
    {
        Notification = new NotificationModel();
    }
    public NotificationModel Notification { get; set; }
    public string TargetPage { get; set; }
    [JsonIgnore]
    public bool IsAppForeground { get; set; }
    public bool IsAlertAutoClosed { get; set; }
}

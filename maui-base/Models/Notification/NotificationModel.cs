namespace MauiAppDemo.Models;

public class NotificationModel : BaseModel
{
    public int NotificationID { get; set; }
    public int NotificationTypeID { get; set; }
    public int DataSourceID { get; set; }
    public string SPUniqueID { get; set; }
    public string Title { get; set; }

    private string description;
    public string Description
    {
        get { return description; }
        set
        {
            SetProperty(ref description, value);
            //RaisePropertyChanged(nameof(IsDescriptionExists));
        }
    }

    string startDate;
    public string StartDate
    {
        get { return startDate; }
        set
        {
            SetProperty(ref startDate, value);
            //RaisePropertyChanged(nameof(IsStartDateExists));
        }
    }

    DateTime? startDateTime;
    public DateTime? StartDateTime
    {
        get { return startDateTime; }
        set { SetProperty(ref startDateTime, value); }
    }

    string endDate;
    public string EndDate
    {
        get { return endDate; }
        set
        {
            SetProperty(ref endDate, value);
            //RaisePropertyChanged(nameof(IsEndDateExists));
        }
    }

    DateTime? endDateTime;
    public DateTime? EndDateTime
    {
        get { return endDateTime; }
        set { SetProperty(ref endDateTime, value); }
    }

    string category;
    public string Category
    {
        get { return category; }
        set { SetProperty(ref category, value); }
    }

    string link;
    public string Link
    {
        get { return link; }
        set { SetProperty(ref link, value); }
    }

    string linkDescription;
    public string LinkDescription
    {
        get { return linkDescription; }
        set { SetProperty(ref linkDescription, value); }
    }

    bool isTopNotification;
    public bool IsTopNotification
    {
        get { return isTopNotification; }
        set { SetProperty(ref isTopNotification, value); }
    }

    string status;
    public string Status
    {
        get { return status; }
        set { SetProperty(ref status, value); }
    }

    byte[] statusImage;
    public byte[] StatusIcon
    {
        get { return statusImage; }
        set { SetProperty(ref statusImage, value); }
    }

    string nextUpdateDate;
    public string NextUpdateDate
    {
        get { return nextUpdateDate; }
        set
        {
            SetProperty(ref nextUpdateDate, value);
            //RaisePropertyChanged(nameof(IsNextUpdateDateExists));
        }
    }

    int? orderNo;
    public int? OrderNo
    {
        get { return orderNo; }
        set { SetProperty(ref orderNo, value); }
    }

    DateTime? sourceCreated;
    public DateTime? SourceCreated
    {
        get { return sourceCreated; }
        set { SetProperty(ref sourceCreated, value); }
    }

    DateTime? sourceModified;
    public DateTime? SourceModified
    {
        get { return sourceModified; }
        set { SetProperty(ref sourceModified, value); }
    }

    public bool IsStartDateExists => !string.IsNullOrWhiteSpace(StartDate);

    public bool IsEndDateExists => !string.IsNullOrWhiteSpace(EndDate);

    public bool IsNextUpdateDateExists => !string.IsNullOrWhiteSpace(NextUpdateDate);

    public bool IsDescriptionExists => !string.IsNullOrWhiteSpace(Description);

    DateTime? spCreated;
    public DateTime? SPCreated
    {
        get { return spCreated; }
        set { SetProperty(ref spCreated, value); }
    }

    DateTime? spModified;

    public DateTime? SPModified
    {
        get { return spModified; }
        set { SetProperty(ref spModified, value); }
    }
}

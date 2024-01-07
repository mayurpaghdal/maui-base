namespace MauiAppDemo.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ChildViewModelAttribute : Attribute
{
    public Type ViewModelType { get; set; }
    public bool IsMainPageChildView { get; set; }

    public ChildViewModelAttribute(Type viewModelType, bool isMainPageChildView = false)
    {
        IsMainPageChildView = isMainPageChildView;
        ViewModelType = viewModelType;
    }
}

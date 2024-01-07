namespace MauiAppDemo.Events
{
    public class DeletedNewsEvent : PubSubEvent<List<int>> { }
    public class NewsViewChangedEvent : PubSubEvent { }
}

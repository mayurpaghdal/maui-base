namespace MauiAppDemo.Events;

public class DialogResultEventArgs<T> : EventArgs
{
    private T _result;

    public DialogResultEventArgs(T result)
    {
        _result = result;
    }

    public T Result
    {
        get { return _result; }
    }
}

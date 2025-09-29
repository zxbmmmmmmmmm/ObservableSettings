using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableSettings;

public partial class ObservableBindingList<T> : BindingList<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        CollectionChanged?.Invoke(index, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        PropertyChanged?.Invoke(this, EventArgsCache.CountPropertyChanged);
        PropertyChanged?.Invoke(this, EventArgsCache.IndexerPropertyChanged);
    }

    protected override void RemoveItem(int index)
    {
        var item = this[index];
        base.RemoveItem(index);
        CollectionChanged?.Invoke(index, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        PropertyChanged?.Invoke(this, EventArgsCache.CountPropertyChanged);
        PropertyChanged?.Invoke(this, EventArgsCache.IndexerPropertyChanged);
    }
}

internal static class EventArgsCache
{
    internal static readonly PropertyChangedEventArgs CountPropertyChanged = new PropertyChangedEventArgs("Count");
    internal static readonly PropertyChangedEventArgs IndexerPropertyChanged = new PropertyChangedEventArgs("Item[]");
    internal static readonly NotifyCollectionChangedEventArgs ResetCollectionChanged = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
}
using System;

namespace ObservableSettings;

[AttributeUsage(AttributeTargets.Property)]
public class NotifyPropertyChangedWhenAttribute(string eventName) : Attribute
{
    public string EventName { get; } = eventName;
}
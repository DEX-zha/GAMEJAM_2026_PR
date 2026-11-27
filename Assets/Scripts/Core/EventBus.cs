using System.Collections.Generic;
using System;

public static class EventBus<T> where T : struct
{
    private static readonly HashSet<IEventBinding<T>> _bindings = new();

    public static void Register(IEventBinding<T> binding) => _bindings.Add(binding);
    public static void Deregister(IEventBinding<T> binding) => _bindings.Remove(binding);

    public static void Raise(T @event)
    {
        foreach (var binding in _bindings)
            binding.OnEvent.Invoke(@event);
    }
}

public interface IEventBinding<T>
{
    Action<T> OnEvent { get; }
}

public class EventBinding<T> : IEventBinding<T> where T : struct
{
    public Action<T> OnEvent { get; }
    public EventBinding(Action<T> onEvent) => OnEvent = onEvent;
}

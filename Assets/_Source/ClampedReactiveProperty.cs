using System;
using System.Collections.Generic;
using R3;

public sealed class ClampedReactiveProperty<T> : ReactiveProperty<T> where T : IComparable<T>
{
    public readonly T Min;
    public readonly T Max;
    
    public ClampedReactiveProperty(T initialValue, T min, T max) : base(initialValue)
    {
        Min = min;
        Max = max;
    }
    
    private static IComparer<T> Comparer { get; } = Comparer<T>.Default;

    protected override void OnValueChanging(ref T value)
    {
        if (Comparer.Compare(value, Min) < 0)
        {
            value = Min;
        }
        else if (Comparer.Compare(value, Max) > 0)
        {
            value = Max;
        }
    }
}
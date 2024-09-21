using System;

public interface IStatisticContext
{
    public bool IsName(ReadOnlySpan<char> name);
    public Statistic GetStatistic(ReadOnlySpan<char> value);
    public IStatisticContext GetContext(ReadOnlySpan<char> value);
}
using Game;
using System;
using UnityEngine;

[Serializable]
public abstract class StatisticSerializedIdentity : Statistic
{
    [SerializeField]
    protected string name;

    [SerializeField]
    protected string definitionId;

    public override StatisticDefinition GetDefinition(IStatisticContext context) => StatisticRepository.GetDefinition(definitionId);
    public override string GetName(IStatisticContext context) => name;
    public override string SetName(IStatisticContext context, string value) => name = value;

    protected StatisticSerializedIdentity()
    {
    }

    protected StatisticSerializedIdentity(string name, string definitionId)
    {
        this.name = name;
        this.definitionId = definitionId;
    }
}
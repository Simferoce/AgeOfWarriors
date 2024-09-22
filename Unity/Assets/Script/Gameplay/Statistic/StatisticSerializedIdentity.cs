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

    public override StatisticDefinition Definition { get => StatisticRepository.GetDefinition(definitionId); set => definitionId = value.HumanReadableId; }
    public override string Name { get => name; set => name = value; }

    protected StatisticSerializedIdentity()
    {
    }

    protected StatisticSerializedIdentity(string name, string definitionId)
    {
        this.name = name;
        this.definitionId = definitionId;
    }
}
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StaggerOnTakeDownPerk", menuName = "Definition/Technology/Archer/StaggerOnTakeDownPerk")]
    public class StaggerOnTakeDownPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, StaggerOnTakeDownPerk>
        {
            public Modifier(ModifierHandler modifiable, StaggerOnTakeDownPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                EventChannelDeath.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(EventChannelDeath.Event evt)
            {
                if ((modifiable.Entity as IAttackSource).RecentlyAttacked(evt.AgentObject.GetCachedComponent<Attackable>()))
                {
                    Character character = modifiable.Entity.GetCachedComponent<Character>();

                    foreach (AgentObject agent in AgentObject.All)
                    {
                        if (agent == character)
                            continue;

                        if (!agent.IsActive)
                            continue;

                        if (agent.Faction == character.Faction)
                            continue;

                        if (!agent.TryGetCachedComponent<Target>(out Target targeteable))
                            continue;

                        if (!agent.TryGetCachedComponent<ModifierHandler>(out ModifierHandler modifiable))
                            continue;

                        if (Mathf.Abs((targeteable.ClosestPoint(character.GetCachedComponent<Target>().CenterPosition) - character.GetCachedComponent<Target>().CenterPosition).x) > definition.distanceEffect)
                            continue;

                        modifiable.AddModifier(new StaggerModifierDefinition.Modifier(modifiable, definition.staggerModifierDefinition, definition.duration, character));
                    }
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                EventChannelDeath.Instance.Unsubcribe(OnUnitDeath);
            }
        }

        [SerializeField] private StaggerModifierDefinition staggerModifierDefinition;
        [SerializeField] private float duration;
        [SerializeField] private float distanceEffect;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}

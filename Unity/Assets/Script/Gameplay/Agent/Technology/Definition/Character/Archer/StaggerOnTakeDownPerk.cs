using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StaggerOnTakeDownPerk", menuName = "Definition/Technology/Archer/StaggerOnTakeDownPerk")]
    public class StaggerOnTakeDownPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, StaggerOnTakeDownPerk>
        {
            public Modifier(StaggerOnTakeDownPerk modifierDefinition) : base(modifierDefinition)
            {
                DeathEventChannel.Instance.Susbribe(OnUnitDeath);
            }

            public void OnUnitDeath(DeathEventChannel.Event evt)
            {
                throw new System.NotImplementedException();
                //if (modifiable.Entity.GetCachedComponent<IAttackSource>().RecentlyAttacked(evt.AgentObject.GetCachedComponent<IAttackable>()))
                //{
                //    Character character = modifiable.Entity.GetCachedComponent<Character>();

                //    foreach (AgentObject agent in AgentObject.All)
                //    {
                //        if (agent == character)
                //            continue;

                //        if (!agent.IsActive)
                //            continue;

                //        if (agent.Faction == character.Faction)
                //            continue;

                //        if (!agent.TryGetCachedComponent<Target>(out Target targeteable))
                //            continue;

                //        if (!agent.TryGetCachedComponent<ModifierHandler>(out ModifierHandler modifiable))
                //            continue;

                //        if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.distanceEffect)
                //            continue;

                //        modifiable.AddModifier(new StaggerModifierDefinition.Modifier(modifiable, definition.staggerModifierDefinition, definition.duration, character));
                //    }

            }

            public override void Dispose()
            {
                base.Dispose();
                DeathEventChannel.Instance.Unsubcribe(OnUnitDeath);
            }
        }

        [SerializeField] private StaggerModifierDefinition staggerModifierDefinition;
        [SerializeField] private float duration;
        [SerializeField] private float distanceEffect;

        public override string ParseDescription()
        {
            return string.Format(Description, duration, distanceEffect);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AnimatorEventChannel : StateMachineBehaviour
    {
        public enum Id
        {
            Ability
        }

        #region Subscription
        public enum Event
        {
            OnEnter,
            OnExit
        }

        private class ChannelManager
        {
            public Dictionary<(Event, Id), Channel> Channels { get; set; } = new Dictionary<(Event, Id), Channel>();
        }

        private class Channel
        {
            private List<Action> subscribers = new List<Action>();

            public void Subscribe(Action action)
            {
                subscribers.Add(action);
            }

            public void Publish()
            {
                foreach (Action action in subscribers)
                {
                    action.Invoke();
                }
            }

            public void Unsubscribe(Action action)
            {
                subscribers.Remove(action);
            }
        }

        private static Dictionary<Animator, ChannelManager> channelManagers = new Dictionary<Animator, ChannelManager>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            channelManagers = new Dictionary<Animator, ChannelManager>();
        }

        public static void Subscribe(Animator animator, Event evt, Id id, Action action)
        {
            Channel channel = GetChannel(animator, evt, id);
            channel.Subscribe(action);
        }

        public static void Unsubscribe(Animator animator, Event evt, Id id, Action action)
        {
            Channel channel = GetChannel(animator, evt, id);
            channel.Unsubscribe(action);
        }

        private static Channel GetChannel(Animator animator, Event evt, Id id)
        {
            if (!channelManagers.ContainsKey(animator))
                channelManagers.Add(animator, new ChannelManager());

            ChannelManager channelManager = channelManagers[animator];
            if (!channelManager.Channels.ContainsKey((evt, id)))
                channelManager.Channels.Add((evt, id), new Channel());

            return channelManager.Channels[(evt, id)];
        }
        #endregion

        [SerializeField] private Id id;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            Channel channel = GetChannel(animator, Event.OnEnter, id);
            channel.Publish();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            Channel channel = GetChannel(animator, Event.OnExit, id);
            channel.Publish();
        }
    }
}
using System;
using Abilities.Core;
using Core;
using UnityEngine;

namespace Abilities.Behaviours
{
    public class AdditionalShot : Ability, IDataReceiver<VisualData>, IDataReceiver<InitData>
    {
        private Behaviour _behaviour;
        
        private class Behaviour
        {
            public MultipleTargetsTracker Tracker;
            public GameObject Button;

            public void DoAction()
            {
                Tracker.SetShotsCount(Tracker.ShotsCount + 1);
                Button.SetActive(false);
            }
        }
        
        public override void Interact()
        {
            _behaviour.DoAction();
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Interact(VisualData data, Action<IAbilityData> callback = null)
        {
            callback?.Invoke(GetData());
        }

        public void Interact(InitData data, Action<IAbilityData> callback = null)
        {
            _behaviour = new Behaviour
            {
                Button = data.Presenter.CreateAbilityButton(_buttonTemplate, GetData()),
                Tracker = FindObjectOfType<MultipleTargetsTracker>()
            };

            callback?.Invoke(GetData());
        }
    }
}

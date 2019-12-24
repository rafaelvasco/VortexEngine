using System;
using System.Collections.Generic;

namespace VortexCore
{
    public abstract class Actor : GameObject
    {
        private readonly Dictionary<Type, int> behaviorMap = new Dictionary<Type, int>();
        private List<Behavior> behaviors = new List<Behavior>(10);

        public override void Update(float dt)
        {
            foreach (var behavior in behaviors)
            {
                if (behavior.Enabled)
                {
                    behavior.Update(dt);
                }
            }
        }

        public T AddBehavior<T>(bool activeAtStart = true) where T : Behavior
        {
            var type = typeof(T);
            T behavior = GetBehavior<T>();

            if (behavior == null)
            {
                behavior = (T)Activator.CreateInstance(type, args: activeAtStart);
                behaviors.Add(behavior);
                behaviorMap.Add(type, behaviors.Count - 1);
                behavior.AttachTo(this);
            }

            return behavior;
        }

        public void RemoveBehavior<T>() where T : Behavior
        {
            var type = typeof(T);
            int index = behaviorMap[type];
            behaviors.RemoveAt(index);
            behaviorMap.Remove(type);
        }

        public T GetBehavior<T>() where T : Behavior
        {
            if (behaviorMap.TryGetValue(typeof(T), out var index))
            {
                return (T)behaviors[index];
            }

            return null;
        }
    }
}
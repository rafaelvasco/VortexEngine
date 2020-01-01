/* 
MIT License
Copyright (c) 2019 Rafael Vasco
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

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
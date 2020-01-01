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

namespace VortexCore
{
    public class SineBehavior : Behavior
    {
        public enum Movement
        {
            /// <summary>
            /// Moves the Actor left and right on the X axis
            /// </summary>
            Horizontal,

            /// <summary>
            /// Moves the Actor up and down on the Y axis
            /// </summary>
            Vertical,

            /// <summary>
            /// Makes the Actor grow and shrink
            /// </summary>
            Size,

            /// <summary>
            /// Rotates the Actor clockwise and anticlockwise
            /// </summary>
            Angle,

            /// <summary>
            /// Oscilates the Actor opacity
            /// </summary>
            Opacity,

            /// <summary>
            /// Stretches the Actor wider and narrower
            /// </summary>
            Width,

            /// <summary>
            /// Stretches the Actor taller and shorter
            /// </summary>
            Height,

            /// <summary>
            /// Modifies a custom given value instead of Actors properties
            /// </summary>
            CustomValue,

            /// <summary>
            /// Moves the Actor in a straight line back and forth along the angle 
            /// the Actor is facing at.
            /// </summary>
            ForwardBackward
        }

        public enum Wave
        {
            /// <summary>
            /// Default smooth oscillating motion based on a sine wave
            /// </summary>
            Sine,

            /// <summary>
            /// A linear back and forth motion
            /// </summary>
            Triangle,

            /// <summary>
            /// Linear motion with a jump back to start
            /// </summary>
            Sawtooth,

            /// <summary>
            /// Reverse linear motion with a jump back to start
            /// </summary>
            ReverseSawtooth,

            /// <summary>
            /// Alternating between two maximum values
            /// </summary>
            Square

        }

        public Movement MovementType { get; set; }

        public Wave WaveType { get; set; }


        /// <summary>
        /// The duration, in seconds, of the complete back-and-forth cycle
        /// </summary>
        public float Period { get; set; }

        /// <summary>
        /// A random number of seconds added to the period for each Actor.
        /// This can help to add variation of movement when a lot of actors are using the Behavior
        /// </summary>
        public float PeriodRandom { get; set; }


        /// <summary>
        /// The initial time in seconds through the cycle. For example,
        /// if the period is 2 seconds and the period offset is 1 second,
        /// the behavior starts half way through the cycle
        /// </summary>
        public float PeriodOffset { get; set; }


        /// <summary>
        /// A random number of seconds added to the period offset for each Actor.
        /// This can help to add variation of movement when a lot of actors are using the Behavior
        /// </summary>
        public float PeriodOffsetRandom { get; set; }

        /// <summary>
        /// The maximum change in the Actor's position, size or angle. This is in pixels for position or size modes, /// or degrees for the angle mode.
        /// </summary>
        public float Magnitude { get; set; }

        /// <summary>
        /// A random value to add to the magnitude for each instance. This can help vary the appearance when a lot of /// instances are using the Sine behavior.
        /// </summary>
        public float MagnitudeRandom {get;set;}

        internal SineBehavior(bool activeOnStart) : base(activeOnStart)
        {
        }

        public override void Update(float dt)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnAttached()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDetached()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDisabled()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnEnabled()
        {
            throw new System.NotImplementedException();
        }
    }
}
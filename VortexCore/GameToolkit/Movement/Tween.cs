using System;
using System.Numerics;

namespace VortexCore
{

    /// <summary>
    /// Takes in progress which is the percentage of the tween completion and
    /// returns the interpolation value that is fed into the lerp function for the
    /// tween.
    /// Scale functions are used to define how the tween function should behave. Example:
    /// Linear, Easing In Quadratic, Easing Out Cicular etc.abstract
    /// </summary>
    /// <param name="progress">The percentage of the tween complete in the range [0.0f, 1.0f]</param>
    /// <returns></returns>
    /// 
    public delegate float TweenScaleFunc(float progress);


    public delegate T TweeenLerpFunc<T>(T start, T end, float progress);

    public delegate void TweenFinishedFunc();

    public enum TweenState
    {
        Running,
        Paused,
        Stopped
    }

    public enum TweenStopBehavior
    {
        /// <summary>
        /// Does not change the current value when stopping.
        /// </summary>
        AsIs,

        /// <summary>
        /// Forces the tween progress to the end value when stopping.
        /// </summary>
        ForceComplete
    }

    public interface ITween
    {
        TweenState State { get; }

        void Pause();

        void Resume();

        void Stop(TweenStopBehavior stopBehavior);

        void Update(float elapsedTime);
    }

    public interface ITween<T> : ITween where T : struct
    {
        T CurrentValue { get; }

        void Start(T start, T end, float duration, TweenScaleFunc scaleFunc);
    }

    public class Tween<T>
    {

        public event TweenFinishedFunc OnFinish;

        private readonly TweeenLerpFunc<T> lerpFunc;

        private float currentTime;

        private float duration;

        private TweenScaleFunc scaleFunc;

        private TweenState state;

        private T start;

        private T end;

        private T value;

        public float CurrentTime { get => currentTime; }

        public float Duration { get => duration; }

        public TweenState State { get => state; }

        public T StartValue { get => start; }

        public T EndValue { get => end; }

        public T CurrentValue { get => value; }

        public TweenStopBehavior StopBehavior {get; private set;}

        public Tween(TweeenLerpFunc<T> lerpFunc)
        {
            this.lerpFunc = lerpFunc;
            state = TweenState.Stopped;
        }

        public void Start(T start, T end, float duration, TweenScaleFunc scaleFunc, TweenStopBehavior stopBehavior = TweenStopBehavior.ForceComplete)
        {
            if (duration <= 0)
            {
                throw new ArgumentException("duration must be greater than 0.");
            }

            if (scaleFunc == null)
            {
                throw new ArgumentNullException("scaleFunc");
            }

            this.StopBehavior = stopBehavior;

            currentTime = 0.0f;

            this.duration = duration;
            this.scaleFunc = scaleFunc;
            state = TweenState.Running;

            this.start = start;
            this.end = end;

            UpdateValue();

        }

        public void Pause()
        {
            if (state == TweenState.Running)
            {
                state = TweenState.Paused;
            }
        }

        public void Resume()
        {
            if (state == TweenState.Paused)
            {
                state = TweenState.Running;
            }
        }

        public void Stop()
        {
            state = TweenState.Stopped;

            if (StopBehavior == TweenStopBehavior.ForceComplete)
            {
                currentTime = duration;
                UpdateValue();
            }

            OnFinish?.Invoke();
        }

        public void Update(float elapsedTime)
        {
            if (state != TweenState.Running)
            {
                return;
            }

            currentTime += elapsedTime;

            if (currentTime >= duration)
            {
                currentTime = duration;
                state = TweenState.Stopped;
                OnFinish?.Invoke();
            }

            UpdateValue();
        }

        private void UpdateValue()
        {
            value = lerpFunc(start, end, scaleFunc(currentTime / duration));
        }

    }

    public class FloatTween : Tween<float>
    {
        private static float LerpFloat(float start, float end, float progress)
        {
            return Calc.Lerp(start, end, progress);
        }

        private static readonly TweeenLerpFunc<float> LerpFunc = LerpFloat;

        public FloatTween() : base(LerpFunc) { }

    }

    public class Vector2Tween : Tween<Vector2>
    {
        private static readonly TweeenLerpFunc<Vector2> LerpFunc = Vector2.Lerp;

        public Vector2Tween() : base(LerpFunc) { }
    }

    public class Vector3Tween : Tween<Vector3>
    {
        private static readonly TweeenLerpFunc<Vector3> LerpFunc = Vector3.Lerp;

        public Vector3Tween() : base(LerpFunc) { }
    }

    public class Vector4Tween : Tween<Vector4>
    {
        private static readonly TweeenLerpFunc<Vector4> LerpFunc = Vector4.Lerp;

        public Vector4Tween() : base(LerpFunc) { }
    }

    public class ColorTween : Tween<Color>
    {
        private static readonly TweeenLerpFunc<Color> LerpFunc = ColorUtils.Lerp;

        public ColorTween() : base(LerpFunc) { }
    }

    public static class TweenScaleFuncs
    {
        /// <summary>
        /// A linear progress scale function.
        /// </summary>
        public static readonly TweenScaleFunc Linear = LinearImpl;

        /// <summary>
        /// A quadratic (x^2) progress scale function that eases in.
        /// </summary>
        public static readonly TweenScaleFunc QuadraticEaseIn = QuadraticEaseInImpl;

        /// <summary>
        /// A quadratic (x^2) progress scale function that eases out.
        /// </summary>
        public static readonly TweenScaleFunc QuadraticEaseOut = QuadraticEaseOutImpl;

        /// <summary>
        /// A quadratic (x^2) progress scale function that eases in and out.
        /// </summary>
        public static readonly TweenScaleFunc QuadraticEaseInOut = QuadraticEaseInOutImpl;

        /// <summary>
        /// A cubic (x^3) progress scale function that eases in.
        /// </summary>
        public static readonly TweenScaleFunc CubicEaseIn = CubicEaseInImpl;

        /// <summary>
        /// A cubic (x^3) progress scale function that eases out.
        /// </summary>
        public static readonly TweenScaleFunc CubicEaseOut = CubicEaseOutImpl;

        /// <summary>
        /// A cubic (x^3) progress scale function that eases in and out.
        /// </summary>
        public static readonly TweenScaleFunc CubicEaseInOut = CubicEaseInOutImpl;

        /// <summary>
        /// A quartic (x^4) progress scale function that eases in.
        /// </summary>
        public static readonly TweenScaleFunc QuarticEaseIn = QuarticEaseInImpl;

        /// <summary>
        /// A quartic (x^4) progress scale function that eases out.
        /// </summary>
        public static readonly TweenScaleFunc QuarticEaseOut = QuarticEaseOutImpl;

        /// <summary>
        /// A quartic (x^4) progress scale function that eases in and out.
        /// </summary>
        public static readonly TweenScaleFunc QuarticEaseInOut = QuarticEaseInOutImpl;

        /// <summary>
        /// A quintic (x^5) progress scale function that eases in.
        /// </summary>
        public static readonly TweenScaleFunc QuinticEaseIn = QuinticEaseInImpl;

        /// <summary>
        /// A quintic (x^5) progress scale function that eases out.
        /// </summary>
        public static readonly TweenScaleFunc QuinticEaseOut = QuinticEaseOutImpl;

        /// <summary>
        /// A quintic (x^5) progress scale function that eases in and out.
        /// </summary>
        public static readonly TweenScaleFunc QuinticEaseInOut = QuinticEaseInOutImpl;

        /// <summary>
        /// A sinusoidal progress scale function that eases in.
        /// </summary>
        public static readonly TweenScaleFunc SineEaseIn = SineEaseInImpl;

        /// <summary>
        /// A sinusoidal progress scale function that eases out.
        /// </summary>
        public static readonly TweenScaleFunc SineEaseOut = SineEaseOutImpl;

        /// <summary>
        /// A sinusoidal progress scale function that eases in and out.
        /// </summary>
        public static readonly TweenScaleFunc SineEaseInOut = SineEaseInOutImpl;

        private static float LinearImpl(float progress) { return progress; }
        private static float QuadraticEaseInImpl(float progress) { return EaseInPower(progress, 2); }
        private static float QuadraticEaseOutImpl(float progress) { return EaseOutPower(progress, 2); }
        private static float QuadraticEaseInOutImpl(float progress) { return EaseInOutPower(progress, 2); }
        private static float CubicEaseInImpl(float progress) { return EaseInPower(progress, 3); }
        private static float CubicEaseOutImpl(float progress) { return EaseOutPower(progress, 3); }
        private static float CubicEaseInOutImpl(float progress) { return EaseInOutPower(progress, 3); }
        private static float QuarticEaseInImpl(float progress) { return EaseInPower(progress, 4); }
        private static float QuarticEaseOutImpl(float progress) { return EaseOutPower(progress, 4); }
        private static float QuarticEaseInOutImpl(float progress) { return EaseInOutPower(progress, 4); }
        private static float QuinticEaseInImpl(float progress) { return EaseInPower(progress, 5); }
        private static float QuinticEaseOutImpl(float progress) { return EaseOutPower(progress, 5); }
        private static float QuinticEaseInOutImpl(float progress) { return EaseInOutPower(progress, 5); }


        private static float EaseInPower(float progress, int power)
        {
            return (float)Math.Pow(progress, power);
        }

        private static float EaseOutPower(float progress, int power)
        {
            int sign = power % 2 == 0 ? -1 : 1;
            return (float)(sign * (Math.Pow(progress - 1, power) + sign));
        }

        private static float EaseInOutPower(float progress, int power)
        {
            progress *= 2;
            if (progress < 1)
            {
                return (float)Math.Pow(progress, power) / 2f;
            }
            else
            {
                int sign = power % 2 == 0 ? -1 : 1;
                return (float)(sign / 2.0 * (Math.Pow(progress - 2, power) + sign * 2));
            }
        }

        private static float SineEaseInImpl(float progress)
        {
            return (float)Math.Sin(progress * Calc.PI_OVER2 - Calc.PI_OVER2) + 1;
        }

        private static float SineEaseOutImpl(float progress)
        {
            return (float)Math.Sin(progress * Calc.PI_OVER2);
        }

        private static float SineEaseInOutImpl(float progress)
        {
            return (float)(Math.Sin(progress * Calc.PI - Calc.PI_OVER2) + 1) / 2;
        }
    }

}
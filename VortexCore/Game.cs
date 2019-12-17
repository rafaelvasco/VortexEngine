using System;
using System.Diagnostics;
using System.Runtime;
using System.Threading;

namespace VortexCore
{
    public class Game : IDisposable
    {
        private const int MaxUpdateFrameLag = 5;

        public Size DisplaySize
        {
            get => GamePlatform.GetDisplaySize();
            set
            {
                var currentSize = GamePlatform.GetDisplaySize();

                if (value.Width == currentSize.Width && value.Height == currentSize.Height)
                {
                    return;
                }

                if (running)
                {
                    displayResizeRequested = true;
                    requestedDisplaySize = value;
                }
                else
                {
                    GamePlatform.SetDisplaySize(value.Width, value.Height);
                }
            }
        }

        public string Title
        {
            get => GamePlatform.GetDisplayTitle();
            set
            {
                GamePlatform.SetDisplayTitle(value);
            }
        }

        public bool Fullscreen
        {
            get => GamePlatform.IsFullscreen();
            set
            {
                if(GamePlatform.IsFullscreen() == value)
                {
                    return;
                }
                if(running)
                {
                    toggleFullscreenRequested = true;
                    
                }
                else
                {
                    GamePlatform.SetDisplayFullscreen(value);
                }

                fullscreen = value;
            }
        }

        public bool ShowCursor
        {
            get => showCursor;
            set
            {
                showCursor = value;
                GamePlatform.ShowCursor(showCursor);
            }
        }

        public GameScene CurrentScene { get; private set; }


        private bool running;
        private Size requestedDisplaySize;
        private bool displayResizeRequested;
        private bool toggleFullscreenRequested;
        private bool fullscreen;
        private bool isFixedTimeStep = true;
        private TimeSpan targetElapsedTime = TimeSpan.FromTicks((long)((1f/144) * TimeSpan.TicksPerSecond));
        private TimeSpan inactiveSleepTime = TimeSpan.FromSeconds(0.02);
        private TimeSpan maxElapsedTime = TimeSpan.FromMilliseconds(500);
        private TimeSpan accumElapsedTime;
        private readonly GameTime gameTime;
        private Stopwatch gameTimer;
        private long previousTicks;
        private int updateFrameLag;
        private bool showCursor = true;
        private readonly Graphics graphics;

        public Game()
        {
            GamePlatform.Initialize(800, 600, false);
            GamePlatform.OnQuit += OnGamePlatformQuit;
            GamePlatform.DisplayResized += OnGamePlatformDisplayResized;
            Assets.Initialize();
            GameScene.Game = this;
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            Input.Init();
            graphics = GamePlatform.Graphics;
            gameTime = new GameTime();

        }

        ~Game()
        {
            ReleaseResources();
        }

        public void Start(GameScene scene = null)
        {
            if(running)
            {
                return;
            }

            CurrentScene = scene;

            CurrentScene?.Init();

            CurrentScene?.Load();

            running = true;

            GamePlatform.ShowDisplay(true);

            Tick();

        }

        private void Tick()
        {
            gameTimer = Stopwatch.StartNew();

            while(running)
            {
                RetryTick:
                if (!GamePlatform.IsActive && inactiveSleepTime.TotalMilliseconds > 1.0)
                {
                    Thread.Sleep((int)inactiveSleepTime.TotalMilliseconds);
                }

                var currentTicks = gameTimer.Elapsed.Ticks;

                accumElapsedTime += TimeSpan.FromTicks(currentTicks - previousTicks);

                if (accumElapsedTime > maxElapsedTime)
                {
                    accumElapsedTime = maxElapsedTime;
                }

                previousTicks = currentTicks;

                if (isFixedTimeStep && accumElapsedTime < targetElapsedTime)
                {
                    goto RetryTick;
                }

                if (isFixedTimeStep)
                {
                    gameTime.ElapsedGameTime = targetElapsedTime;

                    var stepCount = 0;

                    while (accumElapsedTime >= targetElapsedTime && running)
                    {
                        gameTime.TotalGameTime += targetElapsedTime;
                        accumElapsedTime -= targetElapsedTime;
                        ++stepCount;

                        GamePlatform.ProcessEvents();

                        Input.Update();

                        CurrentScene.Update(gameTime);

                        Input.PostUpdate();

                    }

                    updateFrameLag += Math.Max(0, stepCount - 1);

                    if (gameTime.IsRunningSlowly)
                    {
                        if (updateFrameLag == 0)
                        {
                            gameTime.IsRunningSlowly = false;
                        }
                    }
                    else if (updateFrameLag >= MaxUpdateFrameLag)
                    {
                        gameTime.IsRunningSlowly = true;
                    }

                    if (stepCount == 1 && updateFrameLag > 0)
                    {
                        updateFrameLag--;
                    }

                    gameTime.ElapsedGameTime = TimeSpan.FromTicks(targetElapsedTime.Ticks * stepCount);
                }
                else 
                {
                    gameTime.ElapsedGameTime = accumElapsedTime;
                    gameTime.TotalGameTime += accumElapsedTime;
                    accumElapsedTime = TimeSpan.Zero;

                    GamePlatform.ProcessEvents();

                    Input.Update();

                    CurrentScene.Update(gameTime);

                    Input.PostUpdate();

                }

                if (toggleFullscreenRequested)
                {
                    toggleFullscreenRequested = false;
                    GamePlatform.SetDisplayFullscreen(fullscreen);
                }
                else if (displayResizeRequested)
                {
                    displayResizeRequested = false;
                    GamePlatform.SetDisplaySize(requestedDisplaySize.Width, requestedDisplaySize.Height);
                }

                graphics.Begin();

                CurrentScene.Draw(graphics);

                graphics.End();

            }

#if DEBUG

            var gen0 = GC.CollectionCount(0);
            var gen1 = GC.CollectionCount(1);
            var gen2 = GC.CollectionCount(2);

            Console.WriteLine(
                $"Gen-0: {gen0.ToString()} | Gen-1: {gen1.ToString()} | Gen-2: {gen2.ToString()}"
            );
#endif
        }

        public void Quit()
        {
            running = false;
        }

        public void ToggleFullscreen()
        {
            this.Fullscreen = !this.Fullscreen;
        }

        public void Dispose()
        {
            ReleaseResources();
            GC.SuppressFinalize(this);
        }

        private void ReleaseResources()
        {
            Assets.Free();
            GamePlatform.Shutdown();
        }

        private void OnGamePlatformDisplayResized(object sender, Size e)
        {
            throw new NotImplementedException();
        }

        private void OnGamePlatformQuit(object sender, EventArgs e)
        {
            Quit();
        }
    }
}

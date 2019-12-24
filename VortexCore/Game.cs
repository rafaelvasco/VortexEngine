using System;
using System.Diagnostics;
using System.Runtime;
using System.Threading;

namespace VortexCore {
    public abstract class Game : IDisposable {

        private bool running;
        private Size requestedDisplaySize;
        private bool displayResizeRequested;
        private bool toggleFullscreenRequested;
        private bool fullscreen;
        private Stopwatch gameTimer;
        private long previousFrameTicks;
        private bool showCursor = true;
        private readonly Graphics graphics;

        public Size DisplaySize {
            get => GamePlatform.GetDisplaySize ();
            set {
                var currentSize = GamePlatform.GetDisplaySize ();

                if (value.Width == currentSize.Width && value.Height == currentSize.Height) {
                    return;
                }

                if (running) {
                    displayResizeRequested = true;
                    requestedDisplaySize = value;
                } else {
                    GamePlatform.SetDisplaySize (value.Width, value.Height);
                }
            }
        }

        public string Title {
            get => GamePlatform.GetDisplayTitle ();
            set {
                GamePlatform.SetDisplayTitle (value);
            }
        }

        public bool Fullscreen {
            get => GamePlatform.IsFullscreen ();
            set {
                if (GamePlatform.IsFullscreen () == value) {
                    return;
                }
                if (running) {
                    toggleFullscreenRequested = true;

                } else {
                    GamePlatform.SetDisplayFullscreen (value);
                }

                fullscreen = value;
            }
        }

        public bool ShowCursor {
            get => showCursor;
            set {
                showCursor = value;
                GamePlatform.ShowCursor (showCursor);
            }
        }

        public bool LimitFrameRate { get; set; } = true;

        public double DesiredFrameRate { get; set; } = 60.0;

        public float TimeScale { get; set; } = 1.0f;

        public Game () {
            GamePlatform.Initialize (800, 600, false);
            GamePlatform.OnQuit += OnGamePlatformQuit;
            GamePlatform.DisplayResized += OnGamePlatformDisplayResized;
            Assets.Initialize ();
            GameScene.Game = this;
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            Input.Init ();
            graphics = GamePlatform.Graphics;

        }

        ~Game () {
            ReleaseResources ();
        }

        public void Start () {
            if (running) {
                return;
            }

            GamePlatform.ShowDisplay (true);

            Load();

#if RELEASE
            try {
#endif
                Tick ();

#if RELEASE
            } catch (Exception e) {
                LogException (e);
            }
#endif
        }

        private void LogException (Exception e) {
            CrashLogHelper.LogToFile (e, this);
        }

        private void Tick () {

            running = true;

            gameTimer = Stopwatch.StartNew ();

            while (running) {

                double desiredFrameTime = 1000.0 / DesiredFrameRate;

                long currentFrameTicks = gameTimer.ElapsedTicks;

                double deltaMilliseconds = (currentFrameTicks - previousFrameTicks) * (1000.0 / Stopwatch.Frequency);

                while (LimitFrameRate && deltaMilliseconds < desiredFrameTime) {
                    Thread.Sleep (0);
                    currentFrameTicks = gameTimer.ElapsedTicks;
                    deltaMilliseconds = (currentFrameTicks - previousFrameTicks) * (1000.0 / Stopwatch.Frequency);
                }

                previousFrameTicks = currentFrameTicks;

                float deltaSeconds = (float) (deltaMilliseconds) / 1000.0f;

                GamePlatform.ProcessEvents ();

                Input.Update ();

                Update(deltaSeconds * TimeScale);

                Input.PostUpdate ();

                if (toggleFullscreenRequested) {
                    toggleFullscreenRequested = false;
                    GamePlatform.SetDisplayFullscreen (fullscreen);
                } else if (displayResizeRequested) {
                    displayResizeRequested = false;
                    GamePlatform.SetDisplaySize (requestedDisplaySize.Width, requestedDisplaySize.Height);
                }

                graphics.Begin ();

                Draw(graphics);

                graphics.End ();

            }

#if DEBUG

            var gen0 = GC.CollectionCount (0);
            var gen1 = GC.CollectionCount (1);
            var gen2 = GC.CollectionCount (2);

            Console.WriteLine (
                $"Gen-0: {gen0.ToString()} | Gen-1: {gen1.ToString()} | Gen-2: {gen2.ToString()}"
            );
#endif
        }

        public void Quit () {
            running = false;
        }

        public void ToggleFullscreen () {
            this.Fullscreen = !this.Fullscreen;
        }

        public void Dispose () {
            ReleaseResources ();
            GC.SuppressFinalize (this);
        }

        private void ReleaseResources () {
            Assets.Free ();
            GamePlatform.Shutdown ();
        }

        protected abstract void Load();

        protected abstract void Update(float dt);

        protected abstract void Draw(Graphics graphics);

        private void OnGamePlatformDisplayResized (object sender, Size e) {
            throw new NotImplementedException ();
        }

        private void OnGamePlatformQuit (object sender, EventArgs e) {
            Quit ();
        }
    }
}
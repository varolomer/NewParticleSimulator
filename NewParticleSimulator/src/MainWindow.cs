using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using NewParticleSimulator.Render;
using NewParticleSimulator.UserInterface;
using Newtonian_Particle_Simulator.src;
using ImGuiNET;

namespace NewParticleSimulator
{
    public class MainWindow : GameWindow
    {
        ImGuiController ImguiController;
        ViewportRenderer ViewportRenderer;

        public MainWindow() 
            : base(832, 832, new GraphicsMode(0, 0, 0, 0), "NewParticleSimulator") { /*WindowState = WindowState.Fullscreen;*/ }

        private readonly Camera camera = new Camera(new Vector3(0, 0, 15), new Vector3(0, 1, 0));
        private Matrix4 projection;

        private int frames = 0, FPS;
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //Update ImGui controller
            KeyboardManager.Update();
            MouseManager.Update();
            ImguiController.Update(this, (float)e.Time);

            m_ParticleSimulator.Run((float)e.Time);

            //Start the dock space
            this.ImguiController.StartDockspace("MeshOpenGL Dockspace");

            Error.Check();
            ImGui.ShowDemoWindow();

            Error.Check();
            this.ViewportRenderer.DrawViewportWindow();

            Error.Check();
            this.ImguiController.EndDockspace();

            this.ImguiController.Render();

            ImGuiController.CheckGLError("End of frame");

            SwapBuffers();
            frames++;
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (fpsTimer.ElapsedMilliseconds >= 1000)
            {
                FPS = frames;
                Title = $"Newtonian-Particle-Simulator FPS: {FPS}";
                frames = 0;
                fpsTimer.Restart();
            }

            if (Focused)
            {
                KeyboardManager.Update();
                MouseManager.Update();

                if (KeyboardManager.IsKeyTouched(Key.V))
                    VSync = VSync == VSyncMode.Off ? VSyncMode.On : VSyncMode.Off;

                if (KeyboardManager.IsKeyTouched(Key.E))
                {
                    CursorVisible = !CursorVisible;
                    CursorGrabbed = !CursorGrabbed;

                    if (!CursorVisible)
                    {
                        MouseManager.Update();
                        camera.Velocity = Vector3.Zero;
                    }
                }

                if (KeyboardManager.IsKeyTouched(Key.F11))
                    WindowState = WindowState == WindowState.Normal ? WindowState.Fullscreen : WindowState.Normal;


                m_ParticleSimulator.ProcessInputs(this, camera.Position, camera.View, projection);
                if (!CursorVisible)
                    camera.ProcessInputs((float)e.Time);
            }

            if (KeyboardManager.IsKeyDown(Key.Escape))
                Close();

            base.OnUpdateFrame(e);
        }

        private readonly Stopwatch fpsTimer = Stopwatch.StartNew();
        public static ParticleSimulator m_ParticleSimulator;
        protected override void OnLoad(EventArgs e)
        {
            this.ViewportRenderer = new ViewportRenderer(this);

            //Create user interface controller
            this.ImguiController = new ImGuiController(ClientSize.Width, ClientSize.Height);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            VSync = VSyncMode.On;

            Console.WriteLine($"OpenGL: {Helper.APIVersion}");
            Console.WriteLine($"GLSL: {GL.GetString(StringName.ShadingLanguageVersion)}");
            Console.WriteLine($"GPU: {GL.GetString(StringName.Renderer)}");

            if (!Helper.IsCoreExtensionAvailable("GL_ARB_direct_state_access", 4.5))
                throw new NotSupportedException("Your system does not support GL_ARB_direct_state_access");

            if (!Helper.IsCoreExtensionAvailable("GL_ARB_buffer_storage", 4.4))
                throw new NotSupportedException("Your system does not support GL_ARB_buffer_storage");
            
            GL.PointSize(1.1f);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Random rng = new Random();
            Particle[] particles = new Particle[InputManager.NoOfParticles];
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Position = new Vector3((float)rng.NextDouble() * 100 - 50, (float)rng.NextDouble() * 100 - 50, -(float)rng.NextDouble() * 100);
                //particles[i].Position = Helper.RandomUnitVector(rng) * 50.0f;
            }
            
            m_ParticleSimulator = new ParticleSimulator(particles);

            Error.Check();

            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (Width != 0 && Height != 0)
            {
                GL.Viewport(0, 0, Width, Height);
                projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(103.0f), (float)Width / Height, 0.1f, 1000f);
            }

            // Tell ImGui of the new size
            ImguiController.WindowResized(ClientSize.Width, ClientSize.Height);

            base.OnResize(e);
        }
        protected override void OnFocusedChanged(EventArgs e)
        {
            if (Focused)
                MouseManager.Update();
        }
    }
}

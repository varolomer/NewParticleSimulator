using ImGuiNET;
using NewParticleSimulator;
using NewParticleSimulator.Render;
using Newtonian_Particle_Simulator.src;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewParticleSimulator.UserInterface;

public class ViewportRenderer : IDisposable
{
    int fbo;
    int rbo;
    int texColor;
    //int texDepth;
    Vector2 fboSize = default;
    private readonly MainWindow window;

    public ViewportRenderer(MainWindow window)
    {
        this.window = window;
    }

    public void DrawViewportWindow()
    {
        Error.Check();

        ImGui.Begin("Viewport");
        {

            // Using a Child allow to fill all the space of the window.
            // It also alows customization
            ImGui.BeginChild("GameRender");


            // Get the size of the child (i.e. the whole draw size of the windows).
            System.Numerics.Vector2 wsize = ImGui.GetWindowSize();

            // make sure the buffers are the currect size
            Vector2 wsizei = new((int)wsize.X, (int)wsize.Y);
            if (fboSize != wsizei)
            {
                fboSize = wsizei;

                // create our frame buffer if needed
                if (fbo == 0)
                {
                    fbo = GL.GenFramebuffer();
                    // bind our frame buffer
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
                    GL.ObjectLabel(ObjectLabelIdentifier.Framebuffer, fbo, 10, "GameWindow");
                }

                // bind our frame buffer
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);

                if (texColor > 0)
                    GL.DeleteTexture(texColor);

                texColor = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texColor);
                GL.ObjectLabel(ObjectLabelIdentifier.Texture, texColor, 16, "GameWindow:Color");
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, (int)wsizei.X, (int)wsizei.Y, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texColor, 0);

                if (rbo > 0)
                    GL.DeleteRenderbuffer(rbo);

                rbo = GL.GenRenderbuffer();
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
                GL.ObjectLabel(ObjectLabelIdentifier.Renderbuffer, rbo, 16, "GameWindow:Depth");
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32f, (int)wsizei.X, (int)wsizei.Y);
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, rbo);
                //GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

                //texDepth = GL.GenTexture();
                //GL.BindTexture(TextureTarget.Texture2D, texDepth);
                //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32f, 800, 600, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
                //GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, texDepth, 0);

                // make sure the frame buffer is complete
                FramebufferErrorCode errorCode = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
                if (errorCode != FramebufferErrorCode.FramebufferComplete)
                    throw new Exception();
            }
            else
            {
                // bind our frame and depth buffer
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
            }

            Error.Check();
            GL.Viewport(0, 0, (int)wsizei.X, (int)wsizei.Y); // change the viewport to window

            // actually draw the scene
            {
                DrawSeceneForViewport((int)wsizei.X, (int)wsizei.Y);
            }

            // unbind our bo so nothing else uses it
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            GL.Viewport(0, 0, window.ClientSize.Width, window.ClientSize.Height); // back to full screen size

            Error.Check();
            // Because I use the texture from OpenGL, I need to invert the V from the UV.
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texColor);
            ImGui.Image(new IntPtr(texColor), wsize, System.Numerics.Vector2.UnitY, System.Numerics.Vector2.UnitX);
            //ImGui.Image(new IntPtr(texColor), wsize);
            //ImGui.Image(new IntPtr(texColor), wsize, new System.Numerics.Vector2(0, 1));

            Error.Check();
            ImGui.EndChild();
        }
        ImGui.End();
    }

    private void DrawSeceneForViewport(int x, int y)
    {
        GL.ClearColor(Color4.White);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        //Draw the view associated with this viewport
        MainWindow.m_ParticleSimulator.Run(0.01f);

    }

    public void Dispose()
    {
        GL.DeleteFramebuffer(fbo);
    }
}

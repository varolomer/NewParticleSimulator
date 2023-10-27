using OpenTK.Graphics.OpenGL;
using System;

namespace NewParticleSimulator
{
    class Program
    {
        static void Main()
        {
            MainWindow gameWindow = new MainWindow();
            gameWindow.Run(OpenTK.DisplayDevice.Default.RefreshRate);
        }
    }

    static class Error
    {
        public static void Check()
        {
            ErrorCode errorCode = GL.GetError();
            if (errorCode != ErrorCode.NoError)
            {
                throw new InvalidOperationException();
            }
        }
    }
}

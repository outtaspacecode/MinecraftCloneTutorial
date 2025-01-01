using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinecraftCloneTutorial {
    internal class Game : GameWindow {
        
        private int width, height;
        
        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default) {

            this.width = width;
            this.height = height;
            CenterWindow();

        }
        
    }
}
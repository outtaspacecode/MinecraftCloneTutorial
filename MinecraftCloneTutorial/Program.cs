﻿namespace MinecraftCloneTutorial {
    class Program {
        static void Main(string[] args) {
            using (Game game = new Game(1000, 600)) {
                game.Run();
            }
        }
    }
}
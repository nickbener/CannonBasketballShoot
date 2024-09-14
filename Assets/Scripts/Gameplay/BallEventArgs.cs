using System;

namespace Gameplay
{
    public class BallEventArgs : EventArgs
    {
        public Ball Ball { get; private set; }

        public BallEventArgs(Ball ball)
        {
            Ball = ball;
        }
    }
}
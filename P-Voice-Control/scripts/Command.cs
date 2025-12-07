using System;

namespace PVoz
{
    [Serializable]
    public struct Command
    {
        public enum CommandType
        {
            MoveForward,
            MoveBackward,
            StopMovement,
            TurnLeft,
            TurnRight
        }
        
        public string commandText;
        public CommandType commandType;
    }
}
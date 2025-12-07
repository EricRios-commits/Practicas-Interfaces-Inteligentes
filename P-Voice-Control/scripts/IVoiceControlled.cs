namespace PVoz
{
    public interface IVoiceControlled
    {
        public void MoveForward();
        public void MoveBackward();
        public void StopMovement();
        public void TurnLeft();
        public void TurnRight();
    }
}
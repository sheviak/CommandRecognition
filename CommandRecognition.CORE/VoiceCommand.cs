namespace CommandRecognition.CORE
{
    public class VoiceCommand : BaseEntity
    {
        public string Command { get; set; }
        public string Path { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
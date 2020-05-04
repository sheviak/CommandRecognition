using System.Collections.Generic;

namespace CommandRecognition.CORE
{
    public class User : BaseEntity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public ICollection<VoiceCommand> VoiceCommands { get; set; }
    }
}
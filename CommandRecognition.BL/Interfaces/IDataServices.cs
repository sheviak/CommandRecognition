using CommandRecognition.CORE;
using System.Collections.Generic;

namespace CommandRecognition.BL.Interfaces
{
    public interface IDataServices
    {
        IEnumerable<VoiceCommand> GetRecCommand(int userId);
        VoiceCommand AddRecCommand(VoiceCommand recCommand);
        void DeleteRecCommand(int id);
        void UpdateRecCommand(VoiceCommand recCommand);
    }
}
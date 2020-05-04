using CommandRecognition.BL.Interfaces;
using CommandRecognition.CORE;
using CommandRecognition.DAL.Interface;
using System.Collections.Generic;

namespace CommandRecognition.BL.Services
{
    public class DataServices : IDataServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public DataServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<VoiceCommand> GetRecCommand(int userId)
        {
            return _unitOfWork.Repository<VoiceCommand>().Get(x => x.UserId == userId, null, null);
        }

        public VoiceCommand AddRecCommand(VoiceCommand recCommand)
        {
            _unitOfWork.Repository<VoiceCommand>().Insert(recCommand);

            return recCommand;
        }

        public void DeleteRecCommand(int id)
        {
            _unitOfWork.Repository<VoiceCommand>().Delete(id);
        }

        public void UpdateRecCommand(VoiceCommand editRecCommand)
        {
            _unitOfWork.Repository<VoiceCommand>().Update(editRecCommand);
        }
    }
}
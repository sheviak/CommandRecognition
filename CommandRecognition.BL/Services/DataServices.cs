using CommandRecognition.BL.Interfaces;
using CommandRecognition.CORE;
using CommandRecognition.DAL.Interface;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public void AddDefaultDataForUser(int userId)
        {
            string fileName = "data.json";

            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);
                var temp = JsonConvert.DeserializeObject<IEnumerable<VoiceCommand>>(json);
                if (temp.Any())
                {
                    foreach (var item in temp)
                    {
                        _unitOfWork.Repository<VoiceCommand>().Insert(new VoiceCommand
                        {
                            UserId = userId,
                            Command = item.Command, 
                            Path = item.Path
                        });
                    }
                }
            }
        }
    }
}
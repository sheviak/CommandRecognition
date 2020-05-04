using CommandRecognition.CORE;

namespace CommandRecognition.BL.Interfaces
{
    public interface IAccountServices
    {
        (bool status, string message) CreateAccount(string login, string pass, string confPass, string question, string answer);
        string HashPassword(string login, string password);
        User AutorizeUser(string login, string password);
    }
}
using CommandRecognition.BL.Interfaces;
using CommandRecognition.CORE;
using CommandRecognition.DAL.Interface;
using System.Security.Cryptography;
using System.Text;

namespace CommandRecognition.BL.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public (bool status, string message, int? id) CreateAccount(string login, string pass, string confPass, string question, string answer)
        {
            if (!CheckFields(login)) return (false, "Введите логин!", null);

            var user = _unitOfWork.Repository<User>().Get(x => x.Login == login);

            if (user == null)
            {
                if (CheckEqualPasswords(pass, confPass))
                {
                    if (CheckFields(answer) && CheckFields(question))
                    {
                        login = login.Trim();
                        pass = pass.Trim();
                        confPass = confPass.Trim();
                        question = question.Trim();
                        answer = answer.Trim().ToUpper();

                        var hashPass = HashPassword(login, pass);

                        var userApp = new User
                        {
                            Login = login,
                            Password = hashPass,
                            Question = question,
                            Answer = answer
                        };

                        _unitOfWork.Repository<User>().Insert(userApp);

                        return (true, "Вы успешно зарегистрированы!", userApp.Id);
                    }
                    else return (false, "Заполните поля \"Контрольный вопрос\" и \"Ответ на контрольный вопрос\"!", null);
                }
                else return (false, "Пароли не совпадают!", null);
            }
            else return (false, "Данный логин уже используется!", null);
        }

        public User AutorizeUser(string login, string password)
        {
            if (CheckFields(login) && CheckFields(password))
            {
                var hashpass = HashPassword(login, password);
                var user = _unitOfWork.Repository<User>().Get(x => x.Login == login && x.Password == hashpass);
                return user;
            }

            return null;
        }

        public string HashPassword(string login, string password)
        {
            SHA256 sha256 = new SHA256CryptoServiceProvider();

            //compute hash from the bytes of text
            sha256.ComputeHash(ASCIIEncoding.ASCII.GetBytes(login + password));

            //get hash answer after compute it
            byte[] answer = sha256.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < answer.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(answer[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        private bool CheckFields(string data)
        {
            if (data == null || data == "") return false;
            else return true;
        }

        private bool CheckEqualPasswords(string password, string confPassword)
        {
            if (CheckFields(password) && CheckFields(confPassword))
                if (password == confPassword)
                    return true;
            return false;
        }

        public (bool result, User user) CheckUserLogin(string login)
        {
            var user = _unitOfWork.Repository<User>().Get(x => x.Login == login);

            return user == null ? (result: false, user: null) : (result: true, user: user);
        }

        public void ChangePassword(User user, string newpass)
        {
            user.Password = HashPassword(user.Login, newpass);
            _unitOfWork.Repository<User>().Update(user);
            //return (status: true, message: "Пароль был успешно изменен!");
        }
    }
}
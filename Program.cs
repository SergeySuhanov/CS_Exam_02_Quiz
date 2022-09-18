using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CS_Exam_02_Quiz
{
    
    public class User
    {
        public string   Name;
        public string   Password;
        public DateTime BirthDate;
    }

    class Scoreboard
    {
        public string UserName;
        public string Discipline;
        public int    Score;
    }

    class Question
    {
        public int      QID;
        public string   Discipline;
        public string   QuestionText;
        public string[] AnswerText;
        public bool[]   IsCorrect;
        public string   FileName;
        public Question(int qid)
        {
            AnswerText = new string[4];
            IsCorrect  = new bool[4];
            FileName   = $"Questions\\{QID}_{Discipline}.xml";
        }
    }

    class Quiz
    {
        public List<User>       Users;
        public List<Scoreboard> Scoreboards;
        public List<Question>   QuestionsPack;
        public string           RootFolder;

        public Quiz()
        {
            Users         = new List<User>();
            Scoreboards   = new List<Scoreboard>();
            QuestionsPack = new List<Question>();
            RootFolder    = @"C:\CS_Exam_02_Quiz";
        }

        public void LoadUsersList()
        {
            string filePath = Directory.GetCurrentDirectory();
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<User>));
            try
            {
                using (Stream fStream = File.OpenRead($"{RootFolder}\\Users.xml"))
                {
                    this.Users = (List<User>)xmlFormat.Deserialize(fStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void SaveUsersList()
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<User>));
            try
            {
                using (Stream fStream = File.Create($"{RootFolder}\\Users.xml"))
                {
                    xmlFormat.Serialize(fStream, this.Users);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public int CheckUsersList(string userName)
        {
            if (userName == "")
            {
                Console.WriteLine("Введено пустое поле!");
                return -2;
            }
            if (Users != null)
            {
                for (int i = 0; i < this.Users.Count; i++)
                {
                    if (this.Users[i].Name == userName)
                    {
                        //Console.WriteLine("Пользователь с таким именем уже существует!");
                        return i;
                    }
                }
            }
            Console.WriteLine("Ок. Имя свободно");
            return -1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool inGame = true;
            while (inGame)
            {
                Quiz quiz = new Quiz();
                quiz.LoadUsersList();
                Console.WriteLine("Викторина");
                Console.WriteLine("Войдите или Зарегистрируйтесь:");
                Console.WriteLine("1 - Войти по лонину и паролю");
                Console.WriteLine("2 - Регистрация нового пользователя");
                int logreg = Int32.Parse(Console.ReadLine());

                switch (logreg)
                {
                    case 1:
                        string inputName;
                        string inputPass;
                        Console.WriteLine("Вход в систему");
                        do
                        {
                            Console.Write("Введите логин: ");
                            inputName = Console.ReadLine();
                        } while (quiz.CheckUsersList(inputName) < 0);

                        do
                        {
                            Console.Write("Введите пароль: ");
                            inputPass = Console.ReadLine();
                        } while (quiz.Users[quiz.CheckUsersList(inputName)].Password != inputPass);
                        Console.Write("пароль OK");
                        Console.ReadLine();
                        inGame = false;
                        break;
                    case 2:
                        User newUser = new User();
                        string newUserName;
                        string newUserPassword;
                        DateTime newUserBirthDay;
                        Console.WriteLine("Регистрация");
                        do
                        {
                            Console.Write("Введите логин: ");
                            newUserName = Console.ReadLine();
                            newUser.Name = newUserName;
                        } while (quiz.CheckUsersList(newUserName) != -1);

                        Console.Write("Введите пароль: ");
                        newUserPassword = Console.ReadLine();
                        newUser.Password = newUserPassword;

                        Console.Write("Введите дату рождения: ");
                        newUserBirthDay = DateTime.Parse(Console.ReadLine());
                        newUser.BirthDate = newUserBirthDay;

                        quiz.Users.Add(newUser);
                        quiz.SaveUsersList();
                        inGame = false;
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine("Экран Игры");
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1 - Стартовать викторину");
            Console.WriteLine("2 - Смотреть последние результаты");
            Console.WriteLine("3 - Смотреть Топ-20 игроков");
            Console.WriteLine("4 - Редактировать профиль");

            Console.ReadLine();
        }
    }
}



/*quiz.LoadUsersList();
                    foreach (User user in quiz.Users)
                    {
                        Console.WriteLine(user.Name);
                    }*/
//open file stream
/*using (StreamWriter file = File.CreateText($"{filePath}\\users.json"))
{
    JsonSerializer serializer = new JsonSerializer();
    //serialize object directly into file stream
    serializer.Serialize(file, quiz.Users);
}

using (JsonReader file = new JsonReader($"{filePath}\\users.json"))
{
    JsonSerializer serializer = new JsonSerializer();
    //serialize object directly into file stream
    quiz.Users = JsonSerializer.Deserialize< List<User>>(json);
}*/
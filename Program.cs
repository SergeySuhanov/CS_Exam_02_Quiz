using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Exam_02_Quiz
{



    class User
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

    class Answer
    {
        string Text;
        bool   isCorrect;
    }
    class Question
    {
        string Discipline;
        string QuizText;
        List<Answer> Answers;
    }

    class Quiz
    {
        public List<User> Users;
        public List<Scoreboard> Scoreboards;
        public List<Question> QuestionsPack;

        public Quiz()
        {
            List<User> Users = new List<User>();
            List<Scoreboard> Scoreboards = new List<Scoreboard>();
            List<Question> QuestionsPack = new List<Question>();
        }
        public bool CheckFreeUserName(string newUserName)
        {
            if (newUserName == "")
            {
                Console.WriteLine("Введено пустое поле!");
                return false;
            }
            if (Users != null)
            {
                foreach (User existingUser in Users)
                {
                    if (existingUser.Name == newUserName)
                    {
                        Console.WriteLine("Пользователь с таким именем уже существует!");
                        return false;
                    }
                }
            }
            Console.WriteLine("Ок. Имя свободно");
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Quiz quiz = new Quiz();
            Console.WriteLine("Викторина");
            Console.WriteLine("Войдите или Зарегистрируйтесь:");
            Console.WriteLine("1 - Войти по лонину и паролю");
            Console.WriteLine("2 - Регистрация нового пользователя");
            int logreg = Int32.Parse(Console.ReadLine());

            switch (logreg)
            {
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
                    } while (!quiz.CheckFreeUserName(newUserName));

                    Console.Write("Введите пароль: ");
                    newUserPassword = Console.ReadLine();
                    newUser.Password = newUserPassword;

                    Console.Write("Введите дату рождения: ");
                    newUserBirthDay = DateTime.Parse(Console.ReadLine());
                    newUser.BirthDate = newUserBirthDay;

                    quiz.Users.Add(newUser);

                    using (FileStream fs = new FileStream("users.json", FileMode.OpenOrCreate))
                    {
                        JsonSerializer.Serialize(fs, quiz.Users);
                        Console.WriteLine("Data has been saved to file");
                    }
                    break;
                default:
                    break;
            }


            Console.ReadLine();
        }
    }
}

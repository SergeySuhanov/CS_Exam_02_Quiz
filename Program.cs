//using Newtonsoft.Json;
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

    public class Score
    {
        public string   UserName;
        public string   Discipline;
        public int      Result;
        public DateTime Date;
    }

    public class Question
    {
        public int      QID;
        public string   Discipline;
        public string   QuestionText;
        public string[] AnswerText;
        public bool[]   IsCorrect;
        public Question()
        {
            AnswerText = new string[4];
            IsCorrect  = new bool[4];
        }

        public bool AskQuestion()
        {
            bool forNowIsCorrect = false;
            Console.WriteLine(QuestionText);
            for (int i = 0; i < AnswerText.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {AnswerText[i]}");
            }
            int choice = Int32.Parse(Console.ReadLine());
            for (int i = 0; i < IsCorrect.Length; i++)
            {
                if (IsCorrect[i] == true)
                {
                    if (choice - 1 == i)
                        forNowIsCorrect = true;
                    else
                    {
                        forNowIsCorrect = false;
                        break;
                    }
                }
                else
                {
                    if (choice - 1 == i)
                    {
                        forNowIsCorrect = false;
                        break;
                    }
                }
            }
            return forNowIsCorrect;
        }
    }

    class Quiz
    {
        public List<User>     Users;
        public List<Score>    Scoreboard;
        public List<Question> QuestionsPack;
        public string         RootFolder;

        public Quiz()
        {
            Users         = new List<User>();
            Scoreboard    = new List<Score>();
            QuestionsPack = new List<Question>();
            RootFolder    = @"C:\CS_Exam_02_Quiz";
        }

        public void LoadUsersList()
        {
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

        public void LoadQuestion(string filePath)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(Question));
            try
            {
                using (Stream fStream = File.OpenRead(filePath))
                {
                    QuestionsPack.Add((Question)xmlFormat.Deserialize(fStream));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void LoadScoreboard()
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Score>));
            try
            {
                using (Stream fStream = File.OpenRead($"{RootFolder}\\Scoreboard.xml"))
                {
                    Scoreboard = (List<Score>)xmlFormat.Deserialize(fStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void SaveScoreboard()
        {
            // Sort Entries
            Score swap = new Score();
            for (int i = 0; i < Scoreboard.Count; i++)
            {
                for (int j = Scoreboard.Count - 1; j > i; j--)
                {
                    if (Scoreboard[j - 1].Result < Scoreboard[j].Result)
                    {
                        swap = Scoreboard[j - 1];
                        Scoreboard[j - 1] = Scoreboard[j];
                        Scoreboard[j] = swap;
                    }
                    if (Scoreboard[j - 1].Result == Scoreboard[j].Result)
                    {
                        if (Scoreboard[j - 1].Date > Scoreboard[j].Date)
                        {
                            swap = Scoreboard[j - 1];
                            Scoreboard[j - 1] = Scoreboard[j];
                            Scoreboard[j] = swap;
                        }
                    }
                }
            }

            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Score>));
            try
            {
                using (Stream fStream = File.Create($"{RootFolder}\\Scoreboard.xml"))
                {
                    xmlFormat.Serialize(fStream, Scoreboard);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Quiz quiz = new Quiz();
            quiz.LoadUsersList();
            quiz.LoadScoreboard();
            User player = new User();
            int userIndex = -1;

            bool inGame = true;
            while (inGame)
            {
                
                Console.WriteLine("\tВикторина");
                Console.WriteLine("Войдите или Зарегистрируйтесь:\n");
                Console.WriteLine("1 - Войти по лонину и паролю");
                Console.WriteLine("2 - Регистрация нового пользователя");
                int logreg = Int32.Parse(Console.ReadLine());

                switch (logreg)
                {
                    case 1:
                        string inputName;
                        string inputPass;
                        Console.WriteLine("Вход в систему\n");
                        do
                        {
                            Console.Write("Введите логин: ");
                            inputName = Console.ReadLine();
                            userIndex = quiz.CheckUsersList(inputName);
                        } while (userIndex < 0);

                        player = quiz.Users[userIndex];

                        do
                        {
                            Console.Write("Введите пароль: ");
                            inputPass = Console.ReadLine();
                        } while (player.Password != inputPass);
                        Console.Write("пароль OK");
                        Console.ReadLine();
                        inGame = false;
                        break;
                    case 2:
                        User newUser = new User();
                        string newUserName;
                        string newUserPassword;
                        DateTime newUserBirthDay;
                        Console.WriteLine("\tРегистрация\n");
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
                        player = newUser;
                        inGame = false;
                        break;
                    default:
                        break;
                }
            }

            int score = 0;
            Console.WriteLine("\tЭкран Игры");
            Console.WriteLine("Выберите действие:\n");
            Console.WriteLine("1 - Стартовать викторину");
            Console.WriteLine("2 - Смотреть мои результаты");
            Console.WriteLine("3 - Смотреть Топ-20 игроков");
            Console.WriteLine("4 - Редактировать профиль");
            int gameScreen = Int32.Parse(Console.ReadLine());

            switch (gameScreen)
            {
                case 1:
                    string[] questions = Directory.GetFiles($"{quiz.RootFolder}\\Questions");
                    foreach (string file in questions)
                    {
                        quiz.LoadQuestion(file);
                    }
                    
                    foreach (Question q in quiz.QuestionsPack)
                    {
                        if (q.AskQuestion())
                            score++;
                    }

                    Score results = new Score();
                    results.UserName = player.Name;
                    results.Result = score;
                    results.Discipline = "Mixed";
                    results.Date = DateTime.Now;
                    quiz.Scoreboard.Add(results);
                    quiz.SaveScoreboard();
                    break;
                case 2:
                    Console.WriteLine("\tМои лучшие результаты");
                    Console.WriteLine("Score\t Discipline\t Date");
                    foreach (Score entry in quiz.Scoreboard)
                    {
                        if (entry.UserName == player.Name)
                        {
                            Console.WriteLine($"{entry.Result}\t {entry.Discipline}\t\t {entry.Date}");
                        }
                    }
                    break;
                case 3:
                    Console.WriteLine("\tТоп-20 результатов");
                    Console.WriteLine("Score\t Player\t Discipline\t Date");
                    for (int i = 0; i < quiz.Scoreboard.Count; i++)
                    {
                        if (i < 20)
                        {
                            Console.WriteLine($"{quiz.Scoreboard[i].Result}\t {quiz.Scoreboard[i].UserName}\t {quiz.Scoreboard[i].Discipline}\t\t {quiz.Scoreboard[i].Date}");
                        }
                    }
                    break;
                case 4:
                    Console.WriteLine("\tРедактор профиля");
                    Console.WriteLine("Выберите действие:\n");
                    Console.WriteLine("1 - Изменить пароль");
                    Console.WriteLine("2 - Изменить дату рождения");
                    int editChoice = Int32.Parse(Console.ReadLine());
                    switch (editChoice)
                    {
                        case 1:
                            Console.Write("Введите новый пароль: ");
                            player.Password = Console.ReadLine();
                            Console.WriteLine("Пароль принят");
                            break;
                        case 2:
                            Console.Write("Введите дату рождения: ");
                            player.BirthDate = DateTime.Parse(Console.ReadLine());
                            Console.WriteLine("Дата рождения изменена");
                            break;
                        default:
                            break;
                    }
                    quiz.Users[userIndex] = player;
                    quiz.SaveUsersList();
                    break;
                default:
                    break;
            }
            
            Console.WriteLine("Program ended!");
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
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
            Console.WriteLine($"\t{QuestionText}\n");

            for (int i = 0; i < AnswerText.Length; i++) {
                Console.WriteLine($"{i + 1} - {AnswerText[i]}");
            }

            int validAnswers = 0;
            foreach (bool var in IsCorrect) {
                if (var == true) { validAnswers++; }
            }
            Console.WriteLine($"\nПравильных ответов в этом вопросе: {validAnswers}");

            int[] choice = new int[validAnswers];
            bool[] compare = new bool[4];
            for (int i = 0; i < validAnswers; i++) {
                Console.Write($"Введите свой {i+1}-й вариант ответа: ");
                int guess = Int32.Parse(Console.ReadLine()) - 1;
                compare[guess] = true;
            }

            bool forNowIsCorrect = false;
            for (int i = 0; i < IsCorrect.Length; i++) {
                if (IsCorrect[i] == compare[i]) {
                    forNowIsCorrect = true;
                } else {
                    forNowIsCorrect = false;
                    break;
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
            bool in_menu_Lvl_1 = true;
            while (in_menu_Lvl_1)
            {
                Quiz quiz = new Quiz();
                quiz.LoadUsersList();
                quiz.LoadScoreboard();
                User player = new User();
                int userIndex = -1;

                bool in_menu_Lvl_2_1 = true;
                while (in_menu_Lvl_2_1)
                {
                    Console.Clear();
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
                            in_menu_Lvl_2_1 = false;
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
                            in_menu_Lvl_2_1 = false;
                            break;
                        default:
                            break;
                    }
                }

                bool in_menu_Lvl_2_2 = true;
                while (in_menu_Lvl_2_2)
                {
                    
                    Console.Clear();
                    Console.WriteLine("\tЭкран Игры");
                    Console.WriteLine("Выберите действие:\n");
                    Console.WriteLine("1 - Стартовать викторину");
                    Console.WriteLine("2 - Смотреть мои результаты");
                    Console.WriteLine("3 - Смотреть Топ-20 игроков");
                    Console.WriteLine("4 - Редактировать профиль");
                    Console.WriteLine("5 - Выход");
                    int gameScreen = Int32.Parse(Console.ReadLine());

                    switch (gameScreen)
                    {
                        case 1:
                            string path = $"{quiz.RootFolder}\\Questions";
                            string[] questions = Directory.GetFiles(path);
                            string qFile;
                            int score = 0;
                            Score results = new Score();

                            Console.Clear();
                            Console.WriteLine("Выберите категорию:");
                            Console.WriteLine("1 - История");
                            Console.WriteLine("2 - География");
                            Console.WriteLine("3 - Животные");
                            Console.WriteLine("4 - Смешаная");
                            int categChoice = Int32.Parse(Console.ReadLine());

                            switch (categChoice)
                            {
                                case 1:
                                    results.Discipline = "История";
                                    foreach (string file in questions)
                                    {
                                        qFile = file.Substring(path.Length + 6);
                                        if (qFile == "History.xml") { quiz.LoadQuestion(file); }
                                    }
                                    break;
                                case 2:
                                    results.Discipline = "География";
                                    foreach (string file in questions)
                                    {
                                        qFile = file.Substring(path.Length + 6);
                                        if (qFile == "Geography.xml") { quiz.LoadQuestion(file); }
                                    }
                                    break;
                                case 3:
                                    results.Discipline = "Животные";
                                    foreach (string file in questions)
                                    {
                                        qFile = file.Substring(path.Length + 6);
                                        if (qFile == "Animals.xml") { quiz.LoadQuestion(file); }
                                    }
                                    break;
                                case 4:
                                    results.Discipline = "Смешаная";
                                    Random random = new Random();
                                    List<string> qid = new List<string>();
                                    bool isSameQuestion = false;

                                    for (int i = 0; i < questions.Length; i++)
                                    {
                                        if (i < 20)
                                        {
                                            int qIndex = random.Next(0, questions.Length);
                                            qFile = questions[qIndex].Substring(path.Length + 1, 4);
                                            if (qid.Count == 0)
                                            {
                                                qid.Add(qFile);
                                                quiz.LoadQuestion(questions[qIndex]);
                                            }
                                            else
                                            {
                                                isSameQuestion = false;
                                                foreach (string id in qid)
                                                {
                                                    if (qFile == id) { isSameQuestion = true; }
                                                }

                                                if (!isSameQuestion)
                                                {
                                                    qid.Add(qFile);
                                                    quiz.LoadQuestion(questions[qIndex]);
                                                }
                                            }
                                        }  
                                    }
                                    break;
                                default:
                                    break;
                            }

                            foreach (Question q in quiz.QuestionsPack)
                            {
                                Console.Clear();
                                bool right_answer = q.AskQuestion();
                                if (right_answer == true) {
                                    Console.WriteLine("Это правильный ответ!");
                                    score++;
                                } else {
                                    Console.WriteLine("Ответ неверный!");
                                }
                                Console.ReadLine();
                            }

                            results.UserName = player.Name;
                            results.Result = score;
                            results.Date = DateTime.Now;
                            quiz.Scoreboard.Add(results);
                            quiz.SaveScoreboard();
                            Console.WriteLine($"Игра окончена со счётом {score}");
                            Console.ReadLine();
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("\tМои лучшие результаты");
                            Console.WriteLine("Score\t Discipline\t Date");
                            foreach (Score entry in quiz.Scoreboard)
                            {
                                if (entry.UserName == player.Name)
                                {
                                    Console.WriteLine($"{entry.Result}\t {entry.Discipline}\t {entry.Date}");
                                }
                            }
                            Console.WriteLine("\nнажмите любую клавишу...");
                            Console.ReadLine();
                            break;
                        case 3:
                            Console.Clear();
                            Console.WriteLine("\tТоп-20 результатов");
                            Console.WriteLine("Score\t Player\t Discipline\t Date");
                            for (int i = 0; i < quiz.Scoreboard.Count; i++)
                            {
                                if (i < 20)
                                {
                                    Console.WriteLine($"{quiz.Scoreboard[i].Result}\t {quiz.Scoreboard[i].UserName}\t {quiz.Scoreboard[i].Discipline}\t {quiz.Scoreboard[i].Date}");
                                }
                            }
                            Console.WriteLine("\nнажмите любую клавишу...");
                            Console.ReadLine();
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
                                    Console.ReadLine();
                                    break;
                                case 2:
                                    Console.Write("Введите дату рождения: ");
                                    player.BirthDate = DateTime.Parse(Console.ReadLine());
                                    Console.WriteLine("Дата рождения изменена");
                                    Console.ReadLine();
                                    break;
                                default:
                                    break;
                            }
                            quiz.Users[userIndex] = player;
                            quiz.SaveUsersList();
                            break;
                        case 5:
                            in_menu_Lvl_2_2 = false;
                            break;
                        default:
                            break;
                    }
                }
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
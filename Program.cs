using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Exam_02_Quiz
{
    class User
    {
        string Name;
        string Password;
        DateTime BirthDate;
    }

    class Scoreboard
    {
        public string UserName;
        int Score;
    }

    class Answer
    {
        string Text;
        bool isCorrect;
    }
    class Question
    {
        string Discipline;
        string QuizText;
        List<Answer> Answers;
    }

    class Quiz
    {
        List<User> Users;
        List<Scoreboard> Scoreboards;
        List<Question> Questions;
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}

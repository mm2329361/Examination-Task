using System;
using System.Collections.Generic;
using System.Linq;

namespace ExaminationSystem
{
    enum QuestionLevel
    {
        Easy = 1,
        Medium,
        Hard
    }

    abstract class Question
    {
        public string Header { get; set; }
        public int Marks { get; set; }
        public QuestionLevel Level { get; set; }

        public abstract void Display();
        public abstract int CheckAnswer();
    }

    class TrueFalseQuestion : Question
    {
        public bool CorrectAnswer { get; set; }

        public override void Display()
        {
            Console.WriteLine(Header);
            Console.WriteLine("1- True");
            Console.WriteLine("2- False");
        }

        public override int CheckAnswer()
        {
            int ans = int.Parse(Console.ReadLine());
            bool userAnswer = ans == 1;
            return userAnswer == CorrectAnswer ? Marks : 0;
        }
    }

    class ChooseOneQuestion : Question
    {
        public string[] Choices = new string[4];
        public int CorrectChoice;

        public override void Display()
        {
            Console.WriteLine(Header);
            for (int i = 0; i < 4; i++)
                Console.WriteLine($"{i + 1}- {Choices[i]}");
        }

        public override int CheckAnswer()
        {
            int ans = int.Parse(Console.ReadLine());
            return ans == CorrectChoice ? Marks : 0;
        }
    }

    class MultipleChoiceQuestion : Question
    {
        public string[] Choices = new string[4];
        public List<int> CorrectAnswers = new List<int>();

        public override void Display()
        {
            Console.WriteLine(Header);
            for (int i = 0; i < 4; i++)
                Console.WriteLine($"{i + 1}- {Choices[i]}");
            Console.WriteLine("Enter answers separated by comma (e.g. 1,3)");
        }

        public override int CheckAnswer()
        {
            var userAnswers = Console.ReadLine()
                .Split(',')
                .Select(int.Parse)
                .ToList();

            return userAnswers.All(a => CorrectAnswers.Contains(a)) &&
                   userAnswers.Count == CorrectAnswers.Count
                   ? Marks : 0;
        }
    }

    class Program
    {
        static List<Question> QuestionBank = new List<Question>();

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n1- Doctor Mode");
                Console.WriteLine("2- Student Mode");
                Console.WriteLine("3- Exit");
                Console.WriteLine("==================");
                Console.WriteLine("select a choice");

                int choice = int.Parse(Console.ReadLine());

                if (choice == 1) DoctorMode();
                else if (choice == 2) StudentMode();
                else if (choice == 3) break;
                else Console.WriteLine("invalid inbut");
            }
        }

        static void DoctorMode()
        {
            Console.Write("Enter number of questions: ");
            int n = int.Parse(Console.ReadLine());

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("\n1- True/False\n2- Choose One\n3- Multiple Choice");
                int type = int.Parse(Console.ReadLine());

                Console.Write("Enter Header: ");
                string header = Console.ReadLine();

                Console.Write("Enter Marks: ");
                int marks = int.Parse(Console.ReadLine());

                Console.Write("Level (1-Easy, 2-Medium, 3-Hard): ");
                QuestionLevel level = (QuestionLevel)int.Parse(Console.ReadLine());

                if (type == 1)
                {
                    var q = new TrueFalseQuestion
                    {
                        Header = header,
                        Marks = marks,
                        Level = level
                    };

                    Console.Write("Correct Answer (true/false): ");
                    q.CorrectAnswer = bool.Parse(Console.ReadLine());

                    QuestionBank.Add(q);
                }
                else if (type == 2)
                {
                    var q = new ChooseOneQuestion
                    {
                        Header = header,
                        Marks = marks,
                        Level = level
                    };

                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write($"Choice {j + 1}: ");
                        q.Choices[j] = Console.ReadLine();
                    }

                    Console.Write("Correct choice number: ");
                    q.CorrectChoice = int.Parse(Console.ReadLine());

                    QuestionBank.Add(q);
                }
                else
                {
                    var q = new MultipleChoiceQuestion
                    {
                        Header = header,
                        Marks = marks,
                        Level = level
                    };

                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write($"Choice {j + 1}: ");
                        q.Choices[j] = Console.ReadLine();
                    }

                    Console.Write("Correct answers (e.g. 1,3): ");
                    q.CorrectAnswers = Console.ReadLine()
                        .Split(',')
                        .Select(int.Parse)
                        .ToList();

                    QuestionBank.Add(q);
                }
            }
        }

        static void StudentMode()
        {
            Console.WriteLine("1- Practical\n2- Final");
            int examType = int.Parse(Console.ReadLine());

            Console.Write("Level (1-Easy, 2-Medium, 3-Hard): ");
            QuestionLevel level = (QuestionLevel)int.Parse(Console.ReadLine());

            var questions = QuestionBank
                .Where(q => q.Level == level)
                .ToList();

            if (examType == 1)
                questions = questions.Take(questions.Count / 2).ToList();

            int total = 0;
            int result = 0;

            foreach (var q in questions)
            {
                q.Display();
                result += q.CheckAnswer();
                total += q.Marks;
                Console.WriteLine();
            }

            Console.WriteLine($"Your Result: {result} / {total}");
        }
    }
}

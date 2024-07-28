using System.Runtime.CompilerServices;

namespace ConsoleApp1 
{
    public static class PersonExtensions
    {
        public static string GetFullName(this Person person)
        {
            if (person is null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            return $"{person.FirstName} {person.LastName}";
        }
    }

    public class Person
    {
        public delegate int SumFunction(int a, int b);
        public delegate void FirstNameChangedEventHandler(object sender, EventArgs e);
        public Func<int, int, int> ArithmeticOperation { get; set; }
        public event FirstNameChangedEventHandler FirstNameChanged;
        private List<string> Words { get; set; } = new();
        public SumFunction FunctionToSum { get; set; }

        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                if (FirstNameChanged is not null)
                {
                    FirstNameChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public string LastName { get; set; }

        public void AddWord(string word)
        {
            Words.Add(word);
        }

        public int WordsCount()
        {
            return Words.Count;
        }

        public string this[int index]
        {
            get
            {
                return Words[index];
            }
            set
            {
                Words[index] = value;
            }
        }

        public static string operator +(Person first, Person second)
        {
            if (first is null || second is null)
            {
                throw new ArgumentNullException("One or both persons are null");
            }
            return $"{first.GetFullName()} and {second.GetFullName()}";
        }
    }

    public class Program
    {
        public static int SumNumbers(int number1, int number2)
        {
            return number1 + number2;
        }

        public static void Main(string[] args)
        {
            var person = new Person();
            person.FirstNameChanged += (object sender, EventArgs e) => Console.WriteLine($"Name changed to: {((Person)sender).FirstName}");
            person.AddWord("Hello");
            person.AddWord("World");
            person.AddWord("Pizza");
            person[1] = "Planet";
            person.FunctionToSum = SumNumbers;
            for (int i = 0; i < person.WordsCount(); i++)
            {
                Console.WriteLine(person[i]);
            }
            int sumResult = person.FunctionToSum.Invoke(4, 5);
            Console.WriteLine($"Sum result: {sumResult}");
            person.ArithmeticOperation = (int input1, int input2) => input1 * input2;
            int multiplicationResult = person.ArithmeticOperation.Invoke(4, 5);
            Console.WriteLine($"Multiplication result: {multiplicationResult}");
            person.FirstName = "Carlos";
            person.LastName = "Duarte";
            Console.WriteLine(person.GetFullName());
            Person.SumFunction sumFunction = (int input1, int input2) => input1 + input2;
            int sumResult2 = sumFunction(1, 5);
            Console.WriteLine($"Second sum result: {sumResult2}");
            var secondPerson = new Person();
            secondPerson.FirstName = "Juan";
            secondPerson.LastName = "Lopez";
            Console.WriteLine(person + secondPerson);
        }
    }
}
using System;
using System.Text.RegularExpressions;

namespace Lab_CSharp.Models
{
    [Serializable]
    public class Person
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public DateTime BirthDate { get; private set; }

        public int Age { get; private set; }
        public bool IsAdult { get; private set; }
        public string SunSign { get; private set; }
        public string ChineseSign { get; private set; }
        public bool IsBirthdayToday { get; private set; }

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            ValidateInput(email, birthDate);

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;

            PrecalculateFields();
        }

        public void Update(string firstName, string lastName, string email, DateTime birthDate)
        {
            ValidateInput(email, birthDate);

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;

            PrecalculateFields();
        }

        private void PrecalculateFields()
        {
            Age = CalculateAge(BirthDate);
            IsAdult = Age >= 18;
            SunSign = GetWesternZodiac(BirthDate);
            ChineseSign = GetChineseZodiac(BirthDate);
            IsBirthdayToday = BirthDate.Day == DateTime.Today.Day && BirthDate.Month == DateTime.Today.Month;
        }

        private void ValidateInput(string email, DateTime birthDate)
        {
            if (!IsValidEmail(email))
                throw new InvalidEmailException("Invalid email format.");

            if (birthDate > DateTime.Today)
                throw new FutureBirthDateException("Date of birth cannot be in the future.");

            if (DateTime.Today.Year - birthDate.Year > 135)
                throw new TooOldBirthDateException("Date of birth is too far in the past.");
        }

        private int CalculateAge(DateTime date)
        {
            int age = DateTime.Today.Year - date.Year;
            if (date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }

        private string GetWesternZodiac(DateTime date)
        {
            int day = date.Day, month = date.Month;
            return (month, day) switch
            {
                (1, <= 19) => "Capricorn",
                (1, _) => "Aquarius",
                (2, <= 18) => "Aquarius",
                (2, _) => "Pisces",
                (3, <= 20) => "Pisces",
                (3, _) => "Aries",
                (4, <= 19) => "Aries",
                (4, _) => "Taurus",
                (5, <= 20) => "Taurus",
                (5, _) => "Gemini",
                (6, <= 20) => "Gemini",
                (6, _) => "Cancer",
                (7, <= 22) => "Cancer",
                (7, _) => "Leo",
                (8, <= 22) => "Leo",
                (8, _) => "Virgo",
                (9, <= 22) => "Virgo",
                (9, _) => "Libra",
                (10, <= 22) => "Libra",
                (10, _) => "Scorpio",
                (11, <= 21) => "Scorpio",
                (11, _) => "Sagittarius",
                (12, <= 21) => "Sagittarius",
                (12, _) => "Capricorn",
                _ => "Unknown"
            };
        }

        private string GetChineseZodiac(DateTime date)
        {
            string[] signs =
            {
                "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake",
                "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig"
            };
            return signs[date.Year % 12];
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }

    public class FutureBirthDateException : Exception
    {
        public FutureBirthDateException(string message) : base(message) { }
    }

    public class TooOldBirthDateException : Exception
    {
        public TooOldBirthDateException(string message) : base(message) { }
    }

    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string message) : base(message) { }
    }
}

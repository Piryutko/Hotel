using System;

namespace Hotel
{
    public class Guest
    {
        public Guest(string name, string surname, int luggageCount)
        {
            Name = name;
            Surname = surname;
            Id = Guid.NewGuid();
            LuggageCount = luggageCount;
        }
        public string Name { get; }

        public int LuggageCount { get; private set; }

        public string Surname { get; }

        public Guid Id { get; }

    }
}

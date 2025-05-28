using System;

namespace PetSimulator
{
    public enum PetType
    {
        Dog,
        Cat,
        Dragon,
        Rabbit,
        Hamster,
        Bird,
        Turtle,
        Fish
    }

    public class Pet
    {
        public string Name { get; private set; }
        public PetType Type { get; private set; }
        public int Hunger { get; private set; }
        public int Sleep { get; private set; }
        public int Fun { get; private set; }
        public DateTime AdoptionDate { get; private set; }
        public int Level { get; private set; }
        public int Experience { get; private set; }
        public int ExperienceToNextLevel { get; private set; }
        public bool IsAlive { get; private set; }
        public int Age { get; private set; } // Age in days

        public Pet(string name, PetType type)
        {
            Name = name;
            Type = type;
            // Initialize stats to 50 as per requirements
            Hunger = 50;
            Sleep = 50;
            Fun = 50;
            AdoptionDate = DateTime.Now;
            Level = 1;
            Experience = 0;
            ExperienceToNextLevel = 100;
            IsAlive = true;
            Age = 0;
        }

        public void LoadFromSave(PetSaveData saveData)
        {
            Hunger = saveData.Hunger;
            Sleep = saveData.Sleep;
            Fun = saveData.Fun;
            Level = saveData.Level;
            Experience = saveData.Experience;
            ExperienceToNextLevel = saveData.ExperienceToNextLevel;
            IsAlive = saveData.IsAlive;
            Age = saveData.Age;
            AdoptionDate = saveData.AdoptionDate;
        }

        public void UpdateStats(int hungerChange, int sleepChange, int funChange)
        {
            if (!IsAlive) return;

            Hunger = Math.Clamp(Hunger + hungerChange, 0, 100);
            Sleep = Math.Clamp(Sleep + sleepChange, 0, 100);
            Fun = Math.Clamp(Fun + funChange, 0, 100);

            if (Hunger <= 0 || Sleep <= 0 || Fun <= 0)
            {
                IsAlive = false;
                Hunger = 0;
                Sleep = 0;
                Fun = 0;
            }
        }

        public void AddExperience(int amount)
        {
            if (!IsAlive) return;

            Experience += amount;
            while (Experience >= ExperienceToNextLevel)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            Level++;
            Experience -= ExperienceToNextLevel;
            ExperienceToNextLevel = (int)(ExperienceToNextLevel * 1.5); // Increase XP needed for next level

            // Increase stats on level up
            Hunger = Math.Min(100, Hunger + 10);
            Sleep = Math.Min(100, Sleep + 10);
            Fun = Math.Min(100, Fun + 10);
        }

        public void AgeOneDay()
        {
            if (!IsAlive) return;
            Age++;
            
            // Stats decrease over time
            UpdateStats(-5, -5, -5);
        }

        public string GetStatus()
        {
            return IsAlive ? "Alive" : "Deceased";
        }

        public override string ToString()
        {
            if (!IsAlive)
            {
                return $"Name: {Name}\n" +
                       $"Type: {Type}\n" +
                       $"Status: Deceased\n" +
                       $"Age at death: {Age} days";
            }

            return $"Name: {Name}\n" +
                   $"Type: {Type}\n" +
                   $"Level: {Level}\n" +
                   $"Hunger: {Hunger}/100\n" +
                   $"Sleep: {Sleep}/100\n" +
                   $"Fun: {Fun}/100\n" +
                   $"Experience: {Experience}/{Level * 100}";
        }

        public string GetDetailedInfo()
        {
            if (!IsAlive)
            {
                return $"Name: {Name}\n" +
                       $"Type: {Type}\n" +
                       $"Status: Deceased\n" +
                       $"Age at death: {Age} days\n" +
                       $"Adopted: {AdoptionDate:g}";
            }

            return $"Name: {Name}\n" +
                   $"Type: {Type}\n" +
                   $"Level: {Level}\n" +
                   $"Experience: {Experience}/{ExperienceToNextLevel}\n" +
                   $"Age: {Age} days\n" +
                   $"Hunger: {Hunger}\n" +
                   $"Sleep: {Sleep}\n" +
                   $"Fun: {Fun}\n" +
                   $"Adopted: {AdoptionDate:g}\n" +
                   $"Status: Alive";
        }
    }
} 
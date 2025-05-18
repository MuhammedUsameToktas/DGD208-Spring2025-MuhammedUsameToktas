using System;

namespace PetSimulator
{
    public class Pet
    {
        public string Name { get; private set; }
        public int Hunger { get; private set; }
        public int Sleep { get; private set; }
        public int Fun { get; private set; }

        public Pet(string name)
        {
            Name = name;
            // Initialize stats to 50 as per requirements
            Hunger = 50;
            Sleep = 50;
            Fun = 50;
        }

        public void UpdateStats(int hungerChange, int sleepChange, int funChange)
        {
            Hunger = Math.Clamp(Hunger + hungerChange, 0, 100);
            Sleep = Math.Clamp(Sleep + sleepChange, 0, 100);
            Fun = Math.Clamp(Fun + funChange, 0, 100);
        }

        public bool IsAlive()
        {
            return Hunger > 0 && Sleep > 0 && Fun > 0;
        }

        public override string ToString()
        {
            return $"{Name} - Hunger: {Hunger}, Sleep: {Sleep}, Fun: {Fun}";
        }
    }
} 
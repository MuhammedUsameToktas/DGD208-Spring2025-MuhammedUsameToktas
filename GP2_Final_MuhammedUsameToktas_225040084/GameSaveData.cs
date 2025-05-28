using System;
using System.Collections.Generic;

namespace PetSimulator
{
    public class GameSaveData
    {
        public string PetHouseName { get; set; } = "My Pet House";
        public int PlayerCoins { get; set; }
        public int PlayerPetsAdopted { get; set; }
        public int PlayerPetsLost { get; set; }
        public List<string> PlayerAchievements { get; set; } = new List<string>();
        public List<PetSaveData> Pets { get; set; } = new List<PetSaveData>();
    }

    public class PetSaveData
    {
        public string Name { get; set; } = "";
        public PetType Type { get; set; }
        public int Hunger { get; set; }
        public int Sleep { get; set; }
        public int Fun { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int ExperienceToNextLevel { get; set; }
        public bool IsAlive { get; set; }
        public int Age { get; set; }
        public DateTime AdoptionDate { get; set; }
    }
} 
using System;

namespace PetSimulator
{
    public class Player
    {
        public int Coins { get; private set; }
        public int PetsAdopted { get; private set; }
        public int PetsLost { get; private set; }
        public int TotalPlayTime { get; private set; }
        public DateTime GameStartTime { get; private set; }

        public Player()
        {
            Coins = 100; // Starting coins
            PetsAdopted = 0;
            PetsLost = 0;
            TotalPlayTime = 0;
            GameStartTime = DateTime.Now;
        }

        public bool SpendCoins(int amount)
        {
            if (Coins >= amount)
            {
                Coins -= amount;
                return true;
            }
            return false;
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
        }

        public void PetAdopted()
        {
            PetsAdopted++;
        }

        public void PetLost()
        {
            PetsLost++;
        }

        public void UpdatePlayTime()
        {
            TotalPlayTime = (int)(DateTime.Now - GameStartTime).TotalMinutes;
        }

        public string GetStats()
        {
            UpdatePlayTime();
            return $"💰 Coins: {Coins}\n" +
                   $"🐾 Pets Adopted: {PetsAdopted}\n" +
                   $"💔 Pets Lost: {PetsLost}\n" +
                   $"⏱️ Total Play Time: {TotalPlayTime} minutes\n" +
                   $"🎮 Current Pets: {PetsAdopted - PetsLost}";
        }
    }
} 
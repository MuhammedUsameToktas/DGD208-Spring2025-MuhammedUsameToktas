using System;
using System.Collections.Generic;
using System.Linq;

namespace PetSimulator
{
    public class Player
    {
        public int Coins { get; set; } = 100;
        public int PetsAdopted { get; set; }
        public int PetsLost { get; set; }
        public int TotalPlayTime { get; private set; }
        public DateTime GameStartTime { get; private set; }
        private List<string> achievements = new List<string>();
        private Dictionary<string, int> inventory = new Dictionary<string, int>();

        public Player()
        {
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
            AddAchievement("First Pet");
            if (PetsAdopted >= 5) AddAchievement("Pet Collector");
        }

        public void PetLost()
        {
            PetsLost++;
        }

        public void UpdatePlayTime()
        {
            TotalPlayTime = (int)(DateTime.Now - GameStartTime).TotalMinutes;
        }

        public void AddAchievement(string achievement)
        {
            if (!achievements.Contains(achievement))
            {
                achievements.Add(achievement);
            }
        }

        public string GetAchievements()
        {
            return string.Join("\n", achievements);
        }

        public List<string> GetAchievementsList()
        {
            return achievements;
        }

        public string GetStats()
        {
            UpdatePlayTime();
            return $"💰 Coins: {Coins}\n" +
                   $"🐾 Pets Adopted: {PetsAdopted}\n" +
                   $"💔 Pets Lost: {PetsLost}\n" +
                   $"⏱️ Total Play Time: {TotalPlayTime} minutes\n" +
                   $"🎮 Current Pets: {PetsAdopted - PetsLost}\n" +
                   $"🏆 Achievements: {achievements.Count}";
        }

        public void AddItem(string itemName, int quantity = 1)
        {
            if (inventory.ContainsKey(itemName))
            {
                inventory[itemName] += quantity;
            }
            else
            {
                inventory[itemName] = quantity;
            }
        }

        public bool UseItem(string itemName)
        {
            if (inventory.ContainsKey(itemName) && inventory[itemName] > 0)
            {
                inventory[itemName]--;
                if (inventory[itemName] == 0)
                {
                    inventory.Remove(itemName);
                }
                return true;
            }
            return false;
        }

        public Dictionary<string, int> GetInventory()
        {
            return inventory;
        }

        public string GetInventoryDisplay()
        {
            if (inventory.Count == 0)
            {
                return "No items in inventory.";
            }

            var display = "Inventory:\n";
            foreach (var item in inventory)
            {
                display += $"{item.Key}: {item.Value}\n";
            }
            return display;
        }
    }
} 
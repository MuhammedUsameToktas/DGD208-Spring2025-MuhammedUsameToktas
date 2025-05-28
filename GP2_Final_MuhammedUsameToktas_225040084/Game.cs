using System;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.IO;

namespace PetSimulator
{
    public class Game
    {
        private bool isRunning;
        private Menu mainMenu;
        private Menu homeMenu;
        private Menu itemsMenu;
        private PetManager petManager;
        private Player player;
        private DateTime lastUpdateTime;
        private string petHouseName = "My Pet House";
        private bool isGameStarted = false;

        public Game()
        {
            isRunning = true;
            petManager = new PetManager();
            player = new Player();
            lastUpdateTime = DateTime.Now;
            
            mainMenu = new Menu(new[]
            {
                "Pet House",
                "Save Game",
                "Load Game",
                "Credits",
                "Exit Game"
            });

            homeMenu = new Menu(new[]
            {
                "Adopt a Pet",
                "Care for Pet",
                "Play with Pet",
                "Shop",
                "Items",
                "View Stats",
                "Back to Main Menu"
            });

            itemsMenu = new Menu(new[]
            {
                "Use Item",
                "Back to Main Menu"
            });
        }

        public void Run()
        {
            Console.WriteLine("Welcome to Pet Simulator!");
            Console.WriteLine("Created by: Muhammed Usame Toktas");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
            
            while (isRunning)
            {
                DisplayMainMenu();
            }
        }

        private void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Main Menu");
            Console.WriteLine("=========");
                int choice = mainMenu.Display();

            switch (choice)
            {
                case 1: // Pet House
                    if (isGameStarted)
                    {
                        DisplayHome();
                    }
                    else
                    {
                        StartNewGame();
                    }
                    break;
                case 2: // Save Game
                    if (isGameStarted)
                    {
                        SaveGame();
                    }
                    else
                    {
                        UpdateConsole("No game in progress to save!");
                    }
                    break;
                case 3: // Load Game
                    LoadGame();
                    break;
                case 4: // Credits
                    DisplayCredits();
                    break;
                case 5: // Exit Game
                    isRunning = false;
                    Console.WriteLine("Thanks for playing!");
                    break;
            }
        }

        private void DisplayCredits()
        {
            Console.Clear();
            Console.WriteLine("Credits");
            Console.WriteLine("=======");
            Console.WriteLine("Game Developer: Muhammed Usame Toktas");
            Console.WriteLine("Version: 1.0");
            Console.WriteLine("\nPress any key to return to main menu...");
            Console.ReadKey();
            Console.Clear();
        }

        private void StartNewGame()
        {
            Console.Clear();
            Console.WriteLine("Starting New Game");
            Console.WriteLine("================");
            Console.Write("\nEnter your pet house name: ");
            string input = Console.ReadLine()?.Trim() ?? "My Pet House";
            petHouseName = input;
            isGameStarted = true;
            Console.Clear();
            DisplayHome();
        }

        private void DisplayItemsMenu()
        {
            Console.Clear();
            Console.WriteLine("Items Menu");
            Console.WriteLine("==========");
            Console.WriteLine(player.GetInventoryDisplay());
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Use Item");
            Console.WriteLine("2. Back to Home");

            Console.Write("\nEnter your choice: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        UseItemFromInventory();
                        break;
                    case 2:
                        DisplayHome();
                        break;
                }
            }
        }

        private void UseItemFromInventory()
        {
            var inventory = player.GetInventory();
            if (inventory.Count == 0)
            {
                UpdateConsole("You don't have any items in your inventory!");
                return;
            }

            var pets = petManager.GetAllPets();
            if (pets.Count == 0)
            {
                UpdateConsole("You don't have any pets to use items on!");
                return;
            }

            Console.WriteLine("\nYour Items:");
            int index = 1;
            foreach (var item in inventory)
            {
                Console.WriteLine($"{index}. {item.Key} (x{item.Value})");
                index++;
            }

            Console.Write("\nSelect item to use (number): ");
            if (int.TryParse(Console.ReadLine(), out int itemChoice) && itemChoice > 0 && itemChoice <= inventory.Count)
            {
                string itemName = inventory.Keys.ElementAt(itemChoice - 1);

                Console.WriteLine("\nSelect a pet to use the item on:");
                for (int i = 0; i < pets.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {pets[i].Name} ({pets[i].Type})");
                }

                Console.Write("\nEnter pet number: ");
                if (int.TryParse(Console.ReadLine(), out int petChoice) && petChoice > 0 && petChoice <= pets.Count)
                {
                    var selectedPet = pets[petChoice - 1];
                    if (!selectedPet.IsAlive)
                    {
                        UpdateConsole($"{selectedPet.Name} has passed away. You can't use items on them anymore.");
                        return;
                    }

                    if (player.UseItem(itemName))
                    {
                        string message = "";
                        switch (itemName)
                        {
                            case "Pet Food":
                                selectedPet.UpdateStats(30, 0, 0);
                                message = $"You fed {selectedPet.Name} with Pet Food. +30 Hunger!";
                                break;
                            case "Pet Bed":
                                selectedPet.UpdateStats(0, 40, 0);
                                message = $"{selectedPet.Name} slept in the Pet Bed. +40 Sleep!";
                                break;
                            case "Pet Toy":
                                selectedPet.UpdateStats(0, 0, 35);
                                message = $"{selectedPet.Name} played with the Pet Toy. +35 Fun!";
                                break;
                            case "Medicine":
                                selectedPet.UpdateStats(25, 25, 25);
                                message = $"{selectedPet.Name} took Medicine. +25 to all stats!";
                                break;
                            case "Premium Food":
                                selectedPet.UpdateStats(50, 0, 0);
                                message = $"You fed {selectedPet.Name} with Premium Food. +50 Hunger!";
                                break;
                            case "Luxury Bed":
                                selectedPet.UpdateStats(0, 60, 0);
                                message = $"{selectedPet.Name} slept in the Luxury Bed. +60 Sleep!";
                                break;
                            case "Premium Toy":
                                selectedPet.UpdateStats(0, 0, 55);
                                message = $"{selectedPet.Name} played with the Premium Toy. +55 Fun!";
                                break;
                        }
                        UpdateConsole(message);
                    }
                }
                else
                {
                    UpdateConsole("Invalid pet selection. Please try again.");
                }
            }
            else
            {
                UpdateConsole("Invalid item selection. Please try again.");
            }
        }

        private void DisplayHome()
        {
            Console.Clear();
            Console.WriteLine(AsciiArt.GetHomeArt());
            Console.WriteLine($"\n{petHouseName}");
            Console.WriteLine("=============");
            Console.WriteLine($"💰 Coins: {player.Coins}");
            
            var pets = petManager.GetAllPets().Where(p => p.IsAlive).ToList();
            if (pets.Count == 0)
            {
                Console.WriteLine("\nNo pets yet. Adopt one!");
            }
            else
            {
            Console.WriteLine("\nYour Pets:");
            foreach (var pet in pets)
            {
                Console.WriteLine("\n" + AsciiArt.GetPetArt(pet.Type));
                Console.WriteLine(pet.ToString());
                Console.WriteLine("-------------------");
            }
        }

            Console.WriteLine("\nHome Menu:");
            int choice = homeMenu.Display();

            switch (choice)
            {
                case 1:
                    AdoptPet();
                    break;
                case 2:
                    CareForPet();
                    break;
                case 3:
                    PlayWithPet();
                    break;
                case 4:
                    Shop();
                    break;
                case 5:
                    DisplayItemsMenu();
                    break;
                case 6:
                    ViewStats();
                    break;
                case 7:
                    Console.Clear();
                    break;
            }
        }

        private void UpdateConsole(string message)
        {
            Console.Clear();
            if (isGameStarted)
            {
                DisplayHome();
            }
            else
            {
                DisplayMainMenu();
            }
            Console.WriteLine("\n" + message);
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
            if (isGameStarted)
            {
                DisplayHome();
            }
            else
            {
                DisplayMainMenu();
            }
        }

        private void SaveGame()
        {
            Console.Clear();
            Console.WriteLine("Saving Game...");
            
            var saveData = new GameSaveData
            {
                PetHouseName = petHouseName,
                PlayerCoins = player.Coins,
                PlayerPetsAdopted = player.PetsAdopted,
                PlayerPetsLost = player.PetsLost,
                PlayerAchievements = player.GetAchievementsList(),
                Pets = petManager.GetAllPets().Select(p => new PetSaveData
                {
                    Name = p.Name,
                    Type = p.Type,
                    Hunger = p.Hunger,
                    Sleep = p.Sleep,
                    Fun = p.Fun,
                    Level = p.Level,
                    Experience = p.Experience,
                    ExperienceToNextLevel = p.ExperienceToNextLevel,
                    IsAlive = p.IsAlive,
                    Age = p.Age,
                    AdoptionDate = p.AdoptionDate
                }).ToList()
            };

            string json = System.Text.Json.JsonSerializer.Serialize(saveData);
            File.WriteAllText("petsim_save.json", json);
            
            UpdateConsole("Game saved successfully!");
        }

        private void LoadGame()
        {
            Console.Clear();
            Console.WriteLine("Loading Game...");
            
            if (!File.Exists("petsim_save.json"))
                        {
                UpdateConsole("No save file found!");
                return;
            }

            try
            {
                string json = File.ReadAllText("petsim_save.json");
                var saveData = System.Text.Json.JsonSerializer.Deserialize<GameSaveData>(json);

                if (saveData != null)
                {
                    petHouseName = saveData.PetHouseName;
                    player = new Player
                    {
                        Coins = saveData.PlayerCoins,
                        PetsAdopted = saveData.PlayerPetsAdopted,
                        PetsLost = saveData.PlayerPetsLost
                    };
                    
                    foreach (var achievement in saveData.PlayerAchievements)
                    {
                        player.AddAchievement(achievement);
                        }

                    petManager = new PetManager();
                    foreach (var petData in saveData.Pets)
                    {
                        var pet = new Pet(petData.Name, petData.Type);
                        pet.LoadFromSave(petData);
                        petManager.AddPet(pet);
                    }

                    isGameStarted = true;
                    UpdateConsole("Game loaded successfully!");
                }
            }
            catch (Exception ex)
            {
                UpdateConsole($"Error loading game: {ex.Message}");
            }
        }

        private void ViewStats()
        {
            Console.Clear();
            Console.WriteLine("Player Statistics");
            Console.WriteLine("================");
            Console.WriteLine(player.GetStats());
            Console.WriteLine("\nAchievements:");
            Console.WriteLine("=============");
            Console.WriteLine(player.GetAchievements());
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        private void Shop()
        {
            Console.Clear();
            Console.WriteLine("Pet Shop");
            Console.WriteLine("========");
            Console.WriteLine($"Your coins: {player.Coins}");
            Console.WriteLine("\nItems:");
            Console.WriteLine("1. Pet Food (20 coins) - +30 Hunger");
            Console.WriteLine("2. Pet Bed (50 coins) - +40 Sleep");
            Console.WriteLine("3. Pet Toy (30 coins) - +35 Fun");
            Console.WriteLine("4. Medicine (100 coins) - +25 to all stats");
            Console.WriteLine("5. Premium Food (50 coins) - +50 Hunger");
            Console.WriteLine("6. Luxury Bed (100 coins) - +60 Sleep");
            Console.WriteLine("7. Premium Toy (80 coins) - +55 Fun");
            Console.WriteLine("8. Back to Main Menu");

            Console.Write("\nSelect item to buy: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                string message = "";
                switch (choice)
                {
                    case 1:
                        if (player.SpendCoins(20))
                        {
                            player.AddItem("Pet Food");
                            message = "Pet Food purchased!";
                            player.AddAchievement("First Purchase");
                        }
                        else
                        {
                            message = "Not enough coins!";
                        }
                        break;
                    case 2:
                        if (player.SpendCoins(50))
                        {
                            player.AddItem("Pet Bed");
                            message = "Pet Bed purchased!";
                            player.AddAchievement("Luxury Buyer");
                        }
                        else
                        {
                            message = "Not enough coins!";
                        }
                        break;
                    case 3:
                        if (player.SpendCoins(30))
                        {
                            player.AddItem("Pet Toy");
                            message = "Pet Toy purchased!";
                            player.AddAchievement("Toy Collector");
                        }
                        else
                        {
                            message = "Not enough coins!";
                        }
                        break;
                    case 4:
                        if (player.SpendCoins(100))
                        {
                            player.AddItem("Medicine");
                            message = "Medicine purchased!";
                            player.AddAchievement("Pet Doctor");
                        }
                        else
                        {
                            message = "Not enough coins!";
                        }
                        break;
                    case 5:
                        if (player.SpendCoins(50))
                        {
                            player.AddItem("Premium Food");
                            message = "Premium Food purchased!";
                            player.AddAchievement("Gourmet Chef");
                        }
                        else
                        {
                            message = "Not enough coins!";
                        }
                        break;
                    case 6:
                        if (player.SpendCoins(100))
                        {
                            player.AddItem("Luxury Bed");
                            message = "Luxury Bed purchased!";
                            player.AddAchievement("Luxury Lover");
                        }
                        else
                        {
                            message = "Not enough coins!";
                        }
                        break;
                    case 7:
                        if (player.SpendCoins(80))
                        {
                            player.AddItem("Premium Toy");
                            message = "Premium Toy purchased!";
                            player.AddAchievement("Toy Master");
                        }
                        else
                        {
                            message = "Not enough coins!";
                        }
                        break;
                    case 8:
                        return;
                    default:
                        message = "Invalid choice!";
                        break;
                }
                UpdateConsole(message);
            }
        }

        private void AdoptPet()
        {
            Console.Clear();
            Console.WriteLine("Pet Adoption Center");
            Console.WriteLine("==================");
            Console.WriteLine($"Your coins: {player.Coins}");
            Console.WriteLine("\nAvailable Pet Types:");
            var petTypes = Enum.GetValues(typeof(PetType)).Cast<PetType>();
            int index = 1;
            foreach (var type in petTypes)
            {
                Console.WriteLine($"{index}. {type} - 50 coins");
                index++;
            }

            Console.Write("\nSelect pet type (number): ");
            if (int.TryParse(Console.ReadLine(), out int typeChoice) && typeChoice > 0 && typeChoice <= petTypes.Count())
            {
                if (player.SpendCoins(50))
                {
                    PetType selectedType = petTypes.ElementAt(typeChoice - 1);
                    
                    Console.Write("Enter pet name: ");
                    string name = Console.ReadLine()?.Trim();
                    
                    if (!string.IsNullOrEmpty(name))
                    {
                        var pet = new Pet(name, selectedType);
                        petManager.AddPet(pet);
                        player.PetAdopted();
                        UpdateConsole($"Congratulations! You've adopted a {selectedType} named {name}!");
                    }
                    else
                    {
                        player.AddCoins(50); // Refund the coins
                        UpdateConsole("Invalid name. Please try again.");
                    }
                }
                else
                {
                    UpdateConsole("Not enough coins to adopt a pet!");
                }
            }
            else
            {
                UpdateConsole("Invalid selection. Please try again.");
            }
        }

        private void CareForPet()
        {
            var pets = petManager.GetAllPets();
            if (pets.Count == 0)
            {
                UpdateConsole("You don't have any pets yet. Adopt one!");
                return;
            }

            Console.WriteLine("\nSelect a pet to care for:");
            for (int i = 0; i < pets.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {pets[i].Name} ({pets[i].Type})");
            }

            Console.Write("\nEnter pet number: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= pets.Count)
            {
                var selectedPet = pets[choice - 1];
                if (!selectedPet.IsAlive)
                {
                    UpdateConsole($"{selectedPet.Name} has passed away. You can't care for them anymore.");
                    return;
                }

                Console.WriteLine("\nCare Options:");
                Console.WriteLine("1. Feed (10 coins) - +20 Hunger");
                Console.WriteLine("2. Let Sleep (5 coins) - +20 Sleep");
                Console.WriteLine("3. Give Medicine (20 coins) - +10 to all stats");

                Console.Write("\nSelect care option: ");
                if (int.TryParse(Console.ReadLine(), out int careChoice))
                {
                    switch (careChoice)
                    {
                        case 1:
                            if (player.SpendCoins(10))
                            {
                                selectedPet.UpdateStats(20, 0, 0);
                                selectedPet.AddExperience(10);
                                player.AddCoins(5);
                                Console.Clear();
                                Console.WriteLine($"You fed {selectedPet.Name}. They look happier!");
                                Console.WriteLine("\nPress any key to continue...");
                                Console.ReadKey();
                                DisplayHome();
                            }
                            else
                            {
                                UpdateConsole("Not enough coins!");
                            }
                            break;
                        case 2:
                            if (player.SpendCoins(5))
                            {
                                selectedPet.UpdateStats(0, 20, 0);
                                selectedPet.AddExperience(10);
                                player.AddCoins(3);
                                Console.Clear();
                                Console.WriteLine($"{selectedPet.Name} had a good rest!");
                                Console.WriteLine("\nPress any key to continue...");
                                Console.ReadKey();
                                DisplayHome();
                            }
                            else
                            {
                                UpdateConsole("Not enough coins!");
                            }
                            break;
                        case 3:
                            if (player.SpendCoins(20))
                            {
                                selectedPet.UpdateStats(10, 10, 10);
                                selectedPet.AddExperience(15);
                                player.AddCoins(8);
                                Console.Clear();
                                Console.WriteLine($"{selectedPet.Name} is feeling better after the medicine!");
                                Console.WriteLine("\nPress any key to continue...");
                                Console.ReadKey();
                                DisplayHome();
                            }
                            else
                            {
                                UpdateConsole("Not enough coins!");
                            }
                            break;
                        default:
                            UpdateConsole("Invalid option.");
                            break;
                    }
                }
            }
            else
            {
                UpdateConsole("Invalid selection. Please try again.");
            }
        }

        private void PlayWithPet()
        {
            var pets = petManager.GetAllPets();
            if (pets.Count == 0)
            {
                UpdateConsole("You don't have any pets yet. Adopt one!");
                return;
            }

            Console.WriteLine("\nSelect a pet to play with:");
            for (int i = 0; i < pets.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {pets[i].Name} ({pets[i].Type})");
            }

            Console.Write("\nEnter pet number: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= pets.Count)
            {
                var selectedPet = pets[choice - 1];
                if (!selectedPet.IsAlive)
                {
                    UpdateConsole($"{selectedPet.Name} has passed away. You can't play with them anymore.");
                    return;
                }

                Console.WriteLine("\nPlay Options:");
                Console.WriteLine("1. Play Fetch (5 coins) - +15 Fun, -5 Hunger");
                Console.WriteLine("2. Play Hide and Seek (8 coins) - +20 Fun, -10 Sleep");
                Console.WriteLine("3. Teach Tricks (15 coins) - +25 Fun, -15 Hunger, -15 Sleep");

                Console.Write("\nSelect play option: ");
                if (int.TryParse(Console.ReadLine(), out int playChoice))
                {
                    switch (playChoice)
                    {
                        case 1:
                            if (player.SpendCoins(5))
                            {
                                selectedPet.UpdateStats(-5, 0, 15);
                                selectedPet.AddExperience(20);
                                player.AddCoins(10);
                                Console.Clear();
                                Console.WriteLine($"You played fetch with {selectedPet.Name}. They had fun!");
                                Console.WriteLine("\nPress any key to continue...");
                                Console.ReadKey();
                                DisplayHome();
                            }
                            else
                            {
                                UpdateConsole("Not enough coins!");
                            }
                            break;
                        case 2:
                            if (player.SpendCoins(8))
                            {
                                selectedPet.UpdateStats(0, -10, 20);
                                selectedPet.AddExperience(25);
                                player.AddCoins(15);
                                Console.Clear();
                                Console.WriteLine($"{selectedPet.Name} had a great time playing hide and seek!");
                                Console.WriteLine("\nPress any key to continue...");
                                Console.ReadKey();
                                DisplayHome();
                            }
                            else
                            {
                                UpdateConsole("Not enough coins!");
                            }
                            break;
                        case 3:
                            if (player.SpendCoins(15))
                            {
                                selectedPet.UpdateStats(-15, -15, 25);
                                selectedPet.AddExperience(30);
                                player.AddCoins(20);
                                Console.Clear();
                                Console.WriteLine($"{selectedPet.Name} learned some new tricks!");
                                Console.WriteLine("\nPress any key to continue...");
                                Console.ReadKey();
                                DisplayHome();
                            }
                            else
                            {
                                UpdateConsole("Not enough coins!");
                            }
                            break;
                        default:
                            UpdateConsole("Invalid option.");
                            break;
                    }
                }
            }
            else
            {
                UpdateConsole("Invalid selection. Please try again.");
            }
        }

        private void UpdatePets()
        {
            var pets = petManager.GetAllPets();
            foreach (var pet in pets)
            {
                if (pet.IsAlive)
                {
                    pet.AgeOneDay();
                    if (!pet.IsAlive)
                    {
                        player.PetLost();
                        UpdateConsole($"{pet.Name} has passed away. They will be remembered fondly.");
                    }
                }
            }
        }
    }
} 
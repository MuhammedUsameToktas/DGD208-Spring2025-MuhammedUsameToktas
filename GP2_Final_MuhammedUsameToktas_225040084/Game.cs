using System;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace PetSimulator
{
    public class Game
    {
        private bool isRunning;
        private Menu mainMenu;
        private PetManager petManager;
        private Player player;
        private DateTime lastUpdateTime;

        public Game()
        {
            isRunning = true;
            petManager = new PetManager();
            player = new Player();
            lastUpdateTime = DateTime.Now;
            mainMenu = new Menu(new[]
            {
                "Home",
                "Adopt a Pet",
                "Care for Pet",
                "Play with Pet",
                "Shop",
                "View Stats",
                "Exit Game"
            });
        }

        public async Task Run()
        {
            Console.WriteLine("Welcome to Pet Simulator!");
            
            while (isRunning)
            {
                UpdatePets();
                DisplayHome();
                Console.WriteLine("\nMain Menu:");
                int choice = mainMenu.Display();

                switch (choice)
                {
                    case 1:
                        DisplayHome();
                        break;
                    case 2:
                        AdoptPet();
                        break;
                    case 3:
                        CareForPet();
                        break;
                    case 4:
                        PlayWithPet();
                        break;
                    case 5:
                        Shop();
                        break;
                    case 6:
                        ViewStats();
                        break;
                    case 7:
                        isRunning = false;
                        Console.WriteLine("Thanks for playing!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void DisplayHome()
        {
            Console.Clear();
            Console.WriteLine(AsciiArt.GetHomeArt());
            Console.WriteLine("\nYour Pet Home");
            Console.WriteLine("=============");
            Console.WriteLine($"💰 Coins: {player.Coins}");
            
            var pets = petManager.GetAllPets();
            if (pets.Count == 0)
            {
                Console.WriteLine("\nNo pets yet. Adopt one!");
                return;
            }

            Console.WriteLine("\nYour Pets:");
            foreach (var pet in pets)
            {
                Console.WriteLine("\n" + AsciiArt.GetPetArt(pet.Type));
                Console.WriteLine(pet.ToString());
                Console.WriteLine("-------------------");
            }
        }

        private void UpdatePets()
        {
            var now = DateTime.Now;
            if ((now - lastUpdateTime).TotalSeconds >= 30)
            {
                foreach (var pet in petManager.GetAllPets())
                {
                    if (pet.IsAlive)
                    {
                        pet.UpdateStats(-1, -1, -1);
                        if (!pet.IsAlive)
                        {
                            player.PetLost();
                            Console.WriteLine($"\n💔 {pet.Name} has passed away...");
                        }
                    }
                }
                lastUpdateTime = now;
            }
        }

        private void ViewStats()
        {
            Console.Clear();
            Console.WriteLine("Player Statistics");
            Console.WriteLine("================");
            Console.WriteLine(player.GetStats());
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
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
            Console.WriteLine("5. Back to Main Menu");

            Console.Write("\nSelect item to buy: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        if (player.SpendCoins(20))
                        {
                            Console.WriteLine("Pet Food purchased!");
                        }
                        else
                        {
                            Console.WriteLine("Not enough coins!");
                        }
                        break;
                    case 2:
                        if (player.SpendCoins(50))
                        {
                            Console.WriteLine("Pet Bed purchased!");
                        }
                        else
                        {
                            Console.WriteLine("Not enough coins!");
                        }
                        break;
                    case 3:
                        if (player.SpendCoins(30))
                        {
                            Console.WriteLine("Pet Toy purchased!");
                        }
                        else
                        {
                            Console.WriteLine("Not enough coins!");
                        }
                        break;
                    case 4:
                        if (player.SpendCoins(100))
                        {
                            Console.WriteLine("Medicine purchased!");
                        }
                        else
                        {
                            Console.WriteLine("Not enough coins!");
                        }
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
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
                        Console.WriteLine($"\nCongratulations! You've adopted a {selectedType} named {name}!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid name. Please try again.");
                        player.AddCoins(50); // Refund the coins
                    }
                }
                else
                {
                    Console.WriteLine("Not enough coins to adopt a pet!");
                }
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void CareForPet()
        {
            var pets = petManager.GetAllPets();
            if (pets.Count == 0)
            {
                Console.WriteLine("\nYou don't have any pets yet. Adopt one!");
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
                    Console.WriteLine($"\n{selectedPet.Name} has passed away. You can't care for them anymore.");
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
                                player.AddCoins(5); // Earn some coins for caring
                                Console.WriteLine($"\nYou fed {selectedPet.Name}. They look happier!");
                            }
                            else
                            {
                                Console.WriteLine("Not enough coins!");
                            }
                            break;
                        case 2:
                            if (player.SpendCoins(5))
                            {
                                selectedPet.UpdateStats(0, 20, 0);
                                selectedPet.AddExperience(10);
                                player.AddCoins(3); // Earn some coins for caring
                                Console.WriteLine($"\n{selectedPet.Name} had a good rest!");
                            }
                            else
                            {
                                Console.WriteLine("Not enough coins!");
                            }
                            break;
                        case 3:
                            if (player.SpendCoins(20))
                            {
                                selectedPet.UpdateStats(10, 10, 10);
                                selectedPet.AddExperience(15);
                                player.AddCoins(8); // Earn some coins for caring
                                Console.WriteLine($"\n{selectedPet.Name} is feeling better after the medicine!");
                            }
                            else
                            {
                                Console.WriteLine("Not enough coins!");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void PlayWithPet()
        {
            var pets = petManager.GetAllPets();
            if (pets.Count == 0)
            {
                Console.WriteLine("\nYou don't have any pets yet. Adopt one!");
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
                    Console.WriteLine($"\n{selectedPet.Name} has passed away. You can't play with them anymore.");
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
                                player.AddCoins(10); // Earn coins for playing
                                Console.WriteLine($"\nYou played fetch with {selectedPet.Name}. They had fun!");
                            }
                            else
                            {
                                Console.WriteLine("Not enough coins!");
                            }
                            break;
                        case 2:
                            if (player.SpendCoins(8))
                            {
                                selectedPet.UpdateStats(0, -10, 20);
                                selectedPet.AddExperience(25);
                                player.AddCoins(15); // Earn coins for playing
                                Console.WriteLine($"\n{selectedPet.Name} had a great time playing hide and seek!");
                            }
                            else
                            {
                                Console.WriteLine("Not enough coins!");
                            }
                            break;
                        case 3:
                            if (player.SpendCoins(15))
                            {
                                selectedPet.UpdateStats(-15, -15, 25);
                                selectedPet.AddExperience(30);
                                player.AddCoins(20); // Earn coins for playing
                                Console.WriteLine($"\n{selectedPet.Name} learned some new tricks!");
                            }
                            else
                            {
                                Console.WriteLine("Not enough coins!");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
} 
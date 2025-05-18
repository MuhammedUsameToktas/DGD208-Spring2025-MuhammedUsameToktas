using System;
using System.Threading.Tasks;

namespace PetSimulator
{
    public class Game
    {
        private bool isRunning;
        private Menu mainMenu;

        public Game()
        {
            isRunning = true;
            mainMenu = new Menu(new[]
            {
                "Display Creator Info",
                "Adopt a Pet",
                "View Pets",
                "Exit Game"
            });
        }

        public async Task Run()
        {
            Console.WriteLine("Welcome to Pet Simulator!");
            
            while (isRunning)
            {
                Console.WriteLine("\nMain Menu:");
                int choice = mainMenu.Display();

                switch (choice)
                {
                    case 1:
                        DisplayCreatorInfo();
                        break;
                    case 2:
                        // Will implement in Week 2
                        Console.WriteLine("Pet adoption coming soon!");
                        break;
                    case 3:
                        // Will implement in Week 2
                        Console.WriteLine("Pet viewing coming soon!");
                        break;
                    case 4:
                        isRunning = false;
                        Console.WriteLine("Thanks for playing!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void DisplayCreatorInfo()
        {
            Console.WriteLine("\nCreator Information:");
            Console.WriteLine("Name: Muhammed Usame Toktas");
            Console.WriteLine("Student Number: 225040084");
        }
    }
} 
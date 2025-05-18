using System;

namespace PetSimulator
{
    public class Menu
    {
        private string[] options;

        public Menu(string[] options)
        {
            this.options = options;
        }

        public int Display()
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                Console.Write("\nEnter your choice: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= options.Length)
                {
                    return choice;
                }
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
    }
} 
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetSimulator
{
    public class PetManager
    {
        private List<Pet> pets;

        public PetManager()
        {
            pets = new List<Pet>();
        }

        public void AddPet(Pet pet)
        {
            pets.Add(pet);
        }

        public void RemovePet(Pet pet)
        {
            pets.Remove(pet);
        }

        public List<Pet> GetAllPets()
        {
            return pets.ToList();
        }

        public Pet? GetPetByName(string name)
        {
            return pets.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void UpdateAllPets()
        {
            // This will be implemented in Week 2 with the stat decrease mechanism
            // For now, it's just a placeholder
        }
    }
} 
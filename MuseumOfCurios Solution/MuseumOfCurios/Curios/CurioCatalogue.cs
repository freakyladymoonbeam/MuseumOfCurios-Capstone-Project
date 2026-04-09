using MuseumOfCurios.Curios.Common;
using MuseumOfCurios.Curios.Rare;
using MuseumOfCurios.Curios.Epic;
using MuseumOfCurios.Curios.Legendary;
using MuseumOfCurios.Curios.Mythical;
using System.Reflection;

namespace MuseumOfCurios.Curios
{
    public class CurioCatalogue
    {
        private List<Curio> curios; // This list will hold all the curios in the catalogue

        public CurioCatalogue()
        {
            curios = DiscoverCurios(); // Initialize the catalogue by discovering all curios
        }

        private List<Curio> DiscoverCurios()
        {
            var curioType = typeof(Curio);

            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => !t.IsAbstract && curioType.IsAssignableFrom(t) && t.GetConstructor(Type.EmptyTypes) != null).ToList(); // Find all non-abstract types that inherit from Curio

            var instances = new List<Curio>();

            foreach (var type in types)
            {
                var instance = (Curio)Activator.CreateInstance(type); // Create an instance of the curio using the default constructor
                if (instance != null)
                {
                    instances.Add(instance); // Add the instance to the list of curios
                }
            }
            return instances
                .OrderBy(c => c.Rarity)
                .ThenBy(c => c.Name)
                .ToList();
        }

        // Return all curios in the catalogue
        public List<Curio> GetAllCurios()
        {
            return curios;
        }

        // Get a curio by its index
        public Curio GetCurioByIndex(int index)
        {
            if (index >= 0 && index < curios.Count) // Check if the index is within bounds
            {
                return curios[index]; // Return the curio at the specified index

            }
            return null; // Return null if the index is out of bounds
        }
        
        public void AddCurio(CustomCurio curio) // Method to add a new curio to the catalogue
        {
            if (curio != null) // Check if the curio is not null
            {
                curios.Add(curio); // Add the curio to the list
            }
        }
    }
}
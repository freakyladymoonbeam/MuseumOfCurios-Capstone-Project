using MuseumOfCurios.Curios.Common;
using MuseumOfCurios.Curios.Rare;
using MuseumOfCurios.Curios.Epic;
using MuseumOfCurios.Curios.Legendary;
using MuseumOfCurios.Curios.Mythical;

namespace MuseumOfCurios.Curios
{
    public class CurioCatalogue
    {
        private List<Curio> curios; // This list will hold all the curios in the catalogue

        public CurioCatalogue() // Constructor to initialize the catalogue with curios
        {
            curios = new List<Curio> // Initialize the list of curios
            {
                new ScuttlebuttAlledger(),
                new ForgetMeKnotFlag(),
                new ChocolateMedalliyum(),
                new HoneyBuns(),
                new ZizzwizzPillow(),
                new CrudeImage(),
                new Terrorcrow(),
                new DesertRose(),
                new ShipInABottle(),
                new ScintillatingSinter(),
                new BloomingBranch(),
                new ToffsTeaSet(),
                new Zoomshine(),
                new BattenBinnacle(),
                new FaerieQuill(),
                new Ghoulroarer(),
                new LoftyLilts(),
                new MonsterChessSet(),
                new TuskTuskTuskInkwell(),
                new AnnalumRetentium(),
                new SlimeCurio(),
                new MaxiMedal(),
                new CrownOfUptaten(),
                new YggdrasilSapling(),
                new MadalenasLocket(),
                new MaritalOrgan(),
                new CatasTrophy()
            };
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
        
        public void AddCurio(Curio curio) // Method to add a new curio to the catalogue
        {
            if (curio != null) // Check if the curio is not null
            {
                curios.Add(curio); // Add the curio to the list
            }
        }

        public bool RemoveCurioAt(int index, out string message)
        {
            if (index >= 0 && index < curios.Count && curios[index].IsCustom) // Check if the index is within bounds
            {
                Curio removedCurio = curios[index]; // Store the curio to be removed for the message
                curios.RemoveAt(index); // Remove the curio at the specified index
                message = $"Curio \"{removedCurio.Name}\" removed successfully!"; // Set success message
                return true; // Return true to indicate successful removal
            }
            else if (index >= 0 && index < curios.Count && !curios[index].IsCustom)
            {
                message = "Cannot remove a non-custom curio."; // Set error message for non-custom curio
                return false; // Return false to indicate failure
            }
            else
            {
                message = "Invalid entry. Please enter the number of a valid, custom curio."; // Set error message for invalid index
                return false; // Return false to indicate failure
            }
        }
    }
}
using MuseumOfCurios.Curios;

namespace MuseumOfCurios.Core
{
    class LibraryApp
    {
        private CurioCatalogue catalogue = new CurioCatalogue();

        private void DisplayScreen(string lastResult)
        {
            Console.Clear();
            Console.WriteLine("==== Curios ====");
            List<Curio> curios = catalogue.GetAllCurios();

            for (int i = 0; i < curios.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {curios[i].Name}");
            }
            Console.WriteLine("C. Create a new curio");
            Console.WriteLine("X. Exit");

            if (!string.IsNullOrEmpty(lastResult))
            {
                Console.WriteLine("\n" + lastResult);
            }
            Console.Write("\nEnter the number of the curio you would like to examine (or X to exit): ");
        }

        public void Run()
        {
            string lastResult = "";
            bool exitRequested = false;

            while (!exitRequested)
            {
                DisplayScreen(lastResult);

                string userInput = Console.ReadLine().ToLower();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Please enter a valid input.");
                    continue;
                }

                if (userInput == "x")
                {
                    exitRequested = true;
                    Console.WriteLine();
                    Console.WriteLine("Thank you for visiting the Museum of Curios. Goodbye!");
                }
                else if (userInput == "c")
                {
                    CurioCreator creator = new CurioCreator(catalogue);
                    lastResult = creator.CreateCurioFlow();
                }
                else if (int.TryParse(userInput, out int userChoice))
                {
                    Curio selected = catalogue.GetCurioByIndex(userChoice - 1);
                    if (selected != null)
                    {
                        lastResult = CurioInteractionFlow(selected, userChoice - 1);
                    }
                    else
                    {
                        lastResult = "Invalid selection. Please enter a number that corresponds with a curio listing.";
                    }
                }
                else
                {
                    lastResult = "Invalid selection. Please enter a number that corresponds with a curio listing.";
                }
            }
        }

        private string CurioInteractionFlow(Curio curio, int index)
        {
            string result = "";
            bool userDone = false;

            while (!userDone)
            {
                if (curio.IsCustom)
                {
                    Console.WriteLine("[E] Examine");
                    Console.WriteLine("[D] Delete");
                    Console.WriteLine("[T] Edit");
                    Console.WriteLine("[B] Back");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("[E] Examine");
                    Console.WriteLine("[B] Back");
                }

                Console.Write("Choose an action: ");
                string input = Console.ReadLine().ToLower();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input == "e")
                {
                    return curio.Examine();
                    userDone = true;
                }
                else if (input == "d" && curio.IsCustom)
                {
                    bool success = catalogue.RemoveCurioAt(index, out string message);
                    return message;
                    userDone = true;
                }
                else if (input == "t" && curio.IsCustom)
                {
                    result = EditCurioFlow(curio);
                    userDone = true;
                }
                else if (input == "b")
                {
                    return "";
                    userDone = true;
                }
                else
                {
                    Console.WriteLine("Invalid selection. Press Enter to try again.");
                    Console.ReadLine();
                }
            }
            return result;
        }

        private string EditCurioFlow(Curio curio)
        {
            bool done = false;

            string name = curio.Name;
            string description = curio.Description;
            RarityLevel rarity = curio.Rarity;

            while (!done)
            {
                Console.Clear();

                Console.WriteLine("=== Edit Curio ===");
                Console.WriteLine($"1. Name: {name}");
                Console.WriteLine($"2. Description: {description}");
                Console.WriteLine($"3. Rarity: {rarity}");
                Console.WriteLine("C. Confirm");
                Console.WriteLine("X. Cancel");

                Console.Write("\nSelect field to edit: ");
                string input = Console.ReadLine().ToLower();

                if (input == "1")
                {
                    Console.Write("New name: ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName))
                        name = newName;
                }
                else if (input == "2")
                {
                    Console.Write("New description: ");
                    string newDesc = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newDesc))
                        description = newDesc;
                }
                else if (input == "3")
                {
                    rarity = SelectRarityInline();
                }
                else if (input == "c")
                {
                    curio.UpdateForEdit(name, description, rarity);
                    done = true;
                    return "Curio updated successfully.";
                }
                else if (input == "x")
                {
                    done = true;
                    return "Edit cancelled.";
                }
            }
            return "Edit cancelled.";
        }

        private RarityLevel SelectRarityInline()
        {
            Console.Clear();
            Console.WriteLine("=== Rarity Options ===");

            foreach (var value in Enum.GetValues(typeof(RarityLevel)))
            {
                int displayNumber = (int)value + 1;
                Console.WriteLine($"{displayNumber}. {value}");
            }

            Console.Write("\nSelect rarity: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int rarityValue) &&
                rarityValue >= 1 &&
                rarityValue <= Enum.GetValues(typeof(RarityLevel)).Length)
            {
                return (RarityLevel)(rarityValue - 1);
            }

            return RarityLevel.Common;
        }
    }
}
using MuseumOfCurios.Curios;

namespace MuseumOfCurios.Core
{
    // Core application controller.
    // Responsible for maintaining application state and coordinating:
    // - user input handling
    // - exhibit navigation
    // - curio interaction flows
    // - creation and editing workflows
    public class LibraryController
    {
        // Tracks whether user has selected an exhibit or is still in selection mode.
        private bool exhibitSelected = false;

        // Currently active exhibit filter applied to catalogue queries.
        private ExhibitType currentExhibit = ExhibitType.All;

        // Controls termination of the main application loop.
        private bool exitRequested = false;

        // Central in-memory data store for all Curios (system + user-generated).
        private CurioCatalogue catalogue = new CurioCatalogue();

        // -----------------------------
        // MAIN APPLICATION LOOP
        // -----------------------------
        // Drives the entire program lifecycle:
        // - renders UI
        // - reads input
        // - routes commands
        public void Run()
        {
            string lastResult = "";

            while (!exitRequested)
            {
                LibraryApp.DisplayScreen(currentExhibit, exhibitSelected, catalogue, lastResult);

                string input = ReadInput();

                if (string.IsNullOrWhiteSpace(input))
                {
                    lastResult = "Invalid input.";
                    continue;
                }

                // -----------------------------
                // EXHIBIT SELECTION MODE
                // -----------------------------
                if (!exhibitSelected)
                {
                    if (int.TryParse(input, out int exhibitChoice) &&
                        exhibitChoice >= 1 && exhibitChoice <= 8)
                    {
                        currentExhibit = (ExhibitType)(exhibitChoice - 1);
                        exhibitSelected = true;
                        lastResult = $"Now viewing: {FormatExhibitName(currentExhibit)}";
                        continue;
                    }

                    lastResult = "Please select a valid exhibit (1–8).";
                    continue;
                }

                // -----------------------------
                // MAIN VIEWING MODE
                // -----------------------------
                if (input == "x")
                {
                    exitRequested = true;
                    Console.WriteLine("\nThank you for visiting the Museum of Curios. Goodbye!");
                }
                else if (input == "s")
                {
                    exhibitSelected = false;
                    lastResult = "Select an exhibit to view:";
                }
                else if (input == "c")
                {
                    CurioCreator creator = new CurioCreator(catalogue);
                    lastResult = creator.CreateCurioFlow();
                }
                else if (int.TryParse(input, out int userChoice))
                {
                    List<Curio> curios = catalogue.GetCuriosByExhibit(currentExhibit);

                    int index = userChoice - 1;

                    if (index >= 0 && index < curios.Count)
                    {
                        lastResult = CurioInteractionFlow(curios[index], index);
                    }
                    else
                    {
                        lastResult = "Invalid curio selection.";
                    }
                }
                else
                {
                    lastResult = "Invalid input.";
                }
            }
        }

        // Normalizes console input for consistent command handling.
        private string ReadInput()
        {
            return Console.ReadLine()?.ToLower().Trim() ?? "";
        }

        // Converts enum values into user-friendly display strings for UI output.
        // Keeps presentation logic out of raw enum names (e.g., "RollingGrasslands").
        private string FormatExhibitName(ExhibitType exhibit)
        {
            return exhibit switch
            {
                ExhibitType.All => "All Curios",
                ExhibitType.RollingGrasslands => "Rolling Grasslands Exhibit",
                ExhibitType.SunscorchedPlateau => "Sunscorched Plateau Exhibit",
                ExhibitType.TallPalmTropics => "Tall Palm Tropics Exhibit",
                ExhibitType.FrostpeakHighlands => "Frostpeak Highlands Exhibit",
                ExhibitType.SeasAndSands => "Seas and Sands Exhibit",
                ExhibitType.JadePeaks => "Jade Peaks Exhibit",
                ExhibitType.Custom => "Custom Curios Exhibit",
                _ => "Unknown Exhibit"
            };
        }

        // Displays the available actions for a selected curio.
        // Custom curios expose additional management options (edit/delete).
        private void PrintCurioMenu(bool isCustom)
        {
            Console.WriteLine();

            Console.WriteLine("[E] Examine");

            if (isCustom)
            {
                Console.WriteLine("[D] Delete");
                Console.WriteLine("[T] Edit");
            }

            Console.WriteLine("[B] Back");
        }

        // Handles interaction with a selected curio.
        // Delegates actions such as examine, edit, and delete.
        // Returns a result string to be displayed in the UI.
        private string CurioInteractionFlow(Curio curio, int index)
        {
            while (true)
            {
                PrintCurioMenu(curio.IsCustom);

                Console.Write("Choose an action: ");
                string input = ReadInput();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input == "e")
                {
                    return curio.Examine();
                }
                else if (input == "d" && curio.IsCustom)
                {
                    bool success = catalogue.RemoveCurioAt(index, out string message);
                    return message;
                }
                else if (input == "t" && curio.IsCustom)
                {
                    return EditCurioFlow(curio);
                }
                else if (input == "b")
                {
                    return "";
                }
                else
                {
                    Console.WriteLine("Invalid selection. Press Enter to try again.");
                    Console.ReadLine();
                }
            }
        }

        // Handles editing of a custom curio.
        // Allows modification of name, description, and rarity.
        // Changes are only committed when confirmed.
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
                string input = ReadInput();

                if (input == "1")
                {
                    Console.Write("New name: ");
                    string newName = ReadInput();
                    if (!string.IsNullOrWhiteSpace(newName))
                        name = newName;
                }
                else if (input == "2")
                {
                    Console.Write("New description: ");
                    string newDesc = ReadInput();
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
                    return "Curio updated successfully.";
                }
                else if (input == "x")
                {
                    return "Edit cancelled.";
                }
            }

            return "Edit cancelled.";
        }

        // Displays a simple rarity selection menu and converts user input
        // into a valid RarityLevel enum value.
        // Falls back to Common if input is invalid.
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
            string input = ReadInput();

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
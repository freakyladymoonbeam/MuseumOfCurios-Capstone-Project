namespace MuseumOfCurios.Curios
{
    public class CurioCreator
    {
        private readonly CurioCatalogue _catalogue;

        public CurioCreator(CurioCatalogue catalogue)
        {
            _catalogue = catalogue;
        }

        public string CreateCurioFlow()
        {
            bool userRequestedExit = false;

            string name = InputName(ref userRequestedExit);
            if (userRequestedExit) return "Curio creation cancelled.";

            string description = InputDescription(ref userRequestedExit);
            if (userRequestedExit) return "Curio creation cancelled.";

            RarityLevel rarity = SelectRarity(ref userRequestedExit);
            if (userRequestedExit) return "Curio creation cancelled.";

            bool confirmed = ConfirmCurio(ref name, ref description, ref rarity, ref userRequestedExit);
            if (!confirmed) return "Curio creation cancelled.";

            CustomCurio newCurio = new CustomCurio(name, description, rarity);
            _catalogue.AddCurio(newCurio);

            return $"Curio \"{name}\" added successfully!";
        }

        // --- Name Input ---
        private string InputName(ref bool exitFlag)
        {
            string name = "";
            while (!exitFlag)
            {
                Console.Clear();
                Console.Write("Enter curio name (or type 'exit' to cancel): ");
                string input = Console.ReadLine();
                if (input.ToLower() == "exit") { exitFlag = true; break; }
                if (!string.IsNullOrWhiteSpace(input)) { name = input; break; }
                Console.Write("Name cannot be blank! Press Enter to try again.");
                Console.ReadLine();
            }
            return name;
        }

        // --- Description Input ---
        private string InputDescription(ref bool exitFlag)
        {
            string description = "";
            while (!exitFlag)
            {
                Console.Clear();
                Console.Write("Enter curio description (or type 'exit' to cancel): ");
                string input = Console.ReadLine();
                if (input.ToLower() == "exit") { exitFlag = true; break; }
                if (!string.IsNullOrWhiteSpace(input)) { description = input; break; }
                Console.Write("Description cannot be blank! Press Enter to try again.");
                Console.ReadLine();
            }
            return description;
        }

        // --- Rarity Selection ---
        private RarityLevel SelectRarity(ref bool exitFlag)
        {
            RarityLevel rarity = RarityLevel.Common;
            while (!exitFlag)
            {
                Console.Clear();
                Console.WriteLine("=== Rarity Options ===");
                foreach (var value in Enum.GetValues(typeof(RarityLevel)))
                {
                    int displayNumber = (int)value + 1;
                    Console.WriteLine($"{displayNumber}. {value}");
                }

                Console.Write("Select the curio's rarity by number (or type 'exit' to cancel): ");
                string input = Console.ReadLine();
                if (input.ToLower() == "exit") { exitFlag = true; break; }

                if (int.TryParse(input, out int rarityValue) &&
                    rarityValue >= 1 &&
                    rarityValue <= Enum.GetValues(typeof(RarityLevel)).Length)
                {
                    rarity = (RarityLevel)(rarityValue - 1);
                    break;
                }
                Console.Write("Invalid selection. Press Enter to try again.");
                Console.ReadLine();
            }
            return rarity;
        }

        // --- Confirmation & Editing ---
        private bool ConfirmCurio(ref string name, ref string description, ref RarityLevel rarity, ref bool exitFlag)
        {
            while (!exitFlag)
            {
                Console.Clear();
                Console.WriteLine("=== Confirm Curio ===");
                Console.WriteLine($"1. Name: {name}");
                Console.WriteLine($"2. Description: {description}");
                Console.WriteLine($"3. Rarity: {rarity}");
                Console.Write("Type the number to edit, 'C' to confirm, or 'X' to cancel: ");
                string input = Console.ReadLine().ToLower();

                if (input == "x") { exitFlag = true; break; }
                else if (input == "c") return true;
                else if (input == "1") name = InputName(ref exitFlag);
                else if (input == "2") description = InputDescription(ref exitFlag);
                else if (input == "3") rarity = SelectRarity(ref exitFlag);
                else
                {
                    Console.Write("Invalid input. Press Enter to try again.");
                    Console.ReadLine();
                }
            }
            return false;
        }
    }
}
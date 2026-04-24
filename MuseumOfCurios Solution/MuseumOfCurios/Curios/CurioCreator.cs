namespace MuseumOfCurios.Curios
{
    // Responsible for the guided creation flow of CustomCurio objects.
    // This class acts as a temporary state machine that collects user input step-by-step,
    // validates it, and produces a final Curio instance if confirmed.
    public class CurioCreator
    {
        // Reference to the shared catalogue so newly created curios
        // can be persisted into the in-memory collection.
        private readonly CurioCatalogue _catalogue;

        public CurioCreator(CurioCatalogue catalogue)
        {
            _catalogue = catalogue;
        }

        // Entry point for the creation workflow.
        // This method orchestrates the full multi-step input process.
        public string CreateCurioFlow()
        {
            // Shared cancellation flag used across all input stages.
            // Any step can trigger early exit by setting this flag.
            bool userRequestedExit = false;

            // Step 1: Name input
            string name = InputName(ref userRequestedExit);
            if (userRequestedExit) return "Curio creation cancelled.";

            // Step 2: Description input
            string description = InputDescription(ref userRequestedExit);
            if (userRequestedExit) return "Curio creation cancelled.";

            // Step 3: Rarity selection
            RarityLevel rarity = SelectRarity(ref userRequestedExit);
            if (userRequestedExit) return "Curio creation cancelled.";

            // Step 4: Final confirmation step (allows edits before commit)
            bool confirmed = ConfirmCurio(ref name, ref description, ref rarity, ref userRequestedExit);
            if (!confirmed) return "Curio creation cancelled.";

            // Construct final domain object after validation + confirmation
            CustomCurio newCurio = new CustomCurio(name, description, rarity);

            // Persist into catalogue (runtime-only unless persistence layer is added later)
            _catalogue.AddCurio(newCurio);

            return $"Curio \"{name}\" added successfully!";
        }

        // -----------------------------
        // STEP 1: NAME INPUT STAGE
        // -----------------------------
        // Responsible for collecting a valid curio name.
        // Loop continues until valid input is provided or exit is requested.
        private string InputName(ref bool exitFlag)
        {
            string name = "";

            while (!exitFlag)
            {
                Console.Clear();
                Console.Write("Enter curio name (or type 'exit' to cancel): ");

                string input = Console.ReadLine();

                // Global cancellation condition for flow control
                if (input.ToLower() == "exit")
                {
                    exitFlag = true;
                    break;
                }

                // Validation: prevent empty or whitespace-only names
                if (!string.IsNullOrWhiteSpace(input))
                {
                    name = input;
                    break;
                }

                Console.Write("Name cannot be blank! Press Enter to try again.");
                Console.ReadLine();
            }

            return name;
        }

        // -----------------------------
        // STEP 2: DESCRIPTION INPUT STAGE
        // -----------------------------
        // Collects descriptive metadata for the curio.
        private string InputDescription(ref bool exitFlag)
        {
            string description = "";

            while (!exitFlag)
            {
                Console.Clear();
                Console.Write("Enter curio description (or type 'exit' to cancel): ");

                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                {
                    exitFlag = true;
                    break;
                }

                if (!string.IsNullOrWhiteSpace(input))
                {
                    description = input;
                    break;
                }

                Console.Write("Description cannot be blank! Press Enter to try again.");
                Console.ReadLine();
            }

            return description;
        }

        // -----------------------------
        // STEP 3: RARITY SELECTION STAGE
        // -----------------------------
        // Allows user to assign a rarity level from predefined enum values.
        private RarityLevel SelectRarity(ref bool exitFlag)
        {
            RarityLevel rarity = RarityLevel.Common;

            while (!exitFlag)
            {
                Console.Clear();
                Console.WriteLine("=== Rarity Options ===");

                // Display all enum values with numeric selection mapping
                foreach (var value in Enum.GetValues(typeof(RarityLevel)))
                {
                    int displayNumber = (int)value + 1;
                    Console.WriteLine($"{displayNumber}. {value}");
                }

                Console.Write("Select the curio's rarity by number (or type 'exit' to cancel): ");

                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                {
                    exitFlag = true;
                    break;
                }

                // Validate numeric selection against enum range
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

        // -----------------------------
        // STEP 4: CONFIRMATION STAGE
        // -----------------------------
        // Final review step allowing edits before committing creation.
        // This acts as a lightweight "edit buffer" before object instantiation.
        private bool ConfirmCurio(
            ref string name,
            ref string description,
            ref RarityLevel rarity,
            ref bool exitFlag)
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

                if (input == "x")
                {
                    exitFlag = true;
                    break;
                }
                else if (input == "c")
                {
                    return true;
                }
                else if (input == "1")
                {
                    name = InputName(ref exitFlag);
                }
                else if (input == "2")
                {
                    description = InputDescription(ref exitFlag);
                }
                else if (input == "3")
                {
                    rarity = SelectRarity(ref exitFlag);
                }
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
using MuseumOfCurios.Curios;

namespace MuseumOfCurios.Core
{
    // Pure presentation layer responsible only for rendering state.
    // This class contains no business logic, input handling, or mutation.
    public static class LibraryApp
    {
        // Renders the current application state to the console.
        // Displays either:
        // - exhibit selection menu
        // - curio list view for selected exhibit
        public static void DisplayScreen(
            ExhibitType currentExhibit,
            bool exhibitSelected,
            CurioCatalogue catalogue,
            string lastResult)
        {
            Console.Clear();

            Console.WriteLine("==== Curios ====");

            // -----------------------------
            // EXHIBIT SELECTION MODE
            // -----------------------------
            if (!exhibitSelected)
            {
                Console.WriteLine("Select an Exhibit:");
                Console.WriteLine("1. All");
                Console.WriteLine("2. Rolling Grasslands");
                Console.WriteLine("3. Sunscorched Plateau");
                Console.WriteLine("4. Tall Palm Tropics");
                Console.WriteLine("5. Frostpeak Highlands");
                Console.WriteLine("6. Seas and Sands");
                Console.WriteLine("7. Jade Peaks");
                Console.WriteLine("8. Custom Curios");

                Console.WriteLine();

                if (!string.IsNullOrEmpty(lastResult))
                    Console.WriteLine(lastResult);

                Console.Write("\nEnter exhibit number: ");
                return;
            }

            // -----------------------------
            // CURIO VIEW MODE
            // -----------------------------
            // Displays all curios filtered by active exhibit.
            Console.WriteLine($"Current Exhibit: {currentExhibit}");
            Console.WriteLine("\n[S] Switch Exhibit");
            Console.WriteLine("[C] Create Curio");
            Console.WriteLine("[X] Exit");

            Console.WriteLine();

            List<Curio> curios = catalogue.GetCuriosByExhibit(currentExhibit);

            for (int i = 0; i < curios.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {curios[i].Name}");
            }

            if (!string.IsNullOrEmpty(lastResult))
                Console.WriteLine("\n" + lastResult);

            Console.Write("\nEnter curio number, or action: ");
        }
    }
}
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

                string userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Please enter a valid input.");
                    continue;
                }

                if (userInput.ToLower() == "x")
                {
                    exitRequested = true;
                    Console.WriteLine();
                    Console.WriteLine("Thank you for visiting the Museum of Curios. Goodbye!");
                }
                else if (int.TryParse(userInput, out int userChoice))
                {
                    Curio selected = catalogue.GetCurioByIndex(userChoice - 1);
                    if (selected != null)
                    {
                        lastResult = selected.Examine();
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
    }
}
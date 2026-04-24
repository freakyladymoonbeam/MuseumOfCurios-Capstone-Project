using System.Reflection;

namespace MuseumOfCurios.Curios
{
    // Represents the available exhibit filters used by the UI layer.
    // This enum acts as a bridge between user-facing selection (UI)
    // and internal filtering logic based on Curio origin or type.
    public enum ExhibitType
    {
        All,
        RollingGrasslands,
        SunscorchedPlateau,
        TallPalmTropics,
        FrostpeakHighlands,
        SeasAndSands,
        JadePeaks,
        Custom
    }

    public class CurioCatalogue
    {
        // In-memory store of all curios currently available in the system.
        // Includes both reflection-discovered base curios and runtime-created custom curios.
        private List<Curio> curios;

        public CurioCatalogue()
        {
            // Catalogue initialization is delegated to reflection-based discovery.
            // This allows new Curio subclasses to be added without modifying this class.
            curios = DiscoverCurios();
        }

        // Uses reflection to locate and instantiate all valid Curio subclasses in the assembly.
        // This is effectively a "plugin discovery" mechanism for curios.
        private List<Curio> DiscoverCurios()
        {
            var curioType = typeof(Curio);

            // Find all concrete, non-abstract types that inherit from Curio
            // and have a parameterless constructor (required for Activator creation).
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => !t.IsAbstract &&
                            curioType.IsAssignableFrom(t) &&
                            t.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            var instances = new List<Curio>();

            foreach (var type in types)
            {
                // Dynamically create instances of each discovered Curio type.
                // This enables extensibility without manual registration.
                var instance = (Curio)Activator.CreateInstance(type);

                if (instance != null)
                {
                    instances.Add(instance);
                }
            }

            // Apply default ordering to ensure consistent UI presentation:
            // 1. Rarity (primary sort)
            // 2. Name (secondary sort)
            return instances
                .OrderBy(c => c.Rarity)
                .ThenBy(c => c.Name)
                .ToList();
        }

        // Returns the full unfiltered dataset.
        // Used primarily when no exhibit filtering is applied.
        public List<Curio> GetAllCurios()
        {
            return curios;
        }

        // Returns a filtered subset of curios based on the selected exhibit.
        // This is the primary query method used by the UI layer.
        public List<Curio> GetCuriosByExhibit(ExhibitType exhibit)
        {
            // Special case: "All" bypasses filtering entirely.
            if (exhibit == ExhibitType.All)
                return curios;

            return curios.Where(c =>
            {
                return exhibit switch
                {
                    // Region-based filtering maps exhibit selection to Curio Origin
                    ExhibitType.RollingGrasslands => c.Origin == Origin.RollingGrasslands,
                    ExhibitType.SunscorchedPlateau => c.Origin == Origin.SunscorchedPlateau,
                    ExhibitType.TallPalmTropics => c.Origin == Origin.TallPalmTropics,
                    ExhibitType.FrostpeakHighlands => c.Origin == Origin.FrostpeakHighlands,
                    ExhibitType.SeasAndSands => c.Origin == Origin.SeasAndSands,
                    ExhibitType.JadePeaks => c.Origin == Origin.JadePeaks,

                    // Custom exhibit is defined by runtime-created content only
                    ExhibitType.Custom => c.IsCustom,

                    // Fallback safety case (should not normally be reached)
                    _ => true
                };
            }).ToList();
        }

        // Retrieves a curio by its index in the full catalogue list.
        // NOTE: This does NOT account for filtered views — it assumes global indexing.
        public Curio GetCurioByIndex(int index)
        {
            // Defensive bounds check prevents invalid memory access or runtime errors.
            if (index >= 0 && index < curios.Count)
            {
                return curios[index];
            }

            // Null return indicates invalid selection; handled by UI layer.
            return null;
        }

        // Adds a user-created curio to the catalogue at runtime.
        // These entries exist only in memory unless persistence is added later.
        public void AddCurio(CustomCurio curio)
        {
            // Null guard ensures catalogue integrity.
            if (curio != null)
            {
                curios.Add(curio);
            }
        }

        // Removes a curio only if it is user-created (custom).
        // Prevents deletion of base/discovered curios.
        public bool RemoveCurioAt(int index, out string message)
        {
            if (index >= 0 && index < curios.Count && curios[index].IsCustom)
            {
                Curio removedCurio = curios[index];

                curios.RemoveAt(index);

                message = $"Curio \"{removedCurio.Name}\" removed successfully!";
                return true;
            }
            else if (index >= 0 && index < curios.Count && !curios[index].IsCustom)
            {
                message = "Cannot remove a non-custom curio.";
                return false;
            }
            else
            {
                message = "Invalid entry. Please enter the number of a valid, custom curio.";
                return false;
            }
        }
    }
}
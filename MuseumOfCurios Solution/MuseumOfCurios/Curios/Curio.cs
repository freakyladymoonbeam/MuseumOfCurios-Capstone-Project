namespace MuseumOfCurios.Curios
{
    // Defines the rarity tiers used across all curios in the system.
    // This is a global classification system used for ordering, display, and flavor.
    public enum RarityLevel
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Mythical
    }

    // Defines the origin category for a curio.
    // Origins represent the world-region or source context of a curio.
    // This is the primary mechanism used for exhibit filtering in the UI layer.
    public enum Origin
    {
        RollingGrasslands,
        SunscorchedPlateau,
        TallPalmTropics,
        FrostpeakHighlands,
        SeasAndSands,
        JadePeaks,

        // Special case: runtime-generated curios created by the user.
        // These do not belong to a geographic origin and are treated separately in filtering logic.
        Custom
    }

    // Abstract base class representing all collectible objects in the Museum system.
    // A Curio defines immutable identity (Origin) and mutable descriptive metadata.
    public abstract class Curio
    {
        // Display name of the curio.
        // Can be modified only through controlled edit flows.
        public string Name { get; private set; }

        // Narrative or descriptive text associated with the curio.
        public string Description { get; private set; }

        // Classification value used for sorting and presentation logic.
        public RarityLevel Rarity { get; private set; }

        // Defines the source/origin category of the curio.
        // This is intentionally read-only to preserve historical integrity of discovered curios.
        public Origin Origin { get; }

        // Indicates whether this curio was created at runtime by the user.
        // Default implementation assumes all non-custom curios are system-defined artifacts.
        // CustomCurio overrides this to enable editing and deletion functionality.
        public virtual bool IsCustom => false;

        protected Curio(string name, string description, RarityLevel rarity, Origin origin)
        {
            Name = name;
            Description = description;
            Rarity = rarity;
            Origin = origin;
        }

        // Returns a formatted representation of the curio for inspection/examination.
        // This is used in UI display when a user selects a curio.
        public virtual string Examine()
        {
            return $"[{Rarity}] {Name} ({Origin}): {Description}";
        }

        // Allows controlled mutation of a curio's descriptive fields.
        // Used exclusively in edit workflows; does not affect identity (Origin).
        public void UpdateForEdit(string name, string description, RarityLevel rarity)
        {
            Name = name;
            Description = description;
            Rarity = rarity;
        }
    }
}
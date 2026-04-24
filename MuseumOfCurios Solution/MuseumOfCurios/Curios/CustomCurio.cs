namespace MuseumOfCurios.Curios
{
    // Represents a user-generated Curio instance.
    // This class exists to distinguish runtime-created content from
    // system-discovered Curios loaded via reflection.
    //
    // CustomCurio enables:
    // - user-generated entries
    // - edit functionality
    // - deletion support
    // - separate "Custom" exhibit classification
    public class CustomCurio : Curio
    {
        // Overrides base classification to mark this Curio as user-generated.
        // This flag is used by UI and catalogue logic to enable special behaviors
        // such as editing and removal.
        public override bool IsCustom => true;

        // Constructs a Curio that is explicitly categorized as "Custom".
        // Origin is forcibly set to Origin.Custom to ensure consistent filtering behavior.
        public CustomCurio(string name, string description, RarityLevel rarity)
            : base(name, description, rarity, Origin.Custom)
        {
        }
    }
}
namespace MuseumOfCurios.Curios
{
    public class CustomCurio : Curio
    {
        public override bool IsCustom => true; // Override the IsCustom property to return true for this class, indicating that this curio is a custom creation
        public CustomCurio(string name, string description, RarityLevel rarity) : base(name, description, rarity)
        {
        }
    }
}
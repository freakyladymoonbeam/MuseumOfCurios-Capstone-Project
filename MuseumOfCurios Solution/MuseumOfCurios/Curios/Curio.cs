namespace MuseumOfCurios.Curios
{
    public enum RarityLevel
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Mythical
    }

    public abstract class Curio
    {
        public string Name { get; }
        public string Description { get; }
        public RarityLevel Rarity { get; }
        public virtual bool IsCustom => false; // Indicates whether this curio is a custom creation, and automatically returns false for all curios that are not explicitly marked as custom

        protected Curio(
            string name,
            string description,
            RarityLevel rarity = RarityLevel.Common)
        {
            Name = name;
            Description = description;
            Rarity = rarity;
        }

        public virtual string Examine()
        {
            return $"[{Rarity}] {Name}: {Description}";
        }
    }
}
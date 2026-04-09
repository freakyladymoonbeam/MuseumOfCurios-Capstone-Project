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
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public string Description { get; }
        public RarityLevel Rarity { get; }
        public bool IsCustom { get; }

        protected Curio(
            string name,
            string description,
            RarityLevel rarity = RarityLevel.Common,
            bool isCustom = false)
        {
            Name = name;
            Description = description;
            Rarity = rarity;
            IsCustom = isCustom;
        }

        public virtual string Examine()
        {
            return $"[{Rarity}] {Name}: {Description}";
        }
    }
}
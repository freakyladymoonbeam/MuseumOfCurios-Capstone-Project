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

    public enum Origin
    {
        RollingGrasslands,
        SunscorchedPlateau,
        TallPalmTropics,
        FrostpeakHighlands,
        SeasAndSands,
        JadePeaks,
        Custom
    }

    public abstract class Curio
    {
        public string Name { get; private set; } // The name of the curio
        public string Description { get; private set; }  // A brief description of the curio
        public RarityLevel Rarity { get; private set; } // The rarity level of the curio
        public Origin Origin { get; } // The region where the curio was found
        public virtual bool IsCustom => false; // Indicates whether this curio is a custom creation, and automatically returns false for all curios that are not explicitly marked as custom

        protected Curio(string name, string description, RarityLevel rarity, Origin origin)
        {
            Name = name;
            Description = description;
            Rarity = rarity;
            Origin = origin;
        }

        public virtual string Examine()
        {
            return $"[{Rarity}] {Name} ({Origin}): {Description}";
        }

        public void UpdateForEdit(string name, string description, RarityLevel rarity)
        {
            Name = name;
            Description = description;
            Rarity = rarity;
        }


    }
}
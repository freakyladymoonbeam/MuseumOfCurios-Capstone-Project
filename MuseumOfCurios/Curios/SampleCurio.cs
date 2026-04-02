namespace MuseumOfCurios.Curios
{
    // Because Curio.cs defines Curio as an abstract class, we need to create a concrete implementation to instantiate it. This SampleCurio class serves that purpose, allowing us to create sample curios for testing and demonstration.
    class SampleCurio : Curio // Inherits from the abstract Curio class, providing a simple implementation that can be instantiated and added to our catalogue.
    {
        public SampleCurio(string name, string description, RarityLevel rarity) : base(name, description, rarity)
        {
            // No additional properties or methods are needed for this sample curio, but it could easily be extended with more specific behavior or attributes if desired.
            // example: public string Origin { get; set; }, which would allow us to specify where the curio came from, adding more depth to our catalogue.
        }
    }
}
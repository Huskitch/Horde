namespace HoardeGame.Entities
{
    /// <summary>
    /// Defines a provider of the player entity
    /// </summary>
    public interface IPlayerProvider
    {
        /// <summary>
        /// Gets the player entity
        /// </summary>
        EntityPlayer Player { get; }
    }
}
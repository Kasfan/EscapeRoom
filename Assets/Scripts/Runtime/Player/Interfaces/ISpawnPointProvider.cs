namespace Runtime.Networking
{
    /// <summary>
    /// Provides spawn points
    /// </summary>
    public interface ISpawnPointProvider
    {
        /// <summary>
        /// Returns random spawn point
        /// </summary>
        /// <returns>the point or Null if no available point found</returns>
        ISpawnPoint GetRandomPoint();
    }
}
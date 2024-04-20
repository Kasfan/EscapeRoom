using UnityEngine;

namespace Runtime.Networking
{
    /// <summary>
    /// Player in the game
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Player camera anchor
        /// </summary>
        Transform CameraAttach { get; }
    }
}
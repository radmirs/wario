using UnityEngine;

namespace wario.Movement
{
    public interface IMovementDirectionSource
    {
        Vector3 MovementDirection { get; }

    }
}
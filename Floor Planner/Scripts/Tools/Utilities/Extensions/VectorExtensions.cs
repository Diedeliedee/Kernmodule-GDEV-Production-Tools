using Godot;

namespace Joeri.Tools.Utilities
{
    public static class VectorExtensions
    {
        #region Vector2
        /// <summary>
        /// Converts the passed in 3D vector into a 2D vector with X remaining as X, and Z moved to Y.
        /// </summary>
        public static Vector2 Planar(this Vector3 _vector)
        {
            return new Vector2(_vector.X, _vector.Z);
        }
        #endregion

        #region Vector3
        /// /// <summary>
        /// Converts the passed in 2D vector into a 3D vector applied to X, and Z
        /// </summary>
        public static Vector3 Cubular(this Vector2 _vector, float _height = 0f)
        {
            return new Vector3(_vector.X, _height, _vector.Y);
        }
        #endregion
    }
}

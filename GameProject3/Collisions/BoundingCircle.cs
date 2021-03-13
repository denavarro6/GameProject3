using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject3.Collisions
{
    /// <summary>
    /// struct representing circular bounds
    /// </summary>
    public struct BoundingCircle
    {
        /// <summary>
        /// center of boundingcircle
        /// </summary>
        public Vector2 Center;
        /// <summary>
        /// radius of BoundingCircle
        /// </summary>
        public float Radius;
        /// <summary>
        /// constructs a new boundingcircle
        /// </summary>
        /// <param name="center">the center</param>
        /// <param name="radius">the radius</param>
        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }
        /// <summary>
        /// Tests for collision between this and another bounding circle
        /// </summary>
        /// <param name="other">other bounding circle</param>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}

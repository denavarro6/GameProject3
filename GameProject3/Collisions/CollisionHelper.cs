using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
namespace GameProject3.Collisions
{
    public static class CollisionHelper
    {
        /// <summary>
        /// detects collision between 2 bounding circles
        /// </summary>
        /// <param name="a">first bounding circle</param>
        /// <param name="b">second bounding circle</param>
        /// <returns>true for collision, false otherwise</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return a.Radius + b.Radius >= Math.Sqrt(
                Math.Pow(a.Center.X - b.Center.X, 2) + 
                Math.Pow(a.Center.Y - b.Center.Y, 2));
        }
        /// <summary>
        /// detects collision of rectangles
        /// </summary>
        /// <param name="a">first rectangle</param>
        /// <param name="b">second rectangle</param>
        /// <returns>true for collision false if not</returns>
        public static bool Collides( BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Top > b.Bottom || a.Bottom < b.Top);
        }
        /// <summary>
        /// detects collision between rectangle and circle
        /// </summary>
        /// <param name="c">the boundingcircle </param>
        /// <param name="r">the boundingrectangle</param>
        /// <returns>true for collision false if not</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            return Math.Pow(c.Radius, 2) >= Math.Pow(c.Center.X - nearestX, 2) + Math.Pow(c.Center.Y - nearestY, 2);
        }
        
    }
}

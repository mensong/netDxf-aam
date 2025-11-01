using System;
using System.Collections.Generic;
using System.Text;

namespace netDxf.aam
{
    /// <summary>
    /// Represents a 3D bounding box defined by minimum and maximum points.
    /// </summary>
    public class Extends
    {
        #region private fields
        private Vector3 minPoint;
        private Vector3 maxPoint;
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <c>Extends</c> class.
        /// </summary>
        public Extends()
        {
            this.minPoint = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);
            this.maxPoint = new Vector3(double.MinValue, double.MinValue, double.MinValue);
        }
        /// <summary>
        /// Initializes a new instance of the <c>Extends</c> class.
        /// </summary>
        /// <param name="minPoint"></param>
        /// <param name="maxPoint"></param>
        public Extends(Vector3 minPoint, Vector3 maxPoint)
        {
            this.minPoint = minPoint;
            this.maxPoint = maxPoint;
        }
        #endregion

        #region public properties
        /// <summary>
        /// Gets the minimum point of the bounding box.
        /// </summary>
        public Vector3 MinPoint
        {
            get { return this.minPoint; }
            set { this.minPoint = value; }
        }
        /// <summary>
        /// Gets the maximum point of the bounding box.
        /// </summary>
        public Vector3 MaxPoint
        {
            get { return this.maxPoint; }
            set { this.maxPoint = value; }
        }
        /// <summary>
        /// Gets the width of the bounding box (maxPoint.X - minPoint.X).
        /// </summary>
        public double Width
        {
            get { return this.maxPoint.X - this.minPoint.X; }
        }
        /// <summary>
        /// Gets the depth of the bounding box (maxPoint.Z - minPoint.Z).
        /// </summary>
        public double Height
        {
            get { return this.maxPoint.Y - this.minPoint.Y; }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Gets the minimum point of the bounding box.
        /// </summary>
        /// <param name="point"></param>
        public void AddPoint(Vector3 point)
        {
            if (point.X < this.minPoint.X) this.minPoint.X = point.X;
            if (point.Y < this.minPoint.Y) this.minPoint.Y = point.Y;
            if (point.Z < this.minPoint.Z) this.minPoint.Z = point.Z;
            if (point.X > this.maxPoint.X) this.maxPoint.X = point.X;
            if (point.Y > this.maxPoint.Y) this.maxPoint.Y = point.Y;
            if (point.Z > this.maxPoint.Z) this.maxPoint.Z = point.Z;
        }
        /// <summary>
        /// Adds the extends of another <c>Extends</c> object to the current one.
        /// </summary>
        /// <param name="extends"></param>
        public void AddExtends(Extends extends)
        {
            this.AddPoint(extends.MinPoint);
            this.AddPoint(extends.MaxPoint);
        }
        /// <summary>
        /// Extends the bounding box by a given value in all directions.
        /// </summary>
        /// <param name="value"></param>
        public void ExtendsBy(double value)
        {
            this.minPoint.X -= value;
            this.minPoint.Y -= value;
            this.minPoint.Z -= value;
            this.maxPoint.X += value;
            this.maxPoint.Y += value;
            this.maxPoint.Z += value;
        }
        #endregion
    }
}

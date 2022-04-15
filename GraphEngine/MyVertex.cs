namespace GraphEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;

    /// <summary>
    /// This is the class of a vertex object with a point, color, degree and number.
    /// </summary>
    public class MyVertex
    {
        /// <summary>
        /// This is a brush with a color.
        /// </summary>
        private SolidBrush brush; // we will make color changeable in the future.

        /// <summary>
        /// This is a point with x and y coordinate.
        /// </summary>
        private Point point;

        /// <summary>
        /// the radius of the vertex to be drawn.
        /// </summary>
        private double radius;

        /// <summary>
        /// This is the number of the vertex.
        /// </summary>
        private int number;

        /// <summary>
        /// Thisis the degree of our vertex.
        /// </summary>
        private int degree;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyVertex"/> class.
        /// Constructor for the vertex class.
        /// </summary>
        /// <param name="point">point.</param>
        public MyVertex(Point point)
        {
            this.Point = point;
            this.radius = 25;
            this.Brush = new SolidBrush(Color.Gold);
            this.Degree = 0;

            // should give the vertex a number to associate with it.
        }

        /// <summary>
        /// Gets or sets the degree of the vertex.
        /// </summary>
        public int Degree
        {
            get
            {
                return this.degree;
            }

            set
            {
                this.degree = value;
            }
        }

        /// <summary>
        /// Gets or Sets a point.
        /// </summary>
        public Point Point
        {
            get
            {
                return this.point;
            }

            set
            {
                this.point = value;
            }
        }

        /// <summary>
        /// Gets or Sets the brush. Mainly used to set the brush color which is the color of the vertex.
        /// </summary>
        public SolidBrush Brush
        {
            get
            {
                return this.brush;
            }

            set
            {
                this.brush = value;
            }
        }

        /// <summary>
        /// Gets or Sets the number of the vertex.
        /// </summary>
        public int Number
        {
            get
            {
                return this.number;
            }

            set
            {
                this.number = value;
            }
        }

        /// <summary>
        /// Gets or Sets the radius for the vertex.
        /// </summary>
        /// <returns>double.</returns>
        public double Radius()
        {
            return this.radius;
        }
    }
}

// we will need a way to remove vertices, maybe a specific button and then
// we get the mouse coordinates from when button pressed?
// if a vertice is deleted we must delete all the edges connected to it. 
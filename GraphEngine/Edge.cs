namespace GraphEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This is a class to represent edges between vertices.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// The first vertex in the edge.
        /// </summary>
        private MyVertex n; // first vertex // likely read only

        /// <summary>
        /// The second vertex in the edge.
        /// </summary>
        private MyVertex m; // second vertex // likely read only

        /// <summary>
        /// This has to do with the edge type.
        /// </summary>
        private string uniqueEdgeName; // unique identifier of an edge. // read only

        // if n == m then it is a self loop.
        // otherwise draw a straight line from center first vertex to center of second vertex
        // We will need to handle parallel edges but that won't be done here but instead in the forms class.

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class.
        /// </summary>
        /// <param name="mNew">vertex m.</param>
        /// <param name="nNew">vertex n.</param>
        /// <param name="uniqueEdgeNameNew">edge type.</param>
        public Edge(MyVertex mNew, MyVertex nNew, string uniqueEdgeNameNew)
        {
            this.n = nNew;
            this.m = mNew;
            this.uniqueEdgeName = uniqueEdgeNameNew;
        }

        /// <summary>
        /// Gets or Sets the vertex.
        /// </summary>
        public MyVertex N
        {
            get{return this.n; }
            set {this.n = value; }
        }

        /// <summary>
        /// Gets or Sets the Vertex.
        /// </summary>
        public MyVertex M
        {
            get { return this.m; }
            set { this.m = value; }
        }

        /// <summary>
        /// Gets or sets the edge type.
        /// </summary>
        public string UniqueEdgeName
        {
            get { return this.uniqueEdgeName; }
            set { this.uniqueEdgeName = value; }
        }
    }
}

// we will need a way to remove vertices, maybe a specific button and then
// we get the mouse coordinates from when button pressed?
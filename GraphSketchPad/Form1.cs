// <copyright file="Form1.cs" company="Caden Weiner">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/*
* Current Features
* Add Vertex
* Delete Vertex
* Add Edge
* Delete Edge
* Drag Vertex and Edge in real Time.
* Parallel Edges (Not Visualized yet). Deleting Edges may need to be changed for parallel edges.Maybe just add an ofset to each edge that is only used if parallel.
* Vertices are all numbered now.
*/

// Parallel edges. Each edge should be offset in x and y direction by (1, 1) and (-1,-1). Start at (0,0) add a new edge.

/*
 * User instructions
 * Right click for edges
 * Left click for vertices
 * Double left click to delete a vertex
 * Double right click to delete an edge
 * Left click creates a vertex. It does not create one if clicking on an active vertex. If an active vertex is displayed, change color of its edges.
 *
 */

namespace GraphSketchPad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using GraphEngine;

    /// <summary>
    /// This is the class of our form which runs our application.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// The active vertex.
        /// </summary>
        private MyVertex activeVertex;

        /// <summary>
        /// Whether or not it is drawing again.
        /// </summary>
        private bool drawing = false;

        /// <summary>
        /// Displays the degrees of all vertices.
        /// </summary>
        private bool displayDegrees = false;

        /// <summary>
        /// This shows the current edge type.
        /// </summary>
        private string currentEdgeType;

        /// <summary>
        /// Whether or not the vertex is currently being dragged.
        /// </summary>
        private bool dragging = false;

        /// <summary>
        /// This is whether or not an algorithm is being run.
        /// </summary>
        private bool algorithming = false;

        /// <summary>
        /// This is a dictionary of colors.
        /// </summary>
        private Dictionary<string, Color> colorDictionary = new Dictionary<string, Color>();

        /// <summary>
        /// This is the drag offset of the point.
        /// </summary>
        private Point dragOffset;

        /// <summary>
        /// this is the graph we are running for our application.
        /// </summary>
        private Graph graph = new Graph();

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// constructor for our form.
        /// </summary>
        public Form1()
        {
            this.currentEdgeType = "Regular Edge";
            this.InitializeComponent();
            this.AlgorithmComboBox();

            // this.Algorithm.Items.Add("LongestWalk"); // Take a node. Color all the nodes reachable from it and color their edges
            this.InitializeComboBox();
            this.InitializEdgeSettings();
        }

        private void InitializEdgeSettings()
        {
            this.EdgeSettings.Items.Add("Degrees");
            this.EdgeSettings.Items.Add("Links");
            this.EdgeSettings.Items.Add("Bridges");
            this.EdgeSettings.Items.Add("Bridges and Links");
            this.EdgeSettings.Items.Add("Regular Edge");
            this.EdgeSettings.Items.Add("Directed Edge");
            this.EdgeSettings.SelectedItem = this.EdgeSettings.Items[4];
        }

        private void AlgorithmComboBox()
        {
            this.Algorithm.Items.Add("ShortestPath");
            this.Algorithm.Items.Add("ColorRootedTree"); // color the tree by level
            this.Algorithm.Items.Add("InTree"); // this.Algorithm.Items.Add("HamiltonianCircuit");
            this.Algorithm.Items.Add("Identify");
            this.Algorithm.Items.Add("Collapse");
            this.Algorithm.Items.Add("Split");
            this.Algorithm.Items.Add("Adjacency Matrix");
            this.Algorithm.Items.Add("Bipartite");
            this.Algorithm.Items.Add("Prim");
            this.Algorithm.SelectedItem = this.Algorithm.Items[0];
        }

        /// <summary>
        /// This is the event for painting our picture box(displays graphic changes.
        /// </summary>
        /// <param name="sender">where the event is coming from.</param>
        /// <param name="e">the paint event.</param>
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            this.graph.RenumberVertices();
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
            this.graph.MyEdges.OrderBy(c => c.N.Number);
            List<(Edge, int)> parallelEdgeCount = new List<(Edge, int)>();

            // This is the code for prim. It only will display the prims algorithm results.
            if (this.algorithming == true && this.Algorithm.SelectedItem != null && this.Algorithm.SelectedItem.ToString() == "Prim")
            {
                this.DrawRegularEdges(parallelEdgeCount, g, "prim", this.graph.PrimMinSpanningTree());
                this.DrawVertices(g, "notactive");
                this.DrawVertices(g, "active");
                this.VerticesDisplay.Text = this.VerticesDisplay.Tag + this.graph.MyVertices.Count().ToString();
                this.EdgesDisplay.Text = this.EdgesDisplay.Tag + this.graph.MyEdges.Count().ToString();
                this.ComponentsDisplay.Text = this.ComponentsDisplay.Tag + this.graph.NumberOfComponents();
                return;
            }

            parallelEdgeCount = this.graph.FindParallelEdges(this.graph.MyEdges);
            this.DrawRegularEdges(parallelEdgeCount, g, "notactive", this.graph.MyEdges);
            this.DrawRegularEdges(parallelEdgeCount, g, "active", this.graph.MyEdges);

            // why two blocks? This allows us to draw the edges properly.
            // There is a layering so we must draw all the edges then add the vertices
            this.DrawParallelEdges(parallelEdgeCount, g, "notactive");
            this.DrawParallelEdges(parallelEdgeCount, g, "active");

            // Create font and brush.
            Font drawFont = new Font("Times New Roman", 12);
            SolidBrush drawBrush = new SolidBrush(Color.White);

            // Set format of string.
            StringFormat drawFormat = new StringFormat();

            // draws edges
            this.DrawLoops(g, "notactive");
            this.DrawLoops(g, "active");

            int source = 0;
            int target = 0;
            Debug.WriteLine(this.SourceBox.Text);
            Debug.WriteLine(this.TargetBox.Text);
            if (int.TryParse(this.SourceBox.Text, out source) && int.TryParse(this.TargetBox.Text, out target))
            {
                if (source >= 0 & target >= 0 && this.algorithming == true && this.Algorithm.SelectedItem != null && this.graph.MyVertices.Count > source && target < this.graph.MyVertices.Count && this.Algorithm.SelectedItem.ToString() == "ShortestPath")
                {
                    // we got a vailid input for all of them.
                    this.DrawPath(this.graph.ShortestPath(source, target), g, 5);
                }
            }

            if (int.TryParse(this.SourceBox.Text, out source))
            {
                Debug.WriteLine("Trying to draw tree.");
                if (source >= 0 && this.algorithming == true && this.Algorithm.SelectedItem != null && this.graph.MyVertices.Count > source && this.Algorithm.SelectedItem.ToString() == "InTree")
                {
                    // we got a vailid input for all of them.
                    this.graph.IsInTree(this.graph.MyVertices.ElementAt(source));
                }
                else if (source >= 0 && this.algorithming == true && this.Algorithm.SelectedItem != null && this.graph.MyVertices.Count > source && this.Algorithm.SelectedItem.ToString() == "ColorRootedTree")
                {
                    // we got a vailid input for all of them.
                    Debug.WriteLine("Coloring Tree");
                    this.graph.ColorRootedTree(this.graph.MyVertices.ElementAt(source), this.colorDictionary);
                    this.RunButton.Text = "Run";
                    this.algorithming = false;
                }
            }

            // this.DrawRegularEdges(parallelEdgeCount, g, "notactive", this.graph.MyEdges);// there is no longer overlap, no need to cover, need to adject parallel and active
            if (this.EdgeSettings.SelectedItem != null && this.EdgeSettings.SelectedItem.ToString() == "Bridges")
            {
                this.DrawBridgeEdges(this.graph.Bridges(), g); // will never be parallel or loops
            }
            else if (this.EdgeSettings.SelectedItem != null && this.EdgeSettings.SelectedItem.ToString() == "Links")
            {
                List<Edge> myLinks = this.graph.Links();

                // find links here. Draw loops. Draw regular links and parallel links.
                parallelEdgeCount = this.graph.FindParallelEdges(this.graph.Links());
                this.DrawParallelEdges(parallelEdgeCount, g, "links");
                this.DrawRegularEdges(parallelEdgeCount, g, "links", myLinks);
                this.DrawLoops(g, "links"); // all loops are links
            }
            else if (this.EdgeSettings.SelectedItem != null && this.EdgeSettings.SelectedItem.ToString() == "Bridges and Links")
            {
                this.DrawBridgeEdges(this.graph.Bridges(), g); // will never be parallel or loops
                List<Edge> myLinks = this.graph.Links();

                // find links here. Draw loops. Draw regular links and parallel links.
                parallelEdgeCount = this.graph.FindParallelEdges(this.graph.Links());
                this.DrawParallelEdges(parallelEdgeCount, g, "links");
                this.DrawRegularEdges(parallelEdgeCount, g, "links", myLinks);
            }

            // draws vertices
            this.DrawVertices(g, "notactive");
            this.DrawVertices(g, "active");

            this.VerticesDisplay.Text = this.VerticesDisplay.Tag + this.graph.MyVertices.Count().ToString();
            this.EdgesDisplay.Text = this.EdgesDisplay.Tag + this.graph.MyEdges.Count().ToString();
            this.ComponentsDisplay.Text = this.ComponentsDisplay.Tag + this.graph.NumberOfComponents();
        }

        /// <summary>
        /// This is an event for when the mouse goes down on the picture box.
        /// </summary>
        /// <param name="sender">this is where the event is sent from.</param>
        /// <param name="e">this is the mouse event arguement.</param>
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            MyVertex tempVertex = this.activeVertex;
            MyVertex newVertex = this.CheckIfVertexClicked(e.Location);
            if (e.Button != MouseButtons.Left)
            {
                // this must check if vertices are null because an edge must be between two nodes
                if (newVertex != null && tempVertex != null)
                {
                    foreach (var aVertex in this.graph.MyVertices.Where(c => c == tempVertex))
                    {
                        // now edges can't be made with the temporary active vertex, which is just conditional on clicked location, it still returns something even if vertex isn't in entry list so created edges to nothing
                        Edge newEdge = new Edge(newVertex, tempVertex, this.currentEdgeType); // this is working.

                        // only create an edge on right click
                        this.graph.AddEdge(newEdge);

                        // only should switch to a different vertex if we left click it.
                        this.drawing = true;
                    }
                }

                return;
            }

            this.activeVertex = newVertex;
            this.drawing = false;
            this.dragging = false;

            // adding edges should go here

            if (this.activeVertex == null)
            {
                // should not create a 
                this.activeVertex = new MyVertex(e.Location);
                this.drawing = true;
            }
            else
            {
                // we actually selected a vertex
                this.dragging = true;
                this.dragOffset = new Point(this.activeVertex.Point.X - e.Location.X, this.activeVertex.Point.Y - e.Location.Y);
            }

            // checks to see if we are changing the color.
            if (this.activeVertex != null && this.ColorComboBox.SelectedItem != null && this.colorDictionary.ContainsKey(this.ColorComboBox.SelectedItem.ToString()))
            {
                if (this.activeVertex.Brush.Color != this.colorDictionary[this.ColorComboBox.SelectedItem.ToString()])
                {
                    this.drawing = true; // no need to color if already the same color. Fixed the slowness when drawing while coloring.
                    this.activeVertex.Brush = new SolidBrush(this.colorDictionary[this.ColorComboBox.SelectedItem.ToString()]);
                }
            }

            // checks to see if we are spliting the vertex.
            if (this.activeVertex != null && this.algorithming == true && this.Algorithm.SelectedItem != null && this.Algorithm.SelectedItem.ToString() == "Split")
            {
                this.drawing = true;
                this.RunButton.Text = "Run";
                this.algorithming = false;
                this.graph.SplitVertex(this.activeVertex);
            }

            // the checks if bipartite is selected.
            if (this.activeVertex != null && this.algorithming == true && this.Algorithm.SelectedItem != null && this.Algorithm.SelectedItem.ToString() == "Bipartite")
            {
                this.drawing = true;
                this.RunButton.Text = "Run";
                this.algorithming = false;
            }
        }

        /// <summary>
        /// checks if a vertex is clicked.
        /// </summary>
        /// <param name="point">this is the point we clicked, check if a vertex is within it.</param>
        /// <returns>the clicked vertex or null if not clicked.</returns>
        private MyVertex CheckIfVertexClicked(Point point)
        {
            return
                this.graph.MyVertices.FirstOrDefault(
                    vertex =>
                        Math.Abs(vertex.Point.X - point.X) < vertex.Radius() &&
                        Math.Abs(vertex.Point.Y - point.Y) < vertex.Radius());
        }

        /// <summary>
        /// Math and contitional logic to determine if a loop is clicked.
        /// </summary>
        /// <param name="point">the point being clicked</param>
        /// <returns>the clicked loop.</returns>
        private Edge CheckIfLoopClicked(Point point)
        {
            return
                this.graph.MyEdges.FirstOrDefault(
                    edge =>
                        edge.N == edge.M
                        &&
                        Math.Abs(edge.N.Point.X - point.X) < edge.N.Radius() + 3
                        &&
                        Math.Abs(edge.N.Point.Y - 22.5 - point.Y) < edge.N.Radius() + 3
                        &&
                        (Math.Abs(edge.N.Point.X - point.X) > edge.N.Radius() - 7.5
                        ||
                        Math.Abs(edge.N.Point.Y - 22.5 - point.Y) > edge.N.Radius() - 7.5)
                        );
        }

        /// <summary>
        /// Checks if an edge is clicked. Uses knowledge of distances between pointes to calculate.
        /// </summary>
        /// <param name="point">the point clicked.</param>
        /// <returns>the edge clicked.</returns>
        private Edge CheckIfEdgeClicked(Point point)
        {
            return // check that
                this.graph.MyEdges.FirstOrDefault(
                    edge =>
                        (
                            edge.N != edge.M
                            &&
                            this.Distance(edge.M.Point, point) + this.Distance(edge.N.Point, point) >= this.Distance(edge.M.Point, edge.N.Point) - 2
                            &&
                            this.Distance(edge.M.Point, point) + this.Distance(edge.N.Point, point) <= this.Distance(edge.M.Point, edge.N.Point) + 2
                        ));

            // M.x - N.x / (M.y - N.y) * x ~= y
            // Distance (A, C) + Distance (B, C) == Distance (A,B)
            // A---C------B if true otherwise it is a triangle or invalid
            // A ---- B
            // \     /
            //  \   /
            //    C
            // fixed
            // makes use of triangle inequality
            // works now.
        }

        /// <summary>
        /// This determines the distance between points.
        /// </summary>
        /// <param name="a">point 1.</param>
        /// <param name="b">point 2.</param>
        /// <returns>the distance.</returns>
        private double Distance(Point a, Point b)
        {
            return Math.Pow(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2), .5);
        }

        /// <summary>
        /// This determines the midpoint between two values.
        /// </summary>
        /// <param name="x1">coordinate one.</param>
        /// <param name="x2">different coordinate.</param>
        /// <returns>difference</returns>
        private float MidPoint(float x1, float x2)
        {
            return (x1 + x2) / 2;
        }

        /// <summary>
        /// THis is an event for when the mouse moves over the picture box.
        /// </summary>
        /// <param name="sender">the picture box.</param>
        /// <param name="e">the event sent.</param>
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.drawing)
            {
                this.Canvas.Invalidate();
            }
            else if (this.dragging)
            {
                this.activeVertex.Point = new Point(e.Location.X + this.dragOffset.X, e.Location.Y + this.dragOffset.Y);
                this.Canvas.Invalidate();
            }
        }

        /// <summary>
        /// When the mouse click is released.
        /// </summary>
        /// <param name="sender">the sender of the event.</param>
        /// <param name="e">the event that is sent.</param>
        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if (this.activeVertex == null)
            {
                return;
            }

            if (this.dragging)
            {
                this.activeVertex.Point = new Point(e.Location.X + this.dragOffset.X, e.Location.Y + this.dragOffset.Y);
            }
            else if (this.drawing)
            {
                this.graph.AddVertex(this.activeVertex);
            }

            this.dragging = false;
            this.drawing = false;
            this.Canvas.Invalidate();
        }

        /// <summary>
        /// Draws regular edges.
        /// </summary>
        /// <param name="parallelEdgeCount">which edges are parallel.</param>
        /// <param name="g">the graphics used for the picture box.</param>
        /// <param name="edgeStatus">the status of the edge being drawn.</param>
        /// <param name="edges">the list of edges to draw.</param>
        private void DrawRegularEdges(List<(Edge, int)> parallelEdgeCount, Graphics g, string edgeStatus, List<Edge> edges)
        {
            if (edges == null)
            {
                Debug.WriteLine("Can't Draw No Edges");
                return; // can't draw no edges
            }

            if (edgeStatus == "notactive")
            {
                foreach (var vertex in this.graph.MyVertices.Where(c => c != this.activeVertex))
                {
                    foreach (var edge in edges.Where(c => c.M == vertex || c.N == vertex))
                    {
                        if (parallelEdgeCount.FindAll(c => (c.Item1.N == edge.N && c.Item1.M == edge.M) || (c.Item1.M == edge.N && c.Item1.N == edge.M)).Count() == 0)
                        {
                            // didn't find parallel edges, can draw
                            Pen p = new Pen(Color.White);
                            p.Width = 2;
                            AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
                            if (edge.UniqueEdgeName == "Directed Edge")
                            {
                                p.CustomStartCap = bigArrow;
                                this.DrawSpecialEdges(g, edge, p, 0);
                            }
                            else
                            {
                                g.DrawLine(p, edge.M.Point.X, edge.M.Point.Y, edge.N.Point.X, edge.N.Point.Y);
                            }
                        }
                    }
                }
            }
            else if (edgeStatus == "prim")
            {
                foreach (var vertex in this.graph.MyVertices)
                {
                    foreach (var edge in edges.Where(c => c.M == vertex || c.N == vertex))
                    {
                        Debug.WriteLine("Prim"); // our version not for directed.
                        Pen p = new Pen(Color.Purple);
                        p.Width = 5;
                        g.DrawLine(p, edge.M.Point.X, edge.M.Point.Y, edge.N.Point.X, edge.N.Point.Y);
                    }
                }
            }
            else if (edgeStatus == "links")
            {
                Debug.WriteLine("Printing All Links");
                foreach (var vertex in this.graph.MyVertices)
                {
                    foreach (var edge in edges.Where(c => c.M == vertex || c.N == vertex))
                    {
                        if (parallelEdgeCount.FindAll(c => c.Item1.N == edge.N && c.Item1.M == edge.M).Count() == 0)
                        {
                            Pen pActive = new Pen(Color.Orange);
                            pActive.Width = 2;
                            AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
                            if (edge.UniqueEdgeName == "Directed Edge")
                            {
                                pActive.CustomStartCap = bigArrow;
                                this.DrawSpecialEdges(g, edge, pActive, 0);
                            }
                            else
                            {
                                g.DrawLine(pActive, edge.M.Point.X, edge.M.Point.Y, edge.N.Point.X, edge.N.Point.Y);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var edge in this.graph.MyEdges.Where(c => c.M == this.activeVertex || c.N == this.activeVertex))
                {
                    // we will draw parallel edges separately.
                    if (parallelEdgeCount.FindAll(c => (c.Item1.N == edge.N && c.Item1.M == edge.M) || (c.Item1.N == edge.M && c.Item1.M == edge.N)).Count() == 0)
                    {
                        // fixed it. wasn't counting active vertice edges for parallel edges originally. 
                        Pen pActive = new Pen(Color.Cyan);
                        pActive.Width = 2;
                        AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
                        if (edge.UniqueEdgeName == "Directed Edge")
                        {
                            pActive.CustomStartCap = bigArrow;
                            this.DrawSpecialEdges(g, edge, pActive, 0);
                        }
                        else
                        {
                            g.DrawLine(pActive, edge.M.Point.X, edge.M.Point.Y, edge.N.Point.X, edge.N.Point.Y);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draws parallel edges.
        /// </summary>
        /// <param name="parallelEdgeCount">tuples of edges and their count.</param>
        /// <param name="g">graphics</param>
        /// <param name="edgeStatus">the status of the edges.</param>
        private void DrawParallelEdges(List<(Edge, int)> parallelEdgeCount, Graphics g, string edgeStatus)
        {
            // Create font and brush.
            Font drawFont = new Font("Times New Roman", 20, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.White);

            // Set format of string.
            StringFormat drawFormat = new StringFormat();
            if (edgeStatus == "notactive")
            {
                foreach (var tuple in parallelEdgeCount.Where(c => c.Item1.M != this.activeVertex && c.Item1.N != this.activeVertex))
                {
                    // each iteration is the parallel edges between only two nodes.
                    Edge pEdge = tuple.Item1;

                    // grab all the edges like this. Then we will iterate through a stack of them.
                    List<Edge> parEdges = new List<Edge>(this.graph.MyEdges);
                    parEdges = parEdges.Where(c => (c.N == pEdge.N && c.M == pEdge.M) || (c.N == pEdge.M && c.M == pEdge.N)).ToList();

                    Pen p = new Pen(Color.White);

                    // didn't find parallel edges, can draw
                    p.Width = 2;
                    AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
                    for (int i = 0; i < tuple.Item2 / 2 && i < 4; i++)
                    {
                        pEdge = parEdges.ElementAt(0);
                        parEdges.RemoveAt(0);
                        int t = 0;

                        (p, t) = ChooseArrows(pEdge, p, bigArrow);
                        if (t == 1)
                        {
                            g.DrawLine(p, pEdge.M.Point.X + 4*(i + 1), pEdge.M.Point.Y + 4*(i + 1), pEdge.N.Point.X + 4*(i + 1), pEdge.N.Point.Y + 4*(i + 1));
                        }
                        else
                        {
                            this.DrawSpecialEdges(g, pEdge, p, (i + 1) * 4);
                        }

                        // g.DrawLine(p, pEdge.M.Point.X - 4 * (i + 1), pEdge.M.Point.Y - 4 * (i + 1), pEdge.N.Point.X - 4 * (i + 1), pEdge.N.Point.Y - 4 * (i + 1));
                        pEdge = parEdges.ElementAt(0);
                        parEdges.RemoveAt(0);
                        (p, t) = ChooseArrows(pEdge, p, bigArrow);
                        if (t == 1)
                        {
                            g.DrawLine(p, pEdge.M.Point.X - 4*(i + 1), pEdge.M.Point.Y - 4*(i + 1), pEdge.N.Point.X - 4*(i + 1), pEdge.N.Point.Y - 4*(i + 1));
                        }
                        else
                        {
                            this.DrawSpecialEdges(g, pEdge, p, (i + 1) * -4);
                        }
                    }

                    if (tuple.Item2 % 2 == 1)
                    {
                        pEdge = parEdges.ElementAt(0);
                        parEdges.RemoveAt(0);
                        int t = 0;
                        (p, t) = ChooseArrows(pEdge, p, bigArrow);
                        // odd so there is an edge with no offset
                        this.DrawSpecialEdges(g, pEdge, p, 0);
                    }
                }
            }
            else if (edgeStatus == "links")
            {
                foreach (var tuple in parallelEdgeCount.Where(c => c.Item1.M != this.activeVertex && c.Item1.N != this.activeVertex))
                {
                    Edge pEdge = tuple.Item1;

                    // grab all the edges like this. Then we will iterate through a stack of them.
                    List<Edge> parEdges = new List<Edge>(this.graph.MyEdges);
                    parEdges = parEdges.Where(c => (c.N == pEdge.N && c.M == pEdge.M) || (c.N == pEdge.M && c.M == pEdge.N)).ToList();

                    Pen p = new Pen(Color.Orange);

                    // didn't find parallel edges, can draw
                    p.Width = 2;
                    AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
                    for (int i = 0; i < tuple.Item2 / 2 && i < 4; i++)
                    {
                        int t = 0;
                        pEdge = parEdges.ElementAt(0);
                        parEdges.RemoveAt(0);
                        (p, t) = ChooseArrows(pEdge, p, bigArrow);
                        if (t == 1)
                        {
                            g.DrawLine(p, pEdge.M.Point.X - 4 * (i + 1), pEdge.M.Point.Y - 4 * (i + 1), pEdge.N.Point.X - 4 * (i + 1), pEdge.N.Point.Y - 4 * (i + 1));
                        }
                        else
                        {
                            this.DrawSpecialEdges(g, pEdge, p, (i + 1) * -4);
                        }

                        // g.DrawLine(p, pEdge.M.Point.X - 4 * (i + 1), pEdge.M.Point.Y - 4 * (i + 1), pEdge.N.Point.X - 4 * (i + 1), pEdge.N.Point.Y - 4 * (i + 1));
                        pEdge = parEdges.ElementAt(0);
                        parEdges.RemoveAt(0);
                        (p, t) = ChooseArrows(pEdge, p, bigArrow);
                        if (t == 1)
                        {
                            g.DrawLine(p, pEdge.M.Point.X - 4 * (i + 1), pEdge.M.Point.Y - 4 * (i + 1), pEdge.N.Point.X - 4 * (i + 1), pEdge.N.Point.Y - 4 * (i + 1));
                        }
                        else
                        {
                            this.DrawSpecialEdges(g, pEdge, p, (i + 1) * 4);
                        }
                    }

                    if (tuple.Item2 % 2 == 1)
                    {
                        int t = 0;
                        pEdge = parEdges.ElementAt(0);
                        parEdges.RemoveAt(0);
                        (p, t) = ChooseArrows(pEdge, p, bigArrow);
                        if (t == 1)
                        {
                            g.DrawLine(p, pEdge.M.Point.X, pEdge.M.Point.Y, pEdge.N.Point.X, pEdge.N.Point.Y);
                        }
                        else
                        {
                            // odd so there is an edge with no offset
                            this.DrawSpecialEdges(g, pEdge, p, 0);
                        }
                    }
                }
            }
            else
            {
                foreach (var tuple in parallelEdgeCount.Where(c => c.Item1.M == this.activeVertex || c.Item1.N == this.activeVertex))
                {
                    Edge pEdge = tuple.Item1;

                    // grab all the edges like this. Then we will iterate through a stack of them.
                    List<Edge> parEdges = new List<Edge>(this.graph.MyEdges);
                    parEdges = parEdges.Where(c => (c.N == pEdge.N && c.M == pEdge.M) || (c.N == pEdge.M && c.M == pEdge.N)).ToList();

                    Pen p = new Pen(Color.Cyan);

                    // didn't find parallel edges, can draw
                    p.Width = 2;
                    AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
                    for (int i = 0; i < tuple.Item2 / 2 && i < 4; i++)
                    {
                        int t = 0;
                        pEdge = parEdges.ElementAt(0);
                        parEdges.RemoveAt(0);
                        (p, t) = ChooseArrows(pEdge, p, bigArrow);
                        if (t == 1)
                        {
                            g.DrawLine(p, pEdge.M.Point.X - 4 * (i + 1), pEdge.M.Point.Y - 4 * (i + 1), pEdge.N.Point.X - 4 * (i + 1), pEdge.N.Point.Y - 4 * (i + 1));
                        }
                        else
                        {
                            this.DrawSpecialEdges(g, pEdge, p, (i + 1) * -4);
                        }

                        // g.DrawLine(p, pEdge.M.Point.X - 4 * (i + 1), pEdge.M.Point.Y - 4 * (i + 1), pEdge.N.Point.X - 4 * (i + 1), pEdge.N.Point.Y - 4 * (i + 1));
                        pEdge = parEdges.ElementAt(0);
                        parEdges.RemoveAt(0);
                        (p, t) = ChooseArrows(pEdge, p, bigArrow);
                        if (t == 1)
                        {
                            g.DrawLine(p, pEdge.M.Point.X + 4 * (i + 1), pEdge.M.Point.Y + 4 * (i + 1), pEdge.N.Point.X + 4 * (i + 1), pEdge.N.Point.Y + 4 * (i + 1));
                        }
                        else
                        {
                            this.DrawSpecialEdges(g, pEdge, p, (i + 1) * 4);
                        }
                    }

                    if (tuple.Item2 % 2 == 1)
                    {
                        int t = 0;
                        pEdge = parEdges.ElementAt(0);
                        parEdges.RemoveAt(0);
                        (p, t) = ChooseArrows(pEdge, p, bigArrow);

                        // odd so there is an edge with no offset
                        this.DrawSpecialEdges(g, pEdge, p, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the appropriate arrow to the edge pen.
        /// </summary>
        /// <param name="pEdge">edge.</param>
        /// <param name="p">pen.</param>
        /// <param name="bigArrow">arrow.</param>
        /// <returns>new pen.</returns>
        private static (Pen, int) ChooseArrows(Edge pEdge, Pen p, AdjustableArrowCap bigArrow)
        {
            if (pEdge.UniqueEdgeName == "Directed Edge")
            {
                p.CustomStartCap = bigArrow;
                return (p, 0);
            }
            else
            {
                p = new Pen(p.Color);

                // didn't find parallel edges, can draw
                p.Width = 2;
                return (p, 1);
            }
        }

        private void DrawSpecialEdges(Graphics g, Edge pEdge, Pen midPen, int i)
        {
            float x1 = pEdge.M.Point.X + 2 * i / 3;
            float x2 = pEdge.N.Point.X + 2 * i / 3;
            float y1 = pEdge.M.Point.Y + 2 * i / 3;
            float y2 = pEdge.N.Point.Y + 2 * i / 3;
            float l;
            if (i == 0)
            {
                l = (float)this.Distance(pEdge.M.Point, pEdge.N.Point) - Math.Abs(4 * i);
            }
            else
            {
                l = (float)this.Distance(pEdge.M.Point, pEdge.N.Point);
            }
            Pen p = new Pen(midPen.Color);
            p.Width = midPen.Width;
            g.DrawLine(midPen, this.MidPoint(pEdge.N.Point.X, pEdge.M.Point.X) + i, this.MidPoint(pEdge.N.Point.Y, pEdge.M.Point.Y) + i, pEdge.N.Point.X + i, pEdge.N.Point.Y + i); // just a regular edge
            g.DrawLine(p, pEdge.M.Point.X + i, pEdge.M.Point.Y + i, pEdge.N.Point.X + i, pEdge.N.Point.Y + i); // just a regular edge
            // no longer needed // g.DrawLine(p, x1 - ((float)(22.5 / l) * (x1 - x2)), y1 - ((float)(22.5 / l) * (y1 - y2)), x2 + ((float)(22.5 / l) * (x1 - x2)), y2 + ((float)(22.5 / l) * (y1 - y2)));
        }

        /// <summary>
        /// Draws the loops.
        /// </summary>
        /// <param name="g">the graphics.</param>
        /// <param name="edgeStatus">the status of the edges.</param>
        private void DrawLoops(Graphics g, string edgeStatus)
        {
            // Create font and brush.
            Font drawFont = new Font("Times New Roman", 12);
            SolidBrush drawBrush = new SolidBrush(Color.White);

            // Set format of string.
            StringFormat drawFormat = new StringFormat();
            if (edgeStatus == "links")
            {
                Debug.WriteLine("Linked Loop");
                foreach (var edge in this.graph.MyEdges.Where(c => c.M == c.N && c.N != this.activeVertex))
                {
                    // fixed it. wasn't counting active vertice edges for parallel edges originally.
                    Pen pActive = new Pen(Color.Orange);
                    Pen pLoop = new Pen(Color.Orange);
                    pLoop.Width = 2;
                    pActive.Width = 2;
                    Debug.WriteLine("Linked Loop");
                    AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
                    if (edge.UniqueEdgeName == "Directed Edge")
                    {
                        pLoop.CustomStartCap = bigArrow;
                        g.DrawLine(pLoop, edge.M.Point.X - 27, edge.M.Point.Y - 40, edge.N.Point.X - 22, edge.N.Point.Y - 35);
                    }

                    g.DrawEllipse(pActive, (float)(edge.N.Point.X - edge.N.Radius()),
                    (float)(edge.N.Point.Y - 22.5 - edge.N.Radius()), (float)(edge.N.Radius() * 2),
                    (float)(edge.N.Radius() * 2));
                }
            }
            else if (edgeStatus != "active")
            {
                foreach (var edge in this.graph.MyEdges.Where(c => c.M == c.N && c.N != this.activeVertex))
                {
                    // fixed it. wasn't counting active vertice edges for parallel edges originally.
                    Pen pActive = new Pen(Color.White);
                    pActive.Width = 2;
                    Pen pLoop = new Pen(Color.White);
                    pLoop.Width = 2;
                    AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
                    if (edge.UniqueEdgeName == "Directed Edge")
                    {
                        pLoop.CustomStartCap = bigArrow;
                        g.DrawLine(pLoop, edge.M.Point.X - 27, edge.M.Point.Y - 40, edge.N.Point.X - 22, edge.N.Point.Y - 35);
                    }

                    g.DrawEllipse(pActive, (float)(edge.N.Point.X - edge.N.Radius()),
                    (float)(edge.N.Point.Y - 22.5 - edge.N.Radius()), (float)(edge.N.Radius() * 2),
                    (float)(edge.N.Radius() * 2));
                }
            }
            else
            {
                foreach (var edge in this.graph.MyEdges.Where(c => c.M == this.activeVertex && c.N == this.activeVertex))
                {
                    // fixed it. wasn't counting active vertice edges for parallel edges originally.
                    Pen pActive = new Pen(Color.Cyan);
                    pActive.Width = 2;
                    Pen pLoop = new Pen(Color.Cyan);
                    pLoop.Width = 2;
                    AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
                    if (edge.UniqueEdgeName == "Directed Edge")
                    {
                        pLoop.CustomStartCap = bigArrow;
                        g.DrawLine(pLoop, edge.M.Point.X - 27, edge.M.Point.Y - 40, edge.N.Point.X - 22, edge.N.Point.Y - 35);
                    }

                    g.DrawEllipse(pActive, (float)(this.activeVertex.Point.X - this.activeVertex.Radius()),
                    (float)(this.activeVertex.Point.Y - 22.5 - this.activeVertex.Radius()), (float)(this.activeVertex.Radius() * 2),
                    (float)(this.activeVertex.Radius() * 2));
                }
            }
        }

        /// <summary>
        /// Draws all of the vertices.
        /// </summary>
        /// <param name="g">the graphics to draw.</param>
        /// <param name="edgeStatus">the edge status.</param>
        private void DrawVertices(Graphics g, string edgeStatus)
        {
            // Create font and brush.
            Font drawFont = new Font("Times New Roman", 12, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Set format of string.
            StringFormat drawFormat = new StringFormat();
            if (edgeStatus != "active")
            {
                foreach (var vertex in this.graph.MyVertices.Where(c => c != this.activeVertex))
                {
                    g.FillEllipse(vertex.Brush, (float)(vertex.Point.X - vertex.Radius()), (float)(vertex.Point.Y - vertex.Radius()),
                        (float)(vertex.Radius() * 2),
                        (float)(vertex.Radius() * 2));
                    String drawString = vertex.Number.ToString();

                    // Draw string to screen.
                    drawBrush.Color = Color.Black;
                    g.DrawString(drawString, drawFont, drawBrush, vertex.Point.X - 7, vertex.Point.Y - 7, drawFormat);

                    if (this.displayDegrees == true)
                    {
                        drawString = "Degree " + vertex.Degree.ToString();
                        drawBrush.Color = Color.White;
                        g.DrawString(drawString, drawFont, drawBrush, vertex.Point.X + 20, vertex.Point.Y - 7, drawFormat);
                    }
                }
            }
            else
            {
                if (this.activeVertex != null && this.graph.MyVertices.Contains(this.activeVertex))
                {
                    g.FillEllipse(new SolidBrush(Color.Cyan), (float)(this.activeVertex.Point.X - this.activeVertex.Radius() - 3),
                    (float)(this.activeVertex.Point.Y - this.activeVertex.Radius() - 3), (float)(this.activeVertex.Radius() * 2 + 6),
                    (float)(this.activeVertex.Radius() * 2 + 6));
                    g.FillEllipse(this.activeVertex.Brush, (float)(this.activeVertex.Point.X - this.activeVertex.Radius()),
                        (float)(this.activeVertex.Point.Y - this.activeVertex.Radius()), (float)(this.activeVertex.Radius() * 2),
                        (float)(this.activeVertex.Radius() * 2));

                    String drawString = this.activeVertex.Number.ToString();

                    // Draw string to screen.
                    g.DrawString(drawString, drawFont, drawBrush, this.activeVertex.Point.X - 7, this.activeVertex.Point.Y - 7, drawFormat);
                    if (this.displayDegrees == true)
                    {
                        drawString = "Degree " + this.activeVertex.Degree.ToString();
                        drawBrush.Color = Color.White;
                        g.DrawString(drawString, drawFont, drawBrush, this.activeVertex.Point.X + 20, this.activeVertex.Point.Y - 7, drawFormat);
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when the mouse is double clicked.
        /// </summary>
        /// <param name="sender">the sender.</param>
        /// <param name="e">The event.</param>
        private void Canvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MyVertex tempVertex = this.CheckIfVertexClicked(e.Location);
            Edge activeEdge = this.CheckIfEdgeClicked(e.Location);
            Edge activeLoop = this.CheckIfLoopClicked(e.Location);
            this.graph.RenumberVertices();
            if (e.Button != MouseButtons.Right && tempVertex != null)
            {
                // this must check if vertices are null because an edge must be between two nodes
                if (this.graph.MyVertices.Contains(tempVertex))
                {
                    if (this.algorithming == true)
                    {
                        this.RunButton.Text = "Run";
                        this.algorithming = false;
                    }

                    this.graph.RemoveVertex(tempVertex); // need to delete all edges touching the vertex too. We would do it in this part.
                    // Lets setup a function to remove edges and call it here.
                    this.graph.DeleteExpiredEdges(tempVertex);
                    this.graph.RenumberVertices();
                    this.drawing = true;
                }
            }// we want edges second, since the center of the circle is the origin point of the edge.
            else if (e.Button != MouseButtons.Left && (activeEdge != null || activeLoop != null)) // can't delete edge and vertex at same time if close
            {
                if (this.graph.MyEdges.Contains(activeEdge))
                {
                    this.graph.RemoveEdge(activeEdge); // need to delete all edges touching the vertex too. We would do it in this part.

                    // Lets setup a function to remove edges and call it here.
                    this.drawing = true;
                }
                else if (this.graph.MyEdges.Contains(activeLoop))
                {
                    this.graph.RemoveEdge(activeLoop); // need to delete all edges touching the vertex too. We would do it in this part.

                    this.drawing = true;

                    // not working currently, will return to.
                }
            }

            return;
        }

        /// <summary>
        /// Draws a path with vertices.
        /// </summary>
        /// <param name="path">the list of vertices.</param>
        /// <param name="g">the graphics for drawing.</param>
        /// <param name="width">the width of the path.</param>
        private void DrawPath(List<MyVertex> path, Graphics g, float width)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                // if 1 or 0 the shortest path is no shortest path.
                // didn't find parallel edges, can draw
                Pen p = new Pen(Color.Yellow);
                p.Width = width;
                g.DrawLine(p, path.ElementAt(i).Point.X, path.ElementAt(i).Point.Y, path.ElementAt(i + 1).Point.X, path.ElementAt(i + 1).Point.Y);
            }
        }

        // maybe just use typed text instead of combo box since they will constantly loose items and it wont be consistent.

        /// <summary>
        /// Mouse hover event. Not needed currently.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event.</param>
        private void Canvas_MouseHover(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// When the combobox is changed.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event.</param>
        private void Algorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Button click event.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event.</param>
        private void RunButton_Click(object sender, EventArgs e)
        {
            if (this.algorithming == true)
            {
                this.RunButton.Text = "Run";
                this.algorithming = false;
                this.textBox4.SendToBack();
                this.textBox4.Clear();
            }
            else
            {
                this.RunButton.Text = "Stop";
                this.algorithming = true;
            }

            int source = 0;
            if (this.algorithming == true && this.activeVertex != null && this.Algorithm.SelectedItem != null && this.Algorithm.SelectedItem.ToString() == "Identify")
            {
                this.RunButton.Text = "Run";
                this.algorithming = false;
                this.graph.Identify(this.graph.StrToVertices(this.Identify.Text));
            }
            else if (this.algorithming == true && this.activeVertex != null && this.Algorithm.SelectedItem != null && this.Algorithm.SelectedItem.ToString() == "Collapse")
                {
                    this.RunButton.Text = "Run";
                    this.algorithming = false;
                    this.graph.Collapse(this.graph.StrToVertices(this.Identify.Text));
                }
            else if (int.TryParse(this.SourceBox.Text, out source) && this.algorithming == true && this.Algorithm.SelectedItem != null && this.Algorithm.SelectedItem.ToString() == "Bipartite")
            {
                this.textBox4.BringToFront();
                if (this.graph.IsBipartite(source))
                {
                    this.textBox4.Text = "The graph connected to vertex " + source.ToString() + " is bipartite.";
                }
                else
                {
                    this.textBox4.Text = "The graph connected to vertex " + source.ToString() + " is not bipartite.";
                }
            }
            else if (int.TryParse(this.SourceBox.Text, out source) && this.algorithming == true && this.Algorithm.SelectedItem != null && this.Algorithm.SelectedItem.ToString() == "InTree")
            {
                this.textBox4.BringToFront();
                if (source < this.graph.MyVertices.Count && source >= 0)
                {
                    // we know it is a valid label.
                    MyVertex treeSource = this.graph.MyVertices.Find(v => v.Number == source);
                    List<MyVertex> treeVertices = this.graph.IsInTree(treeSource);
                    if (treeVertices != null && treeVertices.Count >= 1)
                    {
                        // it is a tree. Trivial tree (1 vertex) is still a tree.
                        this.textBox4.Text = "The graph connected to vertex " + source.ToString() + " is a tree.";
                    }
                    else
                    {
                        this.textBox4.Text = "The graph connected to vertex " + source.ToString() + " is not a tree.";
                    }
                }
            }
            else if (this.algorithming == true && this.Algorithm.SelectedItem != null && this.Algorithm.SelectedItem.ToString() == "Adjacency Matrix")
            {
                this.textBox4.BringToFront();
                this.textBox4.Text = "Adjacency Matrix";

                // this.graph.ConstructTwoWayAdjacencyMatrix();
                this.textBox4.Text = this.graph.PrintAdjacencyMatrix(this.graph.DisplayAjacencyMatrix());
            }

            this.drawing = true;
        }

        /// <summary>
        /// When the text is changed.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event.</param>
        private void TargetBox_TextChanged(object sender, EventArgs e)
        {
            this.drawing = true;
        }

        /// <summary>
        /// When the text is changed.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event.</param>
        private void SourceBox_TextChanged(object sender, EventArgs e)
        {
            this.drawing = true;
        }

        /// <summary>
        /// When the text is changed.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event.</param>
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Draws all the bridge.
        /// </summary>
        /// <param name="bridges">bridges.</param>
        /// <param name="g">graphics.</param>
        private void DrawBridgeEdges(List<Edge> bridges, Graphics g)
        {
            foreach (var edge in bridges)
            {
                // didn't find parallel edges, can draw
                Pen pActive = new Pen(Color.Red);
                pActive.Width = 2;
                AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
                if (edge.UniqueEdgeName == "Directed Edge")
                {
                    pActive.CustomStartCap = bigArrow;
                    this.DrawSpecialEdges(g, edge, pActive, 0);
                }
                else
                {
                    g.DrawLine(pActive, edge.M.Point.X, edge.M.Point.Y, edge.N.Point.X, edge.N.Point.Y);
                }
            }
        }

        /// <summary>
        /// Initializes the combobox.
        /// </summary>
        private void InitializeComboBox()
        {
            this.colorDictionary["Red"] = Color.Red;
            this.colorDictionary["Blue"] = Color.Blue;
            this.colorDictionary["White"] = Color.White;
            this.colorDictionary["Green"] = Color.Green;
            this.colorDictionary["Yellow"] = Color.Yellow;
            this.colorDictionary["Orange"] = Color.Orange;
            this.colorDictionary["Purple"] = Color.Purple;
            this.colorDictionary["Pink"] = Color.Pink;
            this.colorDictionary["Coral"] = Color.Coral;
            this.colorDictionary["Crimson"] = Color.Crimson;
            this.colorDictionary["DogerBlue"] = Color.DodgerBlue;
            this.colorDictionary["Gold"] = Color.Gold;
            this.colorDictionary["Mint"] = Color.MintCream;
            this.colorDictionary["SteelBlue"] = Color.LightSteelBlue;
            this.ColorComboBox.Items.Add("Don't Color");
            this.ColorComboBox.Items.Add("Gold");
            this.ColorComboBox.Items.Add("Mint");
            this.ColorComboBox.Items.Add("Coral");
            this.ColorComboBox.Items.Add("Crimson");
            this.ColorComboBox.Items.Add("Red");
            this.ColorComboBox.Items.Add("Blue");
            this.ColorComboBox.Items.Add("White");
            this.ColorComboBox.Items.Add("Green");
            this.ColorComboBox.Items.Add("Yellow");
            this.ColorComboBox.Items.Add("Orange");
            this.ColorComboBox.Items.Add("Purple");
            this.ColorComboBox.Items.Add("Pink");
            this.ColorComboBox.Items.Add("SteelBlue");
            this.ColorComboBox.Items.Add("DogerBlue");
            this.ColorComboBox.Text = "Vertex Colors";
        }

        /// <summary>
        /// changes the edge type depending on combo box xhange event.
        /// </summary>
        /// <param name="sender">the sender of the event.</param>
        /// <param name="e">the event.</param>
        private void EdgeSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.EdgeSettings.SelectedItem.ToString() == "Regular Edge")
            {
                this.currentEdgeType = "Regular Edge";
            }
            else if (this.EdgeSettings.SelectedItem.ToString() == "Directed Edge")
            {
                this.currentEdgeType = "Directed Edge";
            }
            else if (this.EdgeSettings.SelectedItem.ToString() == "Degrees")
            {
                this.displayDegrees = true;
                this.drawing = true;
            }

            if (this.EdgeSettings.SelectedItem.ToString() != "Degrees")
            {
                this.displayDegrees = false;
                this.drawing = true;
            }

        }
    }
}

// current problem, if you try to drag, it automatically adds a new edge between current node and the one you want to add
// current problem if you want to delete a vertex double clicking changes active vertex. And delte doesn't occur until click away.
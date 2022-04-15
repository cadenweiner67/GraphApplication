namespace GraphEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class of all graphs.
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// list of vertices in the graph.
        /// </summary>
        private List<MyVertex> myVertices = new List<MyVertex>();

        /// <summary>
        /// adjacency matrix.
        /// </summary>
        private int[,] twoWayAdjacencyMatrix;

        /// <summary>
        /// This is a list of edges in the graph.
        /// </summary>
        private List<Edge> myEdges = new List<Edge>();

        /// <summary>
        /// this is the current number of vertices.
        /// </summary>
        private int currentVertexCount;

        /// <summary>
        /// this is a list of all parallel edges in the graph.
        /// </summary>
        private List<Edge> myParallelEdges = new List<Edge>();

        /// <summary>
        /// this is a bool about whether or not an item was found in a DFS.
        /// </summary>
        private bool found;

        /// <summary>
        /// This is the shortest path we can print
        /// </summary>
        private List<MyVertex> shortestPathToPrint;

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// Graph constructor.
        /// </summary>
        public Graph()
        {
        }

        /// <summary>
        /// Gets or Sets the list of vertices.
        /// </summary>
        public List<MyVertex> MyVertices
        {
            get { return this.myVertices; }
            set { this.myVertices = value; }
        }

        /// <summary>
        /// Gets or Sets the list of edges.
        /// </summary>
        public List<Edge> MyEdges
        {
            get { return this.myEdges; }
            set { this.myEdges = value; }
        }

        /// <summary>
        /// Gets or Sets the current vertex count.
        /// </summary>
        public int CurrentVertexCount
        {
            get { return this.currentVertexCount; }
            set { this.currentVertexCount = value; }
        }

        /// <summary>
        /// Gets or Sets the parallel edges.
        /// </summary>
        public List<Edge> MyParallelEdges
        {
            get { return this.myParallelEdges; }
            set { this.myParallelEdges = value; }
        }

        /// <summary>
        /// Gets or Sets the adjacency matrix.
        /// </summary>
        public int[,] AdjacencyMatrix
        {
            get { return this.twoWayAdjacencyMatrix; }
            set { this.twoWayAdjacencyMatrix = value; }
        }

        /// <summary>
        /// Gets or Sets the shortest path.
        /// </summary>
        public List<MyVertex> ShortestPathToPrint
        {
            get { return this.shortestPathToPrint; }
            set { this.shortestPathToPrint = value; }
        }

        /// <summary>
        /// This gets the number of components and returns it as a string.
        /// </summary>
        /// <returns>string.</returns>
        public string NumberOfComponents()
        {
            List<MyVertex> verticesRemaining = new List<MyVertex>(this.MyVertices);
            int i = 0;
            if (this.myVertices.Count == 0)
            {
                return i.ToString();
            }

            while (verticesRemaining.Count > 0)
            {
                // Debug.WriteLine("Vertices before BFS " + verticesRemaining.Count.ToString() + " items.");
                verticesRemaining = this.BreadthFirstSearch(verticesRemaining, verticesRemaining.ElementAt(0));

                // Debug.WriteLine("Vertices after BFS " + verticesRemaining.Count.ToString() + " items.");
                i++;
            }

            return i.ToString();
        }

        /// <summary>
        /// This is a breadth first function specifically made to find all the nodes that are reachable
        /// from the source and then returning a list of the remaining vertices that can't be reached from the source. 
        /// serves as a helper function to Number of components.
        /// </summary>
        /// <param name="vertices">vertices.</param>
        /// <param name="source">source.</param>
        /// <returns>list of vertices.</returns>
        public List<MyVertex> BreadthFirstSearch(List<MyVertex> vertices, MyVertex source)
        {
            bool[] visited = new bool[vertices.Count+1];
            for (int i = 0; i < vertices.Count; i++)
            {
                visited[i] = false;
            }

            List<MyVertex> toRemove = new List<MyVertex>();

            Queue<MyVertex> q = new Queue<MyVertex>();
            q.Enqueue(source);
            visited[vertices.IndexOf(source)] = true;
            MyVertex v;
            toRemove.Add(source);

            // Debug.WriteLine("Breath First # Vert: " + vertices.Count.ToString());
            while (q.Count > 0)
            {
                v = q.Dequeue();

                // Debug.WriteLine("This item was just removed : Vertex " + v.Number.ToString());
                foreach (var n in this.myEdges.Where(c => c.N.Number == v.Number && c.M.Number != v.Number))
                {
                   // Debug.WriteLine("Looking for neighbors " + n.M.Number.ToString());
                    if (vertices.IndexOf(n.M) >= 0 && visited[vertices.IndexOf(n.M)] == false)
                    {
                        visited[vertices.IndexOf(n.M)] = true;
                        q.Enqueue(n.M);
                        toRemove.Add(n.M);
                    }
                }

                foreach (var n in this.myEdges.Where(c => c.N.Number != v.Number && c.M.Number == v.Number))
                {
                    // Debug.WriteLine("Looking for neighbors" + n.N.Number.ToString());
                    if (vertices.IndexOf(n.N) >= 0 && visited[vertices.IndexOf(n.N)] == false)
                    {
                        // Debug.WriteLine("Enqueue");
                        visited[vertices.IndexOf(n.N)] = true;
                        q.Enqueue(n.N);
                        toRemove.Add(n.N);
                    }
                }
            }

            // Debug.WriteLine("Removing " + vertices.Count.ToString() + "-" + toRemove.Count.ToString() + " items.");
            foreach (var rV in toRemove)
            {
                // Debug.WriteLine("Removing Vertex" + rV.Number.ToString());
                vertices.RemoveAll(c => c.Number == rV.Number);
            }

            // Debug.WriteLine(vertices.Count.ToString() + " remaining items.");
            return vertices;
        }

        /// <summary>
        /// This constructs an adjacency matrix.
        /// </summary>
        public void ConstructTwoWayAdjacencyMatrix()
        {
            this.AdjacencyMatrix = new int[this.myVertices.Count, this.myVertices.Count];
            foreach (var edge in this.myEdges)
            {
                // all our edges go both ways. Parallel edges don't matter for dijkstras algorithm.
                if (edge.UniqueEdgeName == "Directed Edge")
                {
                    this.AdjacencyMatrix[edge.N.Number, edge.M.Number] = 1;
                }
                else
                {
                    this.AdjacencyMatrix[edge.N.Number, edge.M.Number] = 1;
                    this.AdjacencyMatrix[edge.M.Number, edge.N.Number] = 1;
                }
            }
        }

        /// <summary>
        /// This goes through and renumbers the vertices properly.
        /// </summary>
        public void RenumberVertices()
        {
            int count = 0;
            foreach (var v in this.MyVertices)
            {
                v.Number = count;
                count += 1;
            }

            this.currentVertexCount = count;
        }

        /// <summary>
        /// This prints the adjacency matrix.
        /// </summary>
        /// <param name="adjacencyMatrix">an adjacency matrix array.</param>
        public string PrintAdjacencyMatrix(int[,] adjacencyMatrix)
        {
            string s = string.Empty;
            foreach (var y in this.myVertices)
            {
                foreach (var x in this.myVertices)
                {
                    s += adjacencyMatrix[x.Number, y.Number].ToString() + " ";
                }

                s += "\r\n";
            }

            Debug.WriteLine(s);
            return s;
        }

        /// <summary>
        /// determines the shortest path using dijkstras algorithm and returns the vertices along the path in order.
        /// </summary>
        /// <param name="source">the source vertex.</param>
        /// <param name="target">the target vertex.</param>
        /// <returns>List of vertices.</returns>
        public List<MyVertex> ShortestPath(int source, int target)
        {
            List<MyVertex> path = new List<MyVertex>();
            this.ConstructTwoWayAdjacencyMatrix();
            this.PrintAdjacencyMatrix(this.AdjacencyMatrix);
            this.shortestPathToPrint = new List<MyVertex>();

            // this checks for situations where the user input is invalid
            if (source >= this.currentVertexCount || target >= this.currentVertexCount)
            {
                return this.shortestPathToPrint;
            }

            // this creates a list of vertices
            bool[] visited = new bool[this.myVertices.Count];
            for (int i = 0; i < this.myVertices.Count; i++)
            {
                visited[i] = false;
            }

            // initializes the distances for all the vertices.
            int[] distance = new int[this.myVertices.Count];
            for (int i = 0; i < this.myVertices.Count; i++)
            {
                distance[i] = int.MaxValue;
                visited[i] = false;
            }

            // this will be used to keep track of the parents along the shortest path.
            distance[source] = 0;
            int[] parents = new int[this.myVertices.Count];
            for (int i = 0; i < parents.Count();i++)
            {
                parents[i] = -2;
            }

            parents[source] = -1; // source is already there so it needs no parent

            // this is the begining of dijkstra's. Goes through each of the vertices.
            for (int i = 0; i < this.myVertices.Count - 1; i++)
            {
                int min = int.MaxValue;
                int minIndex = -1;

                // determine the min distance to a vertex that hasn't already been visited.
                for (int m = 0; m < this.myVertices.Count; m++)
                {
                    if (visited[m] == false && distance[m] <= min)
                    {
                        min = distance[m]; // update the mindistance.
                        minIndex = m;
                    }
                }

                visited[minIndex] = true;

                // goes through all of the vertices and updates based off of their distance from the minindex we are at.
                for (int j = 0; j < this.myVertices.Count; j++)
                {
                    if (!visited[j] && this.AdjacencyMatrix[minIndex, j] != 0 && distance[minIndex] != int.MaxValue && distance[minIndex] + this.AdjacencyMatrix[minIndex, j] < distance[j])
                    {
                        distance[j] = distance[minIndex] + this.AdjacencyMatrix[minIndex, j];
                        parents[j] = minIndex;

                        // Debug.WriteLine("Adding Element To Path " + myVertices.ElementAt(j).ToString());
                    }
                }
            }

            // if the distance from the target is maxvalue so it can't be reached.
            if (distance[target] == int.MaxValue)
            {
                Debug.WriteLine("Source and Target are Disconnected");
            }
            else
            {
                Debug.WriteLine("Shortest Path is Length" + distance[target].ToString());
            }

            // currently touching and adding edges where no path exists
            Debug.WriteLine("Shortest Path is ");
            this.ReturnPath(target, parents);
            path = this.ShortestPathToPrint;
            foreach (var item in path)
            {
                Debug.Write(item.Number.ToString() + "->");
            }

            Debug.WriteLine("Shortest Path Ended"); // path drawing works but creation is failing
            return path;
        }

        /// <summary>
        /// this creates our shortest path of vertices.
        /// </summary>
        /// <param name="currentVertex">the current vertex we are at.</param>
        /// <param name="parentArray">the array that contains the connections between vertices.</param>
        public void ReturnPath(int currentVertex, int [] parentArray)
        {
            if (currentVertex == -1 || currentVertex == -2)
            {
                return;
            }

            this.ReturnPath(parentArray[currentVertex], parentArray);
            this.ShortestPathToPrint.Add(this.MyVertices.ElementAt(currentVertex)); // this is the source
        }

        // complete bipartite = every node in one set reaches every node in the other
        // regular bipartite(What he recommended) - contains no cycles

        /// <summary>
        /// This determines if a graph is bipartite.
        /// </summary>
        /// <param name="source">the source vertex we are determining if is bipartite.</param>
        /// <returns>boolean that indicates bipartite or not.</returns>
        public bool IsBipartite(int source)
        {
            this.ConstructTwoWayAdjacencyMatrix();

            int[] vertexColors = new int[this.myVertices.Count];
            for (int i = 0; i < this.myVertices.Count; i++)
            {
                vertexColors[i] = -1;
            }

            vertexColors[source] = 1;

            Queue<int> queue = new Queue<int>(); // we know that we can access vertices by their id.
            int currentVertex = 0;
            queue.Enqueue(source);
            while (queue.Count > 0)
            {
                currentVertex = queue.Dequeue();
                if (this.AdjacencyMatrix[currentVertex, currentVertex] == 1)
                {
                    // if the adjacency matrix indicates a self loop, there can't be a bipartite graph.
                    return false;
                }

                for (int i = 0; i < this.myVertices.Count; i++)
                {
                    if (this.AdjacencyMatrix[currentVertex, i] == 1 && vertexColors[i] == -1)
                    {
                        // we haven't visited this vertex yet so we can now color it.
                        vertexColors[i] = 1 - vertexColors[currentVertex]; // color it the opposite color of the previous vertex.
                        queue.Enqueue(i); // put the next node in so we can color its neighbors.
                    }
                    else if (this.AdjacencyMatrix[currentVertex, i] == 1 && vertexColors[currentVertex] == vertexColors[i])
                    {
                        return false; // two vertices share the same color and are connected.
                    }
                }
            }

            // can we use adjacency matrix to determine if bipartide?
            // yes
            // make a list of all the vertices of one color, use our IsContained function
            // check that there exists an edge between all vertices of that color and the vertices of the other color. This is for complete bipartite.
            // make sure only two colors in graph.
            // we can use the adjacency matrix to check edges much easier.
            // can just check the coordinates and the edge.
            // Recolor the matrix if true

            for (int i = 0; i < this.myVertices.Count; i++)
            {
                if (vertexColors[i] == 1)
                {
                    this.myVertices.ElementAt(i).Brush.Color = Color.Purple;
                }
                else if (vertexColors[i] == 0)
                {
                    this.myVertices.ElementAt(i).Brush.Color = Color.Gold;
                }

            }

            // we made it through all of our checks so it is bipartite
            return true;
        }

        /// <summary>
        /// This converts a string to a list of vertices.
        /// </summary>
        /// <param name="vertexString">this is a string of vertices.</param>
        /// <returns>list of vertices.</returns>
        public List<MyVertex> StrToVertices(string vertexString)
        {
            string[] vertices = vertexString.Split(',');
            List<MyVertex> identifiedVertices = new List<MyVertex>();
            int vNumber = 0;
            Debug.WriteLine("Identified Vertice: " + vertexString);
            foreach (var v in vertices)
            {
                int.TryParse(v, out vNumber);
                if (vNumber >= this.myVertices.Count || vNumber < 0)
                {
                    return new List<MyVertex>(); // this isn't valid so it shouldn't identify anything
                }

                identifiedVertices.Add(this.myVertices.ElementAt(vNumber));
                Debug.WriteLine("Identified Vertice: " + vNumber.ToString());
            }

            return identifiedVertices;
        }

        /// <summary>
        /// This identifies the vertices.
        /// </summary>
        /// <param name="verticesToIdentify">the list of vertices that will be identified together.</param>
        public void Identify(List<MyVertex> verticesToIdentify)
        {
            if (verticesToIdentify.Count < 2)
            {
                return;
            }

            MyVertex identifiedVertex = new MyVertex(verticesToIdentify.ElementAt(0).Point); // make the new identified element at this point in the list
            List<Edge> unchangeEdges = new List<Edge>(this.MyEdges);
            List<Edge> edgesIdentified = new List<Edge>();
            Edge createdEdge;
            this.AddVertex(identifiedVertex); // need to make sure it is numbered correctly // fixing the numbering fixed the looping issue.
            foreach (var edge in unchangeEdges)
            {
                // if the vertice is one of the ones we want to identify, add the edge
                if (this.IsContained(verticesToIdentify, edge.M) && this.IsContained(verticesToIdentify, edge.N))
                {
                    Debug.WriteLine("Should be a self loop");

                    // these cases all become self loops
                    createdEdge = new Edge(identifiedVertex, identifiedVertex, edge.UniqueEdgeName);
                    if (!this.IsEdgeContained(this.myEdges, createdEdge.M, createdEdge.N))
                    {
                        // if that edge isn't found add the edge
                        this.AddEdge(createdEdge); // something is currently wrong with self loops
                        Debug.WriteLine("Adding a self loop"); // we are never adding the self loop here
                    }
                }
                else if (this.IsContained(verticesToIdentify, edge.M) && !this.IsContained(verticesToIdentify, edge.N))
                {
                    // only one is connected to a vertice we are identifying. Preserve the edge logic
                    createdEdge = new Edge(identifiedVertex, edge.N, edge.UniqueEdgeName);
                    if (!this.IsEdgeContained(this.myEdges, createdEdge.M, createdEdge.N))
                    {
                        // if that edge isn't found add the edge
                        this.AddEdge(createdEdge);
                    }
                }
                else if (!this.IsContained(verticesToIdentify, edge.M) && this.IsContained(verticesToIdentify, edge.N))
                {
                    // only one is connected to a vertice we are identifying. Preserve the edge logic
                    // opposite of the previous just changed to take into account origins of the edges
                    createdEdge = new Edge(edge.M, identifiedVertex, edge.UniqueEdgeName);
                    if (!this.IsEdgeContained(this.myEdges, createdEdge.M, createdEdge.N))
                    {
                        // if that edge isn't found add the edge
                        this.AddEdge(createdEdge);
                    }
                }
            }

            // parallel edges are removed.
            // edgesIdentified = this.ConvergeEdges(edgesIdentified); // this should give us a list of all the edges we need to add. // this is not needed. We don't want to remove all parallel edges, only those that connect to the new identified vertex

            // clear out all the vertices we identified. This shouldn't give renumbering issues until we call renumber
            foreach (var v in verticesToIdentify)
            {
                this.DeleteExpiredEdges(v);
                this.RemoveVertex(v);
                Debug.WriteLine("Identifying Vertex");
            }
        }

        /// <summary>
        /// this determines if a vertice is included in a list of vertices.
        /// </summary>
        /// <param name="list">this is a list of vertices.</param>
        /// <param name="vertex">this is a specific vertex.</param>
        /// <returns>boolean.</returns>
        public bool IsContained(IEnumerable<MyVertex> list, MyVertex vertex)
        {
            foreach (var v in list)
            {
                if (vertex.Number == v.Number)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if an edge is contained in a list of edges.
        /// </summary>
        /// <param name="list">this is a list of edges.</param>
        /// <param name="n">this is one end of the list.</param>
        /// <param name="m">this is the other end of the list.</param>
        /// <returns>boolean.</returns>
        public bool IsEdgeContained(IEnumerable<Edge> list, MyVertex n, MyVertex m) // we need to make sure no parallel edge already exists. Thiswill tell us if n, m has an edge or m,n has an edge
        {
            foreach (var edge in list)
            {
                if ((edge.N.Number == n.Number && edge.M.Number == m.Number) || (edge.N.Number == m.Number && edge.M.Number == n.Number))
                {
                    Debug.WriteLine("Edge " + n.Number.ToString() + " " + m.Number.ToString() + " is already contained");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This is a function to collapse a vertex.
        /// </summary>
        /// <param name="verticesToCollapse">this is the list of vertices to collapse.</param>
        public void Collapse(List<MyVertex> verticesToCollapse)
        {
            // same as identify just not only unique edges allows parallel.
            if (verticesToCollapse.Count < 2)
            {
                return;
            }

            MyVertex collapsededVertex = new MyVertex(verticesToCollapse.ElementAt(0).Point); // make the new identified element at this point in the list
            List<Edge> unchangeEdges = new List<Edge>(this.MyEdges);
            List<Edge> edgesIdentified = new List<Edge>();
            Edge createdEdge;
            this.AddVertex(collapsededVertex); // need to make sure it is numbered correctly // fixing the numbering fixed the looping issue.
            foreach (var edge in unchangeEdges)
            {
                // if the vertice is one of the ones we want to identify, add the edge
                if (this.IsContained(verticesToCollapse, edge.M) && this.IsContained(verticesToCollapse, edge.N))
                {
                    // Debug.WriteLine("Should be a self loop"); // Debug.WriteLine("Adding a self loop"); // we are never adding the self loop here
                    // these cases all become self loops
                    createdEdge = new Edge(collapsededVertex, collapsededVertex, edge.UniqueEdgeName);
                    this.AddEdge(createdEdge); // something is currently wrong with self loops
                }
                else if (this.IsContained(verticesToCollapse, edge.M) && !this.IsContained(verticesToCollapse, edge.N))
                {
                    // only one is connected to a vertice we are identifying. Preserve the edge logic
                    createdEdge = new Edge(collapsededVertex, edge.N, edge.UniqueEdgeName);
                    this.AddEdge(createdEdge); // parallel edges are possible.
                }
                else if (!this.IsContained(verticesToCollapse, edge.M) && this.IsContained(verticesToCollapse, edge.N))
                {
                    // only one is connected to a vertice we are identifying. Preserve the edge logic
                    // opposite of the previous just changed to take into account origins of the edges
                    createdEdge = new Edge(edge.M, collapsededVertex, edge.UniqueEdgeName);
                    this.AddEdge(createdEdge); // parallel edges are possible.
                }
            }

            // parallel edges still seem to remain
            edgesIdentified = this.ConvergeEdges(edgesIdentified); // this should give us a list of all the edges we need to add.

            // clear out all the vertices we identified. This shouldn't give renumbering issues until we call renumber
            foreach (var v in verticesToCollapse)
            {
                this.DeleteExpiredEdges(v);
                this.RemoveVertex(v);
                Debug.WriteLine("Collapse Vertex");
            }

        }

        /// <summary>
        /// This is a function to split a vertex.
        /// </summary>
        /// <param name="vertexToSplit">splits a vertex.</param>
        public void SplitVertex(MyVertex vertexToSplit)
        {
            Point p = new Point(vertexToSplit.Point.X + 25, vertexToSplit.Point.Y + 25);
            MyVertex newVertex = new MyVertex(p);
            this.MyVertices.Add(newVertex);
            List<Edge> unchangeEdges = new List<Edge>(this.MyEdges);
            Edge createdEdge;

            // towards split vertex
            foreach (var edge in unchangeEdges.Where(c => c.M.Number == vertexToSplit.Number && c.N.Number != vertexToSplit.Number))
            {
                createdEdge = new Edge(newVertex, edge.N, edge.UniqueEdgeName);
                this.AddEdge(createdEdge);
            }

            // away from split vertex
            foreach (var edge in unchangeEdges.Where(c => c.M.Number != vertexToSplit.Number && c.N.Number == vertexToSplit.Number))
            {
                createdEdge = new Edge(edge.M, newVertex, edge.UniqueEdgeName);
                this.AddEdge(createdEdge);
            }

            // loops
            foreach (var edge in unchangeEdges.Where(c => c.M.Number == vertexToSplit.Number && c.N.Number == vertexToSplit.Number))
            {
                createdEdge = new Edge(newVertex,newVertex, edge.UniqueEdgeName);
                this.AddEdge(createdEdge);
            }
        }

        /// <summary>
        /// Colors a rooted tree.
        /// </summary>
        /// <param name="root">the root of the tree.</param>
        public void ColorRootedTree(MyVertex root, Dictionary<string, Color> colorDictionary)
        {

            Dictionary<string, Color> modColorDictionary = new Dictionary<string, Color>();
            int i = 0;
            foreach (var colorKey in colorDictionary.Keys)
            {
                modColorDictionary[i.ToString()] = colorDictionary[colorKey];
                i++; // this will allow us to mod the color dictionary to color trees of any height.
            }

            List<MyVertex> treeVertices = this.IsInTree(root);
            List<MyVertex> currentPath = new List<MyVertex>();
            if (treeVertices != null)
            {
                // we will now assign colors based on path length since we know that the path to the root of a tree is unique.
                foreach (var v in treeVertices)
                {
                    currentPath = this.ShortestPath(root.Number, v.Number); // we know that there exists a shortest path.
                    this.myVertices.ElementAt(v.Number).Brush.Color = modColorDictionary[(currentPath.Count % modColorDictionary.Count).ToString()];
                }
            }
        }

        /// <summary>
        /// This is a function to return if it is a completed graph.
        /// </summary>
        /// <returns>boolean.</returns>
        public bool IsACompleteGraphK()
        {
            return true; // every vertice has an edge to every other vertice
        }

        /// <summary>
        /// Returns a list of all the bridges in the graph.
        /// </summary>
        /// <returns>list of edges.</returns>
        public List<Edge> Bridges()
        {
            List<Edge> allEdges = new List<Edge>(this.myEdges);
            List<Edge> bridges = new List<Edge>();
            Edge tempEdge;
            foreach (var edge in this.myEdges)
            {
                tempEdge = new Edge(edge.M, edge.N, edge.UniqueEdgeName);

                // Debug.WriteLine("We Have " + allEdges.Count.ToString() + " Before removing edge " + tempEdge.N.Number.ToString() + "<->" + tempEdge.M.Number.ToString());
                allEdges.Remove(edge);

                // Debug.WriteLine("We Have " + allEdges.Count.ToString() + "after removing edge" + tempEdge.N.Number.ToString() + "<->" + tempEdge.M.Number.ToString());
                this.DepthFirstSearch(this.myVertices, allEdges, edge.M, edge.N);

                // Debug.WriteLine("Result of DFS " + this.found.ToString());
                // our dfs is currently always failing to return true.
                if (!this.found)
                {
                    // Debug.WriteLine("Adding Bridge" + tempEdge.N.Number.ToString() + "->" + tempEdge.M.Number.ToString()); // Depth first search is failing
                    bridges.Add(tempEdge); // if it fails to find a connection between two vertices that were previously connected, then removing edge increases number of components. 
                }

                this.found = false;
                allEdges.Add(tempEdge); // need to put back the edge we removed.
            }

            // Debug.WriteLine("We Have " + bridges.Count.ToString() + " Bridges");
            return bridges;
        }

        /// <summary>
        /// This returns a list of all of the links.
        /// </summary>
        /// <returns>list of edges.</returns>
        public List<Edge> Links() // reuses our bridge function. If not a bridge it is a link
        {
            List<Edge> allEdges = new List<Edge>(this.myEdges);
            List<Edge> links = new List<Edge>();
            Edge tempEdge;
            foreach (var edge in this.myEdges)
            {

                tempEdge = new Edge(edge.M, edge.N, edge.UniqueEdgeName);

                // Debug.WriteLine("We Have " + allEdges.Count.ToString() + " Before removing edge " + tempEdge.N.Number.ToString() + "<->" + tempEdge.M.Number.ToString());
                allEdges.Remove(edge);

                // Debug.WriteLine("We Have " + allEdges.Count.ToString() + "after removing edge" + tempEdge.N.Number.ToString() + "<->" + tempEdge.M.Number.ToString());
                this.DepthFirstSearch(this.myVertices, allEdges, edge.N, edge.M);

                // Debug.WriteLine("Result of DFS " + this.found.ToString());
                // our dfs is currently always failing to return true.
                if (this.found)
                {
                    // Debug.WriteLine("Adding Bridge" + tempEdge.N.Number.ToString() + "->" + tempEdge.M.Number.ToString()); // Depth first search is failing
                    links.Add(tempEdge); // if it fails to find a connection between two vertices that were previously connected, then removing edge increases number of components. 
                }

                this.found = false;
                allEdges.Add(tempEdge); // need to put back the edge we removed.
            }

            // Debug.WriteLine("We Have " + bridges.Count.ToString() + " Bridges");
            return links;

        }

        /// <summary>
        /// This is a bredth first search function.
        /// </summary>
        /// <param name="vertices">this is a list of vertices.</param>
        /// <param name="edges">this is a list of edges.</param>
        /// <param name="source">this is the vertex we are searching from.</param>
        /// <param name="target">this is the vertex we are searching for.</param>
        /// <returns>boolean to indicate if the vertices are connected.</returns>
        public bool DepthFirstSearch(List<MyVertex> vertices, List<Edge> edges, MyVertex source, MyVertex target)
        {
            HashSet<int> visited = new HashSet<int>();
            this.found = false; // we need to make sure this is false before every search.
            this.DepthFirstSearchUtil(visited, vertices, edges, source, target); // return true if target is reachable from source.
            if (source.Number == target.Number)
            {
                return true; // a node can reach itself.
            }
            else
            {
                return this.found;
            }
        }

        /// <summary>
        /// This is the util function for dfs.
        /// </summary>
        /// <param name="visited">hashset of visited vertices.</param>
        /// <param name="vertices">this is a list of vertices.</param>
        /// <param name="edges">this is a list of edges.</param>
        /// <param name="vertex">this is the vertex we are searching from.</param>
        /// <param name="target">this is the vertex we are searching for.</param>
        public void DepthFirstSearchUtil(HashSet<int> visited, List<MyVertex> vertices, List<Edge> edges, MyVertex vertex, MyVertex target)
        {
            visited.Add(vertex.Number);
            foreach (var n in edges.Where(c => c.N.Number == vertex.Number && c.M.Number != vertex.Number))
            {
                // Debug.WriteLine("Looking for neighbors " + n.M.Number.ToString());
                if (!visited.Contains(n.M.Number))
                {
                    if (n.M.Number == target.Number)
                    {
                        // Debug.WriteLine("Found Target in Depth. " + target.Number.ToString());
                        this.found = true; // if this equals our target return true.
                        return;
                    }

                    this.DepthFirstSearchUtil(visited, vertices, edges, n.M, target);
                }
            }

            foreach (var n in edges.Where(c => c.N.Number != vertex.Number && c.M.Number == vertex.Number))
            {
                // Debug.WriteLine("Looking for neighbors " + n.M.Number.ToString());
                if (!visited.Contains(n.N.Number))
                {
                    if (n.N.Number == target.Number)
                    {
                        // Debug.WriteLine("Found Target in Depth. " + target.Number.ToString());
                        this.found = true; // if this equals our target return true.
                        return;
                    }

                    this.DepthFirstSearchUtil(visited, vertices, edges, n.N, target);
                }
            }
        }

        // adding and deleting edges should be done in graph as well. Add functions to do this.

        /// <summary>
        /// Deletes all of the edges associated with a given vertex.
        /// </summary>
        /// <param name="deletedVertex">the vertex that will be deleted.</param>
        public void DeleteExpiredEdges(MyVertex deletedVertex) // this function works.
        {
            for (int i = 0; i < this.MyEdges.Count; i++)
            {
                if (this.MyEdges[i].M == deletedVertex || this.MyEdges[i].N == deletedVertex)
                {
                    // we can delete, but list size gets smaller
                    this.myEdges[i].M.Degree -= 1;
                    this.myEdges[i].N.Degree -= 1;
                    this.MyEdges.RemoveAt(i);
                    --i;
                }
            }
        }

        /// <summary>
        /// Function to remove edges. must decrement the degree based off of the vertices.
        /// </summary>
        /// <param name="rEdge"></param>
        public void RemoveEdge(Edge rEdge)
        {
            rEdge.M.Degree -= 1;
            rEdge.N.Degree -= 1;
            this.MyEdges.Remove(rEdge);

            // update the degree of the two endpoints
        }

        /// <summary>
        /// Removes a vertice from the list.
        /// </summary>
        /// <param name="rVertex">vertex to remove.</param>
        public void RemoveVertex(MyVertex rVertex)
        {
            this.MyVertices.Remove(rVertex);
        }

        /// <summary>
        /// Function to add an edge.
        /// </summary>
        /// <param name="aEdge">edge to add.</param>
        public void AddEdge(Edge aEdge)
        {
            aEdge.M.Degree += 1;
            aEdge.N.Degree += 1;
            this.MyEdges.Add(aEdge);

            // update the degree of the two endpoints
        }

        /// <summary>
        /// Adds a vertex to the list.
        /// </summary>
        /// <param name="aVertex">add a vertex.</param>
        public void AddVertex(MyVertex aVertex)
        {
            aVertex.Number = this.CurrentVertexCount;
            this.CurrentVertexCount++;
            this.MyVertices.Add(aVertex);
        }

        /// <summary>
        /// This indicates if an edge is in a tree.
        /// </summary>
        /// <param name="treeVertex">the vertex that you want to check if it is in a tree.</param>
        public List<MyVertex> IsInTree(MyVertex treeVertex)
        {
            // determines if a particular vertex is a part of a tree
            // need to determine the connectivity of the vertex. Make sure that the edge count is number of vertices - 1
            List<MyVertex> treeVertices = new List<MyVertex>();
            foreach (var v in this.MyVertices)
            {
                if (this.DepthFirstSearch(this.MyVertices, this.MyEdges, treeVertex, v))
                {
                    // if vertex is found, it is connected to v.
                    treeVertices.Add(v);
                }
            }

            int edgeCount = 0;
            // List<Edge> treeEdges = new List<Edge>();
            // we know know it is connected. Now we must see that the number of edges in the tree is 1 less than the number of vertices. If we have at least connectiveness and number of edges we can determine if it is a tree.
            foreach (var edge in this.myEdges)
            {
                if (this.IsContained(treeVertices, edge.M) || this.IsContained(treeVertices, edge.N))
                {
                    // edges only occur once. So we can just determine if the edge is connected to an edge in the graph and don't need to worry about duplicates.
                    edgeCount += 1;
                    // treeEdges.Add(edge);
                }

                // Do nothing if the edge isn't connected, no need to count.
            }

            if (treeVertices.Count == edgeCount + 1)
            {
                Debug.WriteLine("Vertex " + treeVertex.Number.ToString() + " is in a tree. Tree Vertices count = " + treeVertices.Count.ToString());
                return treeVertices;
            }
            else
            {
                Debug.WriteLine("Vertex " + treeVertex.Number.ToString() + " is not in a tree.  Tree Vertices count = " + treeVertices.Count.ToString());
                return null;
            }
        }

        // Give Edges Weights?
        // Directed Edges?
        // cartesian products of graphs?
        // this works incorrectly if the direction is wrong. Fixed.

        /// <summary>
        /// THis is a function to find parallel edges given a list of parallel edges.
        /// </summary>
        /// <param name="edgeSet">The list of edges.</param>
        /// <returns>A list of edge and edgecount tuples.</returns>
        public List<(Edge, int)> FindParallelEdges(List<Edge> edgeSet)
        {
            List<(Edge, int)> parallelEdgeCount = new List<(Edge, int)>();
            foreach (var vertex in this.MyVertices)
            {
                foreach (var secondVertex in this.MyVertices)
                {
                    int count = edgeSet.FindAll(c => c.N == vertex && c.M == secondVertex).Count(); // search for all parallel edges.
                    count += edgeSet.FindAll(c => c.M == vertex && c.N == secondVertex).Count();

                    // keep track of the total count for both ways of the edges.
                    // Only add to parallel edgecount if vertex.number > secondVertex.number
                    if (count > 1 && vertex.Number > secondVertex.Number)
                    {
                        // it is a parallel edge
                        parallelEdgeCount.Add((edgeSet.Find(c => (c.N == vertex && c.M == secondVertex) || (c.M == vertex && c.N == secondVertex)), count));
                    }
                }
            }

            return parallelEdgeCount;
        }

        /// <summary>
        /// Converge parallel edges into a single edge.
        /// </summary>
        /// <param name="edgeSet">a list of edges that will converge its parallel edges.</param>
        /// <returns>the list of converged edges.</returns>
        public List<Edge> ConvergeEdges(List<Edge> edgeSet)
        {
            List<(Edge, int)> parallelEdgeCount = new List<(Edge, int)>();
            foreach (var vertex in this.MyVertices)
            {
                foreach (var secondVertex in this.MyVertices)
                {
                    int count = edgeSet.FindAll(c => c.N == vertex && c.M == secondVertex).Count(); // search for all parallel edges. 
                    if (count >= 1)
                    {
                        // it is a parallel edge
                        parallelEdgeCount.Add((edgeSet.Find(c => c.N == vertex && c.M == secondVertex), count));
                    }
                }
            }

            List<Edge> convergedEdges = new List<Edge>();
            foreach (var tuple in parallelEdgeCount)
            {
                convergedEdges.Add(tuple.Item1);
            }

            return convergedEdges;
        }

        /// <summary>
        /// Determines the min spanning tree.
        /// </summary>
        /// <returns>list of edges.</returns>
        public List<Edge> PrimMinSpanningTree()
        {
            // this.ConstructTwoWayAdjacencyMatrix();
            this.ConstructTwoWayAdjacencyMatrix();
            if (this.AdjacencyMatrix == null)
            {
                return null; // not instanciated yet
            }
            else if (this.myVertices.Count == 0)
            {
                return null;
            }

            // we will just use the adjacency matrix. That is all that is needed for min spanning tree.
            List<Edge> spanningEdges = new List<Edge>();
            int[] parent = new int[this.myVertices.Count];
            int[] key = new int[this.myVertices.Count];
            bool[] minSpanningTreeSet = new bool[this.myVertices.Count];

            int i = 0;
            while (i < this.myVertices.Count)
            {
                key[i] = int.MaxValue; // set all values to max initially.
                minSpanningTreeSet[i] = false; // all should be set to not visited initially.
                i++;
            }

            key[0] = 0; // the distance from first node to itself is zero always.
            parent[0] = -1; // no parent for root.
            i = 0;
            while (i < this.myVertices.Count - 1)
            {
                i++;
                int min = int.MaxValue;
                int minIndex = -1;

                // determine the next closest edge that hasn't been visited.
                for (int v = 0; v < this.myVertices.Count; v++)
                {
                    if (minSpanningTreeSet[v] == false && key[v] < min)
                    {
                        min = key[v];
                        minIndex = v;
                    }
                }

                if (minIndex == -1)
                {
                    continue; // skip loop if negative 1.
                }

                minSpanningTreeSet[minIndex] = true;
                for (int n = 0; n < this.myVertices.Count; n++)
                {
                    if (this.AdjacencyMatrix[minIndex, n] != 0 && this.AdjacencyMatrix[minIndex, n] < key[n] && minSpanningTreeSet[n] == false)
                    {
                        // if there is an edge and that vertice hasn't been visited yet. Our edges have no weights.
                        parent[n] = minIndex;
                        key[n] = 1; // all key weights shall be 1.
                        Edge nEdge = this.myEdges.Where(e => (e.N.Number == minIndex && e.M.Number == n) || (e.M.Number == minIndex && e.N.Number == n)).First();
                        Debug.WriteLine("Adding Edge to Span");
                        spanningEdges.Add(nEdge);
                    }
                }
            }

            return spanningEdges;
        }

        public int[,] DisplayAjacencyMatrix()
        {
            int[,] adjacencyMatrixParallelsCounted = new int[this.myVertices.Count, this.myVertices.Count]; // create a new matrix // initialized to zero everytime it is called and recalculated
            foreach (var edge in this.myEdges)
            {
                if (edge.M == edge.N)
                {
                    // this handles loops as they only count for 1, also loops are still the same if directed(but it is fine either way.)
                    adjacencyMatrixParallelsCounted[edge.N.Number, edge.M.Number] += 1; // only add 1 for loop.
                }
                else
                {
                    // all our edges go both ways. Parallel edges don't matter for dijkstras algorithm.
                    if (edge.UniqueEdgeName == "Directed Edge")
                    {
                        adjacencyMatrixParallelsCounted[edge.N.Number, edge.M.Number] += 1;
                    }
                    else
                    {
                        adjacencyMatrixParallelsCounted[edge.N.Number, edge.M.Number] += 1;
                        adjacencyMatrixParallelsCounted[edge.M.Number, edge.N.Number] += 1;
                    }
                }
            }

            return adjacencyMatrixParallelsCounted;
        }
    }
}

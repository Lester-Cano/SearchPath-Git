using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchPath
{
    class SearchPath
    {
        private Node _startingPoint;
        private Node _endingPoint;
        private List<List<Node>> _allNodes;
        private Dictionary<Vector2, Node> _block = new Dictionary<Vector2, Node>();
        private Vector2[] _directions = { Vector2.Up, Vector2.Right, Vector2.Down, Vector2.Left };
        private Queue<Node> _queue = new Queue<Node>();
        private Node _searchingPoint;
        private bool _isExploring = true;

        private List<Node> _path = new List<Node>();

        public List<Node> Path
        {
            get
            {
                if (_path.Count == 0)
                {
                    LoadAllBlocks();
                    BFS();
                    CreatePath();
                }

                return _path;
            }
        }



        public SearchPath(List<List<Node>> allNodes, Node startingPoint, Node endingPoint)
        {
            _allNodes = allNodes;
            _startingPoint = startingPoint;
            _endingPoint = endingPoint;

        }


        private void LoadAllBlocks()
        {
            foreach (List<Node> list in _allNodes)
            {
                foreach (Node node in list)
                {
                    Vector2 gridPos = node.Pos;

                    if (_block.ContainsKey(gridPos))
                    {
                        Console.WriteLine("2 Nodes present in same position. i.e nodes overlapped.");
                    }
                    else
                    {
                        _block.Add(gridPos, node);
                    }
                }

            }
        }

        private void BFS()
        {
            _queue.Enqueue(_startingPoint);

            while (_queue.Count > 0 && _isExploring)
            {
                _searchingPoint = _queue.Dequeue();
                OnReachingEnd();
                ExploreNeighbourNodes();

            }
        }

        private void OnReachingEnd()
        {

            if (_searchingPoint == _endingPoint)
            {
                _isExploring = false;
            }
            else
            {
                _isExploring = true;
            }
        }


        private void ExploreNeighbourNodes()
        {
            if (!_isExploring)
            {
                return;
            }

            foreach (Vector2 direction in _directions)
            {

                Vector2 neighbourPos = Vector2.Sum(_searchingPoint.Pos, direction);


                if (_block.ContainsKey(neighbourPos))
                {
                    Node node = _block[neighbourPos];

                    if (!node.isExplored)
                    {
                        _queue.Enqueue(node);
                        node.isExplored = true;

                        node.isExploredFrom = _searchingPoint;
                    }
                }
            }
        }

        public void CreatePath()
        {
            SetPath(_endingPoint);

            Node previousNode = _searchingPoint.isExploredFrom;

            while (previousNode != _startingPoint)
            {
                SetPath(previousNode);
                previousNode = previousNode.isExploredFrom;
            }

            SetPath(_startingPoint);
            _path.Reverse();
        }

        private void SetPath(Node node)
        {

            _path.Add(node);
        }
    }
}
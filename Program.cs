using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SearchPath
{
    class Program
    {
        
    public static void Main(string[] args)
    {
            //Stopwatch

            Stopwatch timer = new Stopwatch();
            timer.Start();



            List<Vector2> wallsTest1 = new List<Vector2>() 
            {
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(1,2),
                new Vector2(1,4),
                new Vector2(1,5),
                new Vector2(2,2),
                new Vector2(3,2),
                new Vector2(3,3),
                new Vector2(3,4),
                new Vector2(4,0),
                new Vector2(4,1),
                new Vector2(4,2),
            };

            /*
        List<Vector2> wallsTest2 = new List<Vector2>()
            {
                new Vector2( 0, 0),
                new Vector2( 1, 0),
                new Vector2( 2, 0),
                new Vector2( 3, 0),
                new Vector2( 4, 0),
                new Vector2( 5, 0),
                new Vector2( 6, 0),
                new Vector2( 7, 0),
                new Vector2(7, 1),
                new Vector2(2, 2),
                new Vector2(3, 2),
                new Vector2(4, 2),
                new Vector2(5, 2),
                new Vector2(7, 2),
                new Vector2(2, 3),
                new Vector2(5, 3),
                new Vector2(7, 3),
                new Vector2(2, 4),
                new Vector2(5, 4),
                new Vector2(7, 4),
                new Vector2(0, 5),
                new Vector2(1, 5),
                new Vector2(2, 5),
                new Vector2(5, 5),
                new Vector2(7, 5),
                new Vector2(0, 6),
                new Vector2(1, 6),
                new Vector2(2, 6),
                new Vector2(5, 6),
                new Vector2(7, 6),
                new Vector2(0, 7),
                new Vector2(1, 7),
                new Vector2(2, 7),
                new Vector2(5, 7),
                new Vector2(7, 7)
            };

    */
        Vector2 sizeTest1 = new Vector2(6, 6);
       // Vector2 sizeTest2 = new Vector2(8, 8);
        MazeGenerator mazeGenerator = new MazeGenerator(sizeTest1, wallsTest1);
        List<List<Node>> maze = mazeGenerator.GetMaze();

        Node startingPointTest1 = new Node(0, 5);
        Node endingPointTest1 = new Node(5, 0);
       /* Node startingPointTest2 = new Node(0, 4);
        Node endingPointTest2 = new Node(6, 7);  */

        SearchPath _searchPath = new SearchPath(maze, startingPointTest1, endingPointTest1);
        List<Node> _path = _searchPath.Path;

        MazeGenerator.PrintResolvedMaze(maze, _path);
        timer.Stop();


        //positions
        Console.WriteLine($"The starting position is {startingPointTest1.Pos.x},{startingPointTest1.Pos.y}");
        Console.WriteLine($"The ending position is {endingPointTest1.Pos.x},{endingPointTest1.Pos.y}");

            // timer
            Console.WriteLine($"The total elapsed time is: {timer.Elapsed.TotalMilliseconds}");

        // Read
        Console.ReadKey();
    }
}

public class MazeGenerator
{
    private List<List<Node>> maze = new List<List<Node>>();

    public MazeGenerator(Vector2 size, List<Vector2> walls)
    {
        maze = GenerateMaze(size, walls);
    }

    private List<List<Node>> GenerateMaze(Vector2 size, List<Vector2> walls)
    {
        List<List<Node>> maze = new List<List<Node>>();

        for (int i = 0; i < size.x; i++)
        {
            List<Node> row = new List<Node>();

            for (int j = 0; j < size.y; j++)
            {
                row.Add(new Node(j, i));
            }

            maze.Add(row);
        }

        foreach (Vector2 wall in walls)
        {
            Node wallNode = new Node(wall.x, wall.y);
            wallNode.isExplored = true;
            maze[wall.y][wall.x] = wallNode;
        }

        return maze;
    }

    public List<List<Node>> GetMaze()
    {
        return this.maze;
    }

    public static void PrintDebugMaze(List<List<Node>> maze)  //this maze was used as an placeholder to know the created path
    {
        foreach (List<Node> x in maze)
        {
            foreach (Node y in x)
            {
                if (y.isExplored)
                {
                    Console.Write($"|{y.Pos.x}:{y.Pos.y}|E| ");
                }
                else
                {
                    Console.Write($"|{y.Pos.x}:{y.Pos.y}|S| ");
                }

            }
            Console.WriteLine("\n");

        }
    }

    public static void PrintResolvedMaze(List<List<Node>> maze, List<Node> resolvedPath)   //this maze it's the maze that it's portrayed in the console
    {
        List<List<Node>> resolvedMaze = new List<List<Node>>(maze);

        foreach (Node resolvedNode in resolvedPath)
        {
            resolvedNode.isFinalPath = true;
            resolvedMaze[resolvedNode.Pos.y][resolvedNode.Pos.x] = resolvedNode;
        }

        foreach (List<Node> x in maze)
        {
            foreach (Node y in x)
            {

                if (y.isFinalPath)
                {
                    Console.Write("|-| ");
                }
                else if (y.isExplored)
                {
                    Console.Write("|1| ");
                }
                else
                {
                    Console.Write("|0| ");
                }

            }
            Console.WriteLine("\n");

        }
    }

}

public class Node
{
    public bool isExplored = false;
    public Node isExploredFrom;
    public bool isFinalPath = false;
    private Vector2 pos;

    public Vector2 Pos
    {
        get => pos;
    }

    public Node(int x, int y)
    {
        pos = new Vector2(x, y);
    }
}

public struct Vector2
{
    public int x;
    public int y;
    public static Vector2 Up = new Vector2(0, -1);
    public static Vector2 Down = new Vector2(0, 1);
    public static Vector2 Left = new Vector2(-1, 0);
    public static Vector2 Right = new Vector2(1, 0);

    public Vector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2 Sum(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x + b.x, a.y + b.y);
    }

    public static Vector2 Sub(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x - b.x, a.y - b.y);
    }

    public static Vector2 Mult(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x * b.x, a.y * b.y);
    }

    public static Vector2 Div(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x / b.x, a.y / b.y);
    }
}
}
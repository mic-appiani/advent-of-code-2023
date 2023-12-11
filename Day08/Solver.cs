using System.Data;
using System.Text.RegularExpressions;

namespace Day08;

public class Solver
{
    private long _solution;
    private Dictionary<string, Node> _nodes = new Dictionary<string, Node>();
    private string? _instructions;

    public long Solve(int part)
    {
        using var sr = new StreamReader("input.txt");
        _instructions = sr.ReadLine();
        sr.ReadLine();

        var pattern = @"(\w+)";
        var regex = new Regex(pattern);
        while (!sr.EndOfStream)
        {
            var input = sr.ReadLine();
            var matches = regex.Matches(input!);

            var node = new Node
            {
                IdPrefix = matches[0].Value.Substring(0, 2),
                IdSuffix = matches[0].Value.Substring(2, 1),
                Left = matches[1].Value,
                Right = matches[2].Value,
            };

            if (!_nodes.TryAdd(node.Id, node))
            {
                throw new Exception("Duplicate node");
            }
        }

        if (part == 1)
        {
            SolvePart1();
        }
        else if (part == 2)
        {
            SolvePart2();
        }

        return _solution;
    }

    private void SolvePart2()
    {
        // take all the nodes with suffix "A"
        // iterate the instructions like on part 1, but only stop when the current nodes have suffix Z
        var startNodes = _nodes
            .Where(x => x.Value.IdSuffix == "A")
            .Select(x => x.Value)
            .ToArray();

        var steps = new long[startNodes.Length];
        // for each node find the steps required to reach z
        for (int i = 0; i < startNodes.Length; i++)
        {
            var node = startNodes[i];
            var idx = 0;

            while (node.IdSuffix != "Z")
            {
                var direction = _instructions[idx];

                var nextKey = direction == 'L'
                    ? node.Left
                    : node.Right;

                node = _nodes[nextKey];
                steps[i]++;

                idx++;

                if (idx >= _instructions.Length)
                {
                    idx = 0;
                }
            }
        }

        _solution = steps.Aggregate((a, b) => Lcm(a, b));
    }
    
    static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
    
    static long Lcm(long a, long b)
    {
        return (a * b) / Gcd(a, b);
    }

    private void SolvePart1()
    {
        var currentNode = _nodes["AAA"];
        while (!currentNode.Id.Equals("ZZZ"))
        {
            var idx = 0;
            while (idx < _instructions.Length)
            {
                var direction = _instructions[idx];
                var nextKey = direction == 'L' ? currentNode.Left : currentNode.Right;
                currentNode = _nodes[nextKey];
                _solution++;
                idx++;

                if (currentNode.Id.Equals("ZZZ"))
                {
                    break;
                }
            }
        }
    }
}

public record Node
{
    public string IdPrefix { get; set; }
    public string IdSuffix { get; set; }

    public string Id
    {
        get => IdPrefix + IdSuffix;
    }

    public string Left { get; set; }
    public string Right { get; set; }
}
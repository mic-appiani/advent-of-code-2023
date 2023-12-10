using System.Data;
using System.Text.RegularExpressions;

namespace Day08;

public class Solver
{
    private int _solution;
    private Dictionary<string, Node> _nodes = new Dictionary<string, Node>();
    private string? _instructions;

    public int Solve(int part)
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
                Id = matches[0].Value,
                Left = matches[1].Value,
                Right = matches[2].Value,
            };

            if (!_nodes.TryAdd(node.Id, node))
            {
                throw new Exception("Duplicate node");
            }
        }

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

        return _solution;
    }
}

public record Node
{
    public string Id { get; set; }
    public string Left { get; set; }
    public string Right { get; set; }
}
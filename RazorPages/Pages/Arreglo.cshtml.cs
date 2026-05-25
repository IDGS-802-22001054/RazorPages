using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages;

public class ArregloModel : PageModel
{
    public List<int> Numbers { get; private set; } = [];
    public List<int> SortedNumbers { get; private set; } = [];
    public int Sum { get; private set; }
    public double Average { get; private set; }
    public double Median { get; private set; }
    public string ModeText { get; private set; } = string.Empty;

    public void OnGet()
    {
        GenerateArray();
    }

    private void GenerateArray()
    {
        Numbers = Enumerable.Range(0, 20)
            .Select(_ => Random.Shared.Next(0, 101))
            .ToList();

        SortedNumbers = Numbers.OrderBy(number => number).ToList();
        Sum = Numbers.Sum();
        Average = Numbers.Average();
        Median = (SortedNumbers[9] + SortedNumbers[10]) / 2d;

        var groups = Numbers
            .GroupBy(number => number)
            .Select(group => new { Number = group.Key, Count = group.Count() })
            .OrderBy(group => group.Number)
            .ToList();

        var highestFrequency = groups.Max(group => group.Count);
        ModeText = highestFrequency == 1
            ? "Sin moda"
            : string.Join(", ", groups.Where(group => group.Count == highestFrequency).Select(group => group.Number));
    }
}

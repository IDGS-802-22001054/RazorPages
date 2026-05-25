using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages;

public class BinomioModel : PageModel
{
    [BindProperty]
    [Display(Name = "a")]
    public double A { get; set; }

    [BindProperty]
    [Display(Name = "b")]
    public double B { get; set; }

    [BindProperty]
    [Display(Name = "x")]
    public double X { get; set; }

    [BindProperty]
    [Display(Name = "y")]
    public double Y { get; set; }

    [BindProperty]
    [Display(Name = "n")]
    [Range(0, 50, ErrorMessage = "n debe estar entre 0 y 50.")]
    public int N { get; set; }

    public bool HasResult { get; private set; }
    public double BaseValue { get; private set; }
    public double Result { get; private set; }
    public List<string> Terms { get; } = [];

    public void OnGet()
    {
    }

    public void OnPost()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        HasResult = true;
        var ax = A * X;
        var by = B * Y;
        BaseValue = ax + by;

        for (var k = 0; k <= N; k++)
        {
            var coefficient = Combination(N, k);
            var axPower = Math.Pow(ax, N - k);
            var byPower = Math.Pow(by, k);
            var termValue = coefficient * axPower * byPower;
            Result += termValue;
            Terms.Add($"C({N},{k})({ax:0.####})^{N - k}({by:0.####})^{k} = {termValue:0.####}");
        }
    }

    private static double Combination(int n, int k)
    {
        if (k > n - k)
        {
            k = n - k;
        }

        var result = 1d;
        for (var i = 1; i <= k; i++)
        {
            result = result * (n - k + i) / i;
        }

        return result;
    }
}

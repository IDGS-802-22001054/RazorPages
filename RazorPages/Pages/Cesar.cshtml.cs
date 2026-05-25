using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages;

public class CesarModel : PageModel
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVXYZ";

    [BindProperty]
    [Display(Name = "Mensaje")]
    [Required(ErrorMessage = "Captura el mensaje.")]
    public string Message { get; set; } = string.Empty;

    [BindProperty]
    [Display(Name = "Valor de n")]
    [Range(1, int.MaxValue, ErrorMessage = "n debe ser un entero positivo.")]
    public int Shift { get; set; } = 3;

    [BindProperty]
    [Display(Name = "Operacion")]
    public string Mode { get; set; } = "encode";

    public bool HasResult { get; private set; }
    public string Result { get; private set; } = string.Empty;

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
        var normalizedShift = Shift % Alphabet.Length;
        Result = Transform(RemoveDiacritics(Message).ToUpperInvariant(), Mode == "decode" ? -normalizedShift : normalizedShift);
    }

    private static string Transform(string input, int shift)
    {
        var output = new StringBuilder(input.Length);

        foreach (var character in input)
        {
            var index = Alphabet.IndexOf(character);
            if (index < 0)
            {
                output.Append(character);
                continue;
            }

            var newIndex = (index + shift + Alphabet.Length) % Alphabet.Length;
            output.Append(Alphabet[newIndex]);
        }

        return output.ToString();
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(character);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}

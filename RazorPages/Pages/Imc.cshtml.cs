using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages;

public class ImcModel : PageModel
{
    [BindProperty]
    [Display(Name = "Peso en kilogramos")]
    [Range(1, 500, ErrorMessage = "El peso debe estar entre 1 y 500 kg.")]
    public double PesoKg { get; set; }

    [BindProperty]
    [Display(Name = "Altura en metros")]
    [Range(0.1, 3, ErrorMessage = "La altura debe estar entre 0.10 y 3.00 m.")]
    public double AlturaM { get; set; }

    public bool HasResult { get; private set; }
    public double Imc { get; private set; }
    public string Classification { get; private set; } = string.Empty;
    public string Recommendation { get; private set; } = string.Empty;
    public string ImagePath { get; private set; } = "/images/imc-normal.svg";

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
        Imc = PesoKg / Math.Pow(AlturaM, 2);

        (Classification, Recommendation, ImagePath) = Imc switch
        {
            < 18 => ("Peso Bajo", "Incrementa tu ingesta de alimentos nutritivos y consulta a un profesional para una evaluacion completa.", "/images/imc-bajo.svg"),
            < 25 => ("Peso Normal", "Mantener actividad fisica regular, hidratacion y alimentacion equilibrada ayuda a conservar este rango.", "/images/imc-normal.svg"),
            < 27 => ("Sobre peso", "Prioriza porciones moderadas, caminatas frecuentes y seguimiento periodico de tus medidas.", "/images/imc-sobrepeso.svg"),
            < 30 => ("Obesidad grado I", "Conviene iniciar un plan supervisado de alimentacion y ejercicio para reducir riesgos metabolicos.", "/images/imc-obesidad1.svg"),
            < 40 => ("Obesidad grado II", "Busca acompanamiento medico y nutricional para definir objetivos seguros y sostenibles.", "/images/imc-obesidad2.svg"),
            _ => ("Obesidad grado III", "Requiere valoracion medica prioritaria para recibir un tratamiento integral y personalizado.", "/images/imc-obesidad3.svg")
        };
    }
}

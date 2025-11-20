using System.Text.RegularExpressions;

namespace MicroservicioComentario.Domain.Validators
{
    public static class InputValidator
    {
        private static readonly string[] SqlInjectionPatterns = new[]
        {
            @"(\bOR\b|\bAND\b).*=.*",
            @"';|--;|\/\*|\*\/",
            @"\bEXEC\b|\bEXECUTE\b",
            @"\bDROP\b|\bDELETE\b|\bUPDATE\b|\bINSERT\b",
            @"\bSELECT\b.*\bFROM\b",
            @"\bUNION\b.*\bSELECT\b",
            @"xp_cmdshell",
            @"\bSCRIPT\b.*>",
            @"<\s*script",
            @"javascript:",
            @"onerror\s*=",
            @"onload\s*="
        };

        public static string SanitizeString(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.Trim();
            input = Regex.Replace(input, @"\s+", " ");
            return input;
        }

        public static bool ContainsSqlInjection(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.ToUpper();

            foreach (var pattern in SqlInjectionPatterns)
            {
                if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
                    return true;
            }

            return false;
        }

        public static string ValidateAndSanitize(string? input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = SanitizeString(input);

            if (ContainsSqlInjection(input))
                throw new ArgumentException($"El campo '{fieldName}' contiene caracteres o patrones no permitidos.");

            return input;
        }

        public static string SanitizeText(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = SanitizeString(input);

            input = Regex.Replace(input, @"<script[^>]*>.*?</script>", "", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"javascript:", "", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"on\w+\s*=", "", RegexOptions.IgnoreCase);

            if (ContainsSqlInjection(input))
                throw new ArgumentException("El texto contiene patrones no permitidos.");

            return input;
        }

        public static DateTime ValidateDate(DateTime date, bool canBePast = true, bool canBeFuture = false)
        {
            var today = DateTime.Now.Date;
            var maxFutureDate = today.AddYears(1);

            if (!canBePast && date.Date < today)
                throw new ArgumentException("La fecha no puede ser anterior a hoy.");

            if (!canBeFuture && date.Date > today)
                throw new ArgumentException("La fecha no puede ser futura.");

            if (date > maxFutureDate)
                throw new ArgumentException("La fecha está demasiado lejos en el futuro.");

            if (date.Year < 1900)
                throw new ArgumentException("La fecha no es válida.");

            return date;
        }
    }
}

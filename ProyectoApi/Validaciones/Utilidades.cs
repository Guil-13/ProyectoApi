namespace ProyectoApi.Validaciones
{
    public static class Utilidades
    {
        //Mensajes de respuesta
        public static string RequiredFieldMenssage = "El campo {PropertyName} es requerido.";
        public static string MaximumLenghtMessage = "El campo {PropertyName} debe tener menos de {MaxLength} caracteres";
        public static string MinimumLenghtMessage = "El campo {PropertyName} debe tener al menos {MinLength} caracteres";
        public static string LenghtMessage = "El campo {PropertyName} debe ser de {MaxLength} caracteres.";
        public static string EmailMessage = "El campo {PropertyName} debe ser una direccion de correo valida.";
        public static string FirstLetterCapitalizedMessage = "El campo {PropertyName} debe comenzar con mayúscula";
        public static string RequiredOneUpperLetter = "Tu password debe contener al menos una letra en mayúscula";
        public static string RequiredOneLowerLetter = "Tu password debe contener al menos una letra en minúscula";
        public static string RequiredOneNumber = "Tu password debe contener al menos un número";
        public static string RequiredOneSpeciaCharacter = "Tu password debe contener al menos un carácter (!? *.-)";

        public static string GreaterThanOrEqualToMessage(DateTime fechaMinima)
        {
            return "El campo {PropertyName} debe ser posterior a " + fechaMinima.ToString("yyyy-MM-dd");
        }

        public static bool IsFirstLetterCapitalized(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return true;
            var primeraLetra = valor[0].ToString();
            return primeraLetra == primeraLetra.ToUpper();
        }

        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}

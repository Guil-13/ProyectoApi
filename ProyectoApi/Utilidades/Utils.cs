using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProyectoApi.Utilidades
{
    public static class Utils
    {
        #region Repositorios
        public static string GetTableName<T>()
        {
            var nombre = typeof(T).Name;
            var ultimo = nombre.Substring(nombre.Length - 1);
            switch (ultimo)
            {
                case "d": nombre = nombre + "es"; break;
                case "n": nombre = nombre + "es"; break;
                case "l": nombre = nombre + "es"; break;
                default: nombre = nombre + "s"; break;
            }
            return nombre;
        }

        public static string[] GetPropertiesNames<T>()
        {
            Type tipo = typeof(T);
            PropertyInfo[] properties = tipo.GetProperties();

            //Se aplica filtro para atributos (que no sea key o not mapped)
            var _propiedades = properties.Where(x => !x.GetCustomAttributes(typeof(KeyAttribute), false).Any() && !x.GetCustomAttributes(typeof(NotMappedAttribute), false).Any());

            var nombres = _propiedades.Select(p => p.Name);

            return nombres.ToArray();
        }
        #endregion
    }
}

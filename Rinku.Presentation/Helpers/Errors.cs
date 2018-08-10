using System.Web.Mvc;

namespace Rinku.Presentation.Helpers
{
    public class Errors
    {
        public static string getModelError(ModelStateDictionary ms)
        {
            string li = "";
            foreach (var item in ms.Values)
            {
                if (item.Errors.Count != 0)
                {
                    li += "<li>" + item.Errors[0].ErrorMessage + "</li>";
                }
            }
            return "<div> <ul>" + li + "</ul></div>";
        }

        public static string getModelWarning(ModelStateDictionary ms)
        {
            string li = "";
            int MaxMessages = 0;
            foreach (var item in ms.Values)
            {
                if (item.Errors.Count != 0)
                {
                    li += "<li>" + item.Errors[0].ErrorMessage + "</li>";
                    MaxMessages++;
                }
                if (MaxMessages >= 5)
                {
                    li += "<li>Otras advertencias...</li>";
                    break;
                }
            }
            return "<div><ul>" + li + "</ul></div>";
        }
    }
}

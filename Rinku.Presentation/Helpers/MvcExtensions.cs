using System;

using System.Web;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Linq.Expressions;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace Rinku.Presentation.Helpers
{
    public static class MvcExtensions
    {
        public static string AppVirtualPath(this WebPageExecutingBase page)
        {
            string path = page.Href("~/");
            if (!path.EndsWith("/"))
            {
                path += "/";
            }
            return path;
        }


        public static IHtmlString JsMinify(this HtmlHelper helper, Func<ViewContext, HelperResult> markup)
        {
            if (helper == null) throw new ArgumentNullException("helper");

            if (markup == null)
            {
                return MvcHtmlString.Empty;
            }

            var result = markup.Invoke(helper.ViewContext);

            if (!BundleTable.EnableOptimizations)
            {
                return result;
            }

            string sourceJs = result.ToString();

            var minifier = new Minifier();
            var minifiedJs = minifier.MinifyJavaScript(sourceJs, new CodeSettings
            {
                EvalTreatment = EvalTreatment.MakeImmediateSafe,
                PreserveImportantComments = false
            });

            return new MvcHtmlString(minifiedJs);
        }


        public static IHtmlString CssMinify(this HtmlHelper helper, Func<ViewContext, HelperResult> markup)
        {
            if (helper == null) throw new ArgumentNullException("helper");

            if (markup == null)
            {
                return MvcHtmlString.Empty;
            }

            var result = markup.Invoke(helper.ViewContext);

            if (!BundleTable.EnableOptimizations)
            {
                return result;
            }

            var sourceCss = result.ToString();

            var minifier = new Minifier();

            var minifiedCss = minifier.MinifyStyleSheet(sourceCss, new CssSettings
            {
                CommentMode = CssComment.None
            });

            return new MvcHtmlString(minifiedCss);
        }

        /* Como usarlo
         * 1: @using Tutorships.Service;
         * 2: depende el caso...Js o Css
    
        <script type="text/javascript">@(Html.JsMinify(@<text>
	        //  JS code here
        </text>))</script>
 
        or
 
        <style type="text/css">@(Html.CssMinify(@<text>
	        /*  CSS rules here * /
        </text>))</style>
 
        */

        /// <summary>
        /// Obtiene el valor de la propiedad del modelo solicitado.
        /// </summary>
        public static string GetDisplayName<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            return ModelMetadata.FromLambdaExpression<TModel, TProperty>(
                expression, new ViewDataDictionary<TModel>(model)).DisplayName;
        }

        public static string SerializaToJson(this object objeto)
        {
            string jsonResult = string.Empty;
            try
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(objeto.GetType());
                MemoryStream ms = new MemoryStream();
                jsonSerializer.WriteObject(ms, objeto);
                jsonResult = Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex) { string err = ex.StackTrace; throw; }
            return jsonResult;
        }

        public static T DeserializarJsonTo<T>(this string jsonSerializado)
        {
            try
            {
                T obj = Activator.CreateInstance<T>();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonSerializado));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                obj = (T)serializer.ReadObject(ms);
                ms.Close();
                ms.Dispose();
                return obj;
            }
            catch (Exception ex) { string err = ex.StackTrace; return default(T); }
        }

        /// <summary>
        /// Deletes a cookie with specified name
        /// </summary>
        /// <param name="controller">extends the controller</param>
        /// <param name="cookieName">cookie name</param>
        public static void DeleteCookie(this Controller controller, string cookieName)
        {
            if (controller.HttpContext.Request.Cookies[cookieName] == null)
                return; //cookie doesn't exist

            var c = new HttpCookie(cookieName)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            controller.HttpContext.Response.Cookies.Add(c);
        }
    }
}
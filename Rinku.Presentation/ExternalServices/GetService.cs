using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rinku.Presentation.Services.Nomina;

namespace Rinku.Presentation.ExternalServices
{
    public sealed class GetService
    {
        public static readonly RinkuServiceClient nominaService = new RinkuServiceClient();
        private GetService() { }   
    }
}
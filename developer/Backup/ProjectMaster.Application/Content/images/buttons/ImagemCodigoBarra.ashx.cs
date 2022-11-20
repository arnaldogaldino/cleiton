using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cms.Content.Theme.System.images
{
    /// <summary>
    /// Summary description for ImagemCodigoBarra
    /// </summary>
    public class ImagemCodigoBarra : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
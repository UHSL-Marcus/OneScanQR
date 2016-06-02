using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneScanWebApp.PayloadObjects
{
    class RecievedStatusData
    {
        public bool RedirectAsFormPost;
        public string RedirectURL;
        public string SessionID;
        public int? Status;
    }
}

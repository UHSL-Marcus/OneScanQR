using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneScanQR.PayloadObjects
{
    class LoginPayload
    {
        public string FriendlyName = "Smartlab";
        public string SiteIdentifier = "EE9D8995-04BB-452A-A4FE-066D3550662A";
        public string LoginMode;
        public string[] Profiles;
    }

    enum LoginTypes
    {
        Register,
        UsernamePassword,
        UserToken,
        TokenOrCredentials
    }
}

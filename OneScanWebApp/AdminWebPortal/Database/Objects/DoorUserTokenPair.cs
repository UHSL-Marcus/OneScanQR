using SQLControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminWebPortal.Database.Objects
{
    public class DoorUserTokenPair : DatabaseTableObject
    {
        [DatabaseColumn]
        public int? DoorID;
        [DatabaseColumn]
        public int? UserToken;
    }
}
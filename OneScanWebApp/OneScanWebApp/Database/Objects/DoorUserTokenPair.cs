﻿using SQLControlsLib;

namespace OneScanWebApp.Database.Objects
{
    public class DoorUserTokenPair : DatabaseTableObject
    {
        [DatabaseID]
        public int? Id;
        public int? DoorID;
        public int? UserToken;
    }
}
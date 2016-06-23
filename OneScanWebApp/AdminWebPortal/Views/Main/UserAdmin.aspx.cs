using AdminWebApp.Database.Objects;
using AdminWebPortal.Database.Objects;
using AdminWebPortal.Utils;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;


namespace AdminWebPortal.Views.Main
{
    public partial class UserAdmin : System.Web.UI.Page
    {

        protected Dictionary<int, string> OpenDoorTables = new Dictionary<int, string>();
        protected Dictionary<int, Tuple<int, string[]>> OpenDoorRegisterCtls = new Dictionary<int, Tuple<int, string[]>>(); // key: UserToken ID. value: first: index of first control. Second: array of control ID's

        protected override void OnInit(EventArgs e)
        {
            Request.Form[doorViewState.UniqueID].TryDeserializeObject(out OpenDoorTables, true, true);
            Request.Form[RegisterDoorCtlsViewState.UniqueID].TryDeserializeObject(out OpenDoorRegisterCtls, true, true);

            fillTable();
        }

        protected override void OnPreRender(EventArgs e)
        {
            doorViewState.Value = OpenDoorTables.SerializeObject(true, true);
            RegisterDoorCtlsViewState.Value = OpenDoorRegisterCtls.SerializeObject(true, true);
        }

        private void DoorViewBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                DoorViewBtnArguments args = JsonUtils.GetObject<DoorViewBtnArguments>(btn.CommandArgument);
                int userTknID = args.UserTokenID.Value;

                TableCell recieveCell; 
                if (btn.FindParent(out recieveCell))
                {
                    if (args.showingDoors)
                    {
                        recieveCell.Controls.Remove(recieveCell.FindControlRecursive(OpenDoorTables[userTknID]));
                        OpenDoorTables.Remove(userTknID);
                        btn.Text = "Show Registered Doors";
                        args.showingDoors = false;
                    }
                    else
                    {
                        OpenDoorTables.Add(userTknID, addDoorViewTable(recieveCell.Controls, userTknID));
                        args.showingDoors = true;
                        btn.Text = "Hide Registered Doors";
                    }

                    btn.CommandArgument = JsonUtils.GetJson(args);
                }

            }


        }

        private void RemoveDoorAuthBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                RemoveAuthBtnArguments args = JsonUtils.GetObject<RemoveAuthBtnArguments>(btn.CommandArgument);

                TableRow RowToRemove;
                if (btn.FindParent(out RowToRemove))
                {
                    Table DoorTable;
                    if (RowToRemove.FindParent(out DoorTable))
                    {
                        DoorUserTokenPair dtp = new DoorUserTokenPair();
                        dtp.DoorID = args.DoorID;
                        dtp.UserToken = args.UserTokenID;
                        SQLControlsLib.Delete.doDelete(dtp);

                        TableCell parentCell;
                        if (DoorTable.FindParent(out parentCell))
                        {
                            parentCell.Controls.Remove(DoorTable);
                            addDoorViewTable(parentCell.Controls, args.UserTokenID.Value);

                            if (OpenDoorRegisterCtls.ContainsKey(args.UserTokenID.Value))
                            {
                                foreach (string id in OpenDoorRegisterCtls[args.UserTokenID.Value].Item2)
                                    parentCell.Controls.Remove(parentCell.FindControlRecursive(id));

                                addDoorRegisterControls(parentCell.Controls, args.UserTokenID.Value, OpenDoorRegisterCtls[args.UserTokenID.Value].Item1);
                            }
                        }
                    }
                }
                  
            }
        }

        private void AddRegDoorCtlsBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                AddRegDoorCtlsBtnArguments args = JsonUtils.GetObject<AddRegDoorCtlsBtnArguments>(btn.CommandArgument);

                TableCell recieveCell;
                if (btn.FindParent(out recieveCell))
                {

                    if (args.showingInput)
                    {

                        foreach (string ID in OpenDoorRegisterCtls[args.UserTokenID.Value].Item2)
                            recieveCell.Controls.Remove(recieveCell.FindControlRecursive(ID));

                        OpenDoorRegisterCtls.Remove(args.UserTokenID.Value);

                        args.showingInput = false;
                        btn.Text = "Register To Door";
                    }
                    else
                    {
                        OpenDoorRegisterCtls.Add(args.UserTokenID.Value, Tuple.Create(args.delBtnIndex.Value + 1, 
                            addDoorRegisterControls(recieveCell.Controls, args.UserTokenID.Value, args.delBtnIndex + 1))
                            );

                        args.showingInput = true;
                        btn.Text = "Close Input";
                    }

                    btn.CommandArgument = JsonUtils.GetJson(args);
                }
            }

        }

        private void AddDoorRegisterBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                AddDoorRegBtnArguments args = JsonUtils.GetObject<AddDoorRegBtnArguments>(btn.CommandArgument);

                TableCell parentCell;
                if (btn.FindParent(out parentCell))
                {
                    DropDownList DropDown;
                    if (parentCell.FindControlRecursive(args.DropDownID, out DropDown))
                    {
                        string doorID = DropDown.SelectedValue;

                        DoorUserTokenPair dtp = new DoorUserTokenPair();
                        dtp.DoorID = int.Parse(doorID);
                        dtp.UserToken = args.UserTokenID;
                        SQLControlsLib.Set.doInsert(dtp);

                        foreach (string id in OpenDoorRegisterCtls[args.UserTokenID.Value].Item2)
                            parentCell.Controls.Remove(parentCell.FindControlRecursive(id));

                        addDoorRegisterControls(parentCell.Controls, args.UserTokenID.Value, OpenDoorRegisterCtls[args.UserTokenID.Value].Item1);

                        if (OpenDoorTables.ContainsKey(args.UserTokenID.Value))
                        {
                            Table DoorViewTable;
                            if (parentCell.FindControlRecursive(OpenDoorTables[args.UserTokenID.Value], out DoorViewTable))
                            {
                                parentCell.Controls.Remove(DoorViewTable);
                                addDoorViewTable(parentCell.Controls, args.UserTokenID.Value);
                            }
                        }  
                    }
                }
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                DeleteBtnArguments args = JsonUtils.GetObject<DeleteBtnArguments>(btn.CommandArgument);

                SQLControlsLib.Delete.doDeleteEntryByColumn("DoorUserTokenPair", args.UserTokenID.Value, "UserToken");
                SQLControlsLib.Delete.doDeleteEntryByColumn("UserInfo", args.UserInfoID.Value, "Id");
                SQLControlsLib.Delete.doDeleteEntryByColumn("UserToken", args.UserTokenID.Value, "Id");

                usersTbl.Rows.Remove((TableRow)usersTbl.FindControlRecursive(args.RowID));

            }
        }

        protected void registerNewUserBtn_Click(object sender, EventArgs e)
        {
            string key = Guid.NewGuid().ToString();
            string secret = Guid.NewGuid().ToString();

            RegistrationToken rt = new RegistrationToken();
            rt.AuthKey = key; rt.Secret = secret;
            if (SQLControlsLib.Set.doInsert(rt))
            {
                string guid = Guid.NewGuid().ToString();
                string query = "guid=" + guid + "&key=" + key;
                string hmac = HMAC.Hash(query, secret);
                query += "&data=" + hmac;

                Response.Redirect("~/Views/QRPages/UserQR?" + query);
            }
        }
    }
}
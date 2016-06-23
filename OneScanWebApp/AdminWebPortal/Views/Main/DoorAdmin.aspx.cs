
using AdminWebPortal.Database.Objects;
using AdminWebPortal.Utils;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdminWebPortal.Views.Main
{
    public partial class DoorAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
            {
                fillTable();
            }
        }

        private void fillTable()
        {
            TableRow headings = DoorsTbl.Rows[0];
            DoorsTbl.Rows.Clear();
            DoorsTbl.Rows.Add(headings);



            List<Door> doors;
            if (SQLControlsLib.Get.doSelect("*", out doors))
            {

                foreach (Door door in doors)
                {
                    TableRow tRow = new TableRow();
                    TableCell tCell;

                    tRow.Cells.AddTextCell(door.DoorID);
                    tRow.Cells.AddTextCell(door.DoorSecret);

                    tCell = new TableCell();

                    Button delBtn = new Button();
                    delBtn.Text = "Delete";
                    delBtn.ID = "deleteDoor" + door.Id.ToString();
                    delBtn.Click += DelBtn_Click;
                    delBtn.CommandArgument = door.Id.ToString();

                    tCell.Controls.Add(delBtn);
                    tRow.Cells.Add(tCell);

                    DoorsTbl.Rows.Add(tRow);
                }
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                int dID = int.Parse(btn.CommandArgument);

                SQLControlsLib.Delete.doDeleteEntryByColumn("DoorUserTokenPair", dID, "DoorID");
                SQLControlsLib.Delete.doDeleteEntryByColumn("Door", dID, "Id");

                fillTable();
            }
        }

        protected void addDoorBtn_Click(object sender, EventArgs e)
        {
            Door door = new Door();
            door.DoorID = newDoorIdTxtBx.Text;
            door.DoorSecret = newDoorSecretTxtBx.Text;

            if (SQLControlsLib.Set.doInsert(door))
            {
                fillTable();
                newDoorIdTxtBx.Text = "";
                newDoorSecretTxtBx.Text = "";
            }
        }
    }
}
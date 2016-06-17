using AdminWebPortal.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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


            DataTableReader reader = SQLControls.getDataReader("SELECT Door.Id, Door.DoorID, Door.DoorSecret FROM Door");

            while (reader.Read())
            {
                TableRow tRow = new TableRow();
                TableCell tCell;

                for (int i = 1; i < 3; i++)
                {
                    tCell = new TableCell();
                    tCell.Text = reader.GetString(i);
                    tRow.Cells.Add(tCell);
                }

                tCell = new TableCell();

                Button delBtn = new Button();
                delBtn.Text = "Delete";
                delBtn.ID = "deleteDoor" + reader.GetInt32(0).ToString();
                delBtn.Click += DelBtn_Click;
                delBtn.CommandArgument = reader.GetInt32(0).ToString();

                tCell.Controls.Add(delBtn);
                tRow.Cells.Add(tCell);

                DoorsTbl.Rows.Add(tRow);
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                string dID = btn.CommandArgument;
                string queryP = "DELETE FROM DoorUserTokenPair WHERE DoorID='" + dID + "'";
                string queryD = "DELETE FROM Door WHERE Id='" + dID + "'";

                SQLControls.doNonQuery(queryP);
                SQLControls.doNonQuery(queryD);

                fillTable();
            }
        }

        protected void addDoorBtn_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> entry = new Dictionary<string, object>();
            entry.Add("Door.DoorID", newDoorIdTxtBx.Text);
            entry.Add("Door.DoorSecret",newDoorSecretTxtBx.Text);

            if (SQLControls.Set.doInsert("Door", entry))
            {
                fillTable();
                newDoorIdTxtBx.Text = "";
                newDoorSecretTxtBx.Text = "";
            }
        }
    }
}
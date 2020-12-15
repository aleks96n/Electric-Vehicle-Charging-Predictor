using ElectricVehicleChargingPredictor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ElectricVehicleChargingPredictor
{
    public partial class Grid_Connection : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var vsm = new VehicleServiceManager();
            var result = new SimpleResult();

            if (!IsPostBack)
            {
                var vehicleModels = new List<VehicleModelModel>();
                result = vsm.GetVehicleModels(ref vehicleModels);

                if (vehicleModels.Count > 0)
                {
                    cmdModel_1.Items.Add("");
                    foreach (var model in vehicleModels)
                    {
                        cmdModel_1.Items.Add(new ListItem(model.model, model.model_id.ToString()));
                    }
                }


            }


        }

        protected void cmdModel_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var vehicles = new List<string>();

            var vsm = new VehicleServiceManager();
            vsm.GetVehicles(ref vehicles, cmdModel_1.SelectedValue.ToString());

            if (vehicles.Count > 0)
            {
                divVehicleID_1.Visible = true;
                cmdVehicleID_1.Items.Clear();

                foreach (var id in vehicles)
                {
                    cmdVehicleID_1.Items.Add(new ListItem(id, id));
                }
            }
            else
            {
                divVehicleID_1.Visible = false;
            }

        }

        protected void btnSubmit_1_Click(object sender, EventArgs e)
        {

            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.InvariantCulture;
            var grids = new List<GridConnectionModel>();

            var vsm = new VehicleServiceManager();
            vsm.Predict_Grid_Connection(ref grids, cmdVehicleID_1.SelectedValue.ToString());

            if (grids.Count > 0)
            {
                pnl_2.Visible = true;
                hdn_xlabels_2.Value = "";
                hdn_data_2.Value = "";
                hdn_legend_2.Value = cmdModel_1.SelectedItem.Text + " (Vehicle ID: " + cmdVehicleID_1.SelectedValue.ToString() + ")";


                tbl_SOC_2.Rows.Clear();

                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.Cells.Add(new TableHeaderCell { Text = "Day", HorizontalAlign = HorizontalAlign.Center });
                headerRow.Cells.Add(new TableHeaderCell { Text = "Time", HorizontalAlign = HorizontalAlign.Center });
                headerRow.Cells.Add(new TableHeaderCell { Text = "Connected", HorizontalAlign = HorizontalAlign.Center });

                tbl_SOC_2.Rows.Add(headerRow);

                foreach (var grid in grids)
                {
                    string time = grid.time.ToString() == "24" ? "00" : grid.time.ToString().PadLeft(2, '0');
                    string s = DateTime.ParseExact(time + "0000", "HHmmss", c).ToString("hh tt", c);
                    hdn_xlabels_2.Value += ",\"" + s + "\"";
                    hdn_data_2.Value += "," + grid.connected.ToString();

                    TableRow row = new TableRow();
                    row.Cells.Add(new TableCell { Text = DateTime.Now.Day.ToString() });
                    row.Cells.Add(new TableCell { Text = time });
                    row.Cells.Add(new TableCell { Text = grid.connected.ToString() });

                    tbl_SOC_2.Rows.Add(row);
                }

                hdn_xlabels_2.Value = "[" + hdn_xlabels_2.Value.Substring(1) + "]";
                hdn_data_2.Value = "[" + hdn_data_2.Value.Substring(1) + "]";

            }
            else
            {
                pnl_2.Visible = false;
            }
        }
    }
}
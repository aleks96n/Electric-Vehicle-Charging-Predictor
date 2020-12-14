using ElectricVehicleChargingPredictor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ElectricVehicleChargingPredictor
{
    public partial class State_of_Charge : Page
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
            var socs = new List<SOCModel>();

            var vsm = new VehicleServiceManager();
            vsm.Predict_SOC(ref socs, cmdVehicleID_1.SelectedValue.ToString());

            if (socs.Count > 0)
            {
                pnl_2.Visible = true;
                hdn_xlabels_2.Value = "";
                hdn_data_2.Value = "";
                hdn_legend_2.Value = cmdModel_1.SelectedItem.Text + " (Vehicle ID: " + cmdVehicleID_1.SelectedValue.ToString() + ")";


                tbl_SOC_2.Rows.Clear();

                TableHeaderRow headerRow = new TableHeaderRow();
                //headerRow.Cells.Add(new TableHeaderCell { Text = "Day", HorizontalAlign = HorizontalAlign.Center });
                headerRow.Cells.Add(new TableHeaderCell { Text = "Time", HorizontalAlign = HorizontalAlign.Center });
                headerRow.Cells.Add(new TableHeaderCell { Text = "State of Charge", HorizontalAlign = HorizontalAlign.Center });

                tbl_SOC_2.Rows.Add(headerRow);

                float prevSOC = 0, currSOC = 0, chargedSOC = 0, usedSOC = 0;

                foreach (var soc in socs)
                {
                    string time = soc.time.ToString() == "24" ? "00" : soc.time.ToString().PadLeft(2, '0');
                    string s = DateTime.ParseExact(time + "0000", "HHmmss", c).ToString("hh tt", c);
                    hdn_xlabels_2.Value += ",\"" + s + "\"";
                    hdn_data_2.Value += "," + soc.soc.ToString();

                    TableRow row = new TableRow();
                    //row.Cells.Add(new TableCell { Text = DateTime.Now.Day.ToString()});
                    row.Cells.Add(new TableCell { Text = time });
                    row.Cells.Add(new TableCell { Text = soc.soc.ToString() });

                    tbl_SOC_2.Rows.Add(row);

                    currSOC = soc.soc;

                    if (currSOC > 0 && prevSOC > 0)
                    {
                        if (currSOC > prevSOC)
                        {
                            chargedSOC += currSOC - prevSOC;
                        }
                        else
                        {
                            usedSOC += prevSOC - currSOC;
                        }
                    }

                    prevSOC = currSOC;

                }

                hdn_xlabels_2.Value = "[" + hdn_xlabels_2.Value.Substring(1) + "]";
                hdn_data_2.Value = "[" + hdn_data_2.Value.Substring(1) + "]" ;
                hdn_used_2.Value = usedSOC.ToString();
                hdn_charged_2.Value = chargedSOC.ToString();

            }
            else
            {
                pnl_2.Visible = false;
            }
        }
    }
}
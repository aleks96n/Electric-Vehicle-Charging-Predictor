<%@ Page Title="Grid Connection Predictor" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Grid_Connection.aspx.cs" Inherits="ElectricVehicleChargingPredictor.Grid_Connection" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/moment@2.24.0"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-adapter-moment@0.1.1"></script>

    <div class="row">
        <div class="col-xs-12 col-sm-12 col-lg-12">
            <h2><%: Title %>.</h2>
        </div>
    </div>
    <div class="row">
        &nbsp;
    </div>
    <div class="row">
        <div class="form-group col-xs-12 col-sm-12 col-lg-4">
            <label for="sel1">Vehicle Models:</label>
            <asp:DropDownList ID="cmdModel_1" class="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmdModel_1_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="form-group col-xs-12 col-sm-12 col-lg-4" runat="server" id="divVehicleID_1" visible="false">
            <label for="sel1">Vehicle ID:</label>
            <asp:DropDownList ID="cmdVehicleID_1" class="form-control" runat="server"></asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="form-group col-xs-12 col-sm-12 col-lg-4">
            <asp:Button ID="btnSubmit_1" runat="server" class="btn btn-primary" OnClick="btnSubmit_1_Click" Text="Predict" />
        </div>

    </div>

    <asp:Panel runat="server" ID="pnl_2" Visible="false">
        <asp:HiddenField runat="server" ID="hdn_xlabels_2" />
        <asp:HiddenField runat="server" ID="hdn_data_2" />
        <asp:HiddenField runat="server" ID="hdn_legend_2" />

        <div class="row">
            <hr />
        </div>

<%--        <div class="row">
            <div class="card text-center col-xs-12 col-sm-6 col-md-6 col-lg-6">
                <div class="card-body">
                    <h4 class="card-title">Total used energy</h4>
                    <p class="card-text">
                        <h1>30</h1>
                    </p>
                </div>
            </div>
            <div class="card text-center col-xs-12 col-sm-6 col-md-6 col-lg-6">
                <div class="card-body">
                    <h4 class="card-title">Total charged energy</h4>
                    <p class="card-text">
                        <h1>40</h1>
                    </p>
                </div>
            </div>
        </div>--%>

        <div class="row">
            &nbsp;
        </div>

        <div class="row">
            <div class="col-xs-12 col-sm-12 col-lg-12" style="width: 80%;">
                <canvas id="canvas"></canvas>
                <br>
                <br>
            </div>

        </div>

        <div class="row">
            <div class="col-xs-12 col-sm-12 col-lg-12">
                <asp:Table ID="tbl_SOC_2" class="table table-striped table-bordered" runat="server" Width="100%">
                    <asp:TableHeaderRow>
                        <asp:TableCell>Day</asp:TableCell>
                        <asp:TableCell>Time</asp:TableCell>
                        <asp:TableCell>Connected</asp:TableCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </div>

        <script>


            var timeFormat = 'MM/DD/YYYY HH:mm';

            var hdn_xlabels_2 = $("#" + '<%= hdn_xlabels_2.ClientID %>').val();
            var hdn_data_2 = $("#" + '<%= hdn_data_2.ClientID %>').val();
            var hdn_legend_2 = $("#" + '<%= hdn_legend_2.ClientID %>').val();

            var xLabel = JSON.parse(hdn_xlabels_2);
            var data = JSON.parse(hdn_data_2);


            function newDate(days) {
                return moment().add(days, 'd').toDate();
            }

            function newDateString(days) {
                return moment().add(days, 'd').format(timeFormat);
            }

            var color = Chart.helpers.color;
            var config = {
                type: 'line',
                data: {
                    labels: xLabel,
                    datasets: [{
                        label: hdn_legend_2,
                        backgroundColor: color(window.chartColors.red).alpha(0.5).rgbString(),
                        borderColor: window.chartColors.red,
                        fill: false,
                        data: data,
                    }]
                },
                options: {
                    plugins: {
                        title: {
                            text: 'Chart.js Time Scale',
                            display: true
                        }
                    },
                    scales: {
                        xAxes: [{
                            scaleLabel: {
                                display: true,
                                labelString: 'ime'
                            }
                        }],
                        yAxes: [{
                            scaleLabel: {
                                display: true,
                                labelString: 'Connected'
                            },
                            ticks: {
                                min: 0
                            }
                        }]
                    },
                }
            };

            window.onload = function () {
                var ctx = document.getElementById('canvas').getContext('2d');
                window.myLine = new Chart(ctx, config);

            };
        </script>
    </asp:Panel>






</asp:Content>

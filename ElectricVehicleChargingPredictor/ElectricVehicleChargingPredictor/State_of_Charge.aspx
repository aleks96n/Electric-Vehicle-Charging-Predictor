<%@ Page Title="State of Charge Predictor" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="State_of_Charge.aspx.cs" Inherits="ElectricVehicleChargingPredictor.State_of_Charge" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/moment@2.24.0"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-adapter-moment@0.1.1"></script>



    <%--    <h3></h3>--%>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-lg-12">
            <h2><%: Title %>.</h2>
        </div>
    </div>
<%--    <div class="row">
        <div class="col-xs-12 col-sm-12 col-lg-4">
            EV ID
        </div>
        <div class="col-xs-12 col-sm-12 col-lg-4">
            Date 
        </div>
        <div class="col-xs-12 col-sm-12 col-lg-4">
            Time
        </div>
    </div>--%>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-lg-12" style="width: 80%;">
            <canvas id="canvas"></canvas>
            <br>
            <br>
        </div>

    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-lg-12">
            <table class="table table-striped table-bordered">
                <thead>
                    <tr>
                        <th>Day</th>
                        <th>Time</th>
                        <th>State of Charge</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>14</td>
                        <td>1</td>
                        <td>36.25</td>
                    </tr>
                    <tr>
                        <td>14</td>
                        <td>2</td>
                        <td>42.85</td>
                    </tr>
                    <tr>
                        <td>14</td>
                        <td>3</td>
                        <td>49.45</td>
                    </tr>
                    <tr>
                        <td>14</td>
                        <td>4</td>
                        <td>56</td>
                    </tr>
                    <tr>
                        <td>14</td>
                        <td>5</td>
                        <td>56</td>
                    </tr>
                    <tr>
                        <td>14</td>
                        <td>6</td>
                        <td>56</td>
                    </tr>
                    <tr>
                        <td>14</td>
                        <td>7</td>
                        <td>47.48</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>



    <script>
        var timeFormat = 'MM/DD/YYYY HH:mm';

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
                labels: [ // Date Objects
                    "Day 14, Time 1",
                    "Day 14, Time 2",
                    "Day 14, Time 3",
                    "Day 14, Time 4",
                    "Day 14, Time 5",
                    "Day 14, Time 6",
                    "Day 14, Time 7",
                    "Day 14, Time 8",
                    "Day 14, Time 9",
                    "Day 14, Time 10",
                    "Day 14, Time 11",
                    "Day 14, Time 12",
                    "Day 14, Time 13",
                    "Day 14, Time 14",
                    "Day 14, Time 15",
                    "Day 14, Time 16",
                    "Day 14, Time 17",
                    "Day 14, Time 18",
                    "Day 14, Time 19",
                    "Day 14, Time 20",
                    "Day 14, Time 21",
                    "Day 14, Time 22",
                    "Day 14, Time 23",
                    "Day 14, Time 24"
                ],
                datasets: [{
                    label: 'EV ID: 1',
                    backgroundColor: color(window.chartColors.red).alpha(0.5).rgbString(),
                    borderColor: window.chartColors.red,
                    fill: false,
                    data: [
                        36.25,
                        42.85,
                        49.45,
                        56,
                        56,
                        56,
                        47.48,
                        54.08,
                        49.06,
                        48.20,
                        40.92,
                        47.52,
                        39.32,
                        34.59,
                        41.19,
                        47.79,
                        36.42,
                        43.02,
                        31.24,
                        30.38,
                        36.98,
                        43.58,
                        34.58,
                        41.18
                    ],
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
                    x: {
                        type: 'time',
                        time: {
                            parser: timeFormat,
                            // round: 'day'
                            tooltipFormat: 'll HH:mm'
                        },
                        scaleLabel: {
                            display: true,
                            labelString: 'Date'
                        }
                    },
                    y: {
                        scaleLabel: {
                            display: true,
                            labelString: 'value'
                        }
                    }
                },
            }
        };

        window.onload = function () {
            var ctx = document.getElementById('canvas').getContext('2d');
            window.myLine = new Chart(ctx, config);

        };

        document.getElementById('randomizeData').addEventListener('click', function () {
            config.data.datasets.forEach(function (dataset) {
                dataset.data.forEach(function (dataObj, j) {
                    if (typeof dataObj === 'object') {
                        dataObj.y = randomScalingFactor();
                    } else {
                        dataset.data[j] = randomScalingFactor();
                    }
                });
            });

            window.myLine.update();
        });

        var colorNames = Object.keys(window.chartColors);
        document.getElementById('addDataset').addEventListener('click', function () {
            var colorName = colorNames[config.data.datasets.length % colorNames.length];
            var newColor = window.chartColors[colorName];
            var newDataset = {
                label: 'Dataset ' + config.data.datasets.length,
                borderColor: newColor,
                backgroundColor: color(newColor).alpha(0.5).rgbString(),
                data: [],
            };

            for (var index = 0; index < config.data.labels.length; ++index) {
                newDataset.data.push(randomScalingFactor());
            }

            config.data.datasets.push(newDataset);
            window.myLine.update();
        });

        document.getElementById('addData').addEventListener('click', function () {
            if (config.data.datasets.length > 0) {
                config.data.labels.push(newDate(config.data.labels.length));

                for (var index = 0; index < config.data.datasets.length; ++index) {
                    if (typeof config.data.datasets[index].data[0] === 'object') {
                        config.data.datasets[index].data.push({
                            x: newDate(config.data.datasets[index].data.length),
                            y: randomScalingFactor(),
                        });
                    } else {
                        config.data.datasets[index].data.push(randomScalingFactor());
                    }
                }

                window.myLine.update();
            }
        });

        document.getElementById('removeDataset').addEventListener('click', function () {
            config.data.datasets.splice(0, 1);
            window.myLine.update();
        });

        document.getElementById('removeData').addEventListener('click', function () {
            config.data.labels.splice(-1, 1); // remove the label first

            config.data.datasets.forEach(function (dataset) {
                dataset.data.pop();
            });

            window.myLine.update();
        });
    </script>

</asp:Content>

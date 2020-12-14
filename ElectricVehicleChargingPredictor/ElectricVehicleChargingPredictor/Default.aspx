<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ElectricVehicleChargingPredictor._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Eesti Energia</h1>
        <p class="lead">Eesti Energia proposing the project for predicting the usage of the electric vehicle in Tartu.</p>
        <p><a href="https://www.energia.ee/et/avaleht" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Driving Behaviour Predictor</h2>
            <p>
                This one will predict about what is the driving behaviour from each of the user for the next 24 hour..
            </p>
            <p>
                <a class="btn btn-default" runat="server" href="~/Driving_Behaviour">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Grid Connection Predictor</h2>
            <p>
                This one will predict the behaviour of the user when they will charge the car or just put it standing for the next 24 hour..
            </p>
            <p>
                <a class="btn btn-default" runat="server" href="~/Grid_Connection">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>State of Charge Predictor</h2>
            <p>
                This one will predict the state of charge of the electric vehicle for the next 24 hour.
            </p>
            <p>
                <a class="btn btn-default" runat="server" href="~/State_of_Charge">Learn more &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>

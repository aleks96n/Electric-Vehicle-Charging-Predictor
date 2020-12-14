<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="ElectricVehicleChargingPredictor.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Background</h3>
    <p>
        <h4>
            Usage of electric vehicles is growing every day and soon we can see electric vehicles as a mainstream transport. 
            <br /><br />
            Therefore, we need to know and predict how electric vehicles behave to optimize the environment and usage of electricity. 
            <br /><br />
            Additionally, the batteries of electric vehicles can store green energy that can be used.
            <br /><br />
            Our application can predict the grid connection, state of charge, and driving behavior for the next 24 hours based on the data of 100 electric vehicles that were provided by Eesti Energia. 
        </h4>
    </p>
</asp:Content>

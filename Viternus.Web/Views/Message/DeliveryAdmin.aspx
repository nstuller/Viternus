<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Viternus - Delivery Administration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>DeliveryAdmin</h1>

<%= Ajax.ActionLink("Process Event Messages To Determine If They Should Be Sent", 
    "MarkEventMessagesForDelivery", "Message", new AjaxOptions { OnSuccess = "ShowMarked" })%>
    
    <div id="MarkedMsg">
        Mark completed
    </div>

<br />
<br />

<%= Ajax.ActionLink("Deliver All Messages That Are Ready", "DeliverOverdueMessages", 
    "Message", new AjaxOptions{OnSuccess="ShowDelivered"})%>
    
    <div id="DeliveredMsg">
        Delivery Completed
    </div>
    
    <br />
<br />

<%= Ajax.ActionLink("Deliver Outstanding Trustee Requests", "DeliverOutstandingRequests", 
    "InnerCircle", new AjaxOptions{OnSuccess="ShowDeliveredIC"})%>
    
    <div id="DeliveredIC">
        Delivery Completed
    </div>
    
    <script type="text/javascript">
        function ShowMarked() {
            $("#MarkedMsg").show("slow");
        }

        function ShowDelivered() {
            $("#DeliveredMsg").show();
        }

        function ShowDeliveredIC() {
            $("#DeliveredIC").show();
        }

        $("#MarkedMsg").hide();
        $("#DeliveredMsg").hide();
        $("#DeliveredIC").hide();
    </script>
</asp:Content>

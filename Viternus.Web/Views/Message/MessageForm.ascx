<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Viternus.Web.ViewModels.MessageEditViewModel>" %>
<%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.", new { style = "width:500px;background:#FFFFFF;" })%>
<% using (Html.BeginForm())
   {%>
<h3>
    Message Information</h3>
<ul>
    <li>
        <label for="Message Type">
            Message Type:</label>
        <%= Html.DropDownList("MessageType", Model.MessageTypes, new { onchange = "ToggleMessageType()" })%>
        <%= Html.ValidationMessage("MessageType", "*") %></li>
    <li>
        <label for="Video" id="VideoLabel">
            Video:</label>
        <%= Html.DropDownList("Video", Model.MyVideos) %>
        <%= Html.ValidationMessage("Video", "*")%></li>
            <li>
        <label for="Title">
            Title:</label>
        <%= Html.TextBox("Title", Model.Message.Title, new { maxlength = 70 })%>
        <%= Html.ValidationMessage("Title", "*") %>
    </li>
    <li>
        <label for="Comments">
            Comments:</label>
        <%= Html.TextArea("Comments", Model.Message.Comments)%>
        <%= Html.ValidationMessage("Comments", "*") %>
    </li>
    <li id="deliverByLi">
        <%= Html.RadioButton("deliverBy", "byDate", Model.Message.DeliverByDate,  new { onclick = "ShowByDate()"})%>
        <span>Deliver By Date </span>
        <% if (Model.User.UserIsAPaidUser)
           { %>
        <%= Html.RadioButton("deliverBy", "byEvent", Model.Message.DeliverByEvent, new { onclick = "ShowByEvent()" })%>
        <span>Deliver By Event </span>
        <% }
           else
           { %>
        <span><a href="#" id="enableMoreOptions" style="padding-left: 20px;" title="Your delivery options are limited because you have not upgraded to be a Premium user.">
            Learn how to enable more options</a></span>
        <% } %></li>
    <li id="byDateLi">
        <label for="ScheduleDate">
            Schedule Date:</label>
        <%= Html.TextBox("ScheduleDate", String.Format("{0:d}", Model.Message.ScheduleDate))%>
        <%= Html.ValidationMessage("ScheduleDate", "*") %>
    </li>
    <li id="byEventLi">
        <label for="EventType">
            Event Type:</label>
        <%= Html.DropDownList("EventType", Model.EventTypes) %>
        <%= Html.ValidationMessage("EventType", "*")%>
        <%= Html.TextBox("DeliveryOffset", Model.Message.EventScheduleOffsetDays, new {style = "float:right;width:30px"})%>
        <label for="DeliveryOffset" style="float: right;">
            Days to Wait:</label>
    </li>
    <li>
        <input type="submit" class="submit" value="Continue" /></li>
</ul>
<% } %>
<div id="dialog" title="Enabling More Event Types" style="display: none;">
    <p>
        Your delivery options are limited because you have not upgraded to be a Premium user.</p>
</div>

<script type="text/javascript">
    function ShowByDate() {
        $("#byDateLi").show();
        $("#byEventLi").hide();
    }
    function ShowByEvent() {
        $("#byDateLi").hide();
        $("#byEventLi").show();
    }
    function ToggleMessageType() {
        $("#VideoLabel").hide();
        $("#Video").hide();

        if ("Video" == $("#MessageType :selected")[0].innerHTML) {
            $("#Video").show();
            $("#VideoLabel").show();
        }
    }

    

    if ($("#deliverByLi input:radio:checked").val() == "byDate")
        ShowByDate();
    else if ($("#deliverByLi input:radio:checked").val() == "byEvent")
        ShowByEvent();

    $(function() {
        // Set up the appropriate controls to be visible on load
        ToggleMessageType();

        $("#ScheduleDate").datepicker({ minDate: 0 });
        $("#dialog").dialog({ autoOpen: false, bgiframe: true, width: 340, height: 200, modal: true });

        $("#enableMoreOptions").click(function() {
            $("#dialog").dialog('open');
        });

    });
</script>


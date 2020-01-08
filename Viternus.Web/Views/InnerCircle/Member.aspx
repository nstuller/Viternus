<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Web.ViewModels.InnerCircleMemberViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Member of other Viternus Users' Inner Circle
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="headingText">
        Member of Others' Inner Circles</h1>
    <div class="rightBox">
        <div class="rightBoxArea">
            <div class="tableArea">
                <%= Html.ValidationSummary("Your action was unsuccessful. Please correct the errors and try again.", new { style = "width:500px;background:#FFFFFF;" })%>
                <table width="735" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <th width="612">
                            Person Who Trusts You
                        </th>
                        <th width="65">
                            Accepted?
                        </th>
                        <th width="380">
                            <%--Action--%>
                        </th>
                    </tr>
                </table>
                <div id="accordion">
                    <% foreach (var item in Model.User.Profile.InnerCircle)
                       {%>
                    <h3>
                        <a href="#">
                            <%= Html.Encode(item.AppUser.Profile.FirstName)%>
                            <%= Html.Encode(item.AppUser.Profile.LastName)%></a>
                        <% using (Html.BeginForm("Accept", "InnerCircle", new { id = item.Id }, FormMethod.Post))
                           {%>
                        <% if (item.AcceptedResponsibility)
                           {%>
                        <%--Have to do it this way in MVC 1.0 so if it's already checked you can't uncheck it--%>
                        <%= Html.CheckBox("accepted", item.AcceptedResponsibility, new { disabled = "true", @float = "left"})%>
                        <% } %>
                        <% else
                            {%>
                        <%= Html.CheckBox("accepted", item.AcceptedResponsibility, new { onclick = "if(confirmAcceptance()) this.form.submit();", @float = "left" })%>
                        <% } %>
                        <% } %>
                    </h3>
                    <div>
                        <div class="leftPart">
                            <fb:profile-pic uid="<%= Html.Encode(item.AppUser.Profile.FacebookId) %>" facebook-logo="false">
                            </fb:profile-pic>
                            <p style="font-size: 9px; font-style: italic;">
                                <%= Html.Encode(item.StatusMessage) %></p>
                        </div>
                        <div class="rightPart">
                            <h4>
                                <% if (!(item.ExistingIncapacitationActionFromLoggedInUser && item.ExistingDeathActionFromLoggedInUser))
                                   {%>
                                Did Something Happen?
                                <%} %></h4>
                            <%   if (!item.ExistingIncapacitationActionFromLoggedInUser)
                                 {%>
                            <a href="#" class="incap" onclick="populateAndOpenDialog('<%= Html.Encode(item.Id)%>', '<%= Html.Encode(item.AppUser.Profile.FirstName)%>', '7d234f58-cf46-4091-8e63-b1fdc48d6804');">
                                Incapacitated</a><% } %>
                            <%--<br />--%>
                            <% if (!item.ExistingDeathActionFromLoggedInUser)
                               { %>
                            <a href="#" class="death" onclick="populateAndOpenDialog('<%= Html.Encode(item.Id)%>', '<%= Html.Encode(item.AppUser.Profile.FirstName)%>', 'bb1f5322-f956-457d-bbf0-9aeb3dc76333');">
                                Deceased</a><% } %>
                        </div>
                    </div>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
    <div id="lifeEventDialog" title="Something has happened" class="dialogForm">
        <% using (Html.BeginForm("LifeEvent", "InnerCircle", null, FormMethod.Post, new {id = "lifeEventForm"}))
           {%>
        <p>
            We are very sorry that something has happened to your friend.<br />
            <br />
            By clicking submit below, you are officially triggering Viternus to release
            <label id="firstNameLabel">
            </label>
            's messages. Please be sure that this is what you want to do.
        </p>
        <br />
        <%= Html.DropDownList("EventType", Model.EventTypes, new { disabled = "true" })%>
        <%--, new { disabled = "true" }--%>
        <%= Html.TextBox("EventDate", Html.Encode(String.Format("{0:d}", Model.DefaultDate)))%>
        <input type="hidden" name="innerCircleId" id="innerCircleId" value="" />
        <input type="submit" class="submit" value="Submit" />
        <% } %>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("table tr:nth-child(odd)").addClass("colorful");
        });

        $('#lifeEventForm').submit(function () {
            $('#EventType').removeAttr('disabled');            
        });

        $(function () {
            $("#lifeEventDialog").dialog({ autoOpen: false, bgiframe: true, width: 420, height: 330, modal: true });
            $("#accordion").accordion();
            $("#EventDate").datepicker({ maxDate: 0, defaultDate: 0 });

        });

        function populateAndOpenDialog(id, firstName, eventDesc) {
            $('#innerCircleId').val(id);
            $('#EventType').val(eventDesc);
            $('#firstNameLabel').html(firstName);
            $('#lifeEventDialog').dialog('open');
        }

        function confirmAcceptance() {
            var answer = confirm("Are you sure you want to accept this responsibility?  You cannot back out later.");
            return answer;
        }
    </script>
</asp:Content>

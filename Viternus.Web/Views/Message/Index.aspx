<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Viternus.Data.Message>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Viternus - View My Outgoing Messages
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="headingText">
        My Outgoing Messages</h1>
    <div class="rightBox">
        <div class="rightBoxArea">
            <div class="tableArea">
                <table width="735" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <th width="234">
                            <span>
                                <img src="../../Content/images/message-box.gif" alt="Message" />Title</span>
                        </th>
                        <th width="65">
                            Type
                        </th>
                        <th width="238">
                            Action
                        </th>
                        <th width="142">
                            Date
                        </th>
                    </tr>
                    <% foreach (var item in Model)
                       {%>
                    <tr>
                        <td>
                            <span>
                                <%= Html.Encode(item.Title)%>
                    </span> </td>
                    <td>
                        <%= Html.Encode(item.MessageType.Description)%>
                    </td>
                    <td>
                        <a href="/Message/Delete/<%= Html.Encode(item.Id)%>">
                            <img src="../../Content/images/delete.gif" alt="Delete" /></a> <a href="/Message/Details/<%= Html.Encode(item.Id)%>">
                                <img src="../../Content/images/preview.gif" alt="Preview" /></a> <a href="/Message/Edit/<%= Html.Encode(item.Id)%>">
                                    <img src="../../Content/images/edit.gif" alt="Edit" /></a>
                    </td>
                    <td>
                        <%= Html.Encode(String.Format("{0:d}", item.ScheduleDate))%>
                    </td>
                    </tr>
                    <% } %>
                </table>
                <a href="/Message/Create" class="roundedGreenLink">Create a New Message </a>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function() { $("table tr:nth-child(odd)").addClass("colorful"); });
    
    </script>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Web.ViewModels.MessageDisplayViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Viternus -- Preview 
    <%= Html.Encode(Model.Message.Title)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1>
                Details Of
                <%= Html.Encode(Model.Message.MessageType.Description)%>
                Message</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <% Html.RenderPartial("MessageDisplay"); %>
                        <li>
                            <ul>
                                <li>Will Be Delivered To:</li>
                                <% foreach (var msgReceiver in Model.Message.MessageRecipient)
                                   { %>
                                <li><a href="#">
                                    <%= Html.Encode(msgReceiver.Profile.FirstName)%>
                                    <%= Html.Encode(msgReceiver.Profile.LastName)%>
                                    <%= Html.Encode(string.Format("({0})", msgReceiver.Profile.Email))%>
                                </a>
                                <%= (null == msgReceiver.VisitedDate) ? "" : Html.Encode(String.Format("  : viewed {0:d}", msgReceiver.VisitedDate)) %>
                                </li>
                                <% } %>
                            </ul>
                        </li>
                        </ul> <span class="bottomLink">
                            <%=Html.ActionLink("Edit", "Edit", new { id = Model.Message.Id })%>
                            |
                            <%=Html.ActionLink("Delete", "Delete", new { id = Model.Message.Id })%>
                            |
                            <%=Html.ActionLink("Back to List", "Index") %></span>
                    </div>
                </div>
                <!-- End boxArea  -->
            </div>
            <!-- End box  -->
        </div>
        <!-- End contentArea  -->
    </div>
    <!-- End center  -->
    <% Html.RenderPartial("RightAd"); %>
</asp:Content>

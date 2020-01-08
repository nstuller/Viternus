<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Web.ViewModels.MessageRecipientDisplayViewModel>" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Html.Encode(Model.Message.Title)%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1>
                View
                <%= Html.Encode(Model.Message.MessageType.Description)%>
                Message</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <% Html.RenderPartial("MessageDisplay"); %>
                        <% if (!Model.RecipientAlreadyIsAUser)
                           { %>
                        <li>
                            <p>
                                <a href="<%= Url.Action("Register","Account", new { email = Model.MessageRecipient.Profile.Email, userName = Model.ProposedUserName}) %>"
                                class="freeTrialBtn">
                                    Free Sign Up
                                </a>
                            </p>
                        </li>
                        <% }%>
                        </ul>
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

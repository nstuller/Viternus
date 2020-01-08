<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Web.ViewModels.MessageEditViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Viternus - Edit Message
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Edit Undelivered Message</h1>
            <!-- Start box  -->
            <div class="box boxSection">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <% Html.RenderPartial("MessageForm"); %>
                        <span class="bottomLink"><%= Html.ActionLink("Back to List", "Index", "Message")%>
                        </span>
                    </div>
                </div>
                <!-- End boxArea  -->
            </div>
            <!-- End box  -->
        </div>
        <!-- End contentArea  -->
    </div>
    <!-- End center  -->
    <% Html.RenderPartial("MessageHelp"); %>
</asp:Content>


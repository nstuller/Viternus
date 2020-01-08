<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Viternus - Message Not Found
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Message Not Found</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <p>Sorry -- but the Message you requested does not exist or was deleted
                        <br /><br />
                        If you copied this link from your email or somewhere else, you should make sure you have the entire link selected and pasted into your browser's address bar.
                        </p>


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

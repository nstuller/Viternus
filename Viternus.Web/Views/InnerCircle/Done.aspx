<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Done Choosing the Viternus Inner Circle
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1>
                Congrats!</h1>
            <!-- Start box  -->
            <div class="box boxSection">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div style="min-height: 200px">
                        <p>
                            An e-mail has been sent to your Inner Circle letting them know of their responsibilities.</p>
                        <p>
                            You will now see more message creation options as in the below image</p>
                        <p>
                            <br />
                            <img alt="New Message Options" src="../../Content/images/screenshot.jpg" /></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <% Html.RenderPartial("RightProgress"); %>
</asp:Content>

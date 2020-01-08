<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Data.Video>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Viternus - Video Upload Pending
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Video Upload Pending</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <p>
                            Your video was successfully uploaded. We're just doing some final processing to
                            it before it will be ready. Feel free to continue using the site while you wait.<br /><br />
                            
                            We estimate your video will appear in your list in <%=Html.Encode(ViewData["EstimatedArrivalSpan"])%> minutes.  You should check your video list then.</p>
                        <span class="bottomLink">
                            <%=Html.ActionLink("Back to List", "Index") %></span></div>
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

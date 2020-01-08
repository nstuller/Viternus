<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Done
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Checkout Successful</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div style="min-height: 200px">
                        <h4>Congratulations!</h4>
                        <p>
                        <br />
                            Your credit card information has been submitted and your account has been upgraded.</p>
                        <p>
                            Please click continue below to set up your Inner Circle.
                        </p>
                        <span class="bottomLink">
                            <%=Html.ActionLink("Continue", "Redirect")%>
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
    <% Html.RenderPartial("RightProgress"); %>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Data.Message>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Viternus - Delete Message
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Delete Confirmation</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <% using (Html.BeginForm())
                           { %>
                        <p class="deleteConf">
                            Please confirm you want to cancel this message: <i>
                                <%=Html.Encode(Model.Comments) %>? </i>
                            <input name="confirmButton" type="submit" class="submit" value="Delete" />
                        </p>
                        <% } %>
                        <span class="bottomLink">
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

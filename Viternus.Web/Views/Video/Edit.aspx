<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Data.Video>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Viternus - Edit Video
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Edit Video</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div class="editVideo">
                        <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.", new { style = "width:500px;background:#FFFFFF;" })%>
                        <% using (Html.BeginForm())
                           {%>
                        <ul>
                            <li>
                                <label for="Description">
                                    Description:</label>
                                <p>
                                    <%= Html.TextBox("Description", Model.Description, new {maxlength="50"}) %>
                                    <%= Html.ValidationMessage("Description", "*") %>
                                </p>
                            </li>
                            <li>
                                <label for="CreationDate">
                                    Creation Date:</label>
                                <p>
                                    <%= Html.Encode(String.Format("{0:g}", Model.CreationDate))%>
                                </p>
                            </li>
                            <li>
                                <% Html.RenderPartial("VideoPlayer"); %>
                            </li>
                            <li>
                                <p>
                                    <input type="submit" class="submit" value="Save" />
                                </p>
                            </li>
                        </ul>
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

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Data.AppUser>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Viternus - Edit Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Edit Account Settings</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div class="accountInfo">
                        <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.", new { style = "width:500px;background:#FFFFFF;" })%>
                        <% using (Html.BeginForm())
                           {%>
                        <ul>
                        <li>
                        <label id="requiredExplanation" >
                                    * means required</label>
                        </li>
                            <li>
                                <label for="FirstName">
                                    First Name:</label><p>
                                        <%= Html.TextBox("FirstName", Model.Profile.FirstName) %>
                                        <%= Html.ValidationMessage("FirstName", "*") %>
                                    </p>
                            </li>
                            <li>
                                <label for="LastName">
                                    Last Name:</label><p>
                                        <%= Html.TextBox("LastName", Model.Profile.LastName)%>
                                        <%= Html.ValidationMessage("LastName", "*") %>
                                    </p>
                            </li>
                            <li>
                                <label for="NickName">
                                    Nick Name:</label><p>
                                        <%= Html.TextBox("NickName", Model.Profile.NickName)%>
                                        <%= Html.ValidationMessage("NickName", "*") %>
                                    </p>
                            </li>
                            <li>
                                <label for="Email">
                                    Email*:</label><p>
                                        <%= Html.TextBox("Email", Model.Profile.Email)%>
                                        <%= Html.ValidationMessage("Email", "*") %>
                                    </p>
                            </li>
                            <li>
                                <label for="EmailAlternate">
                                    Alternate Email:</label><p>
                                        <%= Html.TextBox("EmailAlternate", Model.Profile.EmailAlternate)%>
                                        <%= Html.ValidationMessage("EmailAlternate", "*") %>
                                    </p>
                            </li>
                            <%--<li>
                                <label for="SSN">
                                    Social Security:</label><p>
                                        <%= Html.TextBox("SSN", Model.Profile.SSN)%>
                                        <%= Html.ValidationMessage("SSN", "*") %>
                                    </p>
                                    <p style="margin-top:0px;">(Please, just use numbers and dashes in your SSN)</p>
                            </li>--%>
                            <% if (Context.User.IsInRole("Admin"))
                               { %>
                            <li>
                                <label for="DeceasedDate">
                                    Deceased Date:</label><p>
                                        <%= Html.Encode(Model.Profile.DeceasedDate)%>
                                    </p>
                            </li>
                            <% } %>
                            <li>
                                <label>
                                    Member Since:</label>
                                <p>
                                    <%= Html.Encode(Model.CreationDate)%>
                                </p>
                            </li>
                            <li>
                                <p>
                                    <input type="submit" class="submit" value="Save" />
                                </p>
                            </li>
                            </ul>
                            <% } %>
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

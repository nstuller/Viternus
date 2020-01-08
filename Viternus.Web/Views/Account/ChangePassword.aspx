<%@ Page Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Viternus - Change Password
</asp:Content>
<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Change Password</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <p>
                            Use the form below to change your password.</p>
                        <p>
                            New passwords are required to be a minimum of
                            <%=Html.Encode(ViewData["PasswordLength"])%>
                            characters in length with at least <%=Html.Encode(ViewData["MinRequiredNonAlphanumericCharacters"])%> special character.
                            <a href="#" id="explainPassword"  title="Some examples of special characters are ! # $ % & ( ) * + , - . / : ; < = > ? @ [ \ ] ^ _ { | } '">
            Tell me more.</a>
                        </p>
                        <p>
                            <%= Html.ValidationSummary("Password change was unsuccessful. Please correct the errors and try again.", new { style = "width:500px;background:#FFFFFF;" })%></p>
                        <h4>
                            Account Information</h4>
                        <% using (Html.BeginForm())
                           { %>
                        <ul class="password">
                        <li>
                        <label id="requiredExplanation" >
                                    * means required</label>
                        </li>
                            <li>
                                <label for="currentPassword">
                                    Current Password*:</label>
                                <%= Html.Password("currentPassword") %>
                                <%= Html.ValidationMessage("currentPassword", "*")%></li>
                            <li>
                                <label for="newPassword">
                                    New Password*:</label>
                                <%= Html.Password("newPassword") %>
                                <%= Html.ValidationMessage("newPassword", "*")%></li>
                            <li>
                                <label for="confirmPassword">
                                    Confirm New Password*:</label>
                                <%= Html.Password("confirmPassword") %>
                                <%= Html.ValidationMessage("confirmPassword", "*")%></li>
                            <li>
                                <input type="submit" class="submit" value="Change Password" /></li>
                        </ul>
                        <% } %>
                        <% Html.RenderPartial("PasswordStrengthDialog"); %>
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

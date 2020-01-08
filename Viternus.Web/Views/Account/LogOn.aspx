<%@ Page Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Log On to Leave Your Legacy Online with Viternus
</asp:Content>
<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <% if (Request.IsAuthenticated)
               { %>
            <h1 class="heading">
                Permissions Error</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div class="logon">
                        <p>
                            You are not authorized to perform this action.</p>
                    </div>
                </div>
            </div>
            <% }
               else
               { %>
            <h1 class="heading">
                Log On</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div class="logon">
                        <%= Html.ValidationSummary("Login was unsuccessful.  Please correct the errors and try again.", new {style="width:500px;background:#FFFFFF;" })%>
                        <p>
                            Please enter your username and password.
                            <%= Html.ActionLink("Register", "Register")%>
                            if you don't have an account.
                        </p>
                        <% using (Html.BeginForm())
                           { %>
                        <p>
                            <label for="username">
                                Username:</label>
                            <%= Html.TextBox("username")%>
                        </p>
                        <p>
                            <label for="password">
                                Password:</label>
                            <%= Html.Password("password")%>
                        </p>
                        <p>
                            <%= Html.CheckBox("rememberMe")%>
                            <label for="rememberMe">
                                Remember me?</label>
                        </p>
                        <p>
                            <input type="submit" class="submit" value="Log On" />
                        </p>
                        <p style="padding-top: 20px;">
                            - You Can Also Use Facebook -</p>
                        <p>
                            <fb:login-button id="fbLoginButton" v="2" size="medium" onlogin="facebookLoginReturnAndSubmit();">Login with Facebook</fb:login-button>
                            <input type="hidden" name="facebookId" id="facebookId" value="" /></p>
                        <% } %>
                    </div>
                </div>
                <!-- End boxArea  -->
            </div>
            <!-- End box  -->
            <% } %>
        </div>
        <!-- End contentArea  -->
    </div>
    <% Html.RenderPartial("RightAd"); %>
    <!-- End center -->
    <script type="text/javascript">
        /*FB.Facebook.get_sessionState().waitUntilReady(function(){ 
        FB.Connect.ifUserConnected(facebook_onlogin());  }); */
        function facebookLoginReturnAndSubmit() {
            $("#facebookId").val(FB.Connect.get_loggedInUser());
            $("form").submit();
        } 
    </script>
</asp:Content>

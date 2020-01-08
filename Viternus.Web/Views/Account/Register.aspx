<%@ Page Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Web.ViewModels.AccountRegisterViewModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Register to Leave Your Legacy Online with Viternus
</asp:Content>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Create a New Account</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div class="register" id="accountCreation" style="min-height: 200px">
                        <h4>
                            Fill in the Information Below
                        </h4>
                        <%= Html.ValidationSummary("Account creation was unsuccessful. Please correct the errors and try again.", new { id = "validationElement", style="width:500px;background:#FFFFFF;"  })%>
                        <br />
                        <% using (Html.BeginForm())
                           { %>
                        <ul class="contactList rightBorder" id="fieldList">
                            <li>
                                <label id="requiredExplanation">
                                    * means required</label>
                            </li>
                            <li id="usernameLi">
                                <label for="username">
                                    Username*:</label>
                                <%= Html.TextBox("username", Html.Encode(Model.UserName), new { onfocus = "javascript:changePosition('u'); return false;" })%>
                                <%= Html.ValidationMessage("username", "*")%>
                            </li>
                            <li>
                                <label for="email">
                                    Email*:</label>
                                <%= Html.TextBox("email", Html.Encode(Model.EmailAddress), new { onfocus = "javascript:changePosition('e'); return false;" })%>
                                <%= Html.ValidationMessage("email", "*")%>
                            </li>
                            <li id="passwordInstructions"><span class="listHeading">Password Requirements: <a tabindex="-1" 
                                href="#" id="explainPassword" title="Some examples of special characters are ! # $ % & ( ) * + , - . / : ; < = > ? @ [ \ ] ^ _ { | } '">
                                (?)</a> </span>
                                <ul>
                                    <li class="bulletedList">
                                        <%=Html.Encode(Model.PasswordLength)%>
                                        characters minimum</li>
                                    <li class="bulletedList">
                                        <%=Html.Encode(Model.MinRequiredNonAlphanumericCharacters)%>
                                        special character</li>
                                </ul>
                            </li>
                            <li id="newPasswordLi">
                                <label for="newPassword">
                                    Password*:</label>
                                <%= Html.Password("newPassword") %>
                                <%= Html.ValidationMessage("newPassword", "*")%>
                            </li>
                            <li id="confirmPasswordLi">
                                <label for="confirmPassword">
                                    Confirm Password*:</label>
                                <%= Html.Password("confirmPassword") %>
                                <%= Html.ValidationMessage("confirmPassword", "*")%>
                            </li>
                        </ul>
                        <div id="preFacebookConnect" class="boxPart" style="overflow: hidden;">
                            <div class="boxContent">
                                <div class="boxBg">
                                    <span>Hate having to create accounts everywhere?<br />
                                        <br />
                                        By using Facebook Connect, you can use your Facebook username &amp; password to
                                        login to Viternus.<br />
                                        <br />
                                        Get started by clicking the button below.<br />
                                        <br />
                                    </span>
                                    <fb:login-button v="2" id="fbLoginButton" size="medium" onlogin="facebookLoginReturn();"
                                        style="padding-left: 36px">Connect with Facebook</fb:login-button>
                                </div>
                            </div>
                        </div>
                        <input type="hidden" name="facebookId" id="facebookId" value="" />
                        <%--TODO: I know the CSS in here is gnarly (refactor it)--%>
                        <p style="float:left;" >
                            <br />
                            <label style="width:74px;float:left;">&nbsp;</label>
                            <%= Html.CheckBox("agreedToTOS") %>

                            <label for="agreedToTOS" >
                                I've read & agree with the <a href="/Home/TermsOfService" target="_blank" tabindex="-1">
                                    Terms of Service</a></label>
                            <%= Html.ValidationMessage("agreedToTOS", "*")%>
                            <br />
                            
                            <label style="width:74px;float:left;">&nbsp;</label>
                            <input type="submit" class="submit" value="Register" />
                        </p>
                        <% } %>
                        <% Html.RenderPartial("PasswordStrengthDialog"); %>
                    </div>
                </div>
                <!-- End boxArea  -->
            </div>
            <!-- End box  -->
        </div>
        <!-- End contentArea  -->
        <div class="toolTip" id="fToolTips">
            <div class="toolArea">
                <div class="toolBg">
                    <h2>
                        Facebook Authentication Successful!</h2>
                    <p>
                        Now just enter a UserName and Email and click Continue.</p>
                    <p id="uName" style="font-weight: bold;">
                        Your username will be how the site identifies you.<br />
                        We recommend using the first letter of your first name along with your last name.
                        (e.g. "FFlinstone")</p>
                    <p id="uEmail" style="display: none; font-weight: bold;">
                        Please use the email address at which you would like to receive Viternus messages.</p>
                </div>
            </div>
        </div>
    </div>
    <!-- End center  -->

    <script type="text/javascript">
        //        FB.Facebook.get_sessionState().waitUntilReady(function() {
        //            FB.Connect.ifUserConnected(facebook_onlogin());
        //        });

        var pos;
        function changePosition(f) {
            try {
                if ($("#fToolTips")) {
                    if (f == 'u') {
                        $('#fToolTips').css({ top: pos.top + 62 + 'px' });
                        $('#uEmail').hide();
                        $('#uName').show();
                    }
                    else {
                        $('#fToolTips').css({ top: pos.top + 102 + 'px' });
                        $('#uEmail').show();
                        $('#uName').hide();
                    }
                }
            }
            catch (e) { };
        }

        function facebookLoginReturn() {
            $("#facebookId").val(FB.Connect.get_loggedInUser());
            $("#preFacebookConnect").hide();
            $("#fbLoginButton").hide();
            $("#passwordInstructions").hide();
            $("#newPasswordLi").hide();
            $("#confirmPasswordLi").hide();
            $("#validationElement, .validation-summary-errors").hide();

            $("#fToolTips").show();
            pos = $("#accountCreation").position();
            $('#fToolTips').css({ left: pos.left + 313 + 'px' });
            $('#fToolTips').css({ top: pos.top + 62 + 'px' });
        }


    </script>

</asp:Content>

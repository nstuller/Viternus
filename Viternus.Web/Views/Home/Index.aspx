<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Viternus.Data.Message>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Store Farewell Messages and Condolences for loved ones - Viternus
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start main  -->
    <div class="main">
        <div class="contentSection">
            <h1>
                Welcome to Viternus</h1>
            <p>
                Permanently store farewell messages for your loved ones long after you have passed
                by uploading videos and notes. Viternus will automatically deliver them when the
                time is right.
            </p>
            <ul>
                <li>- Upload & Store Messages</li>
                <li>- Set Delivery Time</li>
                <li>- Connect With loved ones</li>
            </ul>
            <p>
                <br />
                Simply put, Viternus is a way to <i>leave your legacy online</i>, a way to live on perpetually.
            </p>
            <div class="galleryArea">
                <h3>
                    <span><a href="/Message/ViewMessageFromUrl/014cd4f7-b947-4bb9-8914-e32e40c2dd98">Take a
                        Tour</a></span>
                    <%--How Do I Do It? -- This should go on Dashboard--%>
                    or<b><a href="/Account/Register">Signup Now</a></b></h3>
                <span class="preview"><a href="/Message/ViewMessageFromUrl/014cd4f7-b947-4bb9-8914-e32e40c2dd98">
                    <img src="../../Content/images/elderlyCoupleLaughing.png" alt="Loved Ones" /></a> <strong><span>Store
                        videos online. Here are some image samples.
                        <br />
                        <b>Donated to Home Page By - </b><a href="#">Viternus</a></span></strong></span>
                <span class="sample"><a href="/Message/ViewMessageFromUrl/014cd4f7-b947-4bb9-8914-e32e40c2dd98">
                    <img src="../../Content/images/sample.jpg" alt="Farewell Message Sample" /></a><a href="/Message/ViewMessageFromUrl/014cd4f7-b947-4bb9-8914-e32e40c2dd98"><img
                        src="../../Content/images/sample2.jpg" alt="Condolence Message Sample" /></a><a href="/Message/ViewMessageFromUrl/014cd4f7-b947-4bb9-8914-e32e40c2dd98"><img
                            src="../../Content/images/sample3.jpg" alt="Posthumous Message" /></a><a href="/Message/ViewMessageFromUrl/014cd4f7-b947-4bb9-8914-e32e40c2dd98"><img
                                src="../../Content/images/sample4.jpg" alt="Social Experience Image" /></a><a href="/Message/ViewMessageFromUrl/014cd4f7-b947-4bb9-8914-e32e40c2dd98"><img
                                    src="../../Content/images/sample5.jpg" alt="Leave Your Legacy Online" /></a></span>
            </div>
        </div>
    </div>
    <!-- End main  -->
    <!-- Start side  -->
    <div class="side">
        <div class="leftSide">
            <div class="leftSideContent">
                <h2>
                    What's Inside Viternus
                    <img src="../../Content/images/calendar.gif" alt="Calendar" /></h2>
                <p>
                    Secure. Automatic. On-line Message Delivery.</p>
                <dl>
                    <dd>
                        <span>
                            <img src="../../Content/images/clock.gif" alt="Posthumous Messages" /></span><b>Send Time-Delayed
                                or Posthumous Messages to Loved Ones</b></dd>
                    <dd>
                        <span>
                            <img src="../../Content/images/page.gif" alt="Store Video" /></span><b>Store Videos of Yourself
                                for Future Generations to See</b></dd>
                    <dd>
                        <span>
                            <img src="../../Content/images/star.gif" alt="Unfettered Message" /></span><b>Automatic Delivery
                                of Your Unfettered Message</b></dd>
                    <dd>
                        <span>
                            <img src="../../Content/images/watch.gif" alt="Video Wills" /></span><b>Host Video Wills</b>
                    </dd>
                    <dd>
                        <span>
                            <img src="../../Content/images/all-message.gif" alt="Private Messages" /></span><b>All Messages
                                Are Private until Delivered</b>
                    </dd>
                    <dd class="bottom">
                        <span>
                            <img src="../../Content/images/security.gif" alt="Secure Storage" /></span><b>Securely Store
                                Profile and Video Files</b>
                    </dd>
                </dl>
                <a href="/Account/Register" class="join">Join Viternus Today!!!</a>
            </div>
        </div>
        <div class="rightSide">
            <div class="login">
                <% using (Html.BeginForm("LogOn", "Account"))
                   { %>
                <div>
                    <fieldset>
                        <h2>
                            Login Here
                        </h2>
                        <ul>
                            <li>
                                <label for="username">
                                    Username:</label><%= Html.TextBox("username")%></li>
                            <li>
                                <label for="password">
                                    Password:</label><%= Html.Password("password")%></li>
                            <li>
                                <label>
                                    &nbsp;</label>
                                <input type="checkbox" class="check" name="rememberMe" id="rememberMe" value="true" /><input
                                    name="rememberMe" type="hidden" value="false" />
                                <b>Remember me</b><input type="submit" class="submit" value="Log On" /></li>
                            <li>
                                <p>
                                    - You Can Also Use Facebook -</p>
                            </li>
                            <li>
                                <label>
                                    &nbsp;</label><fb:login-button id="fbLoginButton" v="2" size="medium" onlogin="facebookLoginReturnAndSubmit();">Login with Facebook</fb:login-button>
                                <input type="hidden" name="facebookId" id="facebookId" value="" /></li>
                        </ul>
                    </fieldset>
                </div>
                <% } %>
            </div>
            <div class="rightContent">
                <div id="twitter_div">
                    <h2 class="twitter-title">
                        Twitter Updates</h2>
                    <ul id="twitter_update_list">
                    </ul>
                    <a href="http://twitter.com/Viternus" id="twitter-link">follow us on Twitter</a>
                </div>

                <script type="text/javascript" src="http://twitter.com/javascripts/blogger.js"></script>

                <script type="text/javascript" src="http://twitter.com/statuses/user_timeline/Viternus.json?callback=twitterCallback2&amp;count=3"></script>

            </div>
        </div>
    </div>
    <!-- End side -->

    <script type="text/javascript">
        //        FB.Facebook.get_sessionState().waitUntilReady(function() {
        //            FB.Connect.ifUserConnected(facebookLoginReturnAndSubmit());
        //        });

        //        $("#fbLoginButton").hide();

        //        FB_RequireFeatures(["XFBML"], function() {
        //            FB.Facebook.get_sessionState().waitUntilReady(function() {
        //                $("#fbLoginButton").show();
        //            });
        //        });

        //        FB.ensureInit(function() {
        //            $("#fbLoginButton").show();
        //        });

        function facebookLoginReturnAndSubmit() {
            $("#facebookId").val(FB.Connect.get_loggedInUser());

            $("form").submit();
        }
    </script>

</asp:Content>

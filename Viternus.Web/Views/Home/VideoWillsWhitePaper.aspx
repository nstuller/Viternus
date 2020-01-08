<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta name="description" content="Free White Paper Giveaway: Video Wills" />
    <title>White Paper - Six Reasons Why Video Wills Are Superior to Paper</title>
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" media="interactive, braille, emboss, handheld, projection, screen, tty, tv" />
    <link rel="shortcut icon" href="../../Content/images/footer-logo.jpg" />
</head>
<body id="Body1" runat="server">
    <!-- Start wrapper  -->
    <div id="wrapper">
        <!-- Start header  -->
        <div id="header">
            <a href="#" title="Viternus Home">
                <img src="../../Content/images/logoBlue.jpg" alt="Viternus Logo" /></a> <span class="tagline">
                    Your legacy, on your schedule</span>
        </div>
        <!-- Start mainBody  -->
        <div id="mainBody">
            <div class="left">
            </div>
            <div class="right">
                <!-- Start center  -->
                <div class="center">
                    <!-- Start contentArea  -->
                    <div class="contentArea">
                        <h1 class="heading">
                            Free White Paper Giveaway</h1>
                        <!-- Start box  -->
                        <div class="box">
                            <!-- Start box Area  -->
                            <div class="boxArea">
                                <div style="width: 600px;">
                                    <p>
                                        Creating a <b>video will</b> is a great idea that can have tremendous benefits.
                                        Submit your email below to read the "Six Reasons Why Video Wills Are Superior to
                                        Paper." Included in our 9 page guide are additional legal tips to ensure that your wishes are upheld. 
                                    </p>
                                    <p>
                                        <br />
                                        <b>Contents: <span style="font-style: italic; color: #e7473d; padding-left: 6px;">Six
                                            Reasons Why Video Wills Are Superior to Paper</span></b><br />
                                        <br />
                                    </p>
                                    <% using (Html.BeginForm())
                                       { %>
                                    <ul class="contactList">
                                        <li>
                                            <label for="Email">
                                                Email:</label><%= Html.TextBox("Email", null, new { maxlength = 100 })%></li>
                                        <li>
                                            <input type="submit" class="submit" value="Submit" /></li>
                                    </ul>
                                    <% } %>
                                    <p style="color: #ea493f;">
                                        <%= Model ?? "<br/><br/><br /><br />"%>
                                        <br />
                                    </p>
                                    <p style="font-size: 9pt">
                                        <br />
                                        By signing up above with your e-mail address, you will receive the electronic version
                                        (.pdf) of your White Paper. We will use your e-mail solely to provide you with news
                                        & notifications about future Viternus services. We will never share or sell your
                                        information and you can unsubscribe at any time in the future.
                                    </p>
                                </div>
                            </div>
                            <!-- End boxArea  -->
                        </div>
                        <!-- End box  -->
                    </div>
                    <!-- End contentArea  -->
                </div>
                <!-- End center  -->
            </div>
        </div>
        <!-- End mainBody  -->
        <!-- Start footer  -->
        <div class="footer">
            <div>
                <p>
                    <span>&copy; 2009 Software Sourcery, LLC</span></p>
                <img src="../../Content/images/footer-logo.jpg" alt="Viternus" />
            </div>

            <script type="text/javascript">
                var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
                document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
            </script>

            <script type="text/javascript">
                try {
                    var pageTracker = _gat._getTracker("UA-11843587-1");
                    pageTracker._trackPageview();
                } catch (err) { }</script>

        </div>
        <!-- End footer  -->
    </div>
    <!-- End wrapper  -->
</body>
</html>

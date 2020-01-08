<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Explaining the Viternus Inner Circle
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Inner Circle -- Explained</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <h4>
                            Why Upgrade to Premium?</h4>
                        <p>
                            <br />
                            Do you want your messages delivered after you are not able to deliver them yourself?
                            With an upgrade to [Premium] level, Viternus can make sure your message is delivered
                            automatically if something happens to you.
                            <br />
                            <br />
                        </p>
                        <h4>
                            What Next?</h4>
                        <p>
                            <br />
                            A [Premium] subscription is just $29.99 Per Year and you can cancel at any time.
                            This allows you to setup your Inner Circle, which is a collection of people you
                            trust to notify Viternus when a crucial life event happens to you.
                            <br />
                            <br />
                            Just click "Upgrade Me!" below to send us an email to upgrade your account. We will
                            respond immediately with instructions on how to securely pay for a [Premium] subscription.<br />
                            <br />
                            <a href="#" id="explainInnerCircle" style="float: right;">More</a>
                        </p>
                        <span class="bottomLink"><a href="mailto:hello@viternus.com?subject=I would like to upgrade my Viternus account">
                            Upgrade Me!</a>
                            <%--<%=Html.ActionLink("Upgrade Me!", "Card", "Payment")%>--%>
                        </span>
                    </div>
                    <div id="explanationDialog" title="Inner Circle Details" style="display: none; text-align: left;">
                        <p>
                            Your Inner Circle is a group of trusted contacts (Trustees) who will update Viternus
                            if a major life event occurs, such as your incapacitation or death. You choose the
                            member(s) of your Inner Circle and they will be notified of their responsibilities
                            as soon as they are chosen.
                            <br />
                            <br />
                            Your Inner Circle will not be able to gain access to your account. However, they
                            will be able to release your messages in the way you wanted.
                            <br />
                            <br />
                            Make sure these are the right people. We do not recommend changing them later.
                            <br />
                            <br />
                            It does not cost anything from the people you name to be in your Inner Circle.
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
    <script type="text/javascript">
        $(function () {
            $("#explanationDialog").dialog({ autoOpen: false, bgiframe: true, width: 540, height: 600, modal: true });

            $("#explainInnerCircle").click(function () {
                $("#explanationDialog").dialog('open');
            });
        });
    </script>
    <% Html.RenderPartial("RightProgress"); %>
</asp:Content>

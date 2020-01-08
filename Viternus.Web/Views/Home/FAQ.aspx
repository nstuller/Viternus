<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Vita Eternus, Farewell Messages and your Inner Circle - Viternus FAQ
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Frequently Asked Questions</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <h4>
                            What does Viternus mean?
                        </h4>
                        <p><br />
                            Stemming from the latin words <i><b>vita</b> ("life")</i> and <i><b>eternus</b> ("eternal,
                                everlasting, or without end"),</i> Viternus will store your notes or multimedia
                            artifacts online, perpetually. Generations to come can be influenced by your message.
                            <br /><br />
                        </p>
                        <h4>
                            What are some examples for its use?
                        </h4>
                        <p><br />
                            Anniversaries, Birthdays & Wedding Wishes: create a message ahead of time to be
                            delivered on the specified date.<br />
                            <br />
                            Farewell messages, Condolence messages & Time Capsules: store a message until a
                            critical life event, such as becoming incapacitated or passing away.<br /><br />
                        </p>
                        <h4>
                            Can Viternus be used to host Video Wills online?
                        </h4>
                        <p><br />
                            Absolutely! But there are many things to consider.
                            <br />
                            <br />
                            Do not worry, we have a guide to help you with this that you can find here in our
                            free whitepaper:<br />
                            <a href="/Home/VideoWillsWhitePaper">6 Reasons Why Video Wills Are Superior to Paper</a><br /><br />
                        </p>
                        <%--<h4>
                            Is it secure?
                        </h4>
                        <p>
                        </p>--%>
                        <%--<h4>
                            How does Viternus know when someone has died?
                        </h4>
                        <p>
                           Your Inner Circle will notify Viternus.
                        </p>--%>
                        <%--<h4>
                            What is an Inner Circle?
                        </h4>
                        <p>
                        Your Inner Circle is a group of trusted contacts (or Trustees) who will update Viternus if a major life event occurs, such as your incapacitation or death.  You choose the members of your Inner Circle and they will be notified of their responsibilities as soon as they are chosen.<br/><br/>
                        Your Inner Circle will not be able to gain access to your account. However, they will be able to release your messages in the way you wanted.
                        </p>--%>
                        <%--Are there actual questions from users that should go on here?--%>
                        <h4>
                            What is Silverlight?
                        </h4>
                        <p><br />
                            Silverlight is a small install by Microsoft to allow video streaming. It is similar
                            to 'Flash,' which is what you have installed if you ever watch videos on YouTube.<br />
                            <br />
                            If you have ever watched the Olympics live on the Internet or streamed NetFlix movies
                            to your computer, then you already have Silverlight. Otherwise, you can download
                            it by clicking <a href="http://go.microsoft.com/fwlink/?LinkID=149156">here</a>.<br /><br />
                        </p>
                        <h4>
                            How do I make a video?
                        </h4>
                        <p><br />
                            Currently, Viternus only allows you to upload a video you have already made. The
                            best way to do this is through your webcam or by using Windows Movie Maker (on Windows)
                            or iMovie (on Macintosh). We are constantly taking feedback and working to improve
                            our website to enable easy video creation and upload.<br /><br />
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
    <% Html.RenderPartial("RightAd"); %>
</asp:Content>

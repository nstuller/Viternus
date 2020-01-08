<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Subscribe to Notifications of Viternus Updates
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start left  -->
    <div class="left">
    </div>
    <div class="right">
        <!-- Start center  -->
        <div class="center">
            <!-- Start contentArea  -->
            <div class="contentArea">
                <h1 class="heading">
                    Be the first to be notified of Viternus updates</h1>
                <!-- Start box  -->
                <div class="box">
                    <!-- Start box Area  -->
                    <div class="boxArea">
                        <div>
                            <p>
                                Viternus is continually refining this website and adding new features.
                                <br />
                                <br />
                                If you would like to be notified when we come out with something important, you
                                can give us your name and e-mail address and we will keep you informed with major
                                milestones and updates.
                                <br />
                                <br />
                                (We'll never share or sell your information)
                            </p>
                            <% using (Html.BeginForm())
                               { %>
                            <ul class="contactList">
                                <li>
                                    <label for="Name">
                                        Name:</label>
                                    <%= Html.TextBox("Name", null, new { maxlength = 256 })%>
                                </li>
                                <li>
                                    <label for="Email">
                                        Email:</label><%= Html.TextBox("Email", null, new { maxlength = 100 })%></li>
                                <li>
                                    <label for="Message">
                                        Comments:</label><textarea rows="12" cols="9" name="Message" id="Message" maxlength="500"></textarea></li>
                                <li>
                                    <input type="submit" class="submit" value="Submit" /></li>
                            </ul>
                            <% } %>
                        </div>
                        <span class="contactResponse">
                            <%= Html.Encode(Model ?? String.Empty) %></span>
                    </div>
                    <!-- End boxArea  -->
                </div>
                <!-- End box  -->
            </div>
            <!-- End contentArea  -->
        </div>
        <!-- End center  -->
    </div>
</asp:Content>

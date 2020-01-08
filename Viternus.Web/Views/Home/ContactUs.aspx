<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Condolence Messages and Memorial Verses - Contact Viternus HQ
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Contact Us</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <h4>Feedback Welcome</h4>
                        <p>
                            <br />
                            We are interested in what you think about Viternus.
                        </p>
                        <p>
                            Drop us a line to say 'Hello', tell us about an issue, or just spread the word on how you like to use the site.</p>
                        <% using (Html.BeginForm())
                           { %>
                        <ul class="contactList rightBorder">
                            <li>
                                <label for="Name">
                                    Name:</label>
                                <%= Html.TextBox("Name", Html.Encode(Page.User.Identity.Name))%>
                            </li>
                            <li>
                                <label for="Email">
                                    Email:</label><%= Html.TextBox("Email")%></li>
                            <li>
                                <label for="Message">
                                    Message:</label><textarea rows="12" cols="9" name="Message" id="Message"></textarea></li>
                            <li>
                                <input type="submit" class="submit" value="Submit" /></li>
                        </ul>
                        <% } %>
                        
                        <h6 class="title">
                            Viternus</h6>
                        <span class="definition">
                            Cleveland, Ohio, U.S.A<br />
                            <%--Lorem Ipsum Street<br />
                            Dolor Sit Amet Avenue<br />--%>
                            Email: <a href="mailto:hello@viternus.com">hello@viternus.com</a></span>
                    </div>
                    <span class="contactResponse"><%= Html.Encode(Model ?? String.Empty) %></span>
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

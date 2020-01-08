<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Data.Message>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Viternus - Add Message Recipients
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1>
                To Whom Would You Like to Deliver this Message?</h1>
            <!-- Start box  -->
            <div class="box boxSection">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <% using (Html.BeginForm())
                           {%>
                           <h3>
                            Recipient Information</h3>
                        <% Html.RenderPartial("UserSearchForm"); %>
                        <% } %>
                        <dl>
                            <% foreach (var recipient in Model.MessageRecipient)
                               { %>
                            <% using (Html.BeginForm("DeleteMessageRecipient", "Message", new { id = recipient.Id }, FormMethod.Post))
                               {%>
                            <dd>
                                <span><strong>
                                    <%= Html.Encode(null == recipient.Id ? "New Contact" : recipient.Profile.NickName) %><br />
                                </strong>
                                    <%= Html.Encode(recipient.Profile.FirstName) %>
                                    <%= Html.Encode(recipient.Profile.LastName) %>
                                    <a href="mailto:<%= Html.Encode(recipient.Profile.Email) %>">
                                        <%= Html.Encode(recipient.Profile.Email) %></a><a href="mailto:<%= Html.Encode(recipient.Profile.EmailAlternate) %>"><%= Html.Encode(recipient.Profile.EmailAlternate) %></a></span>
                                <input type="submit" class="submit" value="_" style="color: White"><span style="margin-right: 150px;
                                    float: right;">
                                    <fb:profile-pic uid="<%= Html.Encode(recipient.Profile.FacebookId) %>" facebook-logo="false">
                                    </fb:profile-pic></span>
                            </dd>
                            <%   } %>
                            <% } %>
                        </dl>
                        <span class="bottomLink" style="margin-top: 18px">
                            <%=Html.ActionLink("Done - Back to List", "Index") %></span>
                    </div>
                </div>
                <!-- End boxArea  -->
            </div>
            <!-- End box  -->
        </div>
        <!-- End cotentArea  -->
    </div>
    <!-- End center  -->
    <% Html.RenderPartial("RightAd"); %>
</asp:Content>

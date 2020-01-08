<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Web.ViewModels.InnerCircleChooseViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Inner Circle List
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1>
                Inner Circle</h1>
            <!-- Start box  -->
            <div class="box boxSection">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div >
                        <p>
                            Choose
                            <%= Html.Encode(Model.NumberOfTrustees) %>
                            Individual(s)</p>
                        <h4>
                            Add Someone to your Inner Circle</h4>
                        <%= Html.ValidationSummary("Error.", new { id = "validationElement", style="width:500px;background:#FFFFFF;"  })%>
                        <% using (Html.BeginForm())
                           {%>
                        <% Html.RenderPartial("UserSearchForm"); %>
                        <% } %>
                        <dl>
                            <% foreach (var trustee in Model.User.InnerCircle)
                               { %>
                            <% using (Html.BeginForm("Delete", "InnerCircle", new { id = trustee.Id }, FormMethod.Post))
                               {%>
                            <dd>
                                <span><strong>
                                    <%= Html.Encode(null == trustee.Id ? "New User" : trustee.Profile.NickName)%><br />
                                </strong>
                                    <%= Html.Encode(trustee.Profile.FirstName)%>
                                    <%= Html.Encode(trustee.Profile.LastName)%>
                                    <a href="mailto:<%= Html.Encode(trustee.Profile.Email) %>">
                                        <%= Html.Encode(trustee.Profile.Email)%></a><a href="mailto:<%= Html.Encode(trustee.Profile.EmailAlternate) %>"><%= Html.Encode(trustee.Profile.EmailAlternate)%></a></span>
                                <input type="submit" class="submit" value="_" style="color: White" /><span style="margin-right: 150px;
                                    float: right;">
                                    <fb:profile-pic uid="<%= Html.Encode(trustee.Profile.FacebookId) %>" facebook-logo="false">
                                    </fb:profile-pic></span>
                            </dd>
                            <%   } %>
                            <% } %>
                        </dl>
                        <span class="bottomLink">
                            <%=Html.ActionLink("Back", "NumTrustees")%>
                            |
                            <%=Html.ActionLink("Done", "Done")%>
                        </span>
                    </div>
                </div>
                <!-- End boxArea  -->
            </div>
            <!-- End box  -->
        </div>
        <!-- End cotentArea  -->
    </div>
    <!-- End center  -->
    <% Html.RenderPartial("RightProgress"); %>
</asp:Content>

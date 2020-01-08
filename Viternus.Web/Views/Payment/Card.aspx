<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Card
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="headingText">
        Inner Circle Options</h1>
    <div class="fullWidthArea">
        <div class="smoothFont contentArea">
            <p>
                Hello,<br />
                <br />
                We're still putting together the finishing touches on our Inner Circle feature.
                We will be sure to notify you when it is available. Our expectation is that it will
                be ready by June, 2010.<br />
                <br />
                In the meantime, feel free to create as many <a href="/Message/Create">messages</a>
                as you want. You can still deliver messages automatically at a specific time in
                the future.<br />
                <br />
                Some uses for your free messages:
                <br />
            </p>
            <ul>
                <li>Make a prediction</li>
                <li>Send yourself a reminder</li>
                <li>Send someone a nice message on his/her birthday or anniversary</li>
            </ul>
            <p class="bottomLink">
                <%=Html.ActionLink("Home", "Index", "Home")%>
            </p>
        </div>
    </div>
</asp:Content>

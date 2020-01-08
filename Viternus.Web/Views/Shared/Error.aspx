<%@ Page Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Error
</asp:Content>
<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="headingText">
        There was an error</h1>
    <div class="rightBox">
        <div class="rightBoxArea">
            <div class="tableArea">
                <div class="smoothFont">
                <p style="padding:30px;">
                    Sorry, an error occurred while you were using the site.<br />
                    <br />
                
                    Feel free to
                    <%=Html.ActionLink("contact us", "ContactUs", "Home")%>
                    if the problem persists, but we do already have information about this error.</p>
                    </div>
            </div>
        </div>
    </div>
</asp:Content>

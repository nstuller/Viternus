<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Test
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Test</h1>
    <% using (Html.BeginForm("TestUpload", "Video", FormMethod.Post, new { enctype = "multipart/form-data" }))
       { %>
    <input type="file" id="fileUploadInput" name="fileUploadInput" />
    <input type="submit" value="Upload" />
    <% } %>
    <p>
        <% if (Model != null)
           { %>
        <%= Model.ToString()%>
        <% } %>
    </p>
</asp:Content>

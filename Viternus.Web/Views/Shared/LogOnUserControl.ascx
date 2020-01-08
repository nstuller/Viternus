<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="logOnControl">
<%
    if (Request.IsAuthenticated) {
%>
        Welcome <span><%= Html.Encode(Page.User.Identity.Name) %></span>
        [<%= Html.ActionLink("Log Off", "LogOff", "Account") %>]
<%
    }
    else {
%> 
        [<%= Html.ActionLink("Log On", "LogOn", "Account") %>]
<%
    }
%>
</div>
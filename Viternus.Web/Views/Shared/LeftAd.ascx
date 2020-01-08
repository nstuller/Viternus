<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>


<%
    if ((bool)ViewData["showAds"])
    {
%>

<div class="leftAd">
<p><b>Recommended Blogs</b><br />
<a href="http://julliengordon.mvmt.com">J. Gordon, Purpose Finder</a><br />
<a href="http://rememberingyourpresent.wordpress.com">Remembering Your Present</a><br />
<a href="http://shazzacraft.blogspot.com">Shazza's Craft Connection</a><br />
<a href="http://yoncaskitchen.blogspot.com">Yonca is Cooking</a><br />
 </p>
</div>

<%--<img src="../../Content/images/add-space2.gif" alt="Asp.Net MVC" />--%>

<%
    }
%>
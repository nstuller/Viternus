<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="header">
    
   
    <a href="/" title="Viternus Home">
        <img src="../../Content/images/logoBlue.jpg" alt="Viternus Logo" /></a> <span class="tagline">
            Your legacy, on your schedule</span>
    <!-- Start navigation  -->
    <ul>
        <li><a href="/" title="Viternus Home"><span><span>Home</span></span></a></li>
        <li><a href="/Message/ViewMessageFromUrl/014cd4f7-b947-4bb9-8914-e32e40c2dd98" title="Video Tour"><span><span>Tour</span></span></a></li>
        <li><a href="/Home/FAQ" title="Frequently Asked Questions"><span><span>FAQ</span></span></a></li>
        <li><a href="/Home/ContactUs" title="Contact Viternus"><span><span>Contact Us</span></span></a></li>
        <% if (Context.User.IsInRole("Admin"))
           { %>
        <li><a href="/Message/DeliveryAdmin" title="Admin Page"><span><span>Admin</span></span></a>
        </li>
        <% } %>
    </ul>
    <span id="skip"></span>
    <!-- End navigation  -->

</div>

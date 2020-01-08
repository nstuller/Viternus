<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<!-- Start rightMost  -->
<div class="rightMost">
    <p>
        Inner Circle Wizard</p>
    <dl class="rightNav">
        <%
            if ("1" == ViewData["ProgressStep"])
            {
        %>
        <dd>
            <span class="information" style="background-position: 0px -107px;">1. Information</span></dd>
        <% }
            else
            {
        %>
        <dd>
            <span class="information">1. Information</span></dd>
        <%
            }
        %>
        <%
            if ("2" == ViewData["ProgressStep"])
            {
        %>
        <dd>
            <span class="checkout" style="background-position: 0px -107px;">2. Checkout</span></dd>
        <% }
            else
            {
        %>
        <dd>
            <span class="checkout">2. Checkout</span></dd>
        <%
            }
        %>
        <%
            if ("3" == ViewData["ProgressStep"])
            {
        %>
        <dd>
            <span class="num" style="background-position: 0px -107px;">3. Num Trustees </span>
        </dd>
        <% }
            else
            {
        %>
        <dd>
            <span class="num">3. Num Trustees </span>
        </dd>
        <%
            }
        %>
        <%
            if ("4" == ViewData["ProgressStep"])
            {
        %>
        <dd>
            <span class="inner" style="background-position: 0px -107px;">4. Inner Circle </span>
        </dd>
        <% }
            else
            {
        %>
        <dd>
            <span class="inner">4. Inner Circle </span>
        </dd>
        <%
            }
        %>
        <%
            if ("5" == ViewData["ProgressStep"])
            {
        %>
        <dd>
            <span class="final" style="background-position: 0px -107px;">5. Final</span></dd>
        <% }
            else
            {
        %>
        <dd>
            <span class="final">5. Final</span></dd>
        <%
            }
        %>
    </dl>
</div>
<!-- End rightMost  -->

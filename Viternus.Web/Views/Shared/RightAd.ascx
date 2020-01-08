
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>


<!-- Start rightMost  -->
<div class="rightPrompts">
<p><b>Journal Prompts</b><br />
    <%: (string)ViewData["journalPrompt"]  %>
                        <br />
 </p>
</div>
<!-- End rightMost  -->


<%
    if ((bool)ViewData["showAds"])
    {
%>

<div class="rightAd">
<script type="text/javascript"><!--
    google_ad_client = "pub-2823766801187926";
    /* Right 120x240, created 5/12/10 */
    google_ad_slot = "4707441561";
    google_ad_width = 120;
    google_ad_height = 240;
//-->
</script>
<script type="text/javascript"
src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
</script>
</div>
<%
    }
%>
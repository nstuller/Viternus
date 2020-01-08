<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Viternus.Data.Video>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Viternus - View My Videos
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="headingText">
        My Videos</h1>
    <div class="rightBox">
        <div class="rightBoxArea">
            <div class="tableArea">
                <table class="contentTable" width="735" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <th width="321">
                            <span>Description </span>
                        </th>
                        <th width="146">
                        </th>
                        <th width="352">
                            &nbsp;
                        </th>
                    </tr>
                    <% foreach (var item in Model)
                       { %>
                    <tr>
                        <td>
                            <span>
                                <%= Html.Encode(item.Description) %></span>
                        </td>
                        <td>
                            <%--<img src="../Content/images/sample-video.jpg" alt="Video" />--%>
                        </td>
                        <td>
                            <b><a href="/Video/Edit/<%= Html.Encode(item.Id) %>">
                                <img src="../../Content/images/EditPreview.gif" alt="Edit" /></a> <a href="/Video/Delete/<%= Html.Encode(item.Id)%>">
                                    <img src="../../Content/images/delete.gif" alt="Delete" /></a> </b>
                        </td>
                    </tr>
                    <% } %>
                </table>
                <a href="/Video/UploadFiles" class="roundedGreenLink">Upload New Videos </a>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function() { $("table tr:nth-child(odd)").addClass("colorful"); });
    
    </script>

</asp:Content>

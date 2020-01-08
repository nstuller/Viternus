<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Viternus.Web.ViewModels.MessageDisplayViewModel>" %>
<h3>
    Delivered: <span>
        <%= Html.Encode(String.Format("{0:d}", Model.Message.SentDate)) %></span></h3>
<ul>
    <li>
        <label>
            From:</label><p>
                <%= Html.Encode(Model.Message.AppUser.Profile.FirstName)%>
                <%= Html.Encode(Model.Message.AppUser.Profile.LastName)%></p>
    </li>
    <li>
        <label>
            Comments:
        </label>
        <p>
            <%= Html.Encode(Model.Message.Comments)%></p>
    </li>
    <li>
        <% if (Model.Message.Video != null)
           { %>
        <% Html.RenderPartial("VideoPlayer", Model.Message.Video); %>
        <% } %>
    </li>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>


                        <ul class="list">
                            <li>
                                <label id="requiredExplanation">
                                    * means required</label>
                            </li>
                            <li>
                                <label for="FirstName">
                                    First Name:</label>
                                <%= Html.TextBox("FirstName")%>
                                <%= Html.ValidationMessage("FirstName", "*")%></li>
                            <li>
                                <label for="LastName">
                                    Last Name:</label>
                                <%= Html.TextBox("LastName")%>
                                <%= Html.ValidationMessage("LastName", "*")%></li>
                            <li>
                                <label for="Email">
                                    Email Address*:</label>
                                <%= Html.TextBox("Email")%>
                                <%= Html.ValidationMessage("Email", "Required to add recipient")%></li>
                            <li>
                                <input type="submit" class="add" value="Add Recipient" />
                            </li>
                        </ul>
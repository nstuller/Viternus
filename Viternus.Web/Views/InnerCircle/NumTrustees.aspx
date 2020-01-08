<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Web.ViewModels.InnerCircleNumTrusteesViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Choose the Number of Trustees
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1>
                Inner Circle Options</h1>
            <!-- Start box  -->
            <div class="box boxSection">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <h4>
                            Choose the Number of Trustees</h4>
                        <p>
                            <br />
                            A majority of the number of trustees you select must enter info about your event. 
                            <dl class="listPart">
                                <dd>
                                    <%
                                        if ("1" == Model.NumberOfTrustees)
                                        {
                                    %>
                                    <a class="oneTrusteeSelected" href="/InnerCircle/Choose?NumTrustees=1"></a>
                                    <% }
                                        else
                                        {
                                    %>
                                    <a class="oneTrustee" href="/InnerCircle/Choose?NumTrustees=1"></a>
                                    <%
                                        }
                                    %>
                                    <p>
                                        Trust 1 Individual</p>
                                </dd>
                                <dd>
                                    <%
                                        if ("3" == Model.NumberOfTrustees)
                                        {
                                    %>
                                    <a class="threeTrusteesSelected" href="/InnerCircle/Choose?NumTrustees=3"></a>
                                    <% }
                                        else
                                        {
                                    %>
                                    <a class="threeTrustees" href="/InnerCircle/Choose?NumTrustees=3"></a>
                                    <%
                                        }
                                    %>
                                    <p>
                                        Trust 2 out of 3 Individuals</p>
                                </dd>
                                <dd>
                                    <% if ("5" == Model.NumberOfTrustees) { %>
                                    <a class="fiveTrusteesSelected" href="/InnerCircle/Choose?NumTrustees=5"></a>
                                    <% } else { %>
                                    <a class="fiveTrustees" href="/InnerCircle/Choose?NumTrustees=5"></a>
                                    <%
                                        }
                                    %>
                                    <p>
                                        Trust 3 out of 5 Individuals</p>
                                </dd>
                            </dl>
                        </p>
                    </div>
                </div>
                <!-- End boxArea  -->
            </div>
            <!-- End box  -->
        </div>
        <!-- End cotentArea  -->
    </div>
    <!-- End center  -->
    <% Html.RenderPartial("RightProgress"); %>
</asp:Content>

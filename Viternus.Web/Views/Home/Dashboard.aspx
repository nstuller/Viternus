<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Dashboard
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Dashboard</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea" style="min-height:200px;">
                    <div>
                        <h4>
                            Get Started</h4>
                        <p>
                            <br />
                            You're new here.
                            <br />
                            <br />
                            You're probably going to want to do one of the following things:
                            <br />
                            <br />
                            <a href="/Account/Edit"><span>Update Your Account</span></a><br />
                            <a href="/Video/UploadFiles" class="upload"><span>Upload A Video</span></a><br />
                            <a href="/Message/Create" class="message"><span>Create A Message</span></a><br />
                        </p>
                    </div>
                </div>
                <!-- End boxArea  -->
            </div>
            <!-- End box  -->
        </div>
        <!-- End contentArea  -->
    </div>
    <!-- End center  -->
    <% Html.RenderPartial("RightAd"); %>
</asp:Content>

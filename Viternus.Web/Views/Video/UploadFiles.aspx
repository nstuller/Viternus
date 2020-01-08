<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage<Viternus.Web.ViewModels.VideoUploadFilesViewModel>" %>

<%@ Register TagPrefix="kw" Assembly="Krystalware.SlickUpload" Namespace="Krystalware.SlickUpload.Controls" %>
<%@ Register TagPrefix="kw" Assembly="Krystalware.SlickUpload" Namespace="Krystalware.SlickProgress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Viternus - Upload Videos
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="headingText">
        Upload Files</h1>
    <div class="rightBox">
        <div class="rightBoxArea">
            <div class="uploadBox">

                <script type="text/javascript">
                    function SlickUpload_OnClientProgressUpdate(data) {
                        var postProcessProgress = document.getElementById("postProcessProgress");

                        // if we're postprocessing now, show the postprocessing progress section
                        if (postProcessProgress.style.display == "none" && data.state == "PostProcessing") {
                            var uploadProgress = document.getElementById("uploadProgress");

                            uploadProgress.style.display = "none";
                            postProcessProgress.style.display = "";
                        }
                    }
                </script>
                <% if (Model.User.LimitVideoUploads)
                   { %>
                   <p>
                        <i>Reminder:</i> as a free user, you are limited to storing 2 videos in your account at any 1 time.
                        <br /><br />
                        </p>
                <% } %>
                

                <% if (Model.User.RemainingVideoUploads <= 0)
                   { %>
                   <p>
                        You do not have any remaining videos left to use.
                        <br /><br />
                        Please <a href="/InnerCircle/Explain">upgrade</a> to receive unlimited video uploads.
                        </p>
                <% } else { %>
                <% using (Html.BeginForm("UploadResult", "Video", FormMethod.Post, new { id = "uploadForm", enctype = "multipart/form-data" }))
                   { %>
                <%--,.mov,.mp4,.mpeg,.avi--%>
                <%--ValidExtensions=".wmv"--%>
                <kw:SlickUpload ID="SlickUpload1" runat="server" UploadFormId="uploadForm" ShowDuringUploadElements="cancelButton"
                    HideDuringUploadElements="uploadButton" MaxFiles="5" HasPostProcessStep="true"
                    InvalidExtensionMessage="Only Windows Media Videos (*.wmv)" OnClientProgressUpdate="SlickUpload_OnClientProgressUpdate">
                    <DownlevelSelectorTemplate>
                        <input id="fileUploadInput" type="file" />
                    </DownlevelSelectorTemplate>
                    <UplevelSelectorTemplate>
                        <strong>1. Click Browse to select a file for upload. </strong>
                        <p>
                            Note: You may add up to 5 files at one time not to exceed a total of 200 MegaBytes
                            (MB).
                            <br />
                            <br />
                        </p>
                        <input type="button" class="submit" value="Add File" />
                    </UplevelSelectorTemplate>
                    <FileTemplate>
                        <div style="border: solid 1px #ccc; margin: 2px; padding: 8px; width: 670px; font-size: 13px;
                            color: #573d2f;">
                            <kw:FileListRemoveLink ID="FileListRemoveLink1" runat="server" Title="Remove" Style="float: right;">
                                <img width="14" height="14" style="vertical-align: middle" src="<%=ResolveUrl("~/Content/images/cross.gif") %>"
                                    alt="Remove" />
                            </kw:FileListRemoveLink>
                            <span>Video Description:</span>
                            <input type="text" name="fileDescription" maxlength="50" />
                            <kw:FileListFileName ID="FileListFileName1" runat="server" Style="margin-left: 4px;" />
                            <kw:FileListValidationMessage ID="FileListValidationMessage1" runat="server" ForeColor="Red" />
                        </div>
                    </FileTemplate>
                    <ProgressTemplate>
                        <table id="uploadProgress" class="uploadArea">
                            <tr>
                                <td>
                                    Uploading
                                    <kw:UploadProgressElement ID="UploadProgressElement1" runat="server" Element="FileCountText" />
                                    ,
                                    <kw:UploadProgressElement ID="UploadProgressElement2" runat="server" Element="ContentLengthText">
                                        (calculating size)</kw:UploadProgressElement>
                                    .
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Currently uploading:
                                    <kw:UploadProgressElement ID="UploadProgressElement3" runat="server" Element="CurrentFileName" />
                                    , file
                                    <kw:UploadProgressElement ID="UploadProgressElement4" runat="server" Element="CurrentFileIndex">
                                        &nbsp;</kw:UploadProgressElement>
                                    of
                                    <kw:UploadProgressElement ID="UploadProgressElement5" runat="server" Element="FileCount" />
                                    .
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Speed:
                                    <kw:UploadProgressElement ID="UploadProgressElement6" runat="server" Element="SpeedText">
                                        (calculating speed)</kw:UploadProgressElement>
                                    .
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    About
                                    <kw:UploadProgressElement ID="UploadProgressElement7" runat="server" Element="TimeRemainingText">
                                        (calculating time)</kw:UploadProgressElement>
                                    remaining.
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="uploadProgressBar" style="width: 724px;">
                                        <kw:UploadProgressBarElement ID="UploadProgressBarElement1" runat="server" class="uploadProgressFill" />
                                        <div class="uploadProgressText">
                                            <kw:UploadProgressElement ID="UploadProgressElement8" runat="server" Element="PercentCompleteText">
                                                (calculating)</kw:UploadProgressElement>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table id="postProcessProgress" style="display: none" class="uploadArea">
                            <tr>
                                <td>
                                    Please wait while we process the final details for this upload...
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="uploadProgressBar" style="width: 724px;">
                                        <kw:ProgressBarElement ID="ProgressBarElement1" runat="server" class="uploadProgressFill"
                                            ElementKey="percentComplete" />
                                        <div class="uploadProgressText">
                                            <kw:ProgressElement ID="ProgressElement1" runat="server" ElementKey="percentCompleteText">
                                                (calculating)</kw:ProgressElement>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ProgressTemplate>
                </kw:SlickUpload>
                <div class="uploadArea" id="uploadButton">
                    <strong>2. When you are ready to upload the files, click the Upload button.</strong>
                    <p>
                        We support most digital video files, including .flv, .mov, .mp4 and .wmv (among
                        others).<br />
                        <br />
                    </p>
                    <input type="submit" class="submit" value="Upload" />
                </div>
                <p>
                    <a href="javascript:kw.get('<%=SlickUpload1.ClientID %>').cancel()" id="cancelButton"
                        style="display: none">Cancel</a>
                </p>
                <% } %>
                <% if (Model.SlickUploadStatus != null && Model.SlickUploadStatus.State == Krystalware.SlickUpload.Status.UploadState.Terminated && Model.SlickUploadStatus.Reason == Krystalware.SlickUpload.Status.UploadTerminationReason.MaxRequestLengthExceeded)
                   { %>
                   <div class="uploadError" style="width:704px;">
                <p>
                    We could not upload your files.</p>
                <p>
                    <br />
                    This is probably because the size of your upload is above our limit. Please try
                    uploading large files individually. If that does not work, we recommend using a
                    3rd party video editor to reduce the size.
                </p></div>
                <% } %>
                <% } %>
                <span class="bottomLink">
                    <%=Html.ActionLink("Back to List", "Index") %>
                </span>
            </div>
        </div>
    </div>
</asp:Content>

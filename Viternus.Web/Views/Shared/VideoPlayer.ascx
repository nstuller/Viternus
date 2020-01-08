<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Viternus.Data.Video>" %>
<p>
    <object data="data:application/x-silverlight-2," type="application/x-silverlight-2"
        width="400" height="300">
        <param name="source" value="/VideoPlayerM.xap" />
        <param name="background" value="white" />
        <param name="initParams" value="m=<%= Url.Action("StreamPrivate", "Video", new {id=Model.Id})%>" />
        <param name="minRuntimeVersion" value="3.0.40818.0" />
        <param name="autoUpgrade" value="true" />
        <a href="http://go.microsoft.com/fwlink/?LinkID=149156" style="text-decoration: none;">
            <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Install Microsoft Silverlight"
                style="border-style: none" />
        </a>
        
            <br />
            Note: To view the video, click on the "Install Microsoft Silverlight" image above.<br />
            <br />
            Silverlight is a small install produced by Microsoft to allow video streaming. It
            is similar to 'Flash,' which is what you have installed if you ever watch videos
            on YouTube.<br />
            <br />
            If you have ever watched the Olympics live on the Internet or streamed NetFlix movies
            to your computer, then you already have Silverlight. By clicking the above link,
            you can upgrade to a version that is compatible with this site.
        
    </object>
</p>

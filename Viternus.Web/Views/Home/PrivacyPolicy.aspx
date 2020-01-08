<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/LoggedIn.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Privacy Policy
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start center  -->
    <div class="center">
        <!-- Start contentArea  -->
        <div class="contentArea">
            <h1 class="heading">
                Privacy Policy</h1>
            <!-- Start box  -->
            <div class="box">
                <!-- Start box Area  -->
                <div class="boxArea">
                    <div>
                        <h4>
                            What information do we collect?</h4>
                        <p>
                            <br />
                            We collect information from you when you register on our site, place an order or
                            create messages.
                            <br />
                            <br />
                            When ordering or registering on our site, as appropriate, you may be asked to enter
                            your: name, e-mail address, credit card information<%-- or social security number--%>.
                            You may, however, visit our site anonymously.<br />
                            <br />
                        </p>
                        <h4>
                            What do we use your information for?</h4>
                        <p>
                            <br />
                            Any of the information we collect from you may be used in one of the following ways:
                            <br />
                            <br />
                            &#149; To personalize your experience (your information helps us to better respond
                            to your individual needs)<br />
                            <br />
                            &#149; To improve our website (we continually strive to improve our website offerings
                            based on the information and feedback we receive from you)<br />
                            <br />
                            &#149; To process transactions<br />
                        </p>
                        <blockquote>
                            Your information, whether public or private, will not be sold, exchanged, transferred,
                            or given to any other company for any reason whatsoever, without your consent, other
                            than for the express purpose of delivering the purchased product or service requested.</blockquote>
                        <p>
                            <br />
                            &#149; To send periodic emails<br />
                        </p>
                        <blockquote>
                            The email address you provide for order processing, may be used to send you information
                            and updates pertaining to your order, in addition to receiving occasional company
                            news, updates, related product or service information, etc.</blockquote>
                        <p>
                            <br />
                            Note: If at any time you would like to unsubscribe from receiving future emails,
                            we include detailed unsubscribe instructions at the bottom of each email.<br />
                            <br />
                        </p>
                        <h4>
                            How do we protect your information?</h4>
                        <p>
                            <br />
                            We implement a variety of security measures to maintain the safety of your personal
                            information when you place an order or access your personal information.
                            <br />
                            <br />
                            We offer the use of a secure server. All supplied sensitive/credit information is
                            transmitted via Secure Socket Layer (SSL) technology and then encrypted into our
                            Payment gateway providers database only to be accessible by those authorized with
                            special access rights to such systems, who are required to keep the information
                            confidential.<br />
                            <br />
                            After a transaction, your private information (credit cards,
                            <%--social security numbers,--%>
                            financials, etc.) will not be stored on our servers.<br />
                            <br />
                        </p>
                        <h4>
                            Do we use cookies?</h4>
                        <p>
                            <br />
                            Yes (Cookies are small files that a site or its service provider transfers to your
                            computers hard drive through your Web browser (if you allow) that enables the sites
                            or service providers systems to recognize your browser and capture and remember
                            certain information).<br />
                            <br />
                            We use cookies to understand and save your preferences for future visits and keep
                            track of advertisements.<br />
                            <br />
                        </p>
                        <h4>
                            Do we disclose any information to outside parties?</h4>
                        <p>
                            <br />
                            We do not sell, trade, or otherwise transfer to outside parties your personally
                            identifiable information. This does not include trusted third parties who assist
                            us in operating our website, conducting our business, or servicing you, so long
                            as those parties agree to keep this information confidential. We may also release
                            your information when we believe release is appropriate to comply with the law,
                            enforce our site policies, or protect ours or others rights, property, or safety.
                            However, non-personally identifiable visitor information may be provided to other
                            parties for marketing, advertising, or other uses.<br />
                            <br />
                        </p>
                        <h4>
                            Business Transitions</h4>
                        <p>
                            <br />
                            Should Viternus go through a business transition, such as a change of ownership
                            or control, a merger, or sale of a portion of its assets, users' personal information
                            will, in most instances, be part of the assets transferred.<br />
                            <br />
                        </p>
                        <h4>
                            Childrens Online Privacy Protection Act Compliance</h4>
                        <p>
                            <br />
                            We are in compliance with the requirements of COPPA (Childrens Online Privacy Protection
                            Act), we do not collect any information from anyone under 18 years of age. Our website,
                            products and services are all directed to people who are at least 18 years old or
                            older.<br />
                            <br />
                        </p>
                        <h4>
                            Terms and Conditions</h4>
                        <p>
                            <br />
                            Please also visit our Terms and Conditions section establishing the use, disclaimers,
                            and limitations of liability governing the use of our website at <a href="/Home/TermsOfService"
                                target='_blank'>www.viternus.com</a>.
                            <br />
                            <br />
                        </p>
                        <h4>
                            Your Consent</h4>
                        <p>
                            <br />
                            By using our site, you consent to our <a href='/Home/PrivacyPolicy' target='_blank'>
                                web site privacy policy</a>.
                            <br />
                            <br />
                        </p>
                        <h4>
                            Changes to our Privacy Policy</h4>
                        <p>
                            <br />
                            If we decide to change our privacy policy, we will post those changes on this page,
                            and/or update the Privacy Policy modification date below.
                            <br />
                            <br />
                            This policy was last modified on 03/10/2010
                            <br />
                            <br />
                        </p>
                        <h4>
                            Contacting Us</h4>
                        <p>
                            <br />
                            If there are any questions regarding this privacy policy you may contact us using
                            the information below.
                            <br />
                            <br />
                            <span style="font-style: italic">Software Sourcery, LLC</span>
                            <br />
                            <a href="http://www.viternus.com/Home/ContactUs">Contact Us Page</a>
                            <br />
                            <a href="mailto:hello@viternus.com">hello@viternus.com</a>
                            <br />
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

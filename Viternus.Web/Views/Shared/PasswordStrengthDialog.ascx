<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="passwordStrengthDialog" title="Password Strength" style="display: none;
    text-align: left;">
    <p>
        Some examples of "Special Characters" are [! # $ % & ( ) * + , - / : ; < = > ? @
        \ ^ _ { | } ']. We require you to use special characters in your password so we
        can ensure your password cannot be easily guessed.
    </p>
    <br />
    <p>
        The term "Strong Password" refers to a password that is dissimilar to an actual
        word. Requiring a strong password is just one of the many ways Viternus strives
        to keep your information secure.
    </p>
</div>

<script type="text/javascript">
    $(function() {
        $("#passwordStrengthDialog").dialog({ autoOpen: false, bgiframe: true, width: 420, height: 330, modal: true });

        $("#explainPassword").click(function() {
            $("#passwordStrengthDialog").dialog('open');
        });
    });
</script>


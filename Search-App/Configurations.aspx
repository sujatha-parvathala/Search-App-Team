<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master-1.Master" CodeBehind="Configurations.aspx.cs" Inherits="Search_App.Configurations" %>


<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
    <title>LOGIN</title>
    <style>
        #top, #bottom, #left, #right {
            background: #219fd4;
            position: fixed;
        }

        #left, #right {
            top: 0;
            bottom: 0;
            width: 25px;
        }

        #left {
            left: 0;
        }

        #right {
            right: 0;
        }

        #top, #bottom {
            left: 0;
            right: 0;
            height: 25px;
        }

        #top {
            top: 0;
        }

        #bottom {
            bottom: 0;
        }

        /*body {
            max-width: 100%;
            margin: 0;
            padding: 50px 20px 150px 20px;
            height: 100%;
        }*/

        .logo-holder_mittun {
            position: relative;
            margin: 0 auto;
            display: block;
            width: 100%;
            max-width: 360px;
        }

        .message-holder_mittun {
            width: 100%;
            max-width: 600px;
            margin: 10px auto 50px;
            display: block;
            position: relative;
            text-align: center;
        }

        .message-text_mittun {
            color: #666666;
            font-size: 18px;
            border-top: 1px solid #ccc;
            border-bottom: 1px solid #ccc;
            padding: 25px;
        }

        .progress-holder_mittun {
            position: relative;
            margin: 0 auto;
            display: block;
            width: 50px;
        }

        .gif-holder_mittun {
            opacity: .2;
            -moz-opacity: .2;
            -webkit-opacity: .2;
            width: 50px;
        }

        .pulsate {
            -webkit-animation: pulsate 3s ease-out;
            -webkit-animation-iteration-count: infinite;
            opacity: 0.2;
        }

        @-webkit-keyframes pulsate {
            0% {
                opacity: 0.2;
            }

            50% {
                opacity: 1.0;
            }

            100% {
                opacity: 0.2;
            }
        }

        @media
        /* Fairly small screens including iphones */
        only screen and (max-width: 480px) {
            #top, #bottom {
                height: 10px;
            }

            #left, #right {
                width: 10px
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="message-holder_mittun">
        <p class="message-text_mittun pulsate">
            Have fun giving to a cause you care about.
  <br />
            <span style="display: block; padding-top: 12px; font-size: .8em;">(website coming soon)</span>
        </p>
    </div>
    <div class="progress-holder_mittun">
        <img class="gif-holder_mittun" src="https://downloads.mittun.com/files/assets/loader.gif">
    </div>
</asp:Content>

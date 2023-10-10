<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Search_App.Index" %>

 
<html>   
<head>  
<meta name="viewport" content="width=device-width, initial-scale=1">  
<title> Login Page </title>  
<style>   
Body {  
  font-family: Calibri, Helvetica, sans-serif;  
  background-color: white;  
}  
.button {   
       background-color: #4CAF50;   
        width: 100%;  
        color: white;   
        padding: 15px;   
        margin: 10px 0px;   
        border: none;   
        cursor: pointer;   
       }   
 form {   
        border: 3px solid #f1f1f1;   
        margin-top:10%;
    }   
 input[type=text], input[type=password] {   
        width: 100%;   
        margin: 8px 0;  
        padding: 12px 20px;   
        display: inline-block;   
        border: 2px solid green;   
        box-sizing: border-box;   
    }  
 button:hover {   
        opacity: 0.7;   
    }   
  .cancelbtn {   
        width: auto;   
        padding: 10px 18px;  
        margin: 10px 5px;  
    }   
        
     
 .container {
    padding: 25px;
    background-color: lightblue;
    max-width: 400px;
    margin-left: 35%;
    /*margin-top:10%;*/
} 
</style>  
    <script>
        function clear() {
            document.getElementById("lbl_error").textContent = "";
            document.getElementById("txt_username").textContent = "";
            document.getElementById("txt_username").textContent = "";
            return false;
        }
    </script>
</head>    
<body>    
   
    <form runat="server">  
        <div>
            <center> <h1> Intelligent Search </h1> </center>
        </div>
        <div class="container">   
            <center> <h1> Login </h1> </center>   
            <label>Username : </label>   
            <input type="text" runat="server" placeholder="Enter Username" name="txt_username" id="txt_username" required>  
            <label>Password : </label>   
            <input type="password" runat="server" placeholder="Enter Password" name="txt_password" id="txt_password" required> 
            <center>
            <asp:Button runat="server" CssClass="button" Width="75px" Text="Login" OnClick="Login_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" CssClass="button" Width="75px" Text="Clear" name="btn_clear" OnClick="Unnamed2_Click"/>
            </center>
            <asp:Label runat="server" CssClass="label" Text=" " name="lbl_error" id="lbl_error" /> 
            
        </div>   
    </form>     
</body>     
</html>  

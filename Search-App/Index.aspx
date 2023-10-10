<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Search_App.Index" %>


<html>
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>LOGIN </title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.2/css/bootstrap.min.css"
    integrity="sha512-b2QcS5SsA8tZodcDtGRELiGv5SaKSk1vDHDaQRda0htPYWZ6046lr3kJ5bAAQdpV2mmA/4v0wQF9MyU6/pDIAg=="
    crossorigin="anonymous" referrerpolicy="no-referrer" />
    <style>
        body {
            background: #007bff;
            background: linear-gradient(to right, #0062E6, #33AEFF);
        }

        .btn-login {
            font-size: 0.9rem;
            letter-spacing: 0.05rem;
            padding: 0.75rem 1rem;
            width:100% !important;
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

        <!-- This snippet uses Font Awesome 5 Free as a dependency. You can download it at fontawesome.io! -->


        <div class="container">
            <div class="row">
                <div class="col-sm-9 col-md-7 col-lg-5 mx-auto">
                    <div class="card border-0 shadow rounded-3 my-5">
                        <div class="card-body p-4 p-sm-5">
                             <h5 class="card-title text-center mb-3 fw-light fs-5">Intelligent Search</h5>
                            <h5 class="card-title text-center mb-3 fw-light fs-5">Login</h5>
                                <asp:Label runat="server" CssClass="label" Text=" " name="lbl_error" ID="lbl_error" />
                                <div class="form-floating mb-3">
                                     
                                     <input type="text" class="form-control" runat="server" placeholder="Enter Username" name="txt_username" id="txt_username" required>
                                    <label for="floatingInput">Username</label>
                                </div>
                                <div class="form-floating mb-3">
                                    <input type="password" runat="server" class="form-control" placeholder="Enter Password" name="txt_password" id="txt_password" required>
                                    <label for="floatingPassword">Password</label>
                                </div>

                                 
                                <div class="row"> 
                                    <div class="col-md-12">
                                      <asp:Button  runat="server" CssClass="btn btn-primary btn-login text-uppercase fw-bold" Width="75px" Text="Login" OnClick="Login_Click" />
                                        </div>
                                     <div class="col-md-6" style="display:none;">
                                       <asp:Button  runat="server" CssClass="btn btn-danger btn-login text-uppercase fw-bold" Width="75px" Text="Clear" name="btn_clear" OnClick="Unnamed2_Click" />
                                         </div>
                                </div>
                                
                             
                        </div>
                    </div>
                </div>
            </div>
        </div>




         
    </form>
</body>
</html>

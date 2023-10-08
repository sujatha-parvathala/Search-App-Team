<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchPage.aspx.cs" Inherits="Search_App.SearchPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>INTELLIGENCE SEARCH</title>


    <!-- Icons font CSS-->
    <link href="Assets/mdi-font/css/material-design-iconic-font.min.css" rel="stylesheet" />
    <link href="Assets/font-awesome-4.7/css/font-awesome.min.css" rel="stylesheet" />

    <!-- Font special for pages-->
    <link href="https://fonts.googleapis.com/css?family=Lato:100,100i,300,300i,400,400i,700,700i,900,900i" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Poppins:100,100i,200,200i,300,300i,400,400i,500,500i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet" />

    <!-- Main CSS-->

    <link href="Assets/main.css" rel="stylesheet" />
    <!-- Jquery JS-->
    <link href="Assets/jquery.dataTables.min.css" rel="stylesheet" />

    <style>
        .dataTables_wrapper {
            position: relative;
            clear: both;
            background-color: white;
            font-family: "Poppins", "Arial", "Helvetica Neue", "sans-serif";
            font-size: 11px;
            padding-bottom: 10px;
           
        }


        .dataTables_filter {
            padding-bottom: 15px;
        }

        .dataTables_wrapper
        .dataTables_length,
        .dataTables_wrapper
        .dataTables_filter,
        .dataTables_wrapper
        .dataTables_info,
        .dataTables_wrapper
        .dataTables_processing,
        .dataTables_wrapper
        .dataTables_paginate {
            color: inherit;
            padding-right: 10px;
            padding-top: 10px;
            padding-left: 10px;
            font-weight: bold;
        }

        .dataTable thead {
            background-color: #3d2161;
            color: white;
        }


        table.dataTable {
            width: 98%;
            margin: 0 auto;
            clear: both;
            border-collapse: separate;
            border-spacing: 0;
            font-weight: bold;
        }

        .pageHeader {
            border-radius: 10px;
            color: white;
            padding-left: 250px;
            background-color: rgb(0, 77, 145);
            margin-bottom:10px;
        }
    </style>

    <script src="Assets/jquery.min.js"></script>
    <script src="Assets/bootstrap.min.js"></script>
    <script src="Assets/js_jquery.dataTables.min.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            //alert('ok');
            //var dt=new DataTable('#example', {

            //}); 
            var mytable = $('#gv_result').DataTable();

        });


    </script>


</head>
<body>
    <form id="form1" runat="server">

        <div class="page-wrapper bg-img-3 p-t-15 p-b-100">
            <div class="wrapper wrapper--w900">
                <div class="pageHeader">
                    <h1>Intelligence Search</h1>
                </div>
                <div class="card card-6">
                    <div class="card-body">

                        <div class="row row-space">
                            <div class="col-2">
                                <div class="input-group">
                                    <label class="label">Name</label>
                                    <input class="input--style-1" type="text" id="txtName" runat="server" name="txtName" placeholder="Enter name" />
                                </div>
                            </div>
                            <div class="col-2">
                                <div class="input-group">
                                    <label class="label">Address</label>
                                    <input id="txtAddress" class="input--style-1" placeholder="Enter address" name="txt_address" runat="server" />
                                </div>
                            </div>
                        </div>

                        <div class="row row-space">
                            <div class="input-group">

                                <label class="label">State</label>
                                <asp:DropDownList ID="drpState" runat="server" CssClass="input--style-1">
                                </asp:DropDownList>

                            </div>

                            <div class="col-2">
                                <div class="input-group">
                                    <label class="label">City</label>
                                    <asp:DropDownList ID="drpCity" runat="server" CssClass="input--style-1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row row-space">
                            <div class="col-2">
                                <div class="input-group">

                                    <label class="label">Postal Code</label>
                                    <input id="txtPostalCode" class="input--style-1" placeholder="Enter pincode" name="txt_pincode" runat="server" />


                                </div>
                            </div>
                            <div class="col-2">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn-submit m-b-0" OnClick="btnSearch_Click" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>

        </div>

        <div class="page-wrapper bg-img-3 p-t-15 p-b-100">
            <div class="wrapper wrapper--w900">
                <div class="pageHeader">
                    <h1>Search Results</h1>
                </div>
                <div class="card card-6">
                    <div class="card-body">
                        <asp:GridView ID="gv_result" runat="server"></asp:GridView>
                    </div>
                </div>
            </div>
        </div>

    </form>
</body>
</html>

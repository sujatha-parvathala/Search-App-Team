<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchPage.aspx.cs" MasterPageFile="~/Master-1.Master" Inherits="Search_App.SearchPage" %>


<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
    <title>INTELLIGENT SEARCH</title>




    <!-- Main CSS-->


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

        tbody, td, tfoot, th, thead, tr {
            border-color: inherit;
            border-style: solid;
            border-width: 1px;
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
            background-color: #0d6efd;
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
            background-color: #0d6efd;
            margin-bottom: 10px;
        }

        #loadingbarspinner.spinner {
            left: 50%;
            margin-left: -20px;
            top: 50%;
            margin-top: -20px;
            position: absolute;
            z-index: 19 !important;
            animation: loading-bar-spinner 400ms linear infinite;
        }

            #loadingbarspinner.spinner .spinner-icon {
                width: 40px;
                height: 40px;
                border: solid 4px transparent;
                border-top-color: #00C8B1 !important;
                border-left-color: #00C8B1 !important;
                border-radius: 50%;
            }

        @keyframes loadingbarspinner {
            0% {
                transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }
    </style>

    <script src="Assets/jquery.min.js"></script>
    <script src="Assets/bootstrap.min.js"></script>
    <script src="Assets/js_jquery.dataTables.min.js"></script>

    <script type="text/javascript">

              $(document).ready(function () {
                  // setTimeout(function () {
                  var mytable = $('#ContentPlaceHolder1_gv_result').DataTable({
                      order:[[6,'desc']]
                  });
                  // }, 3000); 
              });


    </script>

</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="display: none;" id="loadingbarspinner" class="spinner" runat="server" visible="false">
        <div class="spinner-icon"></div>
    </div>
    <div class="content-wrapper">
        <div class="container-xxl flex-grow-1 container-p-y">
            <!-- Basic Layout & Basic with Icons -->

            <div class="row">
                <!-- Basic Layout -->
                <div class="col-xxl">
                    <div class="card mb-4">
                        <div class="card-header d-flex align-items-center justify-content-between">
                            <h5 class="mb-0">Search</h5>
                            <small style="display:none;" class="text-muted float-end">Default label</small>
                        </div>
                        <div class="card-body">
                            <div class="row mb-3">

                                <div class="col">
                                    <div>Name</div>
                                    <input class="form-control" type="text" id="txtName" runat="server" name="txtName" placeholder="Enter name" />
                                </div>
                                <div class="col">
                                    <div>Street</div>
                                    <textarea id="txtAddress" class="form-control" placeholder="Enter address" name="txt_address" runat="server"></textarea>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col">
                                    <div>State</div>
                                    <asp:DropDownList ID="drpState" runat="server" class="form-control" OnSelectedIndexChanged="drpState_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem>Select State</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col">
                                    <div>City</div>
                                    <asp:DropDownList ID="drpCity" runat="server" class="form-control">
                                        <asp:ListItem>Select City</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>
                            <div class="row mb-3">
                                <div class="col">
                                    <div>Postal Code</div>
                                    <input id="txtPostalCode" class="form-control" placeholder="Enter postal code" name="txt_pincode" runat="server" />
                                </div>
                                <div class="col">
                                    <div>Search Type [Logical Operator between Name, Address] </div>
                                    <asp:DropDownList ID="drLogicalOperator" runat="server" class="form-control">
                                        <asp:ListItem Text="OR" Value="false">OR</asp:ListItem>
                                        <asp:ListItem Text="AND" Value="true">AND</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <%--<div class="col">
                                    <div>Search Type [Logical Operator between Name, Address] </div>

                                    <asp:RadioButton ID="rd_and" GroupName="logicalOperator" runat="server" Text="AND " />
                                    <asp:RadioButton ID="rd_or" GroupName="logicalOperator" runat="server" Text="OR" Checked />
                                </div>
                                >--%>
                            </div>

                        </div>
                        <div class="card-footer">
                            <div class="row">
                                <div class="col">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                </div>
                                <div class="col">
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary" OnClick="btnClear_Click" />
                                </div>

                            </div>
                        </div>
                     
                </div>
                <!-- Basic with Icons -->

            </div>
        </div>
    </div>

        </div>
    <div class="content-wrapper" visible="false" runat="server" id="divResultsSection">
        <div class="container-xxl flex-grow-1 container-p-y">
            <!-- Basic Layout & Basic with Icons -->
            <div class="row">
                <!-- Basic Layout -->
                <div class="col-xxl">
                    <div class="card mb-4">
                        <div class="card-header d-flex align-items-center justify-content-between">
                            <h5 class="mb-0">Results</h5>
                            <small style="display: none;" class="text-muted float-end">Default label</small>
                        </div>
                        <div class="card-body">
                            <asp:GridView ID="gv_result" runat="server"></asp:GridView>
                        </div>
                    </div>
                </div>
                <!-- Basic with Icons -->

            </div>
        </div>
    </div> 
</asp:Content>



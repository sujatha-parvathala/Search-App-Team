<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master-1.Master" CodeBehind="Configurations.aspx.cs" Inherits="Search_App.Configurations" %>


<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
    <title>CONFIGURATIONS</title>
    <style>
        .form-check {
            display: block;
            min-height: 1.5rem;
            padding-left: 1.5em;
            font-size: large;
            margin-bottom: .125rem;
            font-family: monospace;
        }

        .form-check-input[type=checkbox]{
            border-radius:.25em;
            border-color:blue;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="content-wrapper">
        <div class="container-xxl flex-grow-1 container-p-y">
            <!-- Basic Layout & Basic with Icons -->

            <div class="row">
                <!-- Basic Layout -->
                <div class="col-xxl">
                    <div class="card mb-4">
                        <div class="card-header d-flex align-items-center justify-content-between">
                            <h5 class="mb-0">DataSource Configurations</h5>
                            <small style="display: none;" class="text-muted float-end">Default label</small>
                        </div>
                        <div class="card-body" runat="server" id="divConfigurations">






                            <%--<div class="row mb-3">
                                <div class="col">
                                    <input type="checkbox" class="custom-control-input" id="customCheck1" checked="">
                                    <label class="custom-control-label" for="customCheck1">Email each time a vulnerability is found</label>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col">
                                    <input type="checkbox" class="custom-control-input" id="customCheck1" checked="">
                                    <label class="custom-control-label" for="customCheck1">Email each time a vulnerability is found</label>
                                </div>
                            </div>--%>
                        </div>
                        <div class="card-footer">
                            <div class="row">
                                <div class="col">
                                    <asp:Button ID="btnSearch" runat="server" Text="Save" CssClass="btn btn-primary" />
                                </div>
                                <div class="col">
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary" OnClick="btnClear_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



</asp:Content>

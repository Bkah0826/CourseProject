<%@ Page Title="Admin Home" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Admin_Home" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SiteAccess" Runat="Server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.1/Chart.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bluebird/3.3.4/bluebird.min.js%22%3E"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bluebird/3.3.4/bluebird.min.js"></script>

     <style>
         .loader {
            position: fixed;
            top: 47%;
            left: 47%;
            
            opacity:1;            
            color:white;
            width:350px;  
            z-index: 100;             
            text-align:center;
            border: 16px solid white; /* Light grey */
            border-top: 16px solid #3498db; /* Blue */
            border-radius: 50%;
            width: 120px;
            height: 120px;
            animation: spin 2s linear infinite;
          }
            @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
          }

            
         }
              
         .legend {            
            width: 40px;
            height:40px;
         }

         .legendText {
             margin:0 5% 0 1%;
             font-size:10px;
             margin-top:10px;
         }

         h3{
             font-size:19px;
         }
        .containers
        {
            position: relative;
            text-align: center;
            
        }
        .centered
        {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            opacity:1;
            visibility:hidden;
            color:white;
            width:350px;  
            z-index: 100;          
               
        }

         .errorStyle {
             border:solid 1px #468CC8;
             border-radius: 3px;
             padding:2.5%;
             background-color:#468CC8;
         }

        .col-xs-5ths,
        .col-sm-5ths,
        .col-md-5ths,
        .col-lg-5ths {
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 15px;
        }

        .col-xs-5ths {
            width: 20%;
            float: left;
        }

        @media (min-width: 768px) {
            .col-sm-5ths {
                width: 20%;
                float: left;
            }
        }

        @media (min-width: 992px) {
            .col-md-5ths {
                width: 20%;
                float: left;
            }
        }

        @media (min-width: 1200px) {
            .col-lg-5ths {
                width: 20%;
                float: left;
            }
        }
        /*#region Popup styles */
        .deactivate-popup {
            width: 800px;
            background-color: white;
            font-family: 'Raleway', sans-serif !important;   
            text-align : center;
        }

        .deactivate-popup-header {
            background-color: #468CC8;
            color: #fff;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
        }

            .deactivate-popup-header h3 {
                margin: 0;
            }

        .deactivate-popup-body {
            border-left: 2px solid #ddd;
            border-right: 2px solid #ddd;
            border-bottom: 2px solid #ddd;
            border-bottom-left-radius: 8px;
            border-bottom-right-radius: 8px;
        }

        .shadow {
            background-color: rgba(255, 255, 255, 0.4);
        }

        
        #modalContainer {
            background-color: rgba(0, 0, 0, 0.3);
            position: absolute;
            width: 100%;
            height: 100%;
            top: 0px;
            left: 0px;
            z-index: 10000;
            background-image: url(tp.png); /* required by MSIE to prevent actions on lower z-index elements */
            border-left: 1px solid #ddd;
            border-right: 1px solid #ddd;
            border-bottom: 2px solid #ddd;
        }

       #alertBox {
            
            width: 500px;
            min-height: 100px;
            margin-top: 50px;
            background-color: #fff;
            background-repeat: no-repeat;
            background-position: 20px 30px;
            border-bottom-left-radius: 8px;
            border-bottom-right-radius: 8px;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
        }

            #alertBox > p {
                padding: 10px;
            }

            #alertBox > a {
                margin: 15px;
                width: 100%;
                background-color: #286090;
                color: white;
                padding: 5px 20px;
                border-radius: 4px;
                position: relative;
                bottom: 10px;
                left: 205px;
            }

        #alert > a:hover {
            text-decoration: none;
        }

        #modalContainer > #alertBox {
            position: fixed;
        }

        #alertBox h1 {
            padding: 10px 15px;
            margin: 0;
            font: bold 0.9em Raleway;
            background-color: #468CC8;
            color: #FFF;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
            border-left: 1px solid #468CC8;
            border-right: 1px solid #468CC8;
        }


/*#endregion */
    </style>
    
    
    <div id="loading" class="loader" ></div>    
    <div class="row">
        <div class="col-md-12" margin-bottom:2.5%;">
                <h1>Home</h1>                
        </div>
        <div class="col-md-12" style="border-bottom:1px solid #ddd; margin-bottom:2.5%;"><p class="lead" style="font-size:14px;">Welcome to the Meal Satisfaction System's Administration Portal.</p></div>
    </div>    
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />

    <AjaxToolkit:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="discardPass">
    </AjaxToolkit:ConfirmButtonExtender>

    <AjaxToolKit:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="discardPass" OkControlID="btnYes"
                                        CancelControlID="btnNo" BackgroundCssClass="shadow">
    </AjaxToolKit:ModalPopupExtender>
    <asp:Panel ID="pnlPopup" runat="server" CssClass="deactivate-popup" Style="display: none">
        <div class="deactivate-popup-header panel-heading">
            <h3>Confirm Deactivation</h3>
        </div>
        <div class="deactivate-popup-body panel-body">
            <p>Are you sure you want to discard the passcode?</p>
            <p>The passcode will be removed permenantly from circulation.</p>
                                        
            <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="btn btn-success" />
            <asp:Button ID="btnNo" runat="server" Text="No" CssClass="btn btn-danger" />
        </div>
    </asp:Panel>

    <div class="row" style=" display:flex; align-items:center;">            
        <div class="col-md-3" style:"padding-top:2.5%; padding-bottom:2.5%" >
            <h2>Daily Passcode:</h2>
        </div>
        <div class="col-md-3" style="border-left:solid 1px #ddd; padding-top:2.5%; padding-bottom:2.5%" >
            <asp:Label ID="dailyPass" runat="server" Text="Label" style="font-size:32px"></asp:Label>              
        </div>            
        <asp:Button id="discardPass" CssClass="btn btn-danger btn-sm" Text="Discard Passcode" runat="server" OnClick="DiscardPasscode" />                     
    </div>

    <div id= "piResults" class="jumbotron row containers">
        <div class="centered">
            <p id="message" class="centered errorStyle">Loading</p>
        </div>
        
        <div class="row">
            <div class="col-md-12" style="margin-bottom:3%;">
                <h2>Quarterly Survey Results Overview</h2>
            </div>            
        </div>
        
        <div class="row">            
        </div>                            
        <div class="row" style="border-bottom:solid #ddd 1px; margin-bottom:2.5%; text-align:center; padding-bottom:2.5%;">            
            <div class="col-md-3">
                <p id="catLabel1"></p>
                <canvas id="pieChart1" class="" style="width: auto; height:auto; padding: 0;margin: auto; display: block; "> </canvas>
                <p id="pieLabel1" style="font-size:12px; margin-top:5%"></p>
            </div>            
            <div class="col-md-3" style=" border-left:solid 1px #ddd">
                <p id="catLabel2"></p>
                <canvas id="pieChart2" class="" style="width: auto; height:auto; padding: 0;margin: auto; display: block; "> </canvas>
                <p id="pieLabel2" style="font-size:12px;margin-top:5%"></p>
            </div>
            <div class="col-md-3" style=" border-left:solid 1px #ddd">
                <p id="catLabel3"></p>
                <canvas id="pieChart3" class="" style="width: auto; height:auto; padding: 0;margin: auto; display: block; "> </canvas>                
                <p id="pieLabel3" style="font-size:12px;margin-top:5%"></p>
            </div>
            <div class="col-md-3" style=" border-left:solid 1px #ddd">
                <p id="catLabel4"></p>
                <canvas id="pieChart4" class="" style="width: auto; height:auto; padding: 0;margin: auto; display: block; "> </canvas>                
                <p id="pieLabel4"style="font-size:12px;margin-top:5%"></p>
            </div>        
        </div>
        <div class="row" style="text-align:center; ">
            <div class="col-md-3">
                <p id="catLabel5"></p>
                <canvas id="pieChart5" class="" style="width: auto; height:auto; padding: 0;margin: auto; display: block; "> </canvas>            
                <p id="pieLabel5" style="font-size:12px;margin-top:5%"></p>
            </div>            
            <div class="col-md-3" style=" border-left:solid 1px #ddd">
                <p id="catLabel6"></p>
                <canvas id="pieChart6" class="" style="width: auto; height:auto; padding: 0;margin: auto; display: block; "> </canvas>                
                <p id="pieLabel6" style="font-size:12px;margin-top:5%"></p>
            </div>
            <div class="col-md-3" style=" border-left:solid 1px #ddd">
                <p id="catLabel7"></p>
                <canvas id="pieChart7" class="" style="width: auto; height:auto; padding: 0;margin: auto; display: block; "> </canvas>                
                <p id="pieLabel7" style="font-size:12px;margin-top:5%"></p>
            </div>
            <div class="col-md-3" style=" border-left:solid 1px #ddd">
                <p id="catLabel8"></p>
                <canvas id="pieChart8" class="" style="width: auto; height:auto; padding: 0;margin: auto; display: block; "> </canvas> 
                <p id="pieLabel8" style="font-size:12px;margin-top:5%"></p>
            </div>        
        </div>
        <div class="row" style=" display:flex; border-top:solid 1px #ddd; margin-top:2.5%; padding-top:2.5%;" >            
            <%--<div class="col-md-2">                
            </div>
            --%>                        
            <div class="col-md-12" style="display:flex; justify-content:center;">                
                    <div class="legend" id="legend1"></div><span id="legendText1" class="legendText"></span>
                    <div class="legend" id="legend2"></div><span id="legendText2" class="legendText"></span>
                    <div class="legend" id="legend3"></div><span id="legendText3" class="legendText"></span>
                    <div class="legend" id="legend4"></div><span id="legendText4" class="legendText"></span>                                
            </div>            
            <%--<div class="col-md-2">
                <div class="col-md-12">                                  
                </div>                
            </div>--%>
        </div> 
    </div>
    

     <div id= "satisfactionPercentiles" class="jumbotron row containers">
         <div class="row">
             <div class="col-md-12" style="margin-bottom:3%;">
                 <h2>Overall Satisfaction</h2>
             </div>
         </div>
         <div class="row">
                <div class="col-md-5ths">
                    <h3>Surveys Taken</h3>
                    <h2 id="qrtSurveysAmt"></h2>
                </div>
                <div class="col-md-5ths" style="border-left:solid 1px #ddd;">
                    <h3>Very Good & Good</h3>
                    <h2 id="vgpercentile"></h2>
                </div>
                <div class="col-md-5ths" style="border-left:solid 1px #ddd;">
                    <h3>Satisfactory</h3>
                    <h2 id="satPercentile"></h2>
                </div>
                <div class="col-md-5ths" style="border-left:solid 1px #ddd;">
                    <h3>Needs Improvement</h3>
                    <h2 id="poorPercentile"></h2>
                </div>
                <div class="col-md-5ths" style="border-left:solid 1px #ddd;">
                    <h3>No Opinion</h3>
                    <h2 id="noOpinionPercentile"></h2>
                </div>
            </div>
        </div>

        
        <div id="vBarResults" class="row jumbotron containers" style="padding-top:2.5%;">

            <div class="centered">
                <p id="message01" class="centered errorStyle"></p>
            </div>    
            <div class="col-md-6">
                    <p>Quarterly Satisfaction Percentile</p>
                    <canvas id="unitSatisfaction" class="" style="width: auto; height:auto; padding: 0;margin: auto; display: block; "> </canvas>
                </div>
                <div class="col-md-6">
                    <p>Quarterly Surveys Taken by Unit</p>
                    <canvas id="surveysTaken" class="" style="width: auto; height:auto; padding: 0;margin: auto; display: block; "> </canvas>
                <div class="col-md-12" id="testing1"></div>                
             </div>
        </div>
        <div class="row" style="">
            <div class="col-md-12">
                <canvas id="TestLineChart" class="" style="width: auto; height:auto; padding: 0;margin: auto; display: block; "> </canvas>
            </div>       
        </div>
    <script src="../Scripts/custom/home.js"></script>
    <script>
        $("document").ready(load);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="AdminLogin" Runat="Server">
</asp:Content>
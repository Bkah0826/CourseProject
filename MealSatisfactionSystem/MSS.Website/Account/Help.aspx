<%@ Page Title="Help Manual" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Help.aspx.cs" Inherits="Account_Help" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
     /* Android 2.3 :checked fix */
    @keyframes fake {
        from {
            opacity: 1;
        }
        to {
            opacity: 1
        }
    }
    body {        
        animation: fake 1s infinite;
    }

    .radio-tabs .state {
        position: absolute;
        left: -10000px;
    }

    .radio-tabs .tab {
        display: inline-block;
        padding: .5em;
        vertical-align: top;
        background-color: #fff;
        cursor: hand;
        cursor: pointer;
    }
    .radio-tabs .tab:hover {
        background-color: #fff;
    }
    #help-manual[aria-selected] ~ .tabs #help-manual-tab,
    #samples[aria-selected] ~ .tabs #samples-tab,    
    #help-manual:checked ~ .tabs #help-manual-tab,
    #samples:checked ~ .tabs #samples-tab {
        background-color: #fff;
        border: 1px solid #c6c6c6;
        border-radius: 4px;
        border-bottom: .3em solid #fff;
        cursor: default;
    }

    .radio-tabs .panels {
        background-color: #fff;
        padding: .5em;
    }
    .radio-tabs .panel {
        display: none;
    }
    #help-manual[aria-selected] ~ .panels #help-manual-panel,
    #samples[aria-selected] ~ .panels #samples-panel,    
    #help-manual:checked ~ .panels #help-manual-panel,
    #samples:checked ~ .panels #samples-panel{
        display: block;
    }

    h1{
        margin-bottom: 50px;
        margin-top: 0;
        border-bottom: 1px solid #ddd;        
    }
    </style>

    <h1>User Help Manual</h1>
    <div class="radio-tabs" role="tablist">
  
    <input class="state" type="radio" title="Help Manual" name="houses-state" id="help-manual" role="tab" aria-controls="help-manual-panel" aria-selected="true" checked />
    <input class="state" type="radio" title="Samples" name="houses-state" id="samples" role="tab" aria-controls="samples-panel" />    

    <div class="tabs" aria-hidden="true">
        <label for="help-manual" id="help-manual-tab" class="tab" aria-selected="true">Help Manual</label>
        <label for="samples" id="samples-tab" class="tab">Samples</label>
    </div>

    <div class="panels">
        <ul id="help-manual-panel" class="panel active" role="tabpanel" aria-labelledby="help-manual-tab">
            <div>
                <embed src="../Content/userdocs/UserDocumentation.pdf"  style="width:90%;height:1000px;">
            </div>
        </ul>
        <ul id="samples-panel" class="panel" role="tabpanel" aria-labelledby="samples-tab">
            <div>
                <embed src="../Content/userdocs/dynamicChartAppearance_2018-03-27_37.pdf"  style="width:90%;height:1000px;">
                <embed src="../Content/userdocs/WeightedPercentiles_2018-04-21_04.pdf"   style="width:90%;height:1000px;">
            </div>
        </ul>
    </div>

</div>

    <script>
        $('.state').change(function () {
            $(this).parent().find('.state').each(function () {
                if (this.checked) {
                    $(this).attr('aria-selected', 'true');
                } else {
                    $(this).removeAttr('aria-selected');
                }
            });
        });
    </script>

</asp:Content>

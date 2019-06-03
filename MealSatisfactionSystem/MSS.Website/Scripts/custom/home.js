/// <summary>
/// On load deliver all functions.
/// </summary>
function load() {
    var ALERT_TITLE = "Oops!"; //ALERT_TITLE is the title of the alert box presented at an error
    var ALERT_BUTTON_TEXT = "Ok"; //ALERT_BUTTON_TEXT is the text of the alert box presented at an error

    //loading screen animation activator
    $(document).ajaxStop(function () {
        $(".loader").css({ opacity: 1.0, visibility: "visible" }).animate({ opacity: 0 }, 400);
    });

    // If there is a server error returned, show the custom popup, and disable the rest of the page.
    function serverException(errorMessage, overlay) {
        $("#chartBody .loader").addClass("hidden");

        alert("Error: " + errorMessage + ". \n Try reloading the page. If the problem persists contact your system administrator.");

    }


    //removes the alert
    function removeCustomAlert() {
        document.getElementsByTagName("body")[0].removeChild(document.getElementById("modalContainer"));
    }

    /// <summary>
    /// Adds a custiom alert() that matches the ones on the other pages
    /// </summary>
    /// <param name="txt">Contains an exception message to communicate.</param>
    function createCustomAlert(txt) {
        d = document;

        if (d.getElementById("modalContainer")) return;

        mObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
        mObj.id = "modalContainer";
        mObj.style.height = d.documentElement.scrollHeight + "px";

        alertObj = mObj.appendChild(d.createElement("div"));
        alertObj.id = "alertBox";
        if (d.all && !window.opera) alertObj.style.top = document.documentElement.scrollTop + "px";
        alertObj.style.left = (d.documentElement.scrollWidth - alertObj.offsetWidth) / 2 + "px";
        alertObj.style.visiblity = "visible";

        h1 = alertObj.appendChild(d.createElement("h1"));
        h1.appendChild(d.createTextNode(ALERT_TITLE));

        msg = alertObj.appendChild(d.createElement("p"));        
        msg.innerHTML = txt;

        btn = alertObj.appendChild(d.createElement("a"));
        btn.id = "closeBtn";
        btn.appendChild(d.createTextNode(ALERT_BUTTON_TEXT));
        btn.href = "#";
        btn.focus();
        btn.onclick = function () { removeCustomAlert(); return false; }

        alertObj.style.display = "block";

    }

    /// <summary>
    /// Sorts data of arrays by unitID into ascending order
    /// </summary>
    /// <param name="arrayofUnits01">Contains the first list of object to sort by unit ID.</param>
    /// <param name="arrayofUnits02">Contains the second list of object to sort by unit ID.</param>
    /// <returns>The integer signaling if the on the left is greater, less than, or equal to the value on the right.</returns>
    function compare(arrayofUnits01, arrayofUnits02) {
        if (arrayofUnits01.unitID < arrayofUnits02.unitID)
            return -1;
        if (arrayofUnits01.unitID > arrayofUnits02.unitID)
            return 1;
        return 0;
    }
    //---

    /// <summary>
    /// Detects if the user is using Internet Explorer or Edge.
    /// </summary>
    function detectIE() {
        var ua = window.navigator.userAgent;

        var msie = ua.indexOf('MSIE ');
        if (msie > 0) {
            // IE 10 or older => return version number
            return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
        }

        var trident = ua.indexOf('Trident/');
        if (trident > 0) {
            // IE 11 => return version number
            var rv = ua.indexOf('rv:');
            return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
        }

        var edge = ua.indexOf('Edge/');
        if (edge > 0) {
            // Edge (IE 12+) => return version number
            return parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
        }
        // other browser
        return false;
    }

    /// <summary>
    /// Sets the legend of the pie charts based on colours and text which are set within the function.
    /// </summary>
    function setLegend() {
        colors = ["rgba(70, 140, 200,1)", "rgba(217, 99, 97,1)", "rgba(255, 255, 255,1)", "rgba(220, 220, 220)"]; //all colours of the pie chart segments
        legenText = ["Average/Above Average", "Below Average", "No Opinion", "No Response"]; //label list for pie chart
        for (var i = 1; i < colors.length + 1; i++) {
            document.getElementById('legend' + i).style.backgroundColor = colors[i - 1];
            document.getElementById('legend' + i).style.height = "33px";
            document.getElementById('legend' + i).style.width = "33px";
            document.getElementById('legendText' + i).innerText = legenText[i - 1];
        }
    }

    /// <summary>
    /// Calculates the ratio of QuestionRespones in the Mainarray over the total amunt of items based on parameter.
    /// </summary>
    /// <param name="mainArray">This array's count will serve as the numerator of the ratio. It is the desirable collection of results.</param>
    /// <param name="secondaryParameters">This array items (QuestionResponse) are counted if they have the desirable parameter.</param>
    /// <param name="otherParameters">This array items (QuestionResponse) are counted if they have the desirable parameter.</param>
    /// <param name="finalParameters">This array items (QuestionResponse) are counted if they have the desirable parameter.</param>
    /// <param name="param">This parameter represents the question parameter being searched for in the QuestionResponses.</param>
    /// <param name="counter">A reusable counter.</param>
    /// <param name="total">A reusable total counter used to properly sum the results.</param>
    /// <returns>The ratio of desired item over total items of same parameter.</returns>
    function resultsPercentiles(mainArray, secondaryParameters, otherParameters, finalParameters, param, counter, total) {
        //total being the total amount of valid responses for a particular question based on parameter
        for (var i = 0; i < mainArray.length; i++) {
            if (mainArray[i].questionParam == param) {
                counter++
                total++;
            }                
        }
        for (var i = 0; i < secondaryParameters.length; i++) {
            if (secondaryParameters[i].questionParam == param) {
                total++;
            }

        }
        for (var i = 0; i < otherParameters.length; i++) {
            if (otherParameters[i].questionParam == param) {
                total++;
            }

        }
        for (var i = 0; i < finalParameters.length; i++) {
            if (finalParameters[i].questionParam == param) {
                total++;
            }

        }
        return (counter / total);
    }

    /// <summary>
    /// Calculates the waited percentiles of each of the relevant questions.
    /// </summary>
    /// <param name="data">This array's count will serve as the numerator of the ratio. It is the desirable collection of results.</param>
    function setPercentileLabel(data) {
        var aboveMeanCounter = []; //The array to store all QuestionResponses that have a value above mean.
        var meanCounter = []; //The array to store all QuestionResponses that have a value equal to mean.
        var belowMeanCounter = []; //The array to store all QuestionResponses that have a value below mean.
        var noOpinionCounter = []; //The array to store all QuestionResponses that have a value of 0.
        var unanswered = []; //The array to store all QuestionResponses that have a value of -1.
        var paramList = ["Taste", "Appearance", "Variety", "Temperature", "Helpfulness", "Portions"]; //List of parameters of interest.
        var weight = [.3, .25, .15, .15, .10, .05]; //weight scale for weighted average.
        var totalValidSurveys = 0; //initiate counter of valid surveys.

        //sort all relevant responses into declared arrays.
        for (var i = 0; i < data.d.length; i++)
        {        
            if (data.d[i].removed == false) {
                totalValidSurveys++;
                if (data.d[i].maxValue % 2 == 0 && data.d[i].value > 0) //if even amt of possible answers
                {
                    if (data.d[i].value > data.d[i].maxValue / 2) { //above mean
                        aboveMeanCounter.push(data.d[i]);
                    }
                    else if (data.d[i].value == data.d[i].maxValue / 2) { //mean
                        meanCounter.push(data.d[i]);
                    }
                    else{
                        belowMeanCounter.push(data.d[i]); //below mean                    
                    }
                }
                else if (data.d[i].value == 0) {
                    noOpinionCounter.push(data.d[i]);
                }
                else if (data.d[i].maxValue % 2 == 1 && data.d[i].value > 0) {
                    if (data.d[i].value > data.d[i].maxValue / 2) { //above mean
                        aboveMeanCounter.push(data.d[i]);
                    }
                    else if (data.d[i].value < data.d[i].maxValue) { //mean
                        belowMeanCounter.push(data.d[i]);  
                    }
                    else {
                        meanCounter.push(data.d[i]); //mean
                    }
                }
                else {
                    unanswered.push(data.d[i]);
                }            
            }        
        }

        totalValidSurveys = totalValidSurveys / 8; //calculate Surveys taken
        document.getElementById('qrtSurveysAmt').innerText = totalValidSurveys;
    
        //init of counters that represent the final calculated percent values.
        satsfactionofVeryGoodandGoods = 0;
        satsfactionofSatisfactory = 0;
        needsImprovement = 0;
        noOpinion = 0;

        //generates a column as seen in the excel doc of quarterly report
        for (var i = 0; i < paramList.length; i++) {
            veryGoodsandGoods = resultsPercentiles(aboveMeanCounter, meanCounter, belowMeanCounter, noOpinionCounter, paramList[i], 0.0, 0.0);
            fairs = resultsPercentiles(meanCounter, aboveMeanCounter, belowMeanCounter, noOpinionCounter, paramList[i], 0.0, 0.0);
            poors = resultsPercentiles(belowMeanCounter, meanCounter, aboveMeanCounter, noOpinionCounter, paramList[i], 0.0, 0.0);
            noOpinions = resultsPercentiles(noOpinionCounter, belowMeanCounter, meanCounter, aboveMeanCounter, paramList[i], 0.0, 0.0);
            if (!isNaN(veryGoodsandGoods) && veryGoodsandGoods != 0) {
                satsfactionofVeryGoodandGoods = satsfactionofVeryGoodandGoods + veryGoodsandGoods * weight[i];
            }
            if (!isNaN(fairs) && fairs != 0) {
                satsfactionofSatisfactory = satsfactionofSatisfactory + fairs * weight[i];
            }       
            if (!isNaN(poors) && poors !=0) {
                needsImprovement = needsImprovement+ (poors * weight[i]);
            }
            if (!isNaN(noOpinions) && noOpinions != 0) {            
                noOpinion = noOpinion + (noOpinions * weight[i]);    
            }
        }
    
        if (isNaN(totalValidSurveys)) { //error check            
            document.getElementById('vgpercentile').innerText = "No valid data yet";
            document.getElementById('satPercentile').innerText = "No valid data yet";
            document.getElementById('poorPercentile').innerText = "No valid data yet";
            document.getElementById('noOpinionPercentile').innerText = "No valid data yet";        
        }
        else { // assign values to wepage.         
            document.getElementById('vgpercentile').innerText = ((satsfactionofVeryGoodandGoods) * 100).toFixed(1) + "%";
            document.getElementById('satPercentile').innerText = ((satsfactionofSatisfactory) * 100).toFixed(1) + "%";
            document.getElementById('poorPercentile').innerText = ((needsImprovement) * 100).toFixed(1) + "%";
            document.getElementById('noOpinionPercentile').innerText = ((noOpinion) * 100).toFixed(1) + "%";
        }    
    }
    
    /// <summary>
    /// Collects appropriate data and creates a pie chart to display the percentiles of responses that answered based on parameter.
    /// </summary>
    /// <param name="data">This is a list of all data.</param>
    /// <param name="number">Number of pie charts to make.</param>
    function createPie(data, number) {
        //Overall Satisfaction Chart
        canvas = document.getElementById('pieChart' + (number)); //Selector of canvas based  on number.
        summary = document.getElementById('pieLabel' + (number)); //Selector of summary text based  on number.
        label = document.getElementById('catLabel' + (number)); //Selector of label based  on number.
        var color = "rgba(220,220,220,1)";//Color for all segments in the current pie chart.
        canvas.width = canvas.clientWidth;//This is to make the chart look clear.
        canvas.height = canvas.clientHeight;//Without, the visuals are blurry.

        var answersFrequency = [];//Stores frequency of answers. Main data source for JS chart.
        var colors = [];//Array used for dynamic storage of all colors.
        var bordercolors = [];//Array used for dynamic storage of all border colors.
        //---------
        var vals = [];//Filtered source array: stores appropriate JSON values used for the function.
        var ans = [];//Comparative array: stores keys; the values being measured.    
        var text = [];//Stores labels for each segment in the pie chart.
        var count = 0;//Counts all valid QuestionResponses above or equal to mean.
        var total = 0;//

        //grab appropriate responses with an answer associated with it                
        for (var i = 0; i < data.d.length; i++) {
                if (data.d[i].questionID == number && data.d[i].removed == false) { 
                    vals.push(data.d[i]);
                }
        }

        //find responses above or equal to mean value
        for (var i = 0; i < vals.length; i++) {
            if (vals[i].value >= (vals[i].maxValue)/2) {
                count++
            };        
        }
    
        //max value of question    
        //JSON Object: Frequency of answers by answer text
        //sums
        var y = vals.reduce(function (sums, entry) {
            sums[entry.value] = (sums[entry.value] || 0) + 1;
            return sums;
        }, {});
        
        // unique sums of Answer frequency
        // unique array of Answers strings        
        Object.keys(y).forEach(function (key) {
            ans.push(key);                
            answersFrequency.push(y[key]/vals.length*100);        
        });

        label.innerText = vals[0].questionParam; // set column labels

        //Set colors and labels
        var ie = detectIE();

        for (var i = 0; i < ans.length; i++) {
            for (var j = 0; j < vals.length; j++)
            {
                if (vals[j].value == ans[i]) {
                    if (vals[j].value >= 0) {
                        text.push(vals[j].answers);
                    }                
                    else {
                        text.push("No Response");
                    }
                    
                    if (vals[j].value < (vals[j].maxValue/2) && vals[j].value > 0){
                        colors.push("rgba(217, 99, 97,1)");
                        if (ie) {
                            bordercolors.push(color);
                        }
                        else {                            
                            bordercolors.push("rgba(217, 99, 97,1)");
                        }
                        
                    }
                    else if (vals[j].value == 0) {
                        colors.push("rgba(255, 255, 255,1)");
                        if (ie) {
                            bordercolors.push(color);
                        }
                        else {
                            
                            bordercolors.push("rgba(255, 255, 255,1)");
                        }
                        
                    }

                    else if (vals[j].value >= (vals[j].maxValue / 2)) {
                        colors.push("rgba(70, 140, 200,1)");
                        if (ie) {
                            bordercolors.push(color);
                            
                        }
                        else {
                            bordercolors.push("rgb(70, 140, 200,1)");
                        }
                        
                    }
                    else {
                        colors.push("rgba(220, 220, 220,1)");
                        bordercolors.push("rgb(220, 220, 220)");
                    }                
                    break;
                }
            }                
        }

        //set summaries
        var average = (count / vals.length); // text of the percentile of responses above or equal average.
        summary.innerText = (average * 100).toFixed(1) + "%" + " equal or above mean";

        var ctx = canvas.getContext('2d');   
        //PIE
        var myPieChart = new Chart(ctx, {
            type: 'pie',
            data: {
                datasets: [{
                    data: answersFrequency,
                    backgroundColor: colors,
                    borderColor: bordercolors,
                    borderWidth: 1,                
                }],            
            },       
            options:            
                {
                    tooltips: {
                        mode: 'point',
                        callbacks: {                        
                            label: function (t, d) {
                                return text[t['index']] + ": " + (d['datasets'][0]['data'][t['index']]).toFixed(1) + '%';
                            }
                        }
                    },                        
                legend: {
                    display: false

                }
            },
        });
    }

    /// <summary>
    /// Collects appropriate data and creates a vertical bar chart to display averages based on values over maxValue per unit.
    /// </summary>
    /// <param name="data">This is a list of all data.</param>
    function createVBar(data) {
        //Overall Satisfaction Chart
        canvas = document.getElementById('unitSatisfaction');
        canvas.width = canvas.clientWidth; //This is to make the chart look clear.
        canvas.height = canvas.clientHeight; //Without, the visuals are blurry.

        var units = []; //Stores all the units based on QuestionResponse.
        var unitScores = []; // Stores all the scores of the units.
        var averages = []; //Stores all the unit's average scores.
        var maxVals = []; // Stores each maximum scoreable point per QuestionResponse.
        var value = 0; // The overall average of a single unit being processed.        
        var bordercolors = []; //All bar border colours to be used in the bar chart.
        var barColors = []; //All bar charts' bar colours.
        var unitNames = []; //List of the units' names.
        
        //Strip JSON Value and remove duplicates
        //sums 

        var surveyUnitCounter = 0; // will be used to count surveys
        data.d.sort(compare);
        //grab data's unit ID, unit Name, value, and the max Value
        for (var i = 0; i < data.d.length; i++) {

            if (data.d[i].removed == false) {
                units.push(data.d[i].unitID);
                unitNames.push(data.d[i].unitName);
                unitScores.push((data.d[i].value));
                maxVals.push((data.d[i].maxValue));
            }
        
        }

        

        var unitTracker = 0; //Track when a unit changes while going through loop.
        maxScore = 0; //Find each unit's total max score.
        for (var i = 0; i <= units.length; i++) {
                //Find Unit Satisfaction
                if (unitScores[i] > 0) {                
                    value = value + (unitScores[i]);
                    maxScore = maxScore + maxVals[i];
                }
            
                //when unit changes
                if (i <= units.length && units[i] != units[i + 1] && units[i]) {                
                    averages.push(100 * parseFloat(value) / parseFloat(maxScore));
                    //set colours
                    if (parseFloat(value) / parseFloat(maxScore) < 0.50 && parseFloat(value) / parseFloat(maxScore) >= 0) {
                        barColors.push("rgba(217, 99, 97,1)");
                    }
                    else if (parseFloat(value) / parseFloat(maxScore) == 1) {
                        barColors.push("rgba(96,183,96,1)");
                    }
                
                    else {
                        barColors.push("rgb(70, 140, 200)");
                    }
                    //reset
                    value = 0;
                    unitTracker++;
                    maxScore = 0;
                    surveyUnitCounter = 0;
                }
            };
    
        new Chart(document.getElementById("unitSatisfaction"), {
        type: 'horizontalBar',
        data: {
            labels: unitNames.filter(function (item, pos) {
                    return unitNames.indexOf(item) == pos
            }),
            datasets: [
              {
                  label: "Satisfaction (percent)",
                  backgroundColor: barColors,
                  data: averages
              }
            ]
        }
        ,
        options: {
            tooltips: {
                mode: 'point',
                callbacks: {                        
                    label: function (t, d) {
                        return (d['datasets'][0]['data'][t['index']]).toFixed(1) + '%';
                    }
                }
            },
            legend: { display: false },
            title: {
                display: true,
                text: 'Quarterly Satisfaction Percentiles based on Unit'
            },
            scales: {
                xAxes: [{
                    ticks: {
                        beginAtZero: true,
                        min: 0,
                        max:100
                    },                                              
                }]
            }
        }
    });
    
    }

    /// <summary>
    /// Collects appropriate data and creates a vertical bar chart to display survey count per unit.
    /// </summary>
    /// <param name="data">This is a list of all data.</param>

    function createSurveyCountBar(data) {
        //Overall Satisfaction Chart
        canvas = document.getElementById('surveysTaken');
        canvas.width = canvas.clientWidth; //This is to make the chart look clear.
        canvas.height = canvas.clientHeight; //Without, the visuals are blurry

        var units = [];//Stores all the units based on QuestionResponse
        var barColors = [];//Stores all bar charts' bar colours.
        var unitNames = [];//Stores all bar charts' bar labels.


        //sums 

        var unitSurveyCount = [];//Stores the amount of surveys done of all units.
        var surveyUnitCounter = 0;//Stores the count of surveys done in one unit.

        data.d.sort(compare);//sort Source for optimization and accuracy

        
        for (var i = 0; i < data.d.length; i++) {
            if (data.d[i].removed == false) {
                
                units.push(data.d[i].unitID);
                unitNames.push(data.d[i].unitName);
                
            }
        
        }

        for (var i = 0; i <= units.length; i++) {
            surveyUnitCounter++;
            //If a score is below or equal to 0        
            //when unit changes
            if (i <= units.length && units[i] != units[i + 1]) {
                unitSurveyCount.push(surveyUnitCounter / 8);            
                //set colours            
                barColors.push("rgb(70, 140, 200)");            
                //reset            
                surveyUnitCounter = 0;
            }
        };
        
        new Chart(document.getElementById("surveysTaken"), {
            type: 'horizontalBar',
            data: {
                labels: unitNames.filter(function (item, pos) {
                    return unitNames.indexOf(item) == pos
                }),
                datasets: [
                  {
                      label: "Surveys Taken",
                      backgroundColor: barColors,
                      data: unitSurveyCount
                  }
                ]
            }
        ,
            options: {
                legend: { display: false },
                title: {
                    display: true,
                    text: 'Quarterly Count of Surveys by Unit'
                },
                scales: {
                    xAxes: [{
                        ticks: {
                            beginAtZero: true,
                            min: 0,
                            fixedStepSize: 1
                        },
                    }]
                }
            }
        });

    }//end VBAR

    //Main program
        //If no data is returned, display no data msg
        Chart.plugins.register({
            afterDraw: function (chart) {
                var test = chart.data.datasets[0].data.length
                if (test === 0) {
                    // No data is present
                    var ctx = chart.chart.ctx;
                    var width = chart.chart.width;
                    var height = chart.chart.height
                    chart.clear();

                    ctx.save();
                    ctx.textAlign = 'center';
                    ctx.textBaseline = 'middle';
                    ctx.font = "16px normal 'Raleway'";
                    ctx.fillText('No data to display', width / 2, height / 2);
                    ctx.restore();
                }
            }
        });

    $("document").ready(function () {        
            $.ajax(
                {
                    type: "POST",
                    page: 1,
                    rp: 6,
                    url: 'home.aspx/ChartDataRequest',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        setPercentileLabel(result);
                        createVBar(result);
                        createSurveyCountBar(result);
                        setLegend();
                        try {
                            for (var i = 1; i <= 8; i++) {
                                createPie(result, i);
                            }                            
                        }
                        catch (e) {                            
                            $("#piResults").children().css({ "opacity": 0.3 });
                            $("#vBarResults").children().css({ "opacity": 0.3 });
                            $(".centered").css({ "opacity": 1, "visibility": "visible" });
                            $("#message").text("No surveys for the quarter yet")
                            $("#message01").text("No surveys for the quarter yet")
                        }                                            
                    },
                    error: function (x, e) {                        
                        if (x.responseJSON["Message"] == "Error during serialization or deserialization using the JSON JavaScriptSerializer. The length of the string exceeds the value set on the maxJsonLength property.") {
                            var message = "Server Error: Too much data requested. Please try your search again with a narrower search."
                            serverException(message, false)
                        } else {
                            var message = x.statusText
                            serverException(message, true)
                        }
                    }
                    });       
            window.alert = function (txt) {
                createCustomAlert(txt);
            }       
    });
}
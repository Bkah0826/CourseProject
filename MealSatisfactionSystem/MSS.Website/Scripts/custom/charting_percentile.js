var Charting_Percentile = function () {

    /// Template Region ///

    // Uses the extended string method to fill the table with string interpolation
    var table = '<div class="row" style=\"font-family:Raleway\">\
                    <div class="col-md-5ths" >\
                        <h3>Surveys Taken</h3>\
                        <h2 id="qrtSurveysAmt">{0}</h2>\
                    </div>\
                    <div class="col-md-5ths" style="border-left:solid 1px #ddd;">\
                        <h3>Very Good &amp; Good</h3>\
                        <h2 id="vgpercentile">{1}</h2>\
                    </div>\
                    <div class="col-md-5ths" style="border-left:solid 1px #ddd;">\
                        <h3>Satisfactory</h3>\
                        <h2 id="satPercentile">{2}</h2>\
                    </div>\
                    <div class="col-md-5ths" style="border-left:solid 1px #ddd;">\
                        <h3>Needs Improvement</h3>\
                        <h2 id="poorPercentile">{3}</h2>\
                    </div>\
                    <div class="col-md-5ths" style="border-left:solid 1px #ddd;">\
                        <h3>No Opinion</h3>\
                        <h2 id="noOpinionPercentile">{4}</h2>\
                    </div>\
                </div >'

    /// Variables Region ///
    var ALERT_TITLE = "Oops!";
    var ALERT_BUTTON_TEXT = "Ok";
    var url = "ViewReports.aspx/PercentileDataRequest"

    /// API Region ///

    /// <summary>Calls a .NET Webmethod to retrieve data </summary>  
    /// <param name="collection" type="object">Collection of filters gathered from page</param>  
    /// <param name="url" type="string">Contains a URL to the required webservice</param>  
    /// <returns type="Promise">Contains a Promise of data objcet</returns>  
    function callAPI(collection, url) {
        return new Promise(function (resolve, reject) {
            $.ajax(
                {
                    type: "POST",
                    page: 1,
                    rp: 6,
                    url: url,
                    dataType: "json",
                    data: collection,
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        var data = result["d"]
                        if (data.length > 1) {
                            if (data[0].hasOwnProperty("Id") && data[0]["Id"] == -1)
                                serverException(result["d"][0]["Description"])
                            else
                                resolve(result)
                        } else {
                            resolve(result)
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
                        reject(message)
                    }
                });
        });
    }

    /// <summary>Handles any sever errors returned from the callAPI</summary>  
    /// <param name="errorMessage" type="string">Contains an exception message to communicate</param>
    /// <param name="overlay" type="bool">Contains a bool determining if the page should be disabled</param>      
    function serverException(errorMessage, overlay) {
        $("#chartBody .loader").addClass("hidden")

        alert("Error: " + errorMessage + ". \n Try reloading the page. If the problem persists contact your system administrator.")

        if (overlay) {
            $("body").prepend("<div class=\"overlay\"><p class=\"serverError\">Server Error</p></div>");

            $(".overlay").css({
                "position": "absolute",
                "width": $(document).width(),
                "height": $(document).height(),
                "z-index": 9999,
                "background-color": "white"
            }).fadeTo(0, 0.8);
            $(".serverError").css({
                "width": "100px",
                "margin": "200px auto"
            })
        }
    }

    /// <summary>Builds a custom alert element for later display.</summary>  
    /// <param name="txt" type="string">Contains an exception message to communicate</param>
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


    function removeCustomAlert() {
        document.getElementsByTagName("body")[0].removeChild(document.getElementById("modalContainer"));
    }

    /// Page Function Region ///

    /// <summary>Clears the head and body of the reports container</summary>      
    function clearPage() {
        $("#weightedHead").empty()
        $("#weightedBody").empty()
    }

    /// <summary>Retrieves the values from the date pickers</summary>     
    /// <returns>Array holding the two dates</returns>
    function getDates() {
        toDate = $("#toDatePicker_p").datepicker("getDate")
        toDate = toDate == null ? new Date() : toDate //Webservice can't handle null
        fromDate = $("#fromDatePicker_p").datepicker("getDate")
        fromDate = fromDate == null ? $("#fromDatePicker_p").datepicker("option", "minDate") : fromDate

        return [toDate, fromDate]
    }

    /// <summary>Builds the weighted percentile report table.</summary>  
    /// <param name="result" type="object">Contains the data retrieved from the server</param>
    /// <param name="toDate" type="datetime">Contains the end date filter</param>
    /// <param name="fromDate" type="datetime">Contains the start date filter</param>   
    function buildTable(result, toDate, fromDate) {
        var siteName = $('#MainContent_SiteDD_p').find(":selected").text();
        var text = "Weighted Percentiles for range of {0} to {1} {2}".format(moment(fromDate).format("MMMM Do YYYY"), moment(toDate).format("MMMM Do YYYY"),
            siteName == ""? "" : "for " + siteName)
        $("#weightedHead").append(text)
        $("#weightedHead").css("font-family", "Raleway")
        setPercentileLabel(result)
    }

    /// <summary>Creates a downloadable version of the displayed report</summary>      
    function downloadTable() {
        $("#loader_p").removeClass('hidden') //loader
        $("#saved_p").addClass('hidden')
        $("#failure_p").addClass('hidden')

        try{
            html2canvas($("#weightedContainer")).then(function (canvas) {
                var img = canvas.toDataURL();

                var pdf = new jsPDF({
                    orientation: 'landscape'
                });
                pdf.internal.scaleFactor = 3;
                pdf.addImage(img, 'jpeg', 0, 20, 300, 50);
                pdf.setFont("helvetica");
                pdf.setFontType("normal");
                pdf.setFontSize(9);
                pdf.text(5, 10, "Generated on: " + moment().format("YYYY-MM-DD hh:mm:ss"))
                pdf.save("WeightedPercentiles_" + moment().format("YYYY-MM-DD_ss") + ".pdf");
                
                $("#loader_p").addClass('hidden')
                $("#saved_p").removeClass('hidden')
            });
        } catch(e){
            $("#loader_p").addClass('hidden')
            $("#failure_p").removeClass('hidden')
        }
    }

    /// Build Table Region///

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
    /// Calculates the weighted percentiles of each of the relevant questions.
    /// </summary>
    /// <param name="data">This array's count will serve as the numerator of the ratio. It is the desirable collection of results.</param>
    function setPercentileLabel(data) {
        var aboveMeanCounter = []; //The array to store all QuestionResponses that have a value above mean.
        var meanCounter = []; //The array to store all QuestionResponses that have a value equal to mean.
        var belowMeanCounter = []; //The array to store all QuestionResponses that have a value below mean.
        var noOpinionCounter = []; //The array to store all QuestionResponses that have a value of 0.
        var unanswered = []; //The array to store all QuestionResponses that have a value of -1.
        var paramList = ["Taste", "Appearance", "Variety", "Temperature", "Helpfulness", "Portions"]; //List of parameters of interest.
        var weight = [.3, .25, .15, .15, .10, .05]; //weight scale for weied average
        var totalValidSurveys = 0; //initiate counter of valid surveys.

        //sort all relevant responses into declared arrays.
        for (var i = 0; i < data.d.length; i++) {
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
                    else {
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

        //init of counters that represent the final calculated percent values.
        satsfactionofVeryGoodandGoods = 0;
        satsfactionofSatisfactory = 0;
        needsImprovement = 0;
        noOpinion = 0;

        //grnerates a column as seen in the excel doc of quarterly report
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

            if (!isNaN(poors) && poors != 0) {
                needsImprovement = needsImprovement + (poors * weight[i]);
            }

            if (!isNaN(noOpinions) && noOpinions != 0) {
                noOpinion = noOpinion + (noOpinions * weight[i]);

            }
        }

        if (isNaN(totalValidSurveys)) { //error check
            var percentileLabel, vgpercentile, satPercentile, poorPercentile, noOpinionPercentile = "No Data Available."
            $("#weightedBody").append(table.format(totalValidSurveys, vgPercentile, satPercentile, poorPercentile, noOpinionPercentile))

        }
        else { // assign values to wepage.         
            var vgPercentile = ((satsfactionofVeryGoodandGoods) * 100).toFixed(1) + "%";
            var satPercentile = ((satsfactionofSatisfactory) * 100).toFixed(1) + "%";
            var poorPercentile = ((needsImprovement) * 100).toFixed(1) + "%";
            var noOpinionPercentile = ((noOpinion) * 100).toFixed(1) + "%";

            $("#weightedBody").append(table.format(totalValidSurveys, vgPercentile, satPercentile, poorPercentile, noOpinionPercentile))

        }

    }

    /// Buttons Region ///

    /// <summary>Initiates the generation of the report</summary>  
    $("#generate_p").click(function (e) {
        e.preventDefault()
        clearPage()
        $("#weightedContainer .loader").removeClass("hidden")
        $("#download_p").addClass("hidden")

        var data = {
            "siteId" : -1
        }

        dates = getDates()
        data["toDate"] = dates[0]
        data["fromDate"] = dates[1]
        if ($("#MainContent_SiteDD_p").length > 0) {            
            data["siteId"] = $('#MainContent_SiteDD_p').find(":selected").val();
        }

        $.when(callAPI(JSON.stringify(data), url)).then(function (result) {
            buildTable(result, data["toDate"], data["fromDate"])
            $("#weightedContainer .loader").addClass('hidden')
            $("#download_p").removeClass("hidden")
        })        
    })

    $("#download_p").click(function (e) {
        e.preventDefault()
        var el = $(this)
        el.prop('disabled', true);
        setTimeout(function () { el.prop('disabled', false); }, 1000);
        downloadTable()
    })

    /// Page set up

    $("#toDatePicker_p").datepicker({ minDate: new Date(2018, 1 - 1, 1), maxDate: "+0D" })
    $("#fromDatePicker_p").datepicker({ minDate: new Date(2018, 1 - 1, 1), maxDate: "+0D" })

    //Prevent pasting into date fields
    $('#toDatePicker_p').bind("paste", function (e) {
        e.preventDefault();
    });
    $('#fromDatePicker_p').bind("paste", function (e) {
        e.preventDefault();
    });

    //Date presets
    $("#datePresets_p #prevWeek_p").click(function (event) {
        event.preventDefault()
        $("#toDatePicker_p").datepicker("setDate", "+0D")
        $("#fromDatePicker_p").datepicker("setDate", "-7D")
    })

    $("#datePresets_p #prevMonth_p").click(function (event) {
        event.preventDefault()
        $("#toDatePicker_p").datepicker("setDate", "+0D")
        $("#fromDatePicker_p").datepicker("setDate", "-1M")
    })

    $("#datePresets_p #ytd_p").click(function (event) {
        event.preventDefault()
        y = new Date(new Date().getFullYear(), 0, 1);
        $("#toDatePicker_p").datepicker("setDate", "+0D")
        $("#fromDatePicker_p").datepicker("setDate", y)
    })

    //

    // Allows for string interpolation 
    // eg. "{0} {1}".format("Hello","World")
    if (!String.prototype.format) {
        String.prototype.format = function () {
            var args = arguments;
            return this.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] != 'undefined'
                    ? args[number]
                    : match
                    ;
            });
        };
    }

}
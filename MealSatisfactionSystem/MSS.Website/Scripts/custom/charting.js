function Charting() {


    /// Lookup table Region ///

    //Are filled on load. For assigning color and value
    var lookupQuestions = {
    }

    var lookupAnswers = {        
    }

    var lookupSites = {
    }

    var lookupAges = {
        "-1": function () {
            return "All Ages"
        },
        "1": function () {
            return "Under 18"
        },
        "2": function () {
            return "19-34"
        },
        "3": function () {
            return "35-54"
        },
        "4": function () {
            return "55-74"
        },
        "5": function () {
            return "75+"
        }
    }

    /// Templates for charts Region ///

    verticalBarChart = {
        type: 'bar',
        data: {
            datasets: []
        },
        options: {
            tooltips: {
                mode: 'point',
                callbacks: {
                    title: function (t, d) {
                        return moment(t[0].xLabel).format("MMMM Do YYYY")
                    },
                    label: function (t, d) {
                        if (t.datasetIndex === 0) {
                            return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                        } else if (t.datasetIndex === 1) {
                            return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                        } else if (t.datasetIndex === 2) {
                            return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                        }
                    }
                }
            },
            title: {
                display: true,
                text: []
            },
            legend: {
                display: true
            },
            scales: {
                yAxes: [{
                    ticks: {
                        max: 5, 
                        min: 0, 
                        stepSize: 1
                    }
                }],
                xAxes: [{
                    type: 'time',
                    time: {
                        unit: 'day'
                    },
                    distribution: 'series',
                    ticks: {
                        source: 'auto'
                    },
                    offset: true,
                }]
            }
        }
    }

    lineChart = {
        type: 'line',
        data: {
            datasets: []
        },
        options: {
            tooltips: {
                mode: 'point',
                callbacks: {
                    title: function(t,d){
                        return moment(t[0].xLabel).format("MMMM Do YYYY")
                    },                    
                    label: function (t, d) {
                        if (t.datasetIndex === 0) {
                            return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                        } else if (t.datasetIndex === 1) {
                            return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                        } else if (t.datasetIndex === 2) {
                            return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                        }
                    }
                }
            },
            title: {
                display: true,
                text: []
            },
            legend: {
                display: true
            },
            scales: {
                yAxes: [{
                    ticks: {
                        max: 6, 
                        min: 0, 
                        stepSize: 1
                    }
                }],                
                xAxes: [{
                    type: 'time',
                    time: {
                        unit: 'day'
                    },
                    distribution: 'series',
                    ticks: {
                        source: 'auto'
                    },
                    offset : true
                }]
            }
        }
    }

    pieChart = {
        type: 'pie',
        data: {
            labels: [],
            datasets: [{
                data: [],
                backgroundColor: [],
            }]
        },
        options: {
            tooltips: {
                mode: 'label',
                callbacks: {
                    //label: function (tooltipItem, data) {
                    //    return (data['labels'][tooltipItem['index']]).split(" (")[0] + ': ' + data['datasets'][0]['data'][tooltipItem['index']].toFixed(1) + '%';
                    //}
                }
            },
            title: {
                display: true,
                text: 'Unit Representation'
            },
            legend: {
                display: true
            },
            scales: {
                yAxes: [{
                    display: false
                }],
                xAxes: [{
                    display: false
                }]
            }
        }
    }

    /// Global Variable Region ///

    var myChart;

    var selectedChartType;

    var filters;

    var ALERT_TITLE = "Oops!";
    var ALERT_BUTTON_TEXT = "Ok";

    /// API call Region///

    var baseURL = "ViewReports.aspx/ChartDataRequest"
    var lookupURL = "ViewReports.aspx/LookupRequest"

    /// <summary>Calls a .NET Webmethod to retrieve data </summary>  
    /// <param name="collection" type="object">Collection of filters gathered from page</param>  
    /// <param name="url" type="string">Contains a URL to the required webservice</param>  
    /// <returns type="Promise">Contains a Promise of data objcet</returns>  
    function callAPI(collection, url)
    {
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
        //msg.appendChild(d.createTextNode(txt));
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

    /// <summary>Build lookup tables for later referance</summary>           
    function buildLookupTables(){
        var lookupList = ["lookupSites", "lookupQuestions", "lookupAnswers"]

        lookupList.forEach(function (l) {
            var data = JSON.stringify({"type" : l})
            var result = callAPI(data, lookupURL)
            $.when(result).then(function (d) {
                var data = d["d"]

                if(data[0].Type == "lookupSites")
                {
                    for (var i in data) {
                        if (!isNaN(i)) {
                            lookupSites[data[i].Id.toString()] = data[i].Description
                        }
                    }
                }
                if (data[0].Type == "lookupQuestions")
                {
                    for (var i in data) {
                        lookupQuestions[data[i].Description] = data[i].Value
                    }
                }
                if (data[0].Type == "lookupAnswers") {
                    for (var i in data) {
                        lookupAnswers[data[i].Description] = data[i].Value
                    }
                }
            })
        })
    }


    /// Build Charts Region///

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

    Chart.defaults.global.defaultFontFamily = 'Raleway';

    
    /// <summary>Send each parameter to ASP.NET to create our datasets. 
    /// Create manual display of filters.
    /// On return, if multiple pies, set up paging.</summary>  
    /// <param name="collection" type="object">Contains the filters collected from the page</param>
    function sendForChartData(collection) {
        var searchQueries = []

        // Get a dataset for each question based on limiting params(date, age ect)
        collection.SelectedQuestionParameters.forEach(function (param) {
            var paramQueryStringified = JSON.stringify({
                Parameters: param,
                FromDate: collection.FromDate,
                ToDate: collection.ToDate,
                Units: collection.SelectedUnits,
                Genders: collection.SelectedGenders,
                Ages: collection.SelectedAges,
                Site : collection.SelectedSite
            })
            searchQueries.push(paramQueryStringified)
        })

        //Create detailed filter information
        filters = "Active filters: \n"
        for (i in collection) {
            if(collection[i].length > 0)
            {
                filters += i + ": "
                i == "SelectedSite" ? filters += lookupSites[1] + ", "
                : i == "SelectedAges" ? collection[i].forEach(function (c) {
                    filters += lookupAges[c]() + ", "
                })
                : collection[i].forEach(function (c) {
                    filters += c + ", "
                 })
                filters += "\n"
            } else if (i == "ToDate" || i == "FromDate") {
                if (moment(collection[i],"MMMM Do YYYY").isValid()) {
                    filters += i + ": "
                    filters += moment(collection[i]).format("MMMM Do YYYY") + ", "
                    filters += "\n"
                }
            }
        }
        filters = filters.replace(/-1/g, "All")
        filters = filters.replace(/Invalid date/g, "Not Specified")


        var count = 0
        //One call per parameter
        searchQueries.forEach(function (query) {
            var parameterResults = callAPI(query, baseURL)
            $.when(parameterResults).then(function (data) {
                filterCharts(data["d"])
                $("#chartBody .loader").addClass("hidden")
                count++

                if (count > 1 && selectedChartType == "Pie")
                {
                    $("#pieChartPagingGroup").removeClass("hidden")
                    $(".dynChart:nth-of-type(" + count + ")").addClass('hidden')

                    $("#pieChartPagingGroup #demoninator").text(count)
                    $("#pieChartPagingGroup #fraction").text("1")
                } else {
                    $("#pieChartPagingGroup").addClass("hidden")
                }
            })
        })
    }

    /// <summary>Determines which type of chart we are building and send it to the appropriate methods.</summary>  
    /// <param name="data" type="object">Contains the returned data from the server</param>    
    function filterCharts(data) {       
        switch (selectedChartType) {
            case "Pie":
                var modifiedData = rollUpToCounts(data)
                buildPieChart(modifiedData, data)
                break
            case "Line":
                var modifiedData = sortDataByDates(data)
                buildLineChart(modifiedData, data)
                break
            case "Bar":
                var modifiedData = sortDataByDates(data)
                buildBarChart(modifiedData, data)
                break
            default:
                break
        }
    }
    
    /// <summary>Transforms the data into a format usable by the pie charts.
    /// Pie chart needs data aggregated and summed by the answer type.
    /// Creates percent total for all answers recieved</summary >
    /// <param name="data" type="object">Contains the data returned from the server</param>
    /// <returns>Object of tranformed data</returns>
    function rollUpToCounts(data) {        
        // Creates an object based on passed data. eg {"Very good" : 3, "Poor" : 1}
        var summedData = {}

        data.Data.forEach(function (pair) {
            if(summedData.hasOwnProperty(pair.AnswerText)){
                summedData[pair.AnswerText] = summedData[pair.AnswerText] + 1
            } else {
                summedData[pair.AnswerText] = 1
            }
        })

        var total = 0

        for (d in summedData) {
            total += parseInt(summedData[d])
        }

        //turns the # of votes into a % of votes. Uses total from above
        for(d in summedData) {
            var newPercent = parseInt(summedData[d]) / total
            summedData[d] = newPercent*100
        }

        return summedData        
    }

    var is31Days = false;

    /// <summary>Transforms the data into a format usable by the line and bar charts.
    /// Line and Bar need data averaged by the dates received. Time discarded</summary >
    /// <param name="data" type="object">Contains the data returned from the server</param>
    /// <returns>Object of tranformed data</returns>
    function sortDataByDates(data)
    {
        //Sorting each datapoint into an appropriate key(day), creating the key if needed
        var sortedData = []

        //Sort data into dates, including -1 an 0
        data.Data.forEach(function (pair) {
            var day = moment(pair.Date).startOf('day')
            if (sortedData.hasOwnProperty(day)) {
                sortedData[day].push(pair.Data)
            } else {
                sortedData[day] = [pair.Data]
            }
        })

        // remove the -1 and 0 so as to not mess up the avg()
        // -1 and 0 don't add to the avg since they are impartial answers.
        for (var key in sortedData) {
            if (sortedData.hasOwnProperty(key))
                sortedData[key] = sortedData[key].filter( function(i) {
                    return !(i <= 0);
                });
        }

        // If viewing more than a month of data, change the scale(roll data up by the month, vs day)
        if (Object.size(sortedData) > 31) {
            sortedData = rollupByMonth(sortedData)
            is31Days = true
        }

        var timeSeriesData = []
        //Formatting how chartjs expects the data to appear {t: date, y: int}
        // Averages the arrays of responses
        for (var key in sortedData) {
            if (sortedData.hasOwnProperty(key))
                timeSeriesData.push({t:key, y: sortedData[key].avg()})
        }

        //sorting the data based on day
        timeSeriesData.sort(function (obj1, obj2) {
            return moment(obj1.t) > moment(obj2.t) ? 1 : moment(obj1.t) == moment(obj2.t) ? 0 : -1
        })               

        return timeSeriesData
    }
    
    /// <summary>If there is more than a month of data, the chart gets hard to read.
    /// This sorts the daily data into monthly.</summary >
    /// <param name="data" type="object">Contains the data returned from the server</param>
    /// <returns>Object of tranformed data</returns>
    function rollupByMonth(data) {
        var monthlyData = {}

        for (var d in data) {
            if (data.hasOwnProperty(d)) {
                var month = moment(d).startOf("month")
                if (monthlyData.hasOwnProperty(month)) {
                    monthlyData[month] = monthlyData[month].concat(data[d])
                } else {
                    monthlyData[month] = data[d]
                }
            }
        }
               
        return monthlyData
    }
   
    /// <summary>To accomidate the monthly rollups, the chart templates need changing</summary >    
    function updateChartDefaults() {
        selectedChartType == "Line" ?
        lineChart.options.scales.xAxes = [{
            type: 'time',
            time: {
                unit: 'month',
            },
            distribution: 'series',
            ticks: {
                source: "data"
            },
        }] :
        verticalBarChart.options.scales.xAxes = [{
            type: 'time',
            time: {
                unit: 'month',
            },
            distribution: 'series',
            ticks: {
                source: "data"
            },
            offset: true
        }]

        selectedChartType == "Line" ?
        lineChart.options.tooltips.callbacks = {
            title: function (t, d) {
                return moment(t[0].xLabel).format("MMMM YYYY")
            },
            label: function (t, d) {
                if (t.datasetIndex === 0) {
                    return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                } else if (t.datasetIndex === 1) {
                    return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                } else if (t.datasetIndex === 2) {
                    return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                }
            }
        } :
        verticalBarChart.options.tooltips.callbacks = {
            title: function (t, d) {
                return moment(t[0].xLabel).format("MMMM YYYY")
            },
            label: function (t, d) {
                if (t.datasetIndex === 0) {
                    return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                } else if (t.datasetIndex === 1) {
                    return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                } else if (t.datasetIndex === 2) {
                    return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                }
            }
        }

    }
    
    /// <summary>Creation of the pie chart</summary >
    /// <param name="modifiedData" type="object">Contains transformed data returned from the server</param>
    /// <param name="origData" type="object">Contains raw data from the server, including metadata</param>
    function buildPieChart(modifiedData, origData)
    {
        var labels = []
        var values = []
        var colour = []

        // Seperate the sorted modifiedData into label vs value arrays
        // Add percent onto label so downloading carries the % visible
        for (var d in modifiedData) {
            if (d == "") {
                labels.push("No response (" + modifiedData[d].toFixed(1) + "%)")
            } else {
                labels.push(d + " (" + modifiedData[d].toFixed(1) + "%)")
            }
            values.push(modifiedData[d])           
        }

        // Pulls colours from lookup table
        labels.forEach(function (l) {
            var split = l.split(" (")[0]
            colour.push(lookupAnswers[split])
        })

        //Create new chart for each pie needed
        $('#chartBody>div:last-of-type').append('<canvas id="dynamicChart' + origData.Parameter.replace(" ", "") + '" class="dynChart" style="width: 512px; height: 356px; padding: 0;margin: auto;"> </canvas>')
        //Add filters to page
        $("#filters").text(filters)

        //Create canvas to hold the chart
        canvas = $("#dynamicChart" + origData.Parameter.replace(" ", ""))
        canvas.width = canvas.clientWidth; // Prevents blurry text
        canvas.height = canvas.clientHeight;
        
        var ctx = $("#dynamicChart" + origData.Parameter.replace(" ", "")) // Get context of canvas to draw onto

        //Since we are using multi pie chars, we need a new instance of the pie template, otherwise the modifiedData is overwritten
        var newPie = JSON.parse(JSON.stringify(pieChart)) // ghetto new object
        newPie.options.title.text = origData.Subtext.length > 0? [origData.Question,origData.Subtext] : [origData.Question]
        newPie.data.labels = labels
        newPie.data.datasets[0].data = values
        newPie.data.datasets[0].backgroundColor = colour
        newPie.options.showAllTooltips = false

        // Have to add tooltip callback seperatly
        newPie.options.tooltips.callbacks = {
            label: function (tooltipItem, data) {
                return data['labels'][tooltipItem['index']].split(" (")[0] + ': ' + data['datasets'][0]['data'][tooltipItem['index']].toFixed(1) + '%';
            }
        }

        myChart = new Chart(ctx, newPie)// create chart
    }

    /// <summary>Creation of the line chart</summary >
    /// <param name="timeSeriesData" type="object">Contains transformed data returned from the server</param>
    /// <param name="data" type="object">Contains raw data from the server, including metadata</param>
    function buildLineChart(timeSeriesData, data) {
        var ctx;
        // bc we are adding multiple datasets to the same chart, we need to only create it once.        
        if ($("#dynamicChart").length) {
            ctx = $("#dynamicChart")
        } else {
            $('#chartBody>div:last-of-type').append('<canvas id="dynamicChart" class="dynChart" style="width: 512px; height: 356px; padding: 0;margin: auto;"> </canvas>')
            ctx = $("#dynamicChart")
        }

        $("#filters").text(filters)

        var ctx = $("#dynamicChart")

        var title = []
        data.Subtext.length > 0 ? title.push(data.Subtext) : title.push(data.Question)

        //Using the same line template, add new dataset
        lineChart.data.datasets.push({            
            label: data.Parameter,
            fill: false,
            data: timeSeriesData,
            borderColor: lookupQuestions[data.Parameter],
            pointStyle: "circle",
            spanGaps: true
        })
        lineChart.options.title.text.push(title)

        // If not the first dataset entered
        if (lineChart.data.datasets.length > 1) {
            myChart.update()
        } else {
            if(is31Days) updateChartDefaults()
            myChart = new Chart(ctx, lineChart)
        }
    }

    /// <summary>Creation of the bar chart. Similiar to line chart</summary >
    /// <param name="timeSeriesData" type="object">Contains transformed data returned from the server</param>
    /// <param name="data" type="object">Contains raw data from the server, including metadata</param>
    function buildBarChart(timeSeriesData, data) {        
        if ($("#dynamicChart").length) {
            ctx = $("#dynamicChart")
        } else {
            $('#chartBody>div:last-of-type').append('<canvas id="dynamicChart" class="dynChart" style="width: 512px; height: 356px; padding: 0;margin: auto;"> </canvas>')
            ctx = $("#dynamicChart")
        }

        $("#filters").text(filters)

        var ctx = $("#dynamicChart")        

        var title = []
        data.Subtext.length > 0 ? title.push(data.Subtext) : title.push(data.Question)        

        verticalBarChart.data.datasets.push({
            label: data.Parameter,
            fill: false,
            data: timeSeriesData,
            borderColor: lookupQuestions[data.Parameter],
            backgroundColor: lookupQuestions[data.Parameter]
        })
        verticalBarChart.options.title.text.push(title)


        if (verticalBarChart.data.datasets.length > 1) {
            myChart.update()
        } else {
            if (is31Days) updateChartDefaults()
            myChart = new Chart(ctx, verticalBarChart)
        }
    }

    /// Download Region ///

    /*Cycle through all current charts(canvas).
    Turn canvas into data string for jpg saving
    However, platforms that cannot view transparency of pngs turns the background black
    To avoid, have to give the canvas a white background manually.
    Create new canvas with white bg, copy existing chart(canvas) onto it
    Add the filter text.    
    */
    /// <summary>Generates a downloadable version of the charts with a filter list.</summary >    
    function printGraphToPDF() {
        var charts = $("canvas")
        for( c in charts)
        {
            if(!isNaN(c))
            {
                var filtertext = $("#filters").text()
                //
                destinationCanvas = document.createElement("canvas");
                destinationCanvas.width = charts[c].width;
                destinationCanvas.height = charts[c].height+200; //Extend canvas to accommidate the text

                destCtx = destinationCanvas.getContext('2d');

                //create a rectangle with the desired color
                destCtx.fillStyle = "#FFFFFF";
                destCtx.fillRect(0, 0, charts[c].width, charts[c].height+200);

                //draw the original canvas onto the destination canvas
                destCtx.drawImage(charts[c], 0, 0);

                // text styling
                destCtx.fillStyle = "#232123";
                destCtx.lineStyle = "#666666";
                destCtx.font = "14px Raleway";

                //No linebreak capability in canvas. Do it manually
                var splitFilter = filtertext.split("\n")
                for (line in splitFilter) {
                    var height = charts[c].height + 30 + splitFilter.indexOf(line) * 16
                    destCtx.fillText(splitFilter[line], 20, charts[c].height + 30 + (parseInt(line) * 16));
                }

                //finally use the destinationCanvas.toDataURL() method to get the desired output;
                var img = destinationCanvas.toDataURL();

                var pdf = new jsPDF();
                pdf.addImage(img, 'JPEG', 5, 20);
                pdf.setFont("helvetica");
                pdf.setFontType("normal");
                pdf.setFontSize(9);
                pdf.text(5, 10, "Generated on: " + moment().format("YYYY-MM-DD hh:mm:ss"))
                pdf.save(charts[c].id + "_" + moment().format("YYYY-MM-DD_ss") + ".pdf");

                $(".print-btn>.loader").addClass('hidden')
                $(".print-btn>p:first-of-type").removeClass('hidden')                
            }
        }
    }


    /// General functions Region ///

    // Get length of object
    Object.size = function (obj) {
        var size = 0, key;
        for (key in obj) {
            if (obj.hasOwnProperty(key)) size++;
        }
        return size;
    };

    // Sums the contents of an array
    Array.prototype.sum = Array.prototype.sum || function () {
        return this.reduce(function (p, c) { return p + c }, 0);
    };

    // Averages the contents of an array
    Array.prototype.avg = Array.prototype.avg || function () {
        return this.sum() / this.length;
    };

    
    /// <summary> Sets the filters back to default. Does not do date fields</summary>
    function clearFilterParameters(){
        $("#MainContent_genderRadioList>tbody>tr").each(function () {            
            var checkboxText = this.childNodes[1].childNodes[0].value

            if (checkboxText == "-1") {
                this.childNodes[1].childNodes[0].checked = true
            }
            if (checkboxText != "-1") {
                this.childNodes[1].childNodes[0].checked = false
            }
        })

        $("#MainContent_ageRadioList>tbody>tr").each(function () {
            var checkboxStatus = this.childNodes[1].childNodes[0].checked
            var checkboxText = this.childNodes[1].childNodes[0].value

            if (checkboxText == "-1") {
                this.childNodes[1].childNodes[0].checked = true
            } else {
                this.childNodes[1].childNodes[0].checked = false

            }
        })

        $("#dbParameters>input").each(function () {
            this.checked = false
        })

        $("#unitDBParameters>input").each(function () {
            this.checked = false
            if(this.value == "All")
                this.checked = true
        })
    }
    
    /// <summary>Gets all the filters from the page and puts into an object</summary>
    /// <returns>Object of the filters gathered from the page</returns>
    function collectParametersFromPage() {
        // Dates
        toDate = $("#toDatePicker").datepicker("getDate")
        toDate = toDate == null ? new Date() : toDate //Webservice can't handle null
        fromDate = $("#fromDatePicker").datepicker("getDate")
        fromDate = fromDate == null ? $("#fromDatePicker_p").datepicker("option", "minDate") : fromDate

        // Site
        var Site = "-1" //Not Webmaster

        if ($("#MainContent_SiteDD").length > 0)
        {
            Site = $('#MainContent_SiteDD').find(":selected").val();
        }

        // Gender
        var selectedGenders = []
        $("#MainContent_genderRadioList>tbody>tr").each(function () {            
            var checkboxStatus = this.childNodes[1].childNodes[0].checked
            var checkboxText = this.childNodes[1].childNodes[0].value

            if (checkboxStatus) {
                selectedGenders.push(checkboxText)
            }
        })

        // Age
        var selectedAges = []
        $("#MainContent_ageRadioList>tbody>tr").each(function () {
            var checkboxStatus = this.childNodes[1].childNodes[0].checked
            var checkboxText = this.childNodes[1].childNodes[0].value

            if (checkboxStatus) {
                selectedAges.push(checkboxText)
            }
        })

        // QuestionParameters
        var selectedQuestionParameters = []
        $("#dbParameters>input").each(function () {
            var checkboxStatus = this.checked
            var checkboxText = this.nextSibling.textContent

            if (checkboxStatus) {
                selectedQuestionParameters.push(checkboxText)
            }
        })

        // Units
        var selectedUnits = []
        $("#unitDBParameters>input").each(function () {
            var checkboxStatus = this.checked
            var checkboxText = this.nextSibling.textContent

            if (checkboxStatus) {
                selectedUnits.push(checkboxText)
            }
        })

        return {
            FromDate: fromDate,
            ToDate: toDate,
            SelectedGenders: selectedGenders,
            SelectedAges: selectedAges,
            SelectedQuestionParameters: selectedQuestionParameters,
            SelectedUnits: selectedUnits,
            SelectedSite: Site
        }
    }

    /// UX Region///

    // If 'Not specified' is checked, any previously checked boxes are unchecked. 
    // If another option is selected, the 'Not specified' is unselected
    $("#genderParameterSection input").change(function () {
        if (this.value == "-1") {
            $("#MainContent_genderRadioList>tbody>tr").each(function () {
                var checkboxText = this.childNodes[1].childNodes[0].value
                if (checkboxText == "-1") {
                    this.childNodes[1].childNodes[0].checked = true
                }
                if (checkboxText != "-1") {
                    this.childNodes[1].childNodes[0].checked = false
                }
            })
        } else {
            $("#genderParameterSection input").each(function () {
                var checkboxText = this.value
                if (checkboxText == "-1") {
                    this.checked = false
                }
            })
        }  
    })

    $("#ageParameterSection input").change(function () {
        if (this.value == "-1") {
            $("#MainContent_ageRadioList>tbody>tr").each(function () {
                var checkboxText = this.childNodes[1].childNodes[0].value
                if (checkboxText == "-1") {
                    this.childNodes[1].childNodes[0].checked = true
                }
                if (checkboxText != "-1") {
                    this.childNodes[1].childNodes[0].checked = false
                }
            })
        } else {
            $("#ageParameterSection input").each(function () {
                var checkboxText = this.value
                if (checkboxText == "-1") {
                    this.checked = false
                }
            })
        }
    })

    $("#unitDBParameters input").change(function () {
        if (this.value == "All") {
            $("#unitDBParameters input").each(function () {
                var checkboxText = this.value
                if (checkboxText == "All") {
                    this.checked = true
                }
                if (checkboxText != "All") {
                    this.checked = false
                }
            })
        } else {
            $("#unitDBParameters input").each(function () {
                var checkboxText = this.value
                if (checkboxText == "All") {
                    this.checked = false
                }
            })
        }
    })

    // Allow Jan 1 2018 'til today
    $("#toDatePicker").datepicker({ minDate: new Date(2018, 1 - 1, 1), maxDate: "+0D" })
    $("#fromDatePicker").datepicker({ minDate: new Date(2018, 1 - 1, 1), maxDate: "+0D" })

    //Prevent pasting into date fields
    $('#toDatePicker').bind("paste",function(e) {
        e.preventDefault();
    });
    $('#fromDatePicker').bind("paste", function (e) {
        e.preventDefault();
    });

    //Hide msgUsrCtrl
    $(".thePanel").css("display", "none")

    ///  Button assignment Region ///

    $("#clearParameterFilters").click(function (event) {
        event.preventDefault()
        clearFilterParameters()
    });

    // creates downloadable version of the charts
    $("#printGraph").click(function (event) {
        event.preventDefault()
        if ($("canvas").length > 0) {
            $(".print-btn .loader").removeClass('hidden')
            var el = $(this)
            el.prop('disabled', true);
            setTimeout(function () { el.prop('disabled', false); }, 1000); // prevents spam clicking
            printGraphToPDF();
        }
    })

    /// <summary>Resets the page and required variables to default state</summary>
    function resetPage() {
        if (myChart) { // For redraw. Reset
            myChart.destroy()
            $("#chartBody>div:last-of-type").empty()

            is31Days = false;

            lineChart.data.datasets = []
            lineChart.options.title.text = []
            lineChart.options.tooltips.callbacks = {
                title: function (t, d) {
                    return moment(t[0].xLabel).format("MMMM Do YYYY")
                },
                label: function (t, d) {
                    if (t.datasetIndex === 0) {
                        return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                    } else if (t.datasetIndex === 1) {
                        return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                    } else if (t.datasetIndex === 2) {
                        return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                    }
                }
            }
            lineChart.options.scales.xAxes = [{
                type: 'time',
                time: {
                    unit: 'day',
                },
                distribution: 'series',
                ticks: {
                    source: "data"
                },
            }]

            verticalBarChart.data.datasets = []
            verticalBarChart.options.title.text = []
            verticalBarChart.options.tooltips.callbacks = {
                title: function (t, d) {
                    return moment(t[0].xLabel).format("MMMM Do YYYY")
                },
                label: function (t, d) {
                    if (t.datasetIndex === 0) {
                        return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                    } else if (t.datasetIndex === 1) {
                        return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                    } else if (t.datasetIndex === 2) {
                        return d.datasets[t.datasetIndex].label + ": " + t.yLabel.toFixed(1);
                    }
                }
            }
        }
        verticalBarChart.options.scales.xAxes = [{
            type: 'time',
            time: {
                unit: 'day',
            },
            distribution: 'series',
            ticks: {
                source: "data"
            },
            offset: true
        }]

        $(".print-btn>p").addClass('hidden')
        $("#chartBody div img").hide();
        $("#chartBody .loader").removeClass("hidden")
        $("#filters").text("")

        // Reset error msg
        $(".btn-group").css("border", "none")
        $("#btngroup-errormsg").addClass('hidden')
        $("#params-errormsg").addClass('hidden')
    }

    /// <summary>Initiates the generation of the charts</summary>  
    $("#generateChart").click(function (event) {
        event.preventDefault()

        //Prevent double click
        var el = $(this)
        el.prop('disabled', true);
        setTimeout(function () { el.prop('disabled', false); }, 1000);

        resetPage()

        var chartTypeSelected;
        $(".btn-group > .btn").each(function () {            
            if(this.className.indexOf("active") > 0)
            {
                chartTypeSelected = true
                selectedChartType = this.textContent
            }
        })
        
        var collection = collectParametersFromPage();

        var send = true;
        if (collection.SelectedQuestionParameters.length < 1 || collection.SelectedQuestionParameters.length > 3) { // No parameters selected
            $("#params-errormsg").removeClass('hidden')
            send = false
            $("#chartBody .loader").addClass("hidden")
        }
        if (!chartTypeSelected) {//Error: Did not select type of chart
             $(".btn-group").css("border-radius", "5px")
            $(".btn-group").css("border", "red solid 1px")
            $("#btngroup-errormsg").removeClass('hidden')
            $("#chartBody .loader").addClass("hidden")
        }
        if(send){
           sendForChartData(collection);
        }
    })

    // Visual change of chart type buttons
    $(".btn-group > .btn").click(function (event) {
        event.preventDefault()
        $(this).addClass("active").siblings().removeClass("active");
    });

    //Calender resets
    $(".glyphicon-remove").click(function () {
        $.datepicker._clearDate(this.previousSibling);
    })

    //Date presets
    $("#datePresets #prevWeek").click(function (event) {
        event.preventDefault()
        $("#toDatePicker").datepicker("setDate", "+0D")
        $("#fromDatePicker").datepicker("setDate", "-7D")
    })

    $("#datePresets #prevMonth").click(function (event) {
        event.preventDefault()
        $("#toDatePicker").datepicker("setDate", "+0D")
        $("#fromDatePicker").datepicker("setDate", "-1M")
    })

    $("#datePresets #ytd").click(function (event) {
        event.preventDefault()
        y = new Date(new Date().getFullYear(), 0, 1);
        $("#toDatePicker").datepicker("setDate", "+0D")
        $("#fromDatePicker").datepicker("setDate", y)
    })

    // Pie chart paging
    $(".glyphicon-arrow-left").click(function () {
        var currentActiveIndex;
        for(chart in $(".dynChart"))
        {
            if (!isNaN(chart)) {
                var index = parseInt(chart) + 1
                var doesHaveClass = $(".dynChart:nth-of-type(" + index + ")").hasClass("hidden")
                if (!doesHaveClass)//no hidden
                    currentActiveIndex = index
            }
        }
        if (currentActiveIndex > 1)
        {
            $(".dynChart:nth-of-type(" + currentActiveIndex + ")").addClass("hidden")
            var prevIndex = parseInt(currentActiveIndex) - 1
            $(".dynChart:nth-of-type( " + prevIndex + " )").removeClass('hidden')

            $("#fraction").text(prevIndex)
        }
    })

    $(".glyphicon-arrow-right").click(function () {
        var currentActiveIndex;
        var count = $(".dynChart").length

        for (chart in $(".dynChart")) {
            if (!isNaN(chart))
            {
                var index = parseInt(chart) + 1
                var doesHaveClass = $(".dynChart:nth-of-type(" + index + ")").hasClass("hidden")
                if (!doesHaveClass)//no hidden
                    currentActiveIndex = index
            }
        }
        if (currentActiveIndex < count) {           
            $(".dynChart:nth-of-type(" + currentActiveIndex + ")").addClass("hidden")
            var nextIndex = parseInt(currentActiveIndex) + 1
            $(".dynChart:nth-of-type( " + nextIndex + " )").removeClass('hidden')

            $("#fraction").text(nextIndex)
        }
    })

    // Creates custom alert box
    window.alert = function (txt) {
        createCustomAlert(txt);
    }

    // Build the tabs
    $("#tabs").tabs()

    buildLookupTables();
}
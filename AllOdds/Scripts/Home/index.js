var selectedMatchId;
var searchKey = 0;

$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "/Home/getCategories",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var option = $(document.createElement('option'));
            option.text("All Categories");
            option.val(0);
            $("#cboCategories").append(option);

            $(msg).each(function () {
                var option = $(document.createElement('option'));
                option.text(this.category_name);
                option.val(this.category_id);
                $("#cboCategories").append(option);
            });
        },
        error: function (msg) {
            console.log("error al llenar la lista de categorias", msg);
        }
    });
       
    $('#dtOdds').on('draw.dt', function () {
        $('#dtOdds > tbody > tr').each(function () {
            var r = $('#dtOdds').DataTable().row('#dtOdds > tbody > tr');

            for (i = 0; i < selectedRows.length; i++) {
                var row = selectedRows[i];

                if (r.data().match_id == row.data().match_id) {
                    r.child(buildEvents(r.data())).show();
                    break;
                }
            }
        });        
    });

    var child_rows = [];

    initDtp();
        
    $('#cboOddTypes').on('change', function () {
        onChangeOddType();
    })

    setInterval(function () {
        if (searchKey === 1) {
            showMatches();
            onChangeOddType();
            showEvents();
        }
    }, 5000);
});

function onChangeOddType() {
    if (selectedMatchId !== null) {
        var odd_type_id = $("#cboOddTypes option").filter(":selected").val();
        
        if (odd_type_id != 0) {
            var jsonData = {
                match_id: selectedMatchId,
                odd_type_id: odd_type_id
            };

            $.ajax({
                type: 'POST',
                url: '../../home/getOdds/',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(jsonData),
                success: showOddsOnSuccess,
                error: showOddsOnError
            });
        } else {
            $('#tbl-odds').find('.main-table').remove();
        }
    } else {
        $('#tbl-odds').find('.main-table').remove();
    }    
}

function showOddsOnSuccess(data) {
    console.log("showing odds", data);

    var MAIN_TABLE = '<table class="main-table" border=2>';
    var ROW_BOOKMAKER = '<tr class="row-bookmaker" style="background-color:#b1b1b1; padding=3px; font-weight:bold; font-size:14px; text-align:center;">';
    var ROW_ODDS = '<tr class="row-odds" valign="top">';

    for (var b = 0; b < data.data.length; b++) { // ciclo para los bookmakers
        var COL_BOOKMAKER = '<td class="col-bookmaker">' + data.data[b].name + '</td>';
        var TBL_ODDS = '<table class="odd-table" width=500px">';

        if (data.data[b].is_handicap === 1) {
            for (var h = 0; h < data.data[b].handicaps.length; h++) {

                var col_total_handicap = '<tr class="row-total-handicap"><td style="background-color:#FF8000; padding:3px; font.weight:bold; font-style:italic; font-size:14px; text-align:center" class="col-total-handicap" colspan=3>Handicap ==> ' + data.data[b].handicaps[h].odds[0].handicap_name + '</td></tr>';
                var TITLES = '<tr style="background-color:#F7BE81; text-align:center;"><td>Odd Name</td><td>Odd Value</td><td>Handicap</td></tr>';
                TBL_ODDS = TBL_ODDS + col_total_handicap + TITLES;

                var ODD_ROWS = '';

                for (var o = 0; o < data.data[b].handicaps[h].odds.length; o++) {
                    var value = data.data[b].handicaps[h].odds[o].value;
                    value = value - 1;

                    if (value >= 2) {
                        value = (value * 100).toFixed(0);
                    } else {
                        value = ((-100) / value).toFixed(0);
                    }

                    var row = '<tr style="text-align:center;">';
                    var col_odd_name = '<td>' + data.data[b].handicaps[h].odds[o].name + '</td>';
                    var col_odd_value = '<td>' + value + '</td>';
                    var col_handicap = '<td>' + data.data[b].handicaps[h].odds[o].handicap + '</td>';
                    row = row + col_odd_name + col_odd_value + col_handicap + '</tr>';

                    ODD_ROWS = ODD_ROWS + row;
                }

                TBL_ODDS = TBL_ODDS + ODD_ROWS;
            }

            TBL_ODDS = TBL_ODDS + '</table>';

            ROW_BOOKMAKER = ROW_BOOKMAKER + COL_BOOKMAKER;
            ROW_ODDS = ROW_ODDS + '<td style="padding=3px;">' + TBL_ODDS + '</td>';

        } else if (data.data[b].is_total === 1) {

            for (var t = 0; t < data.data[b].totals.length; t++) {

                var col_total_handicap = '<tr><td style="background-color:#FF8000; padding:3px; font.weight:bold; font-style:italic; font-size:14px; text-align:center" colspan=3>Total ==> ' + data.data[b].totals[t].odds[0].total_name + '</td></tr>';
                var TITLES = '<tr style="background-color:#F7BE81; text-align:center;"><td>Odd Name</td><td>Odd Value</td><td>Handicap</td></tr>';
                TBL_ODDS = TBL_ODDS + col_total_handicap + TITLES;

                var ODD_ROWS = '';

                for (var o = 0; o < data.data[b].totals[t].odds.length; o++) {
                    var value = data.data[b].totals[t].odds[o].value;
                    value = value - 1;

                    if (value >= 2) {
                        value = (value * 100).toFixed(0);
                    } else {
                        value = ((-100) / value).toFixed(0);
                    }

                    var row = '<tr style="text-align:center;">';
                    var col_odd_name = '<td>' + data.data[b].totals[t].odds[o].name + '</td>';
                    var col_odd_value = '<td>' + value + '</td>';
                    var col_handicap = '<td>N/A</td>';
                    row = row + col_odd_name + col_odd_value + col_handicap + '</tr>';

                    ODD_ROWS = ODD_ROWS + row;
                }

                TBL_ODDS = TBL_ODDS + ODD_ROWS;
            }

            TBL_ODDS = TBL_ODDS + '</table>';

            ROW_BOOKMAKER = ROW_BOOKMAKER + COL_BOOKMAKER;
            ROW_ODDS = ROW_ODDS + '<td>' + TBL_ODDS + '</td>';

        } else {
            var TITLES = '<tr style="background-color:#F7BE81; text-align:center;"><td>Odd Name</td><td>Odd Value</td><td>Handicap</td></tr>';
            TBL_ODDS = TBL_ODDS + TITLES;

            var ODD_ROWS = '';

            for (var o = 0; o < data.data[b].odds.length; o++) {
                var value = data.data[b].odds[o].value;
                value = value - 1;

                if (value >= 2) {
                    value = (value * 100).toFixed(0);
                } else {
                    value = ((-100) / value).toFixed(0);
                }

                var row = '<tr style="text-align:center;">';
                var col_odd_name = '<td>' + data.data[b].odds[o].name + '</td>';
                var col_odd_value = '<td>' + value + '</td>';
                var col_handicap = '<td>N/A</td>';
                row = row + col_odd_name + col_odd_value + col_handicap + '</tr>';

                ODD_ROWS = ODD_ROWS + row;
            }

            TBL_ODDS = TBL_ODDS + ODD_ROWS;
            

            TBL_ODDS = TBL_ODDS + '</table>';

            ROW_BOOKMAKER = ROW_BOOKMAKER + COL_BOOKMAKER;
            ROW_ODDS = ROW_ODDS + '<td>' + TBL_ODDS + '</td>';
        }
    }

    ROW_BOOKMAKER = ROW_BOOKMAKER + '</tr>';
    ROW_ODDS = ROW_ODDS + '</tr>';
    MAIN_TABLE = MAIN_TABLE + ROW_BOOKMAKER + ROW_ODDS + '</table>';

    $('#tbl-odds').find('.main-table').remove();
    $('#tbl-odds').append(MAIN_TABLE);
}

function showOddsOnError(error) {
    console.log("showing odds error", error);
}

function buildEvents(d) {
    var child = '<table class="child" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">';
    child = child + '<thead><tr class="child-th">' +
    '<td>Type</td><td>Team</td><td>Minute</td><td>Player Name</td><td>Assist Name</td><td>Result</td>' +
    '</tr></thead>';

    child = child + '<tbody>'

    $(d.events).each(function () {
        var row = '<tr class="child-row">';

        var col1 = '<td>' + this.type + '</td>'
        var col2 = '<td>' + this.team + '</td>'
        var col3 = '<td>' + this.minute + '</td>'
        var col4 = '<td>' + this.player_name + '</td>'
        var col5 = '<td>' + this.assist_name + '</td>'
        var col6 = '<td>' + this.result + '</td>'

        row = row + col1 + col2 + col3 + col4 + col5 + col6 + '</tr>';

        child = child + row;
    });

    child = child + '</tbody></table>';

    return child;
}

function initDtp() {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    $('#dtpFrom').val(today);
    $('#dtpTo').val(today);
}

function getMatches() { //from search button
    selectedMatchId = null;
    $('#lbl-match-id').text('No match selected!!!');
    $('#tbl-info-events tbody tr').remove();
    searchKey = 1;
    showMatches();
}

function showMatches() { // from timer
    if (searchKey === 1) {
        if ($('#dtpFrom').val() == '') {
            alert("You should select initial date");
            return;
        }

        if ($('#dtpTo').val() == '') {
            alert("You should select final date");
            return;
        }

        var jsonData = {
            category_id: $('#cboCategories').val(),
            dateFrom: $('#dtpFrom').val(),
            dateTo: $('#dtpTo').val()
        };

        $.ajax({
            type: 'POST',
            url: '../../home/getMatches/',
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify(jsonData),
            success: showMatchesOnSuccess,
            error: showMatchesOnError
        });
    }
}

function showMatchesOnSuccess(data) {
    $('#dtOdds tbody tr').remove();
    

    for (var i = 0; i < data.data.length; i++) {        
        var row = '<tr>' +
            "<td>" + (i+1) + "</td>" +
            '<td><input type="button" value="' + data.data[i].match_id + '" onclick=loadEvents(this) /></td>' +
            '<td class="match-id">' + data.data[i].match_id + '</td>' + // match id
            "<td>" + data.data[i].status + "</td>" + // status
            "<td>" + data.data[i].datetime + "</td>" + // date time
            "<td>" + data.data[i].stadium + "</td>" + // stadium
            "<td>" + data.data[i].category_name + "</td>" + // category
            "<td>" + data.data[i].localteam_name + "</td>" + // localteam name
            "<td>" + data.data[i].localteam_goals + "</td>" + // localteam goals
            "<td>" + data.data[i].visitorteam_name + "</td>" + // visitorteam name
            "<td>" + data.data[i].visitorteam_goals + "</td>" + // visitorteam goals
            "</tr>";

        $('#dtOdds tbody').append(row);
    }
}

function showMatchesOnError(error) {
    console.log("error from ajax", error);
}

function loadEvents(e) {    
    $('#lbl-match-id').text(e.value);
    showOddTypes(e.value);
    selectedMatchId = e.value;
    showEvents();
}

function toggleMatchInfo(info) {
    var x = document.getElementsByClassName('match-info');

    for (var i = 0; i < x.length; i++) {
        x[i].style.display = "none";
    }

    document.getElementById(info).style.display = "block";
}

function showEvents() {
    if (selectedMatchId !== null) {
        var jsonData = {
            match_id: selectedMatchId
        };

        $.ajax({
            type: 'POST',
            url: '../../home/getEvents/',
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify(jsonData),
            success: showEventsOnSuccess,
            error: showEventsOnError
        });
    } else {
        $('#lbl-match-id').text('No match selected!!!');
        $('#tbl-info-events tbody tr').remove();
    }
}

function showEventsOnSuccess(data) {
    $('#tbl-info-events tbody tr').remove();
    for (var i = 0; i < data.data.length; i++) {
        console.log("data", data.data[i]);

        var row = '<tr>' +
            "<td>" + (i + 1) + "</td>" +            
            "<td>" + data.data[i].type + "</td>" + // type
            "<td>" + data.data[i].team + "</td>" + // team
            "<td>" + data.data[i].minute + "</td>" + // minute
            "<td>" + data.data[i].player_name + "</td>" + // player name
            "<td>" + data.data[i].assist_name + "</td>" + // assist name
            "<td>" + data.data[i].result + "</td>" + // result     
            "</tr>";

        $('#tbl-info-events tbody').append(row);
    }
}

function showEventsOnError(error) {
    console.log("error on show events", error);
}

function showOddTypes(match_id) {
    console.log("triggering odd types", match_id);
    var jsonData = {
        match_id: match_id
    };

    $.ajax({
        type: 'POST',
        url: '../../home/getOddTypes/',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(jsonData),
        success: showOddTypesOnSuccess,
        error: showOddTypesOnError
    });
}

function showOddTypesOnSuccess(data) {
    $('#cboOddTypes option').remove();

    var option = $(document.createElement('option'));
    option.text(data.data.length == 0 ? "No odd types..." : "Select an odd type...");
    option.val(0);
    $("#cboOddTypes").append(option);

    for (var i = 0; i < data.data.length; i++) {
        var option = $(document.createElement('option'));
        option.text(data.data[i].name);
        option.val(data.data[i].odd_type_id);
        $("#cboOddTypes").append(option);
    }    
}

function showOddTypesOnError(error) {
    console.log("error show odd types", error);
}
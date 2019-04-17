/*
 * Author: Abdullah A Almsaeed
 * Date: 4 Jan 2014
 * Description:
 *      This is a demo file used only for the main dashboard
 **/

$(function () {

     "use strict";
     var materialList = [];
     var oAssemblyDataTable;

     var itemResult = $("#hdnLabel").val() === undefined || $("#hdnLabel").val() === "" ? "material" : $("#hdnLabel").val();
     $("#hdnLabel").val(itemResult);

     //Make the dashboard widgets sortable Using jquery UI
     $(".connectedSortable").sortable({
          placeholder: "sort-highlight",
          connectWith: ".connectedSortable",
          handle: ".box-header, .nav-tabs",
          forcePlaceholderSize: true,
          zIndex: 999999
     });
     $(".connectedSortable .box-header, .connectedSortable .nav-tabs-custom").css("cursor", "move");

     //jQuery UI sortable for the todo list
     $(".todo-list").sortable({
          placeholder: "sort-highlight",
          handle: ".handle",
          forcePlaceholderSize: true,
          zIndex: 999999
     });

     //bootstrap WYSIHTML5 - text editor
     $(".textarea").wysihtml5();

     $('.daterange').daterangepicker({
          ranges: {
               'Today': [moment(), moment()],
               'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
               'Last 7 Days': [moment().subtract(6, 'days'), moment()],
               'Last 30 Days': [moment().subtract(29, 'days'), moment()],
               'This Month': [moment().startOf('month'), moment().endOf('month')],
               'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
          },
          startDate: moment().subtract(29, 'days'),
          endDate: moment()
     }, function (start, end) {
          window.alert("You chose: " + start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
     });

     /* jQueryKnob */
     $(".knob").knob();

     //jvectormap data
     var visitorsData = {
          "US": 398, //USA
          "SA": 400, //Saudi Arabia
          "CA": 1000, //Canada
          "DE": 500, //Germany
          "FR": 760, //France
          "CN": 300, //China
          "AU": 700, //Australia
          "BR": 600, //Brazil
          "IN": 800, //India
          "GB": 320, //Great Britain
          "RU": 3000 //Russia
     };
     //World map by jvectormap
     $('#world-map').vectorMap({
          map: 'world_mill_en',
          backgroundColor: "transparent",
          regionStyle: {
               initial: {
                    fill: '#e4e4e4',
                    "fill-opacity": 1,
                    stroke: 'none',
                    "stroke-width": 0,
                    "stroke-opacity": 1
               }
          },
          series: {
               regions: [{
                    values: visitorsData,
                    scale: ["#92c1dc", "#ebf4f9"],
                    normalizeFunction: 'polynomial'
               }]
          },
          onRegionLabelShow: function (e, el, code) {
               if (typeof visitorsData[code] != "undefined")
                    el.html(el.html() + ': ' + visitorsData[code] + ' new visitors');
          }
     });

     //Sparkline charts
     var myvalues = [1000, 1200, 920, 927, 931, 1027, 819, 930, 1021];
     $('#sparkline-1').sparkline(myvalues, {
          type: 'line',
          lineColor: '#92c1dc',
          fillColor: "#ebf4f9",
          height: '50',
          width: '80'
     });
     myvalues = [515, 519, 520, 522, 652, 810, 370, 627, 319, 630, 921];
     $('#sparkline-2').sparkline(myvalues, {
          type: 'line',
          lineColor: '#92c1dc',
          fillColor: "#ebf4f9",
          height: '50',
          width: '80'
     });
     myvalues = [15, 19, 20, 22, 33, 27, 31, 27, 19, 30, 21];
     $('#sparkline-3').sparkline(myvalues, {
          type: 'line',
          lineColor: '#92c1dc',
          fillColor: "#ebf4f9",
          height: '50',
          width: '80'
     });

     //The Calender
     $("#calendar").datepicker();

     //SLIMSCROLL FOR CHAT WIDGET
     $('#chat-box').slimScroll({
          height: '250px'
     });

     /* Morris.js Charts */
     // Sales chart
     var area = new Morris.Area({
          element: 'revenue-chart',
          resize: true,
          data: [
            { y: '2011 Q1', item1: 2666, item2: 2666 },
            { y: '2011 Q2', item1: 2778, item2: 2294 },
            { y: '2011 Q3', item1: 4912, item2: 1969 },
            { y: '2011 Q4', item1: 3767, item2: 3597 },
            { y: '2012 Q1', item1: 6810, item2: 1914 },
            { y: '2012 Q2', item1: 5670, item2: 4293 },
            { y: '2012 Q3', item1: 4820, item2: 3795 },
            { y: '2012 Q4', item1: 15073, item2: 5967 },
            { y: '2013 Q1', item1: 10687, item2: 4460 },
            { y: '2013 Q2', item1: 8432, item2: 5713 }
          ],
          xkey: 'y',
          ykeys: ['item1', 'item2'],
          labels: ['Item 1', 'Item 2'],
          lineColors: ['#a0d0e0', '#3c8dbc'],
          hideHover: 'auto'
     });
     var line = new Morris.Line({
          element: 'line-chart',
          resize: true,
          data: [
            { y: '2011 Q1', item1: 2666 },
            { y: '2011 Q2', item1: 2778 },
            { y: '2011 Q3', item1: 4912 },
            { y: '2011 Q4', item1: 3767 },
            { y: '2012 Q1', item1: 6810 },
            { y: '2012 Q2', item1: 5670 },
            { y: '2012 Q3', item1: 4820 },
            { y: '2012 Q4', item1: 15073 },
            { y: '2013 Q1', item1: 10687 },
            { y: '2013 Q2', item1: 8432 }
          ],
          xkey: 'y',
          ykeys: ['item1'],
          labels: ['Item 1'],
          lineColors: ['#efefef'],
          lineWidth: 2,
          hideHover: 'auto',
          gridTextColor: "#fff",
          gridStrokeWidth: 0.4,
          pointSize: 4,
          pointStrokeColors: ["#efefef"],
          gridLineColor: "#efefef",
          gridTextFamily: "Open Sans",
          gridTextSize: 10
     });

     //Donut Chart
     var donut = new Morris.Donut({
          element: 'sales-chart',
          resize: true,
          colors: ["#3c8dbc", "#f56954", "#00a65a"],
          data: [
            { label: "Download Sales", value: 12 },
            { label: "In-Store Sales", value: 30 },
            { label: "Mail-Order Sales", value: 20 }
          ],
          hideHover: 'auto'
     });

     //Fix for charts under tabs
     $('.box ul.nav a').on('shown.bs.tab', function () {
          area.redraw();
          donut.redraw();
          line.redraw();
     });

     /* The todo list plugin */
     $(".todo-list").todolist({
          onCheck: function (ele) {
               window.console.log("The element has been checked");
               return ele;
          },
          onUncheck: function (ele) {
               window.console.log("The element has been unchecked");
               return ele;
          }
     });
});

function createCreatePopupScreen() {
     //var url = "@Html.Raw(Url.Action('Create', 'Material'))";
     $("#data-section").dialog({
          autoOpen: false,
          width: 420,
          resizable: false,
          draggable: true,
          model: true,
          show: 'slide',
          closeText: 'x',
          dialogClass: 'alert',
          closeOnEscape: true,
          open: function (event, ui) {
               //Load the Partial View Here using Controller and Action
               $("#data-section").load("/Material/Create");
          },
          close: function () {
               $("button[id^='controlBtn']").prop("disabled", false);
               $("button[id^='editCtrlBtn']").prop("disabled", false);
               $("button[id^='retireCtrlBtn']").prop("disabled", false);
          }
     });
}

function LoadRetireMaterialSection(itemId) {
     $.ajax({
          url: '/Material/Delete?id=' + itemId,
          contentType: "application/html; charset=utf-8",
          cache: !0,
          type: "GET",
          data: JSON.stringify({ id: itemId }),
          dataType: "html",
          success: function (data) {
               $("#cud-section").html(data);
          },
          error: function (data) {
               var resultingData = JSON.parse(data);
               var message = JSON.stringify(resultingData);
               alert(message);
               setTimeout($(".overlay").hide(), 55000);
          }
     });
}

function LoadUpdateMaterialSection(itemId) {
     $.ajax({
          url: '/Material/Edit?id=' + itemId,
          contentType: "application/html; charset=utf-8",
          cache: !0,
          type: "GET",
          data: JSON.stringify({ id: itemId }),
          dataType: "html",
          success: function (data) {
               $("#cud-section").html(data);
          },
          error: function (data) {
               var resultingData = JSON.parse(data);
               var message = JSON.stringify(resultingData);
               alert(message);
               setTimeout($(".overlay").hide(), 55000);
          }
     });
}

function LoadCreateBOMSection() {
     $.ajax({
          url: '/BOM/Create'
        , contentType: 'application/html;charset=utf-8'
        , cache: !0
        , type: 'GET'
        , dataType: 'html'
        , success: function (data) {
             //LoadAssemblyListForBOM();
             $('#cud-section').html(data);
        }
        , error: function (data) {
             var message = JSON.stringify(data);
             alert(message);
             setTimeout($(".overlay").hide(), 55000);
        }
     });

     $("#data-section").collapse("toggle");
     EnableControlBtns_Assembly();
}

function LoadUpdateBomSection(code) {
     $.ajax({
          url: '/BOM/Edit?code=' + code,
          type: 'GET',
          cache: !0,
          data: JSON.stringify(code),
          dataType: "html",
          contentType: "application/json; charset=utf-8",
          success: function (data) {
               $("#cud-section").html(data);
          },
          error: function (data) {
               var resultingData = JSON.parse(data);
               var message = JSON.stringify(resultingData);
               alert(message);
               setTimeout($(".overlay").hide(), 55000);
          }
     });
}

function LoadCreateAssemblySection() {
     $.ajax({
          url: '/Assembly/Create'
         , contentType: 'application/html;charset=utf-8'
         , cache: !0
         , type: 'GET'
         , dataType: 'html'
         , success: function (data) {
              LoadAssemblyList(undefined);
              $('#cud-section').html(data);
         }
         , error: function (data) {
              var message = JSON.stringify(data);
              alert(message);
              setTimeout($(".overlay").hide(), 55000);
         }
     });

     $("#data-section").collapse("toggle");
     EnableControlBtns_Assembly();
}

function LoadUpdateAssemblySection(itemId) {
     $.ajax({
          url: '/Assembly/Edit?id=' + itemId
         , contentType: 'application/html;charset=utf-8'
         , cache: !0
         , type: 'GET'
         , dataType: 'html'
         , success: function (data) {
              LoadAssemblyList(itemId);
              $('#cud-section').html(data);
         }
         , error: function (data) {
              var message = JSON.stringify(data);
              alert(message);
              setTimeout($(".overlay").hide(), 55000);
         }
     });

     $("#data-section").collapse("toggle");
     EnableControlBtns_Assembly();
}

function LoadCreateMaterialSection() {
     $.ajax({
          url: '/Material/Create',
          contentType: "application/html; charset=utf-8",
          cache: !0,
          type: "GET",
          dataType: "html",
          success: function (data) {
               $("#cud-section").html(data);
          },
          error: function (data) {
               var message = JSON.stringify(data);
               alert(message);
               setTimeout($(".overlay").hide(), 55000);
          }
     });
     $("#data-section").collapse("toggle");
     EnableControlBtns();
}

function deleteAssemblyClick(itemId) {
     if (confirm("Are you sure you want to remove this assembly?")) {
          DeleteAssemblyItem(itemId);
     }
     return false;

     //$("#data-section").collapse("toggle");
     EnableControlBtns_Assembly();
}

function editSelectedAssembly(itemId) {
     $("#data-section").collapse("toggle");
     DisableControlBtns_Assembly();
     LoadUpdateAssemblySection(itemId);
}

function editSelectedMaterial(itemId) {
     $("#data-section").collapse("toggle");
     DisableControlBtns();
     LoadUpdateMaterialSection(itemId);
}

function editSelectedBom(item) {
     $("#data-section").collapse("toggle");
     DisableControlBtns();
     LoadUpdateBomSection(item);
}
function retireSelectedMaterial(itemId) {
     $("#data-section").collapse("toggle");
     DisableControlBtns
     LoadRetireMaterialSection(itemId);
}

function assignEditValues() {
     var descData = $("#descriptionItem").val();
     $("#materialDescription").val(descData);
}

function createRecordRow(selectedTab) {
     switch (selectedTab) {
          case "material": {
               LoadCreateMaterialSection();
          } break;
          case "assembly": {
               LoadCreateAssemblySection();
          } break;
          case "bom": {
               LoadCreateBOMSection();
          } break;
     }
}

function InitializeData() {
     $("#Code").val("");
     $("#UnitPrice").val("");
     $("#Description").val("");
}

function EnableControlBtns() {
     $("button[id^='controlBtn']").prop("disabled", false);
     $("button[id^='editCtrlBtn']").prop("disabled", false);
     $("button[id^='retireCtrlBtn']").prop("disabled", false);
}

function DisableControlBtns() {
     $("button[id^='controlBtn']").prop("disabled", true);
     $("button[id^='editCtrlBtn']").prop("disabled", true);
     $("button[id^='retireCtrlBtn']").prop("disabled", true);
}

function EnableControlBtns_Assembly() {
     $("button[id^='controlBtn']").prop("disabled", false);
     $("button[id^='editCtrlBtn_Assembly']").prop("disabled", false);
     $("button[id^='retireCtrlBtn_Assembly']").prop("disabled", false);
}

function DisableControlBtns_Assembly() {
     $("button[id^='controlBtn']").prop("disabled", true);
     $("button[id^='editCtrlBtn_Assembly']").prop("disabled", true);
     $("button[id^='retireCtrlBtn_Assembly']").prop("disabled", true);
}
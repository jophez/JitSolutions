$(function () {
     "use strict";
});

function LoadMaterialList() {
     $.ajax({
          type: "GET",
          url: "/Material/TableResult",
          dataType: 'json',
          success: function (obj, textstatus) {
               setTimeout($(".overlay").hide(), 55000);
               $('#tblMaterials').DataTable({
                    "destroy": true
                  , "data": JSON.parse(obj)
                  , "scrollY": "500px"
                  , "columns":
                      [
                          { "data": "Id" }
                          , { "data": "Code" }
                          , { "data": "Description" }
                          , { "data": "DateCreated" }
                          , { "data": "IsActive" }
                          , { "data": "UnitPrice" }
                          , {
                               "data": null,
                               "sortable": false,
                               "render":
                               function (data, type, full) {
                                    var item = "";
                                    item = full['Id'];
                                    return "<button class='btn btn-primary bg-green' data-toggle='tooltip' title='Edit Material' id='editCtrlBtn' data-toggle='collapse'  data-target='#data-section' onclick='editSelectedMaterial(" + item + ")'><i class='glyphicon glyphicon glyphicon-edit'></i></button>"
                               }
                          }
                          , {
                               "data": null,
                               "sortable": false,
                               "render":
                               function (data, type, full) {
                                    var item = "";
                                    item = full['Id'];
                                    return "<button class='btn btn-primary bg-green' data-toggle='tooltip' title='Delete Material' id='retireCtrlBtn' data-toggle='collapse'  data-target='#data-section' onclick='retireSelectedMaterial(" + item + ")'><i class='glyphicon glyphicon-trash'></i></button>"
                               }
                          }
                      ]
                  , "columnDefs": [
                      {
                           "targets": [0],
                           "visible": false,
                           "searchable": false
                      }
                      , {
                           "targets": [2],
                           "width": "250px"
                      },
                      {
                           "targets": [3],
                           "width": "100px"
                      },
                      {
                           "targets": [4],
                           "width": "25px"
                      }
                  ]
               });
          },
          error: function (obj, textstatus) {
               alert(obj.msg);
          }
     });
}

function retireMaterialRecord() {
     var materialObject = {
          Code: $("#Code").val(),
          UnitPrice: $("#UnitPrice").val(),
          Description: $("#Description").val(),
          Id: $("#Id").val()
     };

     $(".overlay").show();

     $.ajax({
          url: "/Material/Delete?id=" + materialObject.Id,
          type: "POST",
          data: JSON.stringify(materialObject),
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          success: function (result) {
               if (result["Error"] != null) {
                    var message = JSON.stringify(result);
                    alert(message);
                    setTimeout($(".overlay").hide(), 55000);
               }
               else {
                    RedrawTable();
                    InitializeData();
                    // $("#material-count").val($('#tblMaterials tr').length);
               }
          },
          error: function (result) {
               var message = JSON.stringify(result);
               alert(message);
               setTimeout($(".overlay").hide(), 55000);
          }
     });

     $("#data-section").collapse("toggle");
     EnableControlBtns();

     return false;
}

function createMaterialRecord() {
     var materialObject = {
          Code: $("#Code").val(),
          UnitPrice: $("#UnitPrice").val(),
          Description: $("#Description").val()
     };
     $(".overlay").show();
     $.ajax({
          url: "/Material/Create",
          type: "POST",
          data: JSON.stringify(materialObject),
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          success: function (result) {
               if (result["Error"] != null) {
                    var message = JSON.stringify(result);
                    alert(message);
                    setTimeout($(".overlay").hide(), 55000);
               }
               else {
                    RedrawTable();
                    InitializeData();
               }
          },
          error: function (result) {
               var message = JSON.stringify(result);
               alert(message);
               setTimeout($(".overlay").hide(), 55000);
          }
     });

     $("#data-section").collapse("toggle");
     EnableControlBtns();

     return false;
}

function updateMaterialRecord() {
     var materialObject = {
          Code: $("#Code").val(),
          UnitPrice: $("#UnitPrice").val(),
          Description: $("#Description").val(),
          Id: $("#Id").val()
     };
     $(".overlay").show();

     $.ajax({
          url: "/Material/Edit?id=" + materialObject.Id,
          type: "POST",
          data: JSON.stringify(materialObject),
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          success: function (result) {
               if (result["Error"] != null) {
                    var message = JSON.stringify(result);
                    alert(message);
                    setTimeout($(".overlay").hide(), 55000);
               }
               else {
                    RedrawTable();
                    InitializeData();
               }
          },
          error: function (result) {
               var message = JSON.stringify(result);
               alert(message);
               setTimeout($(".overlay").hide(), 55000);
          }
     });

     $("#data-section").collapse("toggle");
     EnableControlBtns();

     return false;
}

function bindReady() {
     if (readyBound) return;
     readyBound = true;

     // Mozilla, Opera and webkit nightlies currently support this event
     if (document.addEventListener) {
          // Use the handy event callback
          document.addEventListener("DOMContentLoaded", function () {
               document.removeEventListener("DOMContentLoaded", arguments.callee, false);
               jQuery.ready();
          }, false);

          // If IE event model is used
     } else if (document.attachEvent) {
          // ensure firing before onload,
          // maybe late but safe also for iframes
          document.attachEvent("onreadystatechange", function () {
               if (document.readyState === "complete") {
                    document.detachEvent("onreadystatechange", arguments.callee);
                    jQuery.ready();
               }
          });

          // If IE and not an iframe
          // continually check to see if the document is ready
          if (document.documentElement.doScroll && window == window.top) (function () {
               if (jQuery.isReady) return;

               try {
                    // If IE is used, use the trick by Diego Perini
                    // http://javascript.nwbox.com/IEContentLoaded/
                    document.documentElement.doScroll("left");
               } catch (error) {
                    setTimeout(arguments.callee, 0);
                    return;
               }

               // and execute any waiting functions
               jQuery.ready();
          })();
     }

     // A fallback to window.onload, that will always work
     jQuery.event.add(window, "load", jQuery.ready);
}

function RedrawTable() {
     try {
          setTimeout($(".overlay").hide(), 55000);
          location.reload();
     }
     catch (ex) {

     }
}

function assignValues() {
     var descData = $("#materialDescription").val();
     $("#descriptionItem").val(descData);
}

function CancelOperation() {
     InitializeData();
     $("#data-section").collapse("toggle");
     EnableControlBtns();
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

$(function () {
     "use strict";

     $('.custom-close').on('click', function () {
          $('#modal-pole-asselmbly').modal('hide');
          $('#modal-pole-asselmbly-edit').modal('hide');
     })
})

var assemblyList = [];
var poleAssemblyList = [];
var poleAssemblyCollection = [];
var poleList = [];
var bomId = "";

function AssignPoleToBom(d) {
     var poleList = "";
     for (var index = 0; index < d.PoleList.length; index++) {
          var io = d.PoleList[index].BSpan * d.PoleList[index].Wires;
          poleList += '<tr>' +
                '<td>Pole Number :</td>' +
                '<td style="padding:5px">' + d.PoleList[index].Number + '| </td>' +
                '<td style="padding:5px">Pole Code :</td>' +
                '<td style="padding:5px">' + d.PoleList[index].Code + '| </td>' +
                '<td style="padding:5px">BSpan    :</td>' +
                '<td style="padding:5px">' + d.PoleList[index].BSpan + '| </td>' +
                '<td style="padding:5px">Wires    :</td>' +
                '<td style="padding:5px">' + d.PoleList[index].Wires + '| </td>' +
                '<td style="padding:5px">I/O      :</td>' +
                '<td style="padding:5px">' + io + '</td>' +
            '</tr>' +
            '<tr><td>--------------------</td></tr>'
     }
     return '<table cellpadding="5" cellspacing="0" border="0" class="pull-right" style="width:90%;display:inline-block;position:relative">'
     + poleList
     '</table>';
}

function PoleAssemblyForEditPopup(number) {
     $.ajax({
          type: "GET",
          url: "/BOM/PoleAssemblyForEditPopup?code=" + number,
          dataType: "html",
          cache: !0,
          data: JSON.stringify(number),
          contentType: "application/json; charset=utf-8",
          success: function (data) {
               $("#modal-container").html(data);
          },
          error: function (data) {
               var resultingData = JSON.parse(data);
               var message = JSON.stringify(resultingData);
               alert(message);
               setTimeout($(".overlay").hide(), 55000);
          }
     })
}

function PopupPoleAssembly() {
     $.ajax({
          type: "GET",
          url: "/BOM/popupPoleAssembly",
          dataType: "html",
          cache: !0,
          data: JSON.stringify(),
          contentType: "application/json; charset=utf-8",
          success: function (data) {
               $("#modal-container").html(data);
          },
          error: function (data) {
               var resultingData = JSON.parse(data);
               var message = JSON.stringify(resultingData);
               alert(message);
               setTimeout($(".overlay").hide(), 55000);
          }
     })
}

function LoadBomListItems() {
     $(".overlay").show();
     $.ajax({
          type: "GET",
          url: "/BOM/LoadBomListItems",
          dataType: "json",
          cache: false,
          success: function (obj, textstatus) {
               setTimeout($(".overlay").hide(), 55000);
               var oPoleListTable = $("#tblBom").DataTable({
                    "destroy": true,
                    "data": JSON.parse(obj),
                    "scrollY": "500px",
                    "columns": [
                    {
                         "className": 'details-control',
                         "orderable": false,
                         "data": null,
                         "sortable": false,
                         "width": "10px;",
                         "defaultContent": '',
                         "render": function () {
                              return "<i class='fa fa-plus'></i>"
                         }
                    }
                       , { "data": "Name", "width": "100px" }
                       , { "data": "Location", "width": "175px" }
                       , { "data": "Owner", "width": "100px" }
                       , { "data": "Description", "width": "175px" }
                       , { "data": "TotalIo", "width": "100px" }
                       , {
                            "data": null,
                            "sortable": false,
                            "render":
                            function (data, type, full) {
                                 var item = "";
                                 item = full['Code'];
                                 return "<button class='btn btn-primary bg-green' data-toggle='tooltip' title='Edit Assembly' id='editCtrlBtn' data-toggle='collapse'  data-target='#data-section' onclick='editSelectedBom(\"" + item + "\")'><i class='glyphicon glyphicon glyphicon-edit'></i></button>"
                            }
                       }
                    , {
                         "data": null,
                         "sortable": false,
                         "render":
                         function (data, type, full) {
                              var item = "";
                              item = full['Code'];
                              return "<button class='btn btn-primary bg-powder-blue' data-toggle='tooltip' title='Export to Excel' id='exportCtrlBtn' data-toggle='collapse'  data-target='#data-section' onclick='ExportSelectedBom(\"" + item + "\")'><i class='glyphicon glyphicon glyphicon-save'></i></button>"
                         }
                    }
                    ]
               });

               oPoleListTable.$('tr').on("click", "td.details-control", function (e) {
                    var tr = $(this).parents('tr');
                    var row = oPoleListTable.row(tr);

                    if (row.child.isShown()) {
                         // This row is already open - close it
                         row.child.hide();
                         tr.removeClass('shown');
                    }
                    else {
                         // Open this row
                         row.child(AssignPoleToBom(row.data())).show();
                         tr.addClass('shown');
                    }
                    // Prevent click event from propagating to parent
                    e.stopPropagation();
               });
          }
     })
}

function ExportSelectedBom(bomId) {
     $.ajax({
          url: '',
          type: 'GET',
          dataType: 'json',
          success: function (obj, texstatus) {
               alert('BOM : ' + bomId + 'successfully exported.');
          },
          error: function (obj, textstatus) {
               alert(obj.msg);
          }
     });
}

function LoadAssemblyListForBOM() {
     $.ajax({
          url: '/BOM/TableAssemblyBOMResult',
          type: 'GET',
          dataType: 'json',
          success: function (obj, textstatus) {
               oAssemblyDataTable = $('#poleAssemblyTbl').DataTable({
                    "data": JSON.parse(obj)
                     , "paging": false
                     , "scrollY": "250px"
                     , "columns":
                         [
                             {
                                  "data": "Id"
                                 , "sortable": false
                                 , "render":
                                 function (data, type, full, meta) {
                                      var itemId = full['Id'];
                                      return "<input type='checkbox' name='assemblyCheck' id=" + itemId + "/>"
                                 }
                             }
                             , { "data": "Name" }
                             , {
                                  "data": "Classification"
                                 , "width": "350px"
                             }
                             , {
                                  "data": "AssemblyPrice"
                                  , "width": "100px"
                             }
                         ]
               });
          },
          error: function (obj, textstatus) {
               alert(obj.msg);
          }
     });
}

function DeletePole(counter) {
     $("#pole_" + counter).closest("tr").remove();
     poleList.pop(counter);
}

function AddNewPole() {
     var counter = $("#poleAssemblyTbl > tbody > tr").length;
     var newRow = $("<tr>");
     var cols = "";
     var lastRow = $("#poleAssemblyTbl > tbody > tr:last");
     var lastRowId = parseInt(lastRow.find("td").attr("Id").split("_")[1]);

     if (lastRowId === parseInt("0"))
          lastRowId = 1;
     else
          lastRowId++;

     cols += '<td id="pole_' + lastRowId + '">' + lastRowId + '</td>'
     cols += '<td><input type="text" id="pId_' + lastRowId + '" name="pId_' + lastRowId + '" class="form-control"/></td>'
     cols += '<td><input type="number" class="form-control" id="bSpan_' + lastRowId + '" onkeyup="CalculateIO(' + lastRowId + ')" style="width:75px"/></td>';
     cols += '<td><input type="number" class="form-control" id="wires_' + lastRowId + '" onkeyup="CalculateIO(' + lastRowId + ')" style="width:75px"/></td>';
     cols += '<td><input id="ioResult_' + lastRowId + '" name="ioResult_' + lastRowId + '" style="width:100px;" value="0" disabled="disabled"/></td>';
     cols += '<td><input type="button" class="btn btn-md btn-block" value="Add Assembly" onclick="PopupPoleAssembly()" data-target="#modal-container" data-toggle="modal" id="poleAssembly_' + lastRow + '"/></td>'
     //cols += '<td><a href=@Url.Action("popupPoleAssembly","BOM") data-target="#modal-container" data-toggle="modal" id="poleAssembly_' + lastRow + '">Add Assembly</a>';
     cols += '<td><input type="button" class="ibtnDel btn btn-md btn-danger "  value="Delete" onclick="DeletePole(' + lastRowId + ')"></td>';
     newRow.append(cols);

     var poleItem = {
          "poleId": lastRowId,
          "bSpan": $("bSpan_" + lastRowId).val(),
          "wires": $("wires_" + lastRowId).val()
     };

     poleList.push(poleItem);
     $("#poleAssemblyTbl tbody").append(newRow);
}

function AddNewPoleForEdit() {
     var counter = $("#poleAssemblyTbl > tbody > tr").length;
     var newRow = $("<tr>");
     var cols = "";
     var lastRow = $("#poleAssemblyTbl > tbody > tr:last");
     var lastRowId = parseInt(lastRow.find("td").attr("Id").split("_")[1]);

     if (lastRowId === parseInt("0"))
          lastRowId = 1;
     else
          lastRowId++;

     cols += '<td id="pole_' + lastRowId + '">' + lastRowId + '</td>'
     cols += '<td><input type="text" id="pId_' + lastRowId + '" name="pId_' + lastRowId + '" class="form-control"/></td>'
     cols += '<td><input type="number" class="form-control" id="bSpan_' + lastRowId + '" onkeyup="CalculateIO(' + lastRowId + ')" style="width:75px"/></td>';
     cols += '<td><input type="number" class="form-control" id="wires_' + lastRowId + '" onkeyup="CalculateIO(' + lastRowId + ')" style="width:75px"/></td>';
     cols += '<td><input id="ioResult_' + lastRowId + '" name="ioResult_' + lastRowId + '" style="width:100px;" value="0" disabled="disabled"/></td>';
     cols += '<td><input type="button" class="btn btn-md btn-block" onclick="PoleAssemblyForEditPopup(' + lastRowId + ')"  data-target="#modal-container" data-toggle="modal" value="Add Assembly" id="poleAssembly_' + lastRow + '"/></td>';
     cols += '<td><input type="button" class="ibtnDel btn btn-md btn-danger "  value="Delete" onclick="DeletePole(' + lastRowId + ')"></td>';
     newRow.append(cols);

     var poleItem = {
          "poleId": lastRowId,
          "bSpan": $("bSpan_" + lastRowId).val(),
          "wires": $("wires_" + lastRowId).val()
     };

     poleList.push(poleItem);
     $("#poleAssemblyTbl tbody").append(newRow);
}

function CalculateIO(poleCounter) {

     var span = parseInt($("#bSpan_" + poleCounter).val()) || 0;
     var wires = parseInt($("#wires_" + poleCounter).val()) || 0;
     var result = span * wires;
     $("#ioResult_" + poleCounter).val(result);
     //document.getElementById("ioResult_" + poleCounter).innerText = result;
}

function CalculateAssemblyPrice(assemblyId) {
     var qty = parseInt($("#assemblyQty_" + assemblyId).val()) || 0;
     var amount = parseFloat($("#unitPrice_" + assemblyId).val()) || 0;

     $("#assemblyAmount_" + assemblyId).val(qty * amount);
}
function DisplayAssembly() {
     $("#poleAssemblyList").dialog({
          autoOpen: true,
          width: "500",
          resizeable: false,
          title: "Assembly List",
          modal: true,
     });
     $("#poleAssemblyList").dialog('open');
}

function ResetValues() {
     $('input[name=assemblyQty]').val('');
     $("input[name=assemblyAmount]").val('');
     $("input[name=checkAssembly]").each(function () { this.checked = false; })
     poleAssemblyList = [];
}

function ClosePopup() {
     $("#modal-container").modal('hide');
}

function PersistAssembliesToPole() {
     if ($("#pName").val().length === 0)
     { alert("Please fill up the Project Name field ... Current information will not be saved."); return false; }
     if ($("#pLocation").val().length === 0)
     { alert("Please fill up the Project Location field ... Current information will not be saved."); return false; }
     if ($("#pOwner").val().length === 0)
     { alert("Please fill up the Project Owner field ... Current information will not be saved."); return false; }
     else
          poleAssemblyCollection.push(AssembleAssemblyList());
}

function CreateAssemblyRecordBom() {
     var valid = ValidatePoleObject();
     if (!valid) {
          alert("Bill of Materials has invalid entries. Review your input and adjust as necesssary.");
          return false;
     }
     var tmpGuid = CreateGuid();
     var poleAssembly = [];
     var bomItem = {};
     var markup = {};
     var projectInfo = {};
     var rowCount = $('#poleAssemblyTbl > tbody > tr').length;
     var lastRow = $('#poleAssemblyTbl > tbody > tr:last');
     var rowId = parseInt(lastRow.find("td").attr("Id").split("_")[1]);

     for (var idx = 0; idx < rowCount; idx++) { //Ceate the pole and assembly
          var poleAndAssembly = {
               "Number": idx,
               "RelBomCode": tmpGuid,//$("#bCode").val(),
               "Code": $("#pId_" + idx).val(),
               "BSpan": $("#bSpan_" + idx).val(),
               "Wires": $("#wires_" + idx).val(),
               "AssemblyListToSave": poleAssemblyCollection[idx]
          }
          poleAssembly.push(poleAndAssembly);
     }
     markup = {
          "Id": $("#bMarkup").val(),
          "Code": $("#bMarkup").find(":selected").text(),
     };
     projectInfo = {
          "Name": $("#pName").val(),
          "Location": $("#pLocation").val(),
          "Owner": $("#pOwner").val(),
          "Description": $("#pDescription").val(),
          "ControlNumber": tmpGuid
     };
     bomItem = {  //bill of material object
          "Code": tmpGuid,
          "Markup": markup,
          "PoleList": poleAssembly, //include the pole and assembly objects
          "ProjectInfo": projectInfo
     };

     PersistBomRecord(bomItem);
}

function PersistBomRecord(bomItem) {
     $(".overlay").show();
     $.ajax({
          url: "/BOM/Create",
          type: "POST",
          data: JSON.stringify(bomItem),
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          success: function (result) {
               if (result["Error"] != null) {
                    var message = JSON.stringify(result);
                    alert(message);
                    setTimeout($(".overlay").hide(), 55000);
               }
               else {
                    ResetValues();
               }
          },
          error: function (result) {
               var message = JSON.stringify(result);
               alert(message);
               setTimeout($(".overlay").hide(), 55000);
          }
     });

     poleAssemblyCollection = [];
     $("#data-section").collapse("toggle");
     EnableControlBtns();
     location.reload();

     return false;
}
function ValidatePoleObject() {
     if ($("#pName").val().length === 0)
          return false;
     if ($("#pLocation").val().length === 0)
          return false;
     if ($("#pOwner").val().length === 0)
          return false;
     if ($("#bMarkup").find(":selected").text() === "Select ...")
          return false;
     if (poleAssemblyCollection.length === 0 || poleAssemblyCollection === null)
          return false;
     return true;
}

function AssembleAssemblyList() {
     poleAssemblyList = [];
     var lastRow = $("#poleAssemblyTbl > tbody > tr:last");
     var lastRowId = parseInt(lastRow.find("td").attr("Id").split("_")[1]);
     $.each($("input[type='checkbox']:checked"),
          function () {
               var id = $(this).prop('id');
               var assemblyItem = {
                    AssemblyId: parseInt(id),
                    Number: lastRowId,
                    Code: $("#pId_" + lastRowId).val(),
                    Quantity: parseInt($("#assemblyQty_" + id).val())
               }
               if (assemblyItem.AssemblyId !== 0 && !isNaN(assemblyItem.AssemblyId))
                    poleAssemblyList.push(assemblyItem);
          });

     return poleAssemblyList;
}

function UpdateAssemblyRecordBom() {
     if ($("#pName").val().length === 0) {
          alert("Bill of Materials [Name field] has invalid entries. Review your input and adjust as necesssary.");
          return false;
     }
     if ($("#pLocation").val().length === 0) {
          alert("Bill of Materials [Location field] has invalid entries. Review your input and adjust as necesssary.");
          return false;
     }
     if ($("#pOwner").val().length === 0) {
          alert("Bill of Materials [Owner field] has invalid entries. Review your input and adjust as necesssary.");
          return false;
     }
     if ($("#bMarkup").find(":selected").text() === "Select ...") {
          alert("Bill of Materials [Markup field] has invalid entries. Review your input and adjust as necesssary.");
          return false;
     }

     var tmpGuid = $("#BomId").val();
     var tmpId = $("#Id").val();
     var poleAssembly = [];
     var bomItem = {};
     var markup = {};
     var projectInfo = {};
     var rowCount = $('#poleAssemblyTbl > tbody > tr').length;
     var lastRow = $('#poleAssemblyTbl > tbody > tr:last');
     var rowId = parseInt(lastRow.find("td").attr("Id").split("_")[1]);
     if (poleAssemblyCollection.length === 0) {
          var assemblyItem = {
               AssemblyId: parseInt("0"),
               Number: "0",
               Code: "0",
               Quantity: parseInt("0")
          };
          poleAssemblyCollection.push(assemblyItem);
     }

     for (var idx = 0; idx < rowCount; idx++) { //Ceate the pole and assembly
          var poleAndAssembly = {
               "Number": idx,
               "RelBomCode": tmpGuid,//$("#bCode").val(),
               "Code": $("#pId_" + idx).val(),
               "BSpan": $("#bSpan_" + idx).val(),
               "Wires": $("#wires_" + idx).val(),
               "AssemblyListToSave":  poleAssemblyCollection[idx]
          }
          poleAssembly.push(poleAndAssembly);
     }
     markup = {
          "Id": $("#MarkupId").val(),
          "Code": $("#bMarkup").find(":selected").text(),
     };
     projectInfo = {
          "Name": $("#pName").val(),
          "Location": $("#pLocation").val(),
          "Owner": $("#pOwner").val(),
          "Description": $("#pDescription").val(),
          "ControlNumber": tmpGuid
     };
     bomItem = {  //bill of material object
          "Id": tmpId,
          "Code": tmpGuid,
          "ControlNumber": tmpGuid,
          "MarkupCode": markup.Code,
          "Name": projectInfo.Name,
          "Location": projectInfo.Location,
          "Owner": projectInfo.Owner,
          "Description": projectInfo.Description,
          "Markup": markup,
          "PoleList": poleAssembly, //include the pole and assembly objects
          "ProjectInfo": projectInfo
     };

     PersistUpdatedBomRecord(bomItem);
}

function PersistUpdatedBomRecord(bomItem) {
     $(".overlay").show();
     $.ajax({
          url: "/BOM/Edit",
          type: "POST",
          data: JSON.stringify(bomItem),
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          success: function (result) {
               ResetValues();
          },
          error: function (result) {
               var message = JSON.stringify(result);
               alert(message);
               setTimeout($(".overlay").hide(), 55000);
          }
     });

     poleAssemblyCollection = [];
     $("#data-section").collapse("toggle");
     EnableControlBtns();
     location.reload();

     return false;
}
//http://byronsalau.com/blog/how-to-create-a-guid-uuid-in-javascript/  
function CreateGuid() {
     return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
          var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
          return v.toString(16);
     });
}

function CancelOperation() {
     InitializeData();
     $("#data-section").collapse("toggle");
     EnableControlBtns();
}
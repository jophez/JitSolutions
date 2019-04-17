$(function () {
     "use strict";
});

var materialId = 0;
var materialQuantity = 0;
var materialUnit = '';
var materialList = [];
var materialTempList = [];
var materialIsChecked = false;
var assemblyTable = {};
var rows_selected = [];


function DeleteAssemblyItem(itemId) {
     var assemblyObject = {
          Id: itemId
     };
     $(".overlay").show();
     $.ajax({
          url: "/Assembly/Delete?id=" + assemblyObject.Id,
          type: "POST",
          data: JSON.stringify(assemblyObject),
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

     EnableControlBtns_Assembly();
}
function UpdateAssemblyRecord() {
     var values = new Array();
     var index = 0;
     // assignToAssemblyForUpdate(values);
     var assemblyObject = {
          Name: $("#aName").val(),
          Classification: $("#aClassification").val(),
          Id: $("#Id").val(),
          Materials: AssembleMaterials()
     };
     $(".overlay").show();

     $.ajax({
          url: "/Assembly/Edit?id=" + assemblyObject.Id,
          type: "POST",
          data: JSON.stringify(assemblyObject),
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
     materialList = [];
     EnableControlBtns_Assembly();
}

function CreateAssemblyRecord() {
     var assemblyObject = {
          Name: $("#aName").val(),
          Classification: $("#aClassification").val(),
          Materials: AssembleMaterials()
     };
     if (assemblyObject.Name === undefined || assemblyObject.Name === "")
          alert("Assembly name is a required field");
     else if (assemblyObject.Classification === undefined || assemblyObject.Classification === "")
          alert("Assembly classification is a required field");
     else if (assemblyObject.Materials.length === 0)
          alert("Please select at least one (1) material to bind to the assembly");
     else {
          $(".overlay").show();
          $.ajax({
               url: "/Assembly/Create",
               type: "POST",
               data: JSON.stringify(assemblyObject),
               dataType: "json",
               contentType: "application/json; charset=utf-8",
               success: function (result) {
                    if (result["Error"] != null) {
                         var message = JSON.stringify(result);
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
          materialList = [];
          EnableControlBtns_Assembly();
     }

     return false;
}

function AssignMaterialToAssembly(d) {
     var materialList = "";
     for (var index = 0; index < d.Materials.length; index++) {
          var unitItem = "";
          switch (d.Materials[index].Unit) {
               case 0: { unitItem = "Pc"; break; }
               case 1: { unitItem = "Meters"; break; }
               case 2: { unitItem = "Roll"; break; }
          }

          materialList +=
          '<tr>' +
               '<td>Code:</td>' +
               '<td style="padding:5px">' + d.Materials[index].Code + '</td>' +
           '</tr>' +
           '<tr>' +
               '<td>Description:</td>' +
               '<td style="padding:5px">' + d.Materials[index].Description + '</td>' +
           '</tr>' +
           '<tr>' +
               '<td>Quantity:</td>' +
               '<td style="padding:5px">' + d.Materials[index].Quantity + '</td>' +
           '</tr>' +
           '<tr>' +
               '<td>Base Price:</td>' +
               '<td style="padding:5px">' + d.Materials[index].BasePrice + '</td>' +
           '</tr>' +
           '<tr>' +
               '<td>Actual Cost:</td>' +
               '<td style="padding:5px">' + d.Materials[index].ActualCost + '</td>' +
           '</tr>' +
           '<tr>' +
                '<td>Unit :</td>' +
                    '<td style="padding:5px">' + unitItem + '</td>' +
                 '</tr>' + '<tr><td>----------------------------</td></tr>'
     }
     return '<table cellpadding="5" cellspacing="0" border="0" class="pull-right" style="width:85%;display:inline-block;position:relative">' +
     materialList
     '</table>';
}

function LoadAssemblyListItems() {
     $.ajax({
          type: "GET",
          url: "/Assembly/TableAssemblyListResult",
          dataType: "json",
          success: function (obj, textstatus) {
               var oAssemblyTable = $("#tblAssembly").DataTable({
                    "destroy": true
                      , data: JSON.parse(obj)
                      , "scrollY": "500px"
                      , "columns":
                            [
                                {
                                     "className": 'details-control',
                                     "orderable": false,
                                     "data": null,
                                     "sortable": false,
                                     "width": "25px;",
                                     "defaultContent": '',
                                     "render": function () {
                                          return "<i class='fa fa-plus'></i>"
                                     }
                                }
                                , { "data": "Name", "width": "50px;" }
                                , { "data": "Classification", "width": "125px;" }
                                , { "data": "UnitPrice", "width": "75px;" }
                                , {
                                     "data": null,
                                     "sortable": false,
                                     "render":
                                     function (data, type, full) {
                                          var item = "";
                                          item = full['Id'];
                                          return "<button class='btn btn-primary bg-green' data-toggle='tooltip' title='Edit Assembly' id='editCtrlBtn' data-toggle='collapse'  data-target='#data-section' onclick='editSelectedAssembly(" + item + ")'><i class='glyphicon glyphicon glyphicon-edit'></i></button>"
                                     }
                                }
                            ]
               });

               oAssemblyTable.$('tr').on("click", "td.details-control", function () {
                    var tr = $(this).parents('tr');//$(this).closest('tr');
                    var row = oAssemblyTable.row(tr);

                    if (row.child.isShown()) {
                         // This row is already open - close it
                         row.child.hide();
                         tr.removeClass('shown');
                    }
                    else {
                         // Open this row
                         row.child(AssignMaterialToAssembly(row.data())).show();
                         tr.addClass('shown');
                    }
               });
          }
     })
}

function LoadAssemblyList(id) {
     /* Initialize table and make first column non-sortable*/

     $.ajax({
          type: "GET",
          url: id === undefined ? "/Assembly/TableMaterialAssemblyResult" : "/Assembly/TableMaterialAssemblyResult?id=" + id,
          dataType: 'json',
          success: function (obj, textstatus) {
               oAssemblyDataTable = $('#assemblyMaterialTable').DataTable({
                    "data": JSON.parse(obj)
                     , "paging": false
                     , "scrollY": "250px"
                     , "columns":
                         [
                             {
                                  "className": 'select-checkbox'
                                  , "data": "Id"
                                  , "sortable": false
                                  , "render":
                                  function (data, type, full, meta) {
                                       var itemId = full['Id'];
                                       if (full["isChecked"])
                                            return "<input type='checkbox'  id = " + itemId + " checked=true/>";
                                       else
                                            return "<input type='checkbox'  id = " + itemId + " />";
                                  }
                             }
                             , { "data": "Code" }
                             , {
                                  "data": "Description"
                                 , "width": "250px"
                             }
                             , {
                                  "data": id === undefined ? null : "Quantity"
                                 , "sortable": false
                                 , "render": function (data, type, full, meta) {
                                      var itemId = full['Id'];
                                      var result = data["Quantity"] === undefined ? data : data.Quantity;
                                      return "<input class='form-control' name='assemblyMaterialQuantity' type='number' id='assemblyQuantity" + itemId + "' min='0' max='99999' size='5' value='" + result + "'/>";
                                 }
                             }
                             , {
                                  "data": id === undefined ? null : "Unit"
                                 , "sortable": false
                                 , "render": function (data, type, full, meta) {
                                      var itemId = full['Id'];
                                      var control = "<select name='assemblyMaterialUnit' id='assemblyCombo" + itemId + "'>";
                                      var selectedIndex = data["Unit"] === undefined ? data : data.Unit;
                                      switch (selectedIndex) {
                                           case 0: { control += "<option value='pc' selected>Pc</option><option value='meters'>Meters</option><option value='roll'>Roll</option>"; break };
                                           case 1: { control += "<option value='pc'>Pc</option><option value='meters' selected>Meters</option><option value='roll'>Roll</option>"; break };
                                           case 2: { control += "<option value='pc'>Pc</option><option value='meters'>Meters</option><option value='roll' selected>Roll</option>"; break };
                                      }
                                      control += "</select>";
                                      return control;
                                 }
                             }
                         ], "order": [1, 'asc']
                          , 'rowCallback': function (row, data, dataIndex) {
                               // Get row ID
                               var rowId = data[0];

                               // If row ID is in the list of selected row IDs
                               if ($.inArray(rowId, rows_selected) !== -1) {
                                    $(row).find('input[type="checkbox"]').prop('checked', true);
                                    $(row).addClass('selected');
                               }
                          },
                    error: function (obj, textstatus) {
                         alert(obj.msg);
                    }
               });

               $('#assemblyMaterialTable tbody').on('click', 'input[type="checkbox"]', function (e) {
                    var $row = $(this).closest('tr');
                    var dataObj = {};
                    // Get row data
                    var data = oAssemblyDataTable.row($row).data();

                    // Complete the obj
                    dataObj = data;
                    dataObj["Quantity"] = parseInt(data["Quantity"]);
                    // Get row ID
                    var rowId = dataObj.Id;//data[0];

                    // Determine whether row ID is in the list of selected row IDs 
                    var index = $.inArray(rowId, rows_selected);

                    // If checkbox is checked and row ID is not in list of selected row IDs
                    if (this.checked && index === -1) {
                         rows_selected.push(rowId);

                         // Otherwise, if checkbox is not checked and row ID is in list of selected row IDs
                    } else if (!this.checked && index !== -1) {
                         rows_selected.splice(index, 1);
                    }

                    if (this.checked) {
                         $row.addClass('selected');
                    } else {
                         $row.removeClass('selected');
                    }

                    // Prevent click event from propagating to parent
                    e.stopPropagation();
               });
          }
     });
}

function AssembleMaterials() {
     materialList = [];
     $.each($("input[type='checkbox']:checked"),
           function () {
                var id = $(this).prop("id");
                var materialItem = {
                     Id: id,
                     Quantity: $("#assemblyQuantity" + id).val() === undefined ? 0 : parseInt($("#assemblyQuantity" + id).val()),
                     Unit: $("#assemblyCombo" + id).prop("selectedIndex")
                }
                if (materialItem.Id !== "0" && materialItem.Id !== undefined && materialItem.Id !== "")
                     materialList.push(materialItem);
           });
     return materialList;
}
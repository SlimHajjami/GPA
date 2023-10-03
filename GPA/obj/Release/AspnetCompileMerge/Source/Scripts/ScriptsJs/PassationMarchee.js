
function onSelect(e) {
   
    var treeView = $("#treeview").data("kendoTreeView"); 
    // alert(treeView);
    var selectedDataItem = treeView.dataItem(e.node); 
    var parentNode = selectedDataItem.parentNode();
    var Id = selectedDataItem.id;
    var parentId = parentNode?parentNode.id:null;
    var type_node = $(e.node).data("project");

    
    if (type_node == 'PROJECTS') {

    } else if (type_node == "TYPE_SERVICES") {
        kendo.ui.progress($("#main-section"), true);
        $.ajax({
            url: '../Actions/ActionView/',
            data: { typeService: Id, projetId: parentId },
            type: "GET",
            dataType: "html",
            cache: false,
            success: function (data) {
                // Fill div with results
                $("#main-section").html(data);
                kendo.ui.progress(element, false);
            },
            error: function (xhr, status, error) {
                alert(xhr.responseText);
                kendo.ui.progress($("#main-section"), false);
            }
        });
    } else if (type_node == 'ACTIONS') {
        $("main_detail1-section").html('');
        $("main_detail2-section").html('');
        $.ajax({
            url: '../Actions/PartialEtapeAction/',
            data: { IdAction: Id},
            type: "GET",
            dataType: "html",
            cache: false,
            success: function (data) {
                //Fill div with results
                $("#main-section").html('');
                $("#main-section").html(data);
            },
            error: function (xhr, status, error) {
                alert(xhr.responseText);
            }
        });

        $.ajax({
            url: '../Actions/PartialDetailsAction/',
            data: { IdAction: Id },
            type: "GET",
            dataType: "html",
            cache: false,
            success: function (data) {
                
                $("#main-section").append(data);
            },
            error: function (xhr, status, error) {
                alert(xhr.responseText);
            }
        });
    } else {
        $("#main-section").html('');
}

}



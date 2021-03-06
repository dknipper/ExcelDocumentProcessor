// ---------------------------------------------------------------------
// Custom JavaScript functions 
// ---------------------------------------------------------------------

var GRID = function (path, tableName, databaseType, nonEditableColumns) {
    return {
        setDisplayButtons: function (row) {
            var eb = "<input style='padding-left: 5px; padding-right: 5px' type='image' src='" + path + "images/edit.gif' value='E' onclick=\"grid" + tableName + ".editRow('" + row + "')\" />";
            $("#" + row + " td:first-child").html(eb);
        },
        setEditorButtons: function(row) {
            var sb = "<input style='padding-left: 5px; padding-right: 5px' type='image' src='" + path + "images/save.gif' value='S' onclick=\"grid" + tableName + ".saveRow('" + row + "')\" />",
                cb = "<input style='padding-left: 5px; padding-right: 5px' type='image' src='" + path + "images/cancel.png' value='C' onclick=\"grid" + tableName + ".cancelRow('" + row + "')\" />";

            $("#" + row + " td:first-child").html(sb + cb);
        },
        editRow: function(row) {
            var inline = $("#" + tableName + "inlineEditing").val();

            if (inline === "true") {
                $('#' + tableName + 'GridTable').editRow(row);
                $('#' + tableName + 'GridTable_frozen').editRow(row);
                var ind = $("#" + tableName + "GridTable").getInd(row, true);
                if ($(ind).attr("editable") === "1") {
                    $('#' + tableName + 'GridTable_frozen' + " #" + row).height($('#' + tableName + 'GridTable' + " #" + row).height());
                }
                this.setEditorButtons(row);
            }
            else {
                var widthToCenter = ($(window).width() / 2) - 225;
                $('#'+tableName+'GridTable').editGridRow(row, {
                    top: 150,
                    left: widthToCenter,
                    width: 450,
                    afterShowForm: function ($form) {
                        $form.css({'max-height': '450px'});
                    },
                    onclickSubmit: function (options, postdata) {
                        for (var i = 0; i < nonEditableColumns.length; i++) {
                            postdata[nonEditableColumns[i]] = $(this).jqGrid("getCell", postdata[tableName + "GridTable_id"], nonEditableColumns[i]);
                        }
                        return {
                            tableName: JSON.stringify(tableName),
                            postData: JSON.stringify(postdata),
                            databaseType: JSON.stringify(databaseType)
                        };
                    }
                });
            }
        },
        cancelRow: function(row) {
            $('#' + tableName + 'GridTable').restoreRow(row);
            $('#' + tableName + 'GridTable_frozen').restoreRow(row);
            this.setDisplayButtons(row);
        },
        saveRow: function (row) {
            // copy all frozen information to underlying columns
            $('#' + tableName + 'GridTable_frozen' + ' #' + row + ' > td + td').each(function () {
                var myCol = $(this).attr('aria-describedby');
                $('#' + tableName + 'GridTable' + ' #' + row + ' > td[aria-describedby="' + myCol + '"]')
                    .children(':first').val($(this).children(':first').val());
            });
            $('#'+tableName+'GridTable').saveRow(row);
            var ind = $("#" + tableName + "GridTable").getInd(row, true);
            if ($(ind).attr("editable") === "0")
            {
                this.setDisplayButtons(row);
            }
        },
        createGrid: function (jqColNames, jqColModel, controller) {
            var $this = this;
            $("#" + tableName + "GridTable")
                .jqGrid({
                    url: path + controller + '/GetGridData',
                    editurl: path + controller + '/SaveGridData',
                    datatype: "json",
                    mtype: 'POST',
                    colNames: jqColNames,
                    colModel: jqColModel,
                    rowNum: 20,
                    rowList: [20, 50, 100, -1],
                    pager: "#" + tableName + "GridPager",
                    sortname: "",
                    scroll: false,
                    sortorder: "ASC",
                    loadui: 'disable',
                    viewrecords: true,
                    altRows: false,
                    shrinkToFit: false,         // shrinkToFit & width properties req for frozen cell functionality
                    width: $("#" + tableName + "Parent").width() - 2,
                    height: "auto",
                    hoverrows: false,
                    postData: { tableName: function () { return tableName; }, databaseType: function () { return databaseType; } },
                    gridComplete: function () {
                        $this.fixPositionsOfFrozenDivs.call(this);

                        ids = $(this).jqGrid('getDataIDs');
                        for (var i = 0; i < ids.length; i++) {
                            $this.setDisplayButtons(ids[i]);
                        }
                    },
                    loadBeforeSend: function() {
                        var headerWidth = $('#gview_' + tableName + 'GridTable .ui-jqgrid-hdiv div:first').width();
                        $('#gview_' + tableName + 'GridTable .ui-jqgrid-bdiv div:first').css({
                            width: headerWidth,
                            height: 1
                        })
                    },
                    resizeStop: function () {
                        $this.fixPositionsOfFrozenDivs.call(this);
                    },
                    serializeRowData: function (postdata) {
                        var tds = $("#" + postdata.id + ">td"),
                            textTds = tds.contents().filter(function () {
                                return this.nodeType === 3;
                            });
                        for (var i = 0; i < textTds.length; i++) {
                            var column = textTds[i].parentNode.getAttribute('aria-describedby').replace("" + tableName + "GridTable_", "");
                            postdata[column] = textTds[i].data;
                        }
                        return {
                            tableName: JSON.stringify(tableName),
                            postData: JSON.stringify(postdata),
                            databaseType: JSON.stringify(databaseType),
                        };
                    }
                })
                .setGridWidth($("#" + tableName + "Parent").width() - 2)    // sets the gridwidth based on parent's width
                .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

            $("#gview_" + tableName + "GridTable .searchDropDown")  // instantiate dropdowns for "excel-like" filters
            .prop('selectedIndex', -1)
            .multiselect({
                selectedList: 2,
                height: "200px",
                checkAllText: "all",
                uncheckAllText: "no",
                uncheckAll: true,
                noneSelectedText: "Any",
                beforeopen: function (event) {
                    $select = this;
                    // load dropdown options
                    $.ajax({
                        type: 'GET',
                        dataType: 'json',
                        data: {
                            table: tableName,
                            column: this.id.replace("gs_", ""),
                            databaseType: 3
                        },
                        cache: false,
                        url: path + controller + '/GetDropdownOptions',
                        async: false,
                        success: function (data) {
                            // store currently selected option(s)
                            var values = new Array();
                            $("#gview_" + tableName + "GridTable #" + $select.id)
                                .multiselect("getChecked")
                                .each(function (index, item) {
                                    values.push($(item).val());
                                });

                            $("#gview_" + tableName + "GridTable #" + $select.id).empty();
                            for (var i = 0; i < data.length; i++) {
                                var opt = document.createElement('option');
                                opt.value = data[i];
                                opt.innerHTML = data[i];
                                opt.selected = values.indexOf(data[i]) > -1 ? true : false;
                                $select.appendChild(opt);
                            }
                            $("#gview_" + tableName + "GridTable #" + $select.id).multiselect("refresh");
                        }
                    });
                }
            }).multiselectfilter({
                label: ""
            });

            $("#" + tableName + "GridTable")                .jqGrid('navGrid', '#'+tableName+'GridPager', {
                    edit: false,
                    add: false,
                    del: false,
                    search: false,
                    refresh: false,
                })                .jqGrid('navButtonAdd', '#' + tableName + 'GridPager', {    // Add icon for future "advanced search"
                    caption: "Advanced search",
                    buttonicon: "ui-icon-search",
                    onClickButton: function () {
                        $("#" + tableName + "GridTable")
                            .jqGrid('searchGrid', {
                                multipleSearch: true,
                                multipleGroup: true,
                                groupOps: [{ op: "AND", text: "AND" }, { op: "OR", text: "OR" }]
                            });
                    },
                    position: "first"
                })                .jqGrid('setFrozenColumns')
                .parents('div.ui-jqgrid-bdiv').css({ "overflow-y": "auto" }).css("max-height", "450px");

            $('#' + tableName + 'GridPager option[value=-1]').text('All');

            //$('#' + tableName + 'GridPager select').change(function () {
            //    if (this.value === "-1") {
            //        $('#' + tableName + 'GridTable').setGridParam({ scroll: true });
            //        $this.refreshData();
            //    }
            //});
        },
        hasData: function () {
            return ($("#" + tableName + "GridTable").jqGrid('getGridParam', 'reccount') > 0);
        },
        refreshData: function () {
            $("#" + tableName + "GridTable").trigger("reloadGrid");
        },
        fixPositionsOfFrozenDivs: function () {
            var $rows;
            if (typeof this.grid.fbDiv !== "undefined") {
                $rows = $('>div>table.ui-jqgrid-btable>tbody>tr', this.grid.bDiv);
                $('>table.ui-jqgrid-btable>tbody>tr', this.grid.fbDiv).each(function (i) {
                    var rowHight = $($rows[i]).height(), rowHightFrozen = $(this).height();
                    if ($(this).hasClass("jqgrow")) {
                        $(this).height(rowHight);
                        rowHightFrozen = $(this).height();
                        if (rowHight !== rowHightFrozen) {
                            $(this).height(rowHight + (rowHight - rowHightFrozen));
                        }
                    }
                });
                $(this.grid.fbDiv).height(this.grid.bDiv.clientHeight);
                $(this.grid.fbDiv).css($(this.grid.bDiv).position());
            }
            if (typeof this.grid.fhDiv !== "undefined") {
                $rows = $('>div>table.ui-jqgrid-htable>thead>tr', this.grid.hDiv);
                $('>table.ui-jqgrid-htable>thead>tr', this.grid.fhDiv).each(function (i) {
                    var rowHight = $($rows[i]).height(), rowHightFrozen = $(this).height();
                    $(this).height(rowHight);
                    rowHightFrozen = $(this).height();
                    if (rowHight !== rowHightFrozen) {
                        $(this).height(rowHight + (rowHight - rowHightFrozen));
                    }
                });
                $(this.grid.fhDiv).height(this.grid.hDiv.clientHeight);
                $(this.grid.fhDiv).css($(this.grid.hDiv).position());
            }
        }
    }
};

var STATIC_GRID = function (path, tableName) {
    return {
        createGrid: function (jqColNames, jqColModel, controller) {
            var $this = this;
            $("#" + tableName + "GridTable")
                .jqGrid({
                    url: path + controller + '/GetGridData',
                    editurl: path + controller + '/SaveGridData',
                    datatype: "json",
                    mtype: 'POST',
                    colNames: jqColNames,
                    colModel: jqColModel,
                    rowNum: 20,
                    rowList: [20, 50, 100],
                    pager: "#" + tableName + "GridPager",
                    sortname: "",
                    sortorder: "ASC",
                    loadui: 'disable',
                    viewrecords: true,
                    altRows: false,
                    hoverrows: false,
                    width: $("#" + tableName + "Parent").width() - 2,
                    postData: { tableName: function () { return tableName; } },
                    gridComplete: function () {
                        ids = jQuery("#" + tableName + "GridTable").jqGrid('getDataIDs');
                        for (var i = 0; i < ids.length; i++) {
                            $this.setDisplayButtons(ids[i]);
                        }
                    },
                    serializeRowData: function (postdata) {
                        var tds = $("#" + postdata.id + ">td"),
                            textTds = tds.contents().filter(function () {
                                return this.nodeType === 3;
                            });
                        for (var i = 0; i < textTds.length; i++) {
                            var column = textTds[i].parentNode.getAttribute('aria-describedby').replace("" + tableName + "GridTable_", "");
                            postdata[column] = textTds[i].data;
                        }
                        return {
                            tableName: JSON.stringify(tableName),
                            postData: JSON.stringify(postdata),
                            databaseType: JSON.stringify(databaseType),
                        };
                    }
                })
                .setGridWidth($("#" + tableName + "Parent").width() - 2);

            $("#" + tableName + "GridTable")                .jqGrid('navGrid', '#' + tableName + 'GridPager', {
                    edit: false,
                    add: false,
                    del: false,
                    search: false,
                    refresh: false,
                });
        },
        hasData: function () {
            return ($("#" + tableName + "GridTable").jqGrid('getGridParam', 'reccount') > 0);
        },
        refreshData: function () {
            $("#" + tableName + "GridTable").trigger("reloadGrid");
        }
    }
};

var allowIndexOf = function () {
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function (searchElement, fromIndex) {
            if (this === undefined || this === null) {
                throw new TypeError('"this" is null or not defined');
            }

            var length = this.length >>> 0; // Hack to convert object.length to a UInt32

            fromIndex = +fromIndex || 0;

            if (Math.abs(fromIndex) === Infinity) {
                fromIndex = 0;
            }

            if (fromIndex < 0) {
                fromIndex += length;
                if (fromIndex < 0) {
                    fromIndex = 0;
                }
            }

            for (; fromIndex < length; fromIndex++) {
                if (this[fromIndex] === searchElement) {
                    return fromIndex;
                }
            }

            return -1;
        };
    }
}
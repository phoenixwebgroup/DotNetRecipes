$(function () {
	//Enables to select rows in a table.
	//Write chosen attribute of selected rows to any other element. hidden or textbox to handle postbacks.

	//Creating namespace for rowselect
	jQuery.rowSelect = {};

	//Namespace for settings
	jQuery.rowSelect.settings =
    {
    	tableClassName: 'rowSelect',
    	rowSelectedClassName: 'selected'
    }

	//Default values for a rowSelect table
	jQuery.rowSelect.defaults = function () {
		this.attrName = '';
		this.allowMulti = false;
		this.selectionChanged = function () { }
	}

	//an array to hold settings for individual tables
	jQuery.rowSelect.allSettings = new Array();

	//Namespace for functions
	jQuery.rowSelect.api = {
		getSelected : function (selector) {
			var t = $(selector);
			if (!t || t.Length == 0) return;
			return $("tr.selected", t)
		},
		getSelectedValues : function (selector) {
			var vals = new Array();
			var tSettings = jQuery.rowSelect.api.getSettings(selector);
			var rows = jQuery.rowSelect.api.getSelected(selector);
			for (var x = 0; x < rows.length; x++) {
				vals[x] = $(rows[x]).attr(tSettings.attrName);
			}
			return vals;
		},
		getSettings : function(selector) {
			var t = $(selector);
			if (!t || t[0] == undefined) return null;
			return $.rowSelect.allSettings[t[0].id];
		}
	}
	

	//The main plugin..
	jQuery.fn.rowSelect = function (settings) {

		//Creating settings object for this call
		var s = new $.rowSelect.defaults();
		$.extend(s, settings);
		
		this.setSelected = function(values){
			//Looing trough all tr of the table, to check if it should be selected
			$("tbody tr", this).each(function () {
				for (var x = 0; x < values.length; x++) {
					if ($(this).attr(s.attrName) == values[x]) {
						$(this).addClass($.rowSelect.settings.rowSelectedClassName)
					}
				}
			})
		};

		this.each(function (i) {
			//Only tables is allowed to be selected..
			if (this.tagName.toUpperCase() != 'TABLE') return;

			//Adding the classname to the tbody of the table.
			//Maybe change this to table?
			$("tbody", this).addClass($.rowSelect.settings.tableClassName);

			//Storing the values for this table in then allSettings array
			$.rowSelect.allSettings[this.id] = s;

			//Attaching the click event to the tbody of the table. 
			$("tbody", this).bind("click",
                function (e) {
                	var tr, $tr;
                	var table;

                	//Getting the tr and not anything else the user might have clicked...
                	if (e.target.tagName != 'tr') {
                		tr = $(e.target).parents('tr:first');
                	} else {
                		tr = e.target;
                	}
                	///////////////////////////////

                	$tr = $(tr);

                	//Same thing with the table....
                	table = $tr.parents('table:first');

                	//if the table is not found.. Something is wrong 'aborting'
                	if (table.length == 0) return;

                	//Getting the settings object for this table.
                	//There is a possible 'hickup' here if the table does't have an id. 
                	var tSettings = $.rowSelect.allSettings[table[0].id];

                	//Check if it is multi or single select and handle accordingly...

                	if (tSettings.allowMulti) {
                		//toggle the class for this line
                		$tr.toggleClass($.rowSelect.settings.rowSelectedClassName);

                		var values = jQuery.rowSelect.api.getSelectedValues(table[0].id);
						tSettings.selectionChanged(values);
                	}
                	else {
                		$("tr", table).removeClass($.rowSelect.settings.rowSelectedClassName);
                		var otherTables = $('table tr').removeClass($.rowSelect.settings.rowSelectedClassName);
                		$tr.addClass($.rowSelect.settings.rowSelectedClassName);

                		//If current line is selected set the value to the attached input
                		if ($tr.hasClass($.rowSelect.settings.rowSelectedClassName)) {
                			var thisVal = $tr.attr(tSettings.attrName);
							tSettings.selectionChanged([thisVal]);
                		}
                	}
                })
		}
        )
		return this;
	}
});
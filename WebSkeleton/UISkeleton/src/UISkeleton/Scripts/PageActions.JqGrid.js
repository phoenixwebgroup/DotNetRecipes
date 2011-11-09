$.pageActions.JqGrid =
{
	 For : function(gridSelector) {
		$.pageActions.GetSelectedRecordData = $.pageActions.JqGrid.GetSelectedRecordData;
		$.pageActions.GetDisabledActions = $.pageActions.JqGrid.GetDisabledActions;
		$.pageActions.RefreshData = $.pageActions.JqGrid.RefreshData;
		
		$(function(){
			$(gridSelector).jqGrid('setGridParam', { onSelectRow: $.pageActions.JqGrid.SelectionChanged });
			$(gridSelector).jqGrid('setGridParam', { onSelectAll: $.pageActions.JqGrid.SelectionChanged });
			$(gridSelector).jqGrid('setGridParam', { gridComplete: $.pageActions.JqGrid.GridComplete } );
			
			$.pageActions.SelectionChanged();
		});
	},
	
	GetDisabledActions : function() {
		var grid = jQuery('#Grid');
		if($.pageActions.JqGrid.LastSelectedValues == undefined){
			return [];
		}
		return JSLINQ($.pageActions.JqGrid.LastSelectedValues)
			.Select(function(v) { return grid.getRowData(v); })
			.Select(function(d) { return d.disablePageActionsList; })
			.Where(function(l) { return l != undefined; })
			.SelectMany(function(l){ return l.split(' '); })
			.Distinct()
			.ToArray();
	},
	
	GetSelectedRecordData : function() {
		// Default is to get the key off of a jqGrid
		var grid = jQuery('#Grid');
		if(grid == undefined) {
			showWarning('Please setup a source of items to pick from and overload pageActions.GetSelectedRecordData as necessary');
			return [];
		}
		
		var parser = $.pageActions.JqGrid.ParseId;
		if (grid.getGridParam('multiselect') == true) {
			var keys = JSLINQ(grid.getGridParam('selarrrow'));
			if(keys.Count() == 0)
			{
				return null;
			}
			var jsonData = parser(keys.First());
			var jsonDataIds = { ids: keys.Select(parser).ToArray() };
			$.extend(jsonData, jsonDataIds);
			return jsonData;
		}
		var selectedRowId = grid.getGridParam('selrow');
		if(selectedRowId == undefined)
		{
			return null;
		}
		return parser(selectedRowId);
	},
	
	ParseId : function(value) {
		return { id: value };
	},
	
	RefreshData : function() {
		if (jQuery('#Grid') != undefined) {
			jQuery('#Grid').trigger('reloadGrid');
		}
	},
	
	LastSelectedValues : null,
	
	SelectionChanged : function() {
		var grid = jQuery('#Grid');
		if (grid.getGridParam('multiselect') == true) {
			$.pageActions.JqGrid.LastSelectedValues = grid.getGridParam('selarrrow');
		}
		else{
			var selectedId = grid.getGridParam('selrow');
			$.pageActions.JqGrid.LastSelectedValues = (selectedId != undefined ? [ selectedId ] : []);
		}
		$.pageActions.SelectionChanged();
	},
	
	GridComplete : function() {
		var grid = jQuery('#Grid');
		if($.pageActions.JqGrid.LastSelectedValues != undefined){
			JSLINQ($.pageActions.JqGrid.LastSelectedValues)
				.ForEach(function(v) { grid.setSelection(v, false); });
		}
		$.pageActions.SelectionChanged();
	}
}
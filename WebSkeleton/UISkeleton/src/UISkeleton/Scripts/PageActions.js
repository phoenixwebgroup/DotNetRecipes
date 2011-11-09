
$.pageActions = {};

$.pageActions.contextDataForRequests = null;

// Returns a json object of the parameters
$.pageActions.GetContextDataForRequests = function () {
	if ($.pageActions.contextDataForRequests != null) {
		return $.pageActions.contextDataForRequests;
	}
	return {};
}

$.pageActions.ShouldOpenNewWindow = function (button) {
	return button.attr('OpenNewWindow') == "true";
}

$.pageActions.GetCloseWindowOptions = function (button) {
    var options = {
        Close: button.attr('CloseWindow') != undefined,
        Delay: button.attr('CloseWindow')
    };
    return options;
}

$.pageActions.OpenNewWindow = function (action) 
{
	window.open(action);
}

$.pageActions.ButtonActionForSelected = function (buttonId, action) {
	var button = $('#' + buttonId);
	button.addClass('buttonActionForSelected');
	
	var options = 
	{
		action: action,
		title: button.val(),
		openNewWindow: $.pageActions.ShouldOpenNewWindow(button)
	};

	button.click(function () {
		$.pageActions.ButtonActionForSelected.Execute(options);
	});
}

$.pageActions.ButtonActionForSelected.Execute = function (options) {
	var selectedData = $.pageActions.GetSelectedRecordData();
	if (selectedData == undefined) {
		showWarning('Please select an item');
		return;
	}
	if($.pageActions.NumberSelected() > 1)
	{
		showWarning('Action not allowed with multiple selections');
		return;
	}	
	$.pageActions.DoOpenWindow(options.action, options.title, options.openNewWindow, selectedData);
}

$.pageActions.ButtonCommandForSelected = function (buttonId, action) {
	var button = $('#' + buttonId);
	button.addClass('buttonCommandForSelected');
    var options = 
	{
		action: action,
		title: button.val(),
		closeWindow: $.pageActions.GetCloseWindowOptions(button)
	};

    button.click(function() {
		$.pageActions.ButtonCommandForSelected.Execute(options);
	});
}

$.pageActions.ButtonCommandForSelected.Execute = function (options) {
	var selectedData = $.pageActions.GetSelectedRecordData();
	if (selectedData == undefined) {
		showWarning('Please select an item');
		return;
	}
	
	if (window.confirm('Are you sure you want to ' + options.title + '?'))
	{
		var contextData = $.pageActions.GetContextDataForRequests();
		var url = options.action;
		$.extend(selectedData, contextData);
		$.ajax({ 
			type: 'POST',
			url: url,
			contentType: 'application/json; charset=utf-8',
			data: $.toJSON(selectedData),
			success: function (data) {
				showNotification(data);
				$.pageActions.RefreshData();
				if(options.closeWindow.Close)
				{
					 setTimeout('$.pageActions.CloseWindow()', options.closeWindow.Delay);
				}
			},
			error: function (xhr) {
				showWarning(xhr.responseText);
			}
		});
	}
}

$.pageActions.ButtonAction = function (buttonId, action) {
	var button = $('#' + buttonId);
	button.addClass('buttonAction');

	var options =
	{
		action: action,
		title: button.val(),
		openNewWindow: $.pageActions.ShouldOpenNewWindow(button)
	};

	button.click(function () {
		$.pageActions.ButtonAction.Execute(options);
	});
}

$.pageActions.ButtonAction.Execute = function(options) {
	$.pageActions.DoOpenWindow(options.action, options.title, options.openNewWindow, {});
}

$.pageActions.DoOpenWindow = function (action, title, openNewWindow, selectedData) {
	var contextData = $.pageActions.GetContextDataForRequests();
	$.extend(selectedData, contextData);
	action = action + "?" + $.param(selectedData);
	
	if (openNewWindow) {
		$.pageActions.OpenNewWindow(action);
	}
	else {
		$.pageActions.OpenWindow(action, title);
	}
}

$.pageActions.OpenWindow = function (action, title) {
	window.location = action;
};

// Returns a json object of the selected key(s)
$.pageActions.GetSelectedRecordData = null;

$.pageActions.GetDisabledActions = null;

$.pageActions.RefreshFilterForm = function(formSelector) {
	$.pageActions.RefreshData = function() {
		$(formSelector).submit();
	}
}

$.pageActions.RefreshData = function() {};

$.pageActions.ResetFilters = function () {  };

$.pageActions.SelectionChanged = function () {
	$.pageActions.DisableActions();
};

$.pageActions.EnableAllActions = function () {
	$('.pageAction').removeAttr("disabled");
}

$.pageActions.DisableActions = function () {
	$.pageActions.EnableAllActions();
	if($.pageActions.NumberSelected() != 1){
		$('.buttonActionForSelected').attr("disabled", "disabled");
	}
	if($.pageActions.NumberSelected() == 0){
		$('.buttonCommandForSelected').attr("disabled", "disabled");
	}
	$($.pageActions.GetDisabledActions().join(', ')).attr("disabled", "disabled");
}

$.pageActions.NumberSelected = function()
{
	var selectedData = $.pageActions.GetSelectedRecordData();
	if(selectedData == undefined)
	{
		return 0;
	}
	if(selectedData.ids == undefined)
	{
		return 1;
	}
	return selectedData.ids.length;
}

$.pageActions.CloseWindow = function () {
	$.ajaxFormsExtensions.AjaxForm.Cancel();
}
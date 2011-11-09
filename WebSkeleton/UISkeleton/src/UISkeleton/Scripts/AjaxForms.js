$.ajaxFormsExtensions = {};

$.ajaxFormsExtensions.GenerateFilterForm = function (formSelector, targetSelector, autoSubmit) {
    $(formSelector).ajaxForm({
        target: targetSelector,
        beforeSubmit: $.ajaxFormsExtensions.AjaxForm.Validate
    });
	$(formSelector + ' .exportButton').click(function (event) { $.ajaxFormsExtensions.ExportButtonClicked(formSelector, event); });
    if (autoSubmit) {
        $(formSelector + ' select, ' + formSelector + ' input').change(function () { $.ajaxFormsExtensions.FilterFormSubmit(formSelector); });
		
		 // Load form on first load!
		$.ajaxFormsExtensions.FilterFormSubmit(formSelector);
    }
}

$.ajaxFormsExtensions.FilterForm = function (formSelector, targetSelector) {
    $.ajaxFormsExtensions.GenerateFilterForm(formSelector, targetSelector, true);
}

$.ajaxFormsExtensions.FilterFormSubmit = function (formSelector) {
	$.ajaxFormsExtensions.SetOutputType('');
	$(formSelector).submit();
}

$.ajaxFormsExtensions.SetOutputType = function (type) {
	var outputType = $('#outputType');
	if (outputType != undefined) {
		outputType.val(type);
	}
}

$.ajaxFormsExtensions.ExportButtonClicked = function (formSelector, event) {
	if (!$(formSelector).valid()) {
		return;
	}

	var outputType = $('#' + event.target.id).attr('outputType');
	$.ajaxFormsExtensions.SetOutputType(outputType);
	window.open($(formSelector).attr('action') + '?' + $(formSelector).formSerialize());
	$.ajaxFormsExtensions.SetOutputType('');
}

$.ajaxFormsExtensions.MakeFormsAjax = function() {
	$('form').ajaxForm({
		success: $.ajaxFormsExtensions.AjaxForm.Success,
		error: $.ajaxFormsExtensions.AjaxForm.Error,
		beforeSubmit: $.ajaxFormsExtensions.AjaxForm.Validate
	});
}

$.ajaxFormsExtensions.AjaxForm = {};

$.ajaxFormsExtensions.AjaxForm.Cancel = function() {
	window.history.back(1);
}

$.ajaxFormsExtensions.AjaxForm.Success = function(data) {
	alert(data);
	window.history.back(1);
}

$.ajaxFormsExtensions.AjaxForm.Error = function(xhr) {
	alert(xhr.responseText);
}

$.ajaxFormsExtensions.AjaxForm.Validate = function (arr, $form, options) {
	return	$form.valid();
}
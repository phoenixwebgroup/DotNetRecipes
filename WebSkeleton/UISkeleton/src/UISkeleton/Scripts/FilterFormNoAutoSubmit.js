$(function(){
	$.ajaxFormsExtensions.FilterForm = function (formSelector, targetSelector) {
		$.ajaxFormsExtensions.GenerateFilterForm(formSelector, targetSelector, false);
		$.pageActions.RefreshFilterForm(formSelector);
	};
});
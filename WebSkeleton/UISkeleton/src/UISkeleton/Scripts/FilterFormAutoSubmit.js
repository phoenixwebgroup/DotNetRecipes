$(function(){
	$.ajaxFormsExtensions.FilterForm = function (formSelector, targetSelector) {
		$.ajaxFormsExtensions.GenerateFilterForm(formSelector, targetSelector, true);
		$.pageActions.RefreshFilterForm(formSelector);
	};
});
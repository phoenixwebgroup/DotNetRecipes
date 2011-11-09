$(function()
{
	$.ajaxFormsExtensions.FilterForm = function (formSelector, targetSelector) {
		$.ajaxFormsExtensions.GenerateFilterForm(formSelector, targetSelector, true);
	};
	$("#FilterForm,.FilterForm").each(function(){
		 var form = $(this);
		 form.attr("onSubmit","$(defaultGridName).trigger('reloadGrid');"); 
		 form.attr("action","javascript:"); 
	});
});